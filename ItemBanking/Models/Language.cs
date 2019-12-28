using System.Collections.Generic;

namespace ItemBanking.Models
{
    public class Language
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public bool IsRightToLeft { get; set; }

        public ICollection<ItemBank> ItemBanks { get; set; }
    }
}
