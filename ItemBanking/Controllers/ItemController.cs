using ItemBanking.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace ItemBanking.Controllers
{
    public class ItemController : Controller
    {
        private readonly ItemBankContext _context;
        private readonly ILogger<ItemController> _logger;

        public ItemController(ItemBankContext context, ILogger<ItemController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IActionResult> Index(int id)
        {
            var itemBank = await _context.ItemBanks.Where(x => x.Id == id).Include("Categories.Items").SingleAsync();
            return View(itemBank);
        }
    }
}