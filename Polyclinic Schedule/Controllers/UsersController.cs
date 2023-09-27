using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Polyclinic_Schedule.DB;
using Polyclinic_Schedule.Model;
using Polyclinic_Schedule.Tools;
using System.Security.Cryptography;
using System.Text;
using System.Text.Unicode;

namespace Polyclinic_Schedule.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        readonly User30Context db;

        public UsersController(User30Context db)
        {
            this.db = db;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user">юзер с логином, паролем и email</param>
        /// <returns>id клиента</returns>
        [HttpPost("Registration")]
        public async Task<ActionResult<int>> Registration(
            [FromBody]UserData user)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(user.Login) ||
                    string.IsNullOrWhiteSpace(user.Password) ||
                    string.IsNullOrWhiteSpace(user.Email))
                    return BadRequest("мало данных. введите еще.");

                var checkUser = await db.Users.AnyAsync(s => s.Email == user.Email);
                if (checkUser)
                    return BadRequest("email уже используется. Мб это твой? Восстанови сам");

                string hashPass = Hash.HashPassword(user);

                User newUser = new User
                {
                    Login = user.Login,
                    Password = hashPass,
                    Email = user.Email,
                };
                
                await db.Users.AddAsync(newUser);
                await db.SaveChangesAsync();
                return Ok(newUser.Id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Наш супер метод
        /// </summary>
        /// <param name="user">UserData</param>
        /// <returns>id клиента</returns>
        [HttpPost("Authorization")]
        public async Task<ActionResult<int>> Authorization([FromBody]UserData user)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(user.Login) ||
                    string.IsNullOrWhiteSpace(user.Password))
                    return BadRequest("мало данных. введите еще.");

                var findUser = await db.Users.FirstOrDefaultAsync(s => s.Login == user.Login);
                if (findUser == null)
                    return BadRequest("Логин не найден. Регистрироваться будешь, а?");

                string hashPass = Hash.HashPassword(user);
                if (findUser.Password != hashPass)
                    return BadRequest("Пароль неверный. Подумай еще. Спроси родителей");
                
                return Ok(findUser.Id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
