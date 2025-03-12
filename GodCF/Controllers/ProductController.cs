using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using GodCF.Models;
using GodCF.Repositories;
using Microsoft.AspNetCore.Hosting;

namespace GodCF.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductController(
            IProductRepository productRepository,
            ICategoryRepository categoryRepository,
            IWebHostEnvironment webHostEnvironment)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Product
        public IActionResult Index()
        {
            var products = _productRepository.GetAll();
            return View(products);
        }

        // GET: Product/Create
        public IActionResult Create()
        {
            ViewBag.Categories = new SelectList(_categoryRepository.GetAll(), "Id", "Name");
            return View();
        }

        // POST: Product/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Description,Price,CategoryId")] Product product, List<IFormFile> images)
        {
            
                // Save product first to get the ID
                _productRepository.Add(product);

                // Process and save images
                if (images != null && images.Count > 0)
                {
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images", "products");
                    Directory.CreateDirectory(uploadsFolder);

                    foreach (var image in images)
                    {
                        string uniqueFileName = Guid.NewGuid().ToString() + "_" + image.FileName;
                        string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await image.CopyToAsync(fileStream);
                        }

                        product.Images.Add(new ProductImage { ImageUrl = "/images/products/" + uniqueFileName });
                    }

                    _productRepository.Update(product);
                }

                return RedirectToAction(nameof(Index));
           
        }

        //// GET: Product/Edit/5
        //public IActionResult Edit(int id)
        //{
        //    var product = _productRepository.GetById(id);
        //    if (product == null)
        //    {
        //        return NotFound();
        //    }

        //    ViewBag.Categories = new SelectList(_categoryRepository.GetAll(), "Id", "Name");
        //    return View(product);
        //}

        //// POST: Product/Edit/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,Price,CategoryId")] Product product, List<IFormFile> newImages, List<int> imagesToDelete)
        //{
        //    if (id != product.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        var existingProduct = _productRepository.GetById(id);
        //        existingProduct.Name = product.Name;
        //        existingProduct.Description = product.Description;
        //        existingProduct.Price = product.Price;
        //        existingProduct.CategoryId = product.CategoryId;

        //        // Delete selected images
        //        if (imagesToDelete != null)
        //        {
        //            var imagesToRemove = existingProduct.Images.Where(i => imagesToDelete.Contains(i.Id)).ToList();
        //            foreach (var image in imagesToRemove)
        //            {
        //                // Delete physical file
        //                var filePath = Path.Combine(_webHostEnvironment.WebRootPath, image.ImageUrl.TrimStart('/'));
        //                if (System.IO.File.Exists(filePath))
        //                {
        //                    System.IO.File.Delete(filePath);
        //                }
        //                existingProduct.Images.Remove(image);
        //            }
        //        }

        //        // Add new images
        //        if (newImages != null && newImages.Count > 0)
        //        {
        //            string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images", "products");
        //            Directory.CreateDirectory(uploadsFolder);

        //            foreach (var image in newImages)
        //            {
        //                string uniqueFileName = Guid.NewGuid().ToString() + "_" + image.FileName;
        //                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

        //                using (var fileStream = new FileStream(filePath, FileMode.Create))
        //                {
        //                    await image.CopyToAsync(fileStream);
        //                }

        //                existingProduct.Images.Add(new ProductImage { ImageUrl = "/images/products/" + uniqueFileName });
        //            }
        //        }

        //        _productRepository.Update(existingProduct);
        //        return RedirectToAction(nameof(Index));
        //    }

        //    ViewBag.Categories = new SelectList(_categoryRepository.GetAll(), "Id", "Name");
        //    return View(product);
        //}

        //// POST: Product/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public IActionResult DeleteConfirmed(int id)
        //{
        //    var product = _productRepository.GetById(id);
        //    if (product != null)
        //    {
        //        // Delete physical image files
        //        foreach (var image in product.Images)
        //        {
        //            var filePath = Path.Combine(_webHostEnvironment.WebRootPath, image.ImageUrl.TrimStart('/'));
        //            if (System.IO.File.Exists(filePath))
        //            {
        //                System.IO.File.Delete(filePath);
        //            }
        //        }

        //        _productRepository.Delete(id);
        //    }

        //    return RedirectToAction(nameof(Index));
        //}
    }
}