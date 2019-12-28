using CsvHelper;
using ItemBanking.Data;
using ItemBanking.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.IO;
using System.Linq;
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
            _logger.LogInformation("Executed endpoint '/ItemBank/Index'");
            var itemBanks = await _context.ItemBanks.Include("Language").ToListAsync();
            return View(itemBanks);
        }

        public async Task<IActionResult> Export(int id)
        {
            _logger.LogInformation($"Executed endpoint '/ItemBank/Export/{id}'");
            var table = await _context.Categories.Where(x => x.ItemBank.Id == id).SelectMany(x => x.Items.DefaultIfEmpty(), (c, i) =>
                new
                {
                    ItemBank = c.ItemBank.Name,
                    CategoryId = c.Id,
                    CategoryName = c.Name,
                    ItemId = i.Id,
                    ItemName = i.Name,
                    Content = i.Content
                }).ToListAsync();

            var items = table.Select(x => new
            {
                x.CategoryId,
                x.CategoryName,
                x.ItemId,
                x.ItemName,
                x.Content
            });

            string itemBankName = table.Select(x => x.ItemBank).First();

            using (var stream = new MemoryStream())
            {
                using (var writer = new StreamWriter(stream))
                {
                    using (var csv = new CsvWriter(writer))
                    {
                        csv.WriteRecords(items);
                        writer.Flush();
                    }
                }
                return File(stream.ToArray(), "text/csv", $"{itemBankName}.csv");
            }
        }

        public IActionResult Privacy()
        {
            _logger.LogInformation("Executed endpoint '/ItemBank/Privacy'");
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
