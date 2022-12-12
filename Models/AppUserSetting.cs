using System;
using System.Collections.Generic;

namespace GroceryListAPI.Models;

public partial class AppUserSetting
{
    public int AppUserId { get; set; }

    public bool ShowCustom { get; set; }

    public bool DarkMode { get; set; }

    public virtual AppUser AppUser { get; set; } = null!;
}
