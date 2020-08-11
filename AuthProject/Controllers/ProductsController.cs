using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AuthProject;
using AuthProject.Models;
using Microsoft.AspNetCore.Authorization;

namespace AuthProject.Controllers
{
    [Authorize]
    public class ProductsController : BaseController
    {
        private readonly UserContext _context;

        public ProductsController(UserContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var model = await _context.Products.Where(x => x.UserId == UserId).ToListAsync();
            return View(model);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Price,Count")] Product product)
        {
            var result = new ResultVM();
            try
            {
                if (ModelState.IsValid)
                {
                    product.UserId = UserId.Value;
                    _context.Add(product);
                    await _context.SaveChangesAsync();
                    result.Success = true;
                    result.Message = "Товар успешно создан.";
                }
                else
                {
                    result.Message = "Данные в полях не верны!";
                }
            }

            catch (Exception)
            {
                result.Message = "Произошла ошибка!";
            }
            return Json(result);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Price,Count")] Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    product.UserId = UserId.Value;
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var result = new ResultVM();
            try
            {
                var product = await _context.Products.FirstOrDefaultAsync(m => m.Id == id && m.UserId == UserId);
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();

                result.Success = true;
                result.Message = "Товар успешно удален.";
            }
            catch (Exception)
            {
                result.Message = "Произошла ошибка!";
            }
            return Json(result);
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}
