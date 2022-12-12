using System;
using System.Collections.Generic;

namespace GroceryListAPI.Models;

public partial class GroceryList
{
    public int Id { get; set; }

    public int AppUserId { get; set; }

    public string Name { get; set; } = null!;

    public bool ShowCrossedOff { get; set; }

    public string ItemsJson { get; set; } = null!;

    public virtual AppUser AppUser { get; set; } = null!;
}
