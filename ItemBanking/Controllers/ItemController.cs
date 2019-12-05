using ItemBanking.Data;
using ItemBanking.Models;
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

        public async Task<IActionResult> Index(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var itemBank = await _context.ItemBanks.Include("Categories.Items").Include("Language").SingleOrDefaultAsync(x => x.Id == id);
            if (itemBank == null)
            {
                return NotFound();
            }
            return View(itemBank);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _context.Items.Include("Category.ItemBank.Language").SingleOrDefaultAsync(x => x.Id == id);
            if (item == null)
            {
                return NotFound();
            }
            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Content")]Item item)
        {
            if (id != item.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                _context.Update(item);
                await _context.SaveChangesAsync();
                int categoryId = await _context.Items.Where(x => x.Id == id).Select(x => x.Category.ItemBank.Id).SingleOrDefaultAsync();
                return RedirectToAction(nameof(Index), new { id = categoryId });
            }
            return View(item);
        }
    }
}