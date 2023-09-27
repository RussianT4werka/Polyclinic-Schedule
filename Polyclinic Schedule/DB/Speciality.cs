using System;
using System.Collections.Generic;
using Polyclinic_Schedule.DTO;

namespace Polyclinic_Schedule.DB;

public partial class Speciality
{
    public int Id { get; set; }

    public string? Title { get; set; }

    public virtual ICollection<Doctor> Doctors { get; set; } = new List<Doctor>();
}
