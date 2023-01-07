namespace GroceryListAPI.Models.DTOs
{
    public class AppUserSettingDto
    {
        public int AppUserId { get; set; }

        public bool ShowCustom { get; set; }

        public bool DarkMode { get; set; }

        public bool ShowCrossedOff { get; set; }
    }
}
