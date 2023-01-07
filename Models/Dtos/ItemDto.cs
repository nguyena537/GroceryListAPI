namespace GroceryListAPI.Models.DTOs
{
    public class ItemDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string Unit { get; set; } = null!;

        public string? PhotoUrl { get; set; }

        public string CategoryName { get; set; } = string.Empty;
        
        public bool IsCustom { get; set; }
    }
}
