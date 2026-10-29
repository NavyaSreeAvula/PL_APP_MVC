using System;
using System.Collections.Generic;

namespace PL_APP_MVC.Models;

public partial class Rulesmaster
{
    public int Id { get; set; }

    public string? Rulename { get; set; }

    public int? Minvalue { get; set; }

    public int? Maxvalue { get; set; }
}
