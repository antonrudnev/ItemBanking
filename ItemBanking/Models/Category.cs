using System.Collections.Generic;

namespace ItemBanking.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ItemBank ItemBank { get; set; }
        public Category ParentCategory { get; set; }

        public ICollection<Category> SubCategories { get; set; }
        public ICollection<Item> Items { get; set; }
    }
}