using Fertilizer360.Data;
using Fertilizer360.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Fertilizer360.Controllers
{
    public class ShopkeeperController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ShopkeeperController(AppDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            return View();
        }
        // Display Fertilizers
        public async Task<IActionResult> FertilizerManagement()
        {
            var fertilizers = await _context.Fertilizers.Include(f => f.Shop).ToListAsync();
            return View(fertilizers);
        }

        // Add Fertilizer View
        public IActionResult AddFertilizer()
        {
            ViewBag.Shops = _context.Shops.ToList();
            return View();
        }

        // Add Fertilizer Logic
        [HttpPost]
        public async Task<IActionResult> AddFertilizer(Fertilizer fertilizer, IFormFile ImageFile)
        {
            ViewBag.Shops = _context.Shops.ToList();

            if (!int.TryParse(Request.Form["ShopId"], out int shopId))
            {
                ModelState.AddModelError("ShopId", "Invalid Shop selection.");
                return View(fertilizer);
            }
            fertilizer.ShopId = shopId;

            if (!ModelState.IsValid)
            {
                return View(fertilizer);
            }

            try
            {
                if (fertilizer.ShopId == 0)
                {
                    TempData["ErrorMessage"] = "Please select a valid shop.";
                    return View(fertilizer);
                }

                if (ImageFile != null && ImageFile.Length > 0)
                {
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads/fertilizers");
                    Directory.CreateDirectory(uploadsFolder);
                    string uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(ImageFile.FileName);
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await ImageFile.CopyToAsync(fileStream);
                    }
                    fertilizer.Image = "/uploads/fertilizers/" + uniqueFileName;
                }

                _context.Fertilizers.Add(fertilizer);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Fertilizer added successfully!";
                return RedirectToAction("FertilizerManagement");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error adding fertilizer: " + ex.Message;
                return View(fertilizer);
            }
        }

        // Edit Fertilizer View
        public async Task<IActionResult> EditFertilizer(int id)
        {
            var fertilizer = await _context.Fertilizers.FindAsync(id);
            if (fertilizer == null)
            {
                return NotFound();
            }
            ViewBag.Shops = _context.Shops.ToList();
            return View(fertilizer);
        }

        // Edit Fertilizer Logic
        [HttpPost]
        public async Task<IActionResult> EditFertilizer(Fertilizer fertilizer, IFormFile ImageFile)
        {
            ViewBag.Shops = _context.Shops.ToList();

            if (!ModelState.IsValid)
            {
                return View(fertilizer);
            }

            try
            {
                var existingFertilizer = await _context.Fertilizers.FindAsync(fertilizer.Id);
                if (existingFertilizer == null)
                {
                    return NotFound();
                }

                existingFertilizer.Name = fertilizer.Name;
                existingFertilizer.Price = fertilizer.Price;
                existingFertilizer.Description = fertilizer.Description;
                existingFertilizer.Stocks = fertilizer.Stocks;
                existingFertilizer.ShopId = fertilizer.ShopId;

                if (ImageFile != null && ImageFile.Length > 0)
                {
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads/fertilizers");
                    Directory.CreateDirectory(uploadsFolder);
                    string uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(ImageFile.FileName);
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await ImageFile.CopyToAsync(fileStream);
                    }
                    existingFertilizer.Image = "/uploads/fertilizers/" + uniqueFileName;
                }

                _context.Fertilizers.Update(existingFertilizer);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Fertilizer updated successfully!";
                return RedirectToAction("FertilizerManagement");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error updating fertilizer: " + ex.Message;
                return View(fertilizer);
            }
        }

        // Delete Fertilizer
        [HttpPost]
        public async Task<IActionResult> DeleteFertilizer(int id)
        {
            var fertilizer = await _context.Fertilizers.FindAsync(id);
            if (fertilizer == null)
            {
                return NotFound();
            }

            _context.Fertilizers.Remove(fertilizer);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Fertilizer deleted successfully!";
            return RedirectToAction("FertilizerManagement");
        }

        // View All Orders
        public async Task<IActionResult> AllOrders()
        {
            var orders = await _context.Orders
                .Include(o => o.Fertilizer)
                .Include(o => o.Shop)
                .ToListAsync();
            return View(orders);
        }

        // Edit Order (GET)
        public async Task<IActionResult> EditOrder(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
                return NotFound();

            ViewBag.Fertilizers = new SelectList(_context.Fertilizers.ToList(), "Id", "Name", order.FertilizerId);
            ViewBag.Shops = new SelectList(_context.Shops.ToList(), "Id", "Name", order.ShopId);

            return View(order);
        }

        // Edit Order (POST)
        [HttpPost]
        public async Task<IActionResult> EditOrder(int id, Order order)
        {
            if (id != order.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("AllOrders");
                }
                catch (DbUpdateConcurrencyException)
                {
                    return StatusCode(500, "Error updating order");
                }
            }

            ViewBag.Fertilizers = new SelectList(_context.Fertilizers.ToList(), "Id", "Name", order.FertilizerId);
            ViewBag.Shops = new SelectList(_context.Shops.ToList(), "Id", "Name", order.ShopId);

            return View(order);
        }

        // Delete Order
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
                return NotFound();

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
            return RedirectToAction("AllOrders");
        }
    }
}
