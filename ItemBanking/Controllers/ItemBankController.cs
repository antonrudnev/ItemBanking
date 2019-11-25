using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using ItemBanking.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ItemBanking.Models;
using Microsoft.EntityFrameworkCore;

namespace ItemBanking.Controllers
{
    public class ItemBankController : Controller
    {
        private readonly ItemBankContext _context;
        private readonly ILogger<ItemBankController> _logger;

        public ItemBankController(ItemBankContext context, ILogger<ItemBankController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var itemBanks = await _context.ItemBanks.ToListAsync();
            return View(itemBanks);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}