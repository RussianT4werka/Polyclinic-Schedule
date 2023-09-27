namespace Polyclinic_Schedule.DTO
{
    public class User
    {
        public int Id { get; set; }

        public string LastName { get; set; }

        public string FirstName { get; set; }

        public string PatronymicName { get; set; }

        public string Login { get; set; }

        public string Email { get; set; }

        public static explicit operator User(DB.User user)
        {
            return new User
            {
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Login = user.Login,
                PatronymicName = user.PatronymicName,
                Id = user.Id
            };
        }
    }
}
