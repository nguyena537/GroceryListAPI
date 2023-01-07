namespace GroceryListAPI.Models.DTOs
{
    public class GroceryListDto
    {
        public int Id { get; set; }

        public int AppUserId { get; set; }

        public string Name { get; set; } = null!;

        public bool ShowCrossedOff { get; set; }
        public int NumItems { get; set; }
    }
}
