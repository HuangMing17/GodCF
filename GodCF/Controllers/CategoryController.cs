using Microsoft.AspNetCore.Mvc;
using GodCF.Models;
using GodCF.Repositories;

namespace GodCF.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        // GET: Category
        public IActionResult Index()
        {
            var categories = _categoryRepository.GetAll();
            return View(categories);
        }

        public IActionResult Create()
        {
            return View();
        }

        // POST: Category/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category category)
        {
            

            category.IsDeleted = false; // Đảm bảo category mới luôn có IsDeleted = false
            _categoryRepository.Add(category);
            return RedirectToAction(nameof(Index));
        }


        //// GET: Category/Edit/5
        //public IActionResult Edit(int id)
        //{
        //    var category = _categoryRepository.GetById(id);
        //    if (category == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(category);
        //}

        //// POST: Category/Edit/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult Edit(int id, [Bind("Id,Name,Description")] Category category)
        //{
        //    if (id != category.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        _categoryRepository.Update(category);
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(category);
        //}

        //// POST: Category/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public IActionResult DeleteConfirmed(int id)
        //{
        //    _categoryRepository.Delete(id);
        //    return RedirectToAction(nameof(Index));
        //}
    }
}