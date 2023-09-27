using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Polyclinic_Schedule.DB;
using System.Collections.Generic;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Polyclinic_Schedule.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserCabinetController : ControllerBase
    {
        readonly User30Context db;

        public UserCabinetController(User30Context db)
        {
            this.db = db;
        }

        [HttpGet("GetUserInfo")]
        public async Task<ActionResult<DTO.User>> GetUser(int id)
        {
            try
            {
                var user = await db.Users.
                    FirstOrDefaultAsync(s => s.Id == id);
                if (user == null)
                    return NotFound();
                var userDTO = (DTO.User)user;
                return Ok(userDTO);
            }
            catch (Exception ex)
            {
                return BadRequest($"{ex.Message}");
            }
        }

        [HttpPost]
        public async Task<ActionResult> EditUser(DTO.User data)
        {
            try
            {
                var user = await db.Users.
                   FirstOrDefaultAsync(s => s.Id == data.Id);
                if (user == null)
                    return NotFound();

                user.FirstName = data.FirstName;
                user.LastName = data.LastName;
                user.PatronymicName = data.PatronymicName;

                await db.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest($"{ex.Message}");
            }
        }

        [HttpGet("GetUserSchedule")]
        public async Task<ActionResult<List<DTO.UserSchedule>>>
            ListUserSchedule(int id)
        {
            try
            {
                var user = await db.Users.
                    Include(s => s.Schedules).
                    Include("Schedules.IdDoctorNavigation").
                    Include("Schedules.IdDoctorNavigation.IdSpecialityNavigation").
                    Include("Schedules.IdDoctorNavigation.Cabinets").
                   FirstOrDefaultAsync(s => s.Id == id);
                if (user == null)
                    return NotFound();

                List<DTO.UserSchedule> result = new();
                foreach (var schedule in user.Schedules)
                {
                    result.Add(new DTO.UserSchedule
                    {
                        StartTime = schedule.StartTime,
                        Id = schedule.Id,
                        IdUser = schedule.IdUser,
                        IdDoctor = schedule.IdDoctor,
                        DoctorFirstName = schedule.IdDoctorNavigation?.FirstName,
                        DoctorLastName = schedule.IdDoctorNavigation?.LastName,
                        DoctorPatronymicName = schedule.IdDoctorNavigation?.PatronymicName,
                        DoctorSpeciality = schedule.IdDoctorNavigation?.IdSpecialityNavigation?.Title,
                        CabinetNumber = schedule.IdDoctorNavigation?.Cabinets?.First().Number
                    });
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest($"{ex.Message}");
            }
        }
    }
}
