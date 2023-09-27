using System;
using System.Collections.Generic;
using Polyclinic_Schedule.DB;

public partial class Doctor
{
    public int Id { get; set; }

    public string? LastName { get; set; }

    public string? FirstName { get; set; }

    public string? PatronymicName { get; set; }

    public int? IdSpeciality { get; set; }

    public virtual ICollection<Cabinet> Cabinets { get; set; } = new List<Cabinet>();

    public virtual Speciality? IdSpecialityNavigation { get; set; }

    public virtual ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();
}
