using System.Collections.Generic;

namespace ItemBanking.Models
{
    public class ItemBank
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Language Language { get; set; }

        public ICollection<Category> Categories { get; set; }
    }
}