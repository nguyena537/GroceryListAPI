using System;
using System.Collections.Generic;

namespace GroceryListAPI.Models;

public partial class Category
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? PhotoUrl { get; set; }

    public bool IsCustom { get; set; }

    public int? AppUserId { get; set; }

    public virtual AppUser? AppUser { get; set; }

    public virtual ICollection<Item> Items { get; } = new List<Item>();
}
