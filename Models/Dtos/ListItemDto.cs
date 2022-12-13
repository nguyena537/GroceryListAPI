namespace GroceryListAPI.Models.DTOs
{
    public class ListItemDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string Unit { get; set; } = null!;

        public string? PhotoUrl { get; set; }

        public int? CategoryId { get; set; }

        public string? Category { get; set; }

        public float? quantity { get; set; }

        public bool crossedOff { get; set; } = false;

        public string? note { get; set; } = null!;

    }
}
