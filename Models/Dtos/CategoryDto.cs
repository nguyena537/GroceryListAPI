namespace GroceryListAPI.Models.DTOs
{
    public class CategoryDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string? PhotoUrl { get; set; }

        public bool IsCustom { get; set; }

        public int? AppUserId { get; set; }
    }
}
