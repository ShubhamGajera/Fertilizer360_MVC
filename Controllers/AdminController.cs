using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Fertilizer360.Models;
using Fertilizer360.Data;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;

[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly AppDbContext _context;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public AdminController(AppDbContext context, IWebHostEnvironment webHostEnvironment)
    {
        _context = context;
        _webHostEnvironment = webHostEnvironment;
    }

    public IActionResult Index()
    {
        return View();
    }

    public async Task<IActionResult> Shop()
    {
        var shopList = await _context.Shops.ToListAsync();
        return View(shopList);
    }

    [HttpGet]
    public IActionResult AddShop()
    {
        return View();
    }


    [HttpPost]
    public async Task<IActionResult> AddShop(Shop shop, IFormFile ImageFile)
    {
        if (!ModelState.IsValid)
        {
            return View(shop);
        }

        try
        {
            if (ImageFile != null && ImageFile.Length > 0)
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
                Directory.CreateDirectory(uploadsFolder);

                string uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(ImageFile.FileName);
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await ImageFile.CopyToAsync(fileStream);
                }

                shop.Image = "/uploads/" + uniqueFileName;
            }

            _context.Shops.Add(shop);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Shop added successfully!";
            return RedirectToAction("Shop");
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = "Error saving shop: " + ex.Message;
            return View(shop);
        }
    }

    [HttpGet]
    public IActionResult EditShop(int id)
    {
        var shop = _context.Shops.Find(id);
        if (shop == null)
        {
            return NotFound();
        }
        return View(shop);
    }

    [HttpPost]
    public async Task<IActionResult> EditShop(Shop shop, IFormFile ImageFile)
    {
        if (ModelState.IsValid)
        {
            try
            {
                var existingShop = _context.Shops.Find(shop.Id);
                if (existingShop == null)
                {
                    return NotFound();
                }

                if (ImageFile != null && ImageFile.Length > 0)
                {
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
                    Directory.CreateDirectory(uploadsFolder);

                    string uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(ImageFile.FileName);
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await ImageFile.CopyToAsync(fileStream);
                    }

                    existingShop.Image = "/uploads/" + uniqueFileName;
                }

                existingShop.Name = shop.Name;
                existingShop.Address = shop.Address;
                existingShop.PhoneNumber = shop.PhoneNumber;
                existingShop.WorkTime = shop.WorkTime;
                existingShop.Description = shop.Description;
                existingShop.State = shop.State;
                existingShop.District = shop.District;
                existingShop.VillageOrTaluka = shop.VillageOrTaluka;

                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Shop updated successfully!";
                return RedirectToAction("Shop");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error updating shop: " + ex.Message);
            }
        }
        return View(shop);
    }

    [HttpPost]
    public async Task<IActionResult> DeleteShop(int id)
    {
        var shop = await _context.Shops.FindAsync(id);
        if (shop == null)
        {
            return NotFound();
        }

        _context.Shops.Remove(shop);
        await _context.SaveChangesAsync();

        TempData["SuccessMessage"] = "Shop deleted successfully!";
        return RedirectToAction("Shop");
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
        ViewBag.Shops = _context.Shops.ToList(); // Fetch all shops from DB
        return View();
    }

    // Add Fertilizer Logic
    [HttpPost]
    public async Task<IActionResult> AddFertilizer(Fertilizer fertilizer, IFormFile ImageFile)
    {
        ViewBag.Shops = _context.Shops.ToList(); // Ensure dropdown list is available

        // Validate ShopId (Fix casting issue)
        if (!int.TryParse(Request.Form["ShopId"], out int shopId))
        {
            ModelState.AddModelError("ShopId", "Invalid Shop selection.");
            return View(fertilizer);
        }
        fertilizer.ShopId = shopId;

        if (!ModelState.IsValid)
        {
            Console.WriteLine("ModelState is Invalid");
            foreach (var key in ModelState.Keys)
            {
                foreach (var error in ModelState[key].Errors)
                {
                    Console.WriteLine($"Key: {key}, Error: {error.ErrorMessage}");
                }
            }
            return View(fertilizer);
        }

        try
        {
            // Debugging: Log fertilizer details
            Console.WriteLine($"Fertilizer Name: {fertilizer.Name}");
            Console.WriteLine($"Fertilizer Price: {fertilizer.Price}");
            Console.WriteLine($"Fertilizer Description: {fertilizer.Description}");
            Console.WriteLine($"Fertilizer Stocks: {fertilizer.Stocks}");
            Console.WriteLine($"Fertilizer ShopId: {fertilizer.ShopId}");

            // Ensure ShopId is assigned correctly
            if (fertilizer.ShopId == 0)
            {
                TempData["ErrorMessage"] = "Please select a valid shop.";
                return View(fertilizer);
            }

            // Save image if uploaded
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
            Console.WriteLine($"Exception: {ex.Message}");
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
        ViewBag.Shops = _context.Shops.ToList(); // Fetch all shops for dropdown
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

            // Update Image if new one is uploaded
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
    public async Task<IActionResult> EditOrder(int id, int Quantity)
    {
        var order = await _context.Orders.FindAsync(id);
        if (order == null)
            return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                order.Quantity = Quantity;
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
