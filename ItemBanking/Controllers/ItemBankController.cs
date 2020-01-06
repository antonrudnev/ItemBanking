using CsvHelper;
using ItemBanking.Data;
using ItemBanking.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ItemBanking.Controllers
{
    public class ItemBankController : Controller
    {
        private readonly ItemBankDbContext _context;
        private readonly ILogger<ItemBankController> _logger;

        public ItemBankController(ItemBankDbContext context, ILogger<ItemBankController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var ip = Request.HttpContext.Connection.RemoteIpAddress.MapToIPv4();
            _logger.LogInformation($"Executed endpoint '/ItemBank/Index' from {ip}");
            var itemBanks = await _context.ItemBanks.Include("Language").ToListAsync();
            return View(itemBanks);
        }

        public async Task<IActionResult> Export(int id)
        {
            var ip = Request.HttpContext.Connection.RemoteIpAddress.MapToIPv4();
            _logger.LogInformation($"Executed endpoint '/ItemBank/Export/{id}' from {ip}");
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Import(int id, IFormFile file)
        {
            if (file != null)
            {
                using (var stream = new MemoryStream())
                {
                    using (var reader = new StreamReader(file.OpenReadStream()))
                    using (var csv = new CsvReader(reader))
                    {
                        var records = csv.GetRecords(new
                        {
                            CategoryId = default(int),
                            CategoryName = string.Empty,
                            ItemId = default(int?),
                            ItemName = string.Empty,
                            Content = string.Empty
                        });
                        var items = records.Where(x => x.ItemId != null).Select(x => new Item
                        {
                            Id = x.ItemId,
                            Name = x.ItemName,
                            Content = x.Content
                        });
                        foreach (var item in items)
                        {
                            _context.Attach(item);
                            _context.Entry(item).Property(x => x.Name).IsModified = true;
                            _context.Entry(item).Property(x => x.Content).IsModified = true;
                        }
                        await _context.SaveChangesAsync();
                    }
                }
            }
            return RedirectToAction("Index", "Item", new { id });
        }

        public IActionResult Privacy()
        {
            var ip = Request.HttpContext.Connection.RemoteIpAddress.MapToIPv4();
            _logger.LogInformation($"Executed endpoint '/ItemBank/Privacy' from {ip}");
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
