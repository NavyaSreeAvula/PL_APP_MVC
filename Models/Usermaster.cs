using System;
using System.Collections.Generic;

namespace PL_APP_MVC.Models;

public partial class Usermaster
{
    public int Id { get; set; }

    public string? Username { get; set; }

    public string? Password { get; set; }

    public string? Name { get; set; }

    public string? Role { get; set; }
}
