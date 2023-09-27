using System;
using System.Collections.Generic;
using Polyclinic_Schedule.DTO;

namespace Polyclinic_Schedule.DB;

public partial class Cabinet
{
    public int Number { get; set; }

    public int? IdDoctor { get; set; }

    public virtual Doctor? IdDoctorNavigation { get; set; }
}
