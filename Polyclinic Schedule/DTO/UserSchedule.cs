using Polyclinic_Schedule.DB;

namespace Polyclinic_Schedule.DTO
{
    public class UserSchedule
    {
        public int Id { get; set; }
        public int IdDoctor { get; set; }
        public int? IdUser { get; set; }

        public DateTime StartTime { get; set; }
        public string DoctorLastName { get; set; }
        public string DoctorFirstName { get; set; }
        public string DoctorPatronymicName { get; set; }
        public string DoctorSpeciality { get; set; }

        public int? CabinetNumber { get; set; }
    }
}
