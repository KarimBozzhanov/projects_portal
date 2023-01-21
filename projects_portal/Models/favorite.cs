namespace projects_portal.Models
{
    public class favorite
    {
        public int Id { get; set; }
        public int userId { get; set; }
        public string userNameFavorite { get; set; }
        public int postFavoriteId { get; set; }
    }
}
