using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Polyclinic_Schedule.DB;

namespace Polyclinic_Schedule.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScheduleController : ControllerBase
    {
        readonly User30Context db;

        public ScheduleController(User30Context db)
        {
            this.db = db;
        }

        [HttpGet("ListSpeciality")]
        public async Task<ActionResult<List<DTO.Speciality>>>
            ListSpeciality()
        {
            try
            {
                var array = await db.Specialities.ToListAsync();
                List<DTO.Speciality> result = new();
                foreach (var item in array)
                {
                    result.Add(new DTO.Speciality
                    {
                        Id = item.Id,
                        Title = item.Title
                    });
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("ListDoctorBySpeciality")]
        public async Task<ActionResult<List<DTO.Doctor>>>
            ListDoctorBySpeciality(int idSpeciality)
        {
            try
            {
                var array = await db.Doctors.Where(s => s.IdSpeciality == idSpeciality).ToListAsync();
                List<DTO.Doctor> result = new();
                foreach (var item in array)
                {
                    result.Add(new DTO.Doctor
                    {
                        Id = item.Id,
                        FirstName = item.FirstName,
                        LastName = item.LastName,
                        PatronymicName = item.PatronymicName
                    });
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetDoctorSchedule")]
        public async Task<ActionResult<List<DTO.UserSchedule>>>
            ListDoctorSchedule(int id, DateTime start, DateTime finish)
        {
            try
            {
                var schedules = await db.Schedules.
                    Include("IdDoctorNavigation.Cabinets").
                    Include("IdDoctorNavigation").
                    Include("IdDoctorNavigation.IdSpecialityNavigation").
                   Where(s => s.IdDoctor == id &&
                   s.StartTime >= start && s.StartTime <= finish).
                   ToListAsync();
                if (schedules == null)
                    return NotFound();

                List<DTO.UserSchedule> result = new();
                foreach (var schedule in schedules)
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

        [HttpPost]
        public async Task<ActionResult> AddUserSchedule(DTO.UserSchedule userSchedule)
        { 
            try
            { 
                var schedule = await db.Schedules.FindAsync(userSchedule.Id);
                if (schedule == null) 
                    return NotFound();
                if (schedule.IdUser != null)
                    return BadRequest("Занято!");
                schedule.IdUser = userSchedule.IdUser;
                await db.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest($"{ex.Message}");
            }
        }

    }
}
