namespace ItemBanking.Models
{
    public class Item
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public Category Category { get; set; }
    }
}