using System;
using System.Collections.Generic;

namespace GroceryListAPI.Models;

public partial class AppUser
{
    public int Id { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public virtual AppUserSetting? AppUserSetting { get; set; }

    public virtual ICollection<Category> Categories { get; } = new List<Category>();

    public virtual ICollection<GroceryList> GroceryLists { get; } = new List<GroceryList>();
}
