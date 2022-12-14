namespace GroceryListAPI.Models.DTOs
{
    public class ListItemDto
    {
        public ListItemDto(int _ItemId)
        {
            ItemId = _ItemId;
        }

        public int ItemId { get; set; }

        public string? Category { get; set; }

        public float? Quantity { get; set; }

        public bool CrossedOff { get; set; } = false;

        public string? Note { get; set; } = null!;

    }
}
