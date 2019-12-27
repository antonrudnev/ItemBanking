using ItemBanking.Data;
using ItemBanking.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Threading.Tasks;

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
            _logger.LogInformation("Executed endpoint 'ItemBankController.Index'");
            var itemBanks = await _context.ItemBanks.Include("Language").ToListAsync();
            return View(itemBanks);
        }

        public IActionResult Privacy()
        {
            _logger.LogInformation("Executed endpoint 'ItemBankController.Privacy'");
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}