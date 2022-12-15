using System;
using System.Collections.Generic;

namespace GroceryListAPI.Models;

public partial class Item
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Unit { get; set; }

    public string? PhotoUrl { get; set; }

    public int? CategoryId { get; set; }

    public bool IsCustom { get; set; }

    public virtual Category? Category { get; set; }
}
