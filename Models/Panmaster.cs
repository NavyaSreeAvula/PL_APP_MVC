using System;
using System.Collections.Generic;

namespace PL_APP_MVC.Models;

public partial class Panmaster
{
    public int Id { get; set; }

    public string? Pancardnumber { get; set; }

    public string? Firstname { get; set; }

    public string? Middlename { get; set; }

    public string? Lastname { get; set; }

    public string? Fathername { get; set; }

    public DateOnly? Dob { get; set; }
}
