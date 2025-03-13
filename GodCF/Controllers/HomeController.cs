using System.Diagnostics;
using GodCF.Models;
using GodCF.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace GodCF.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;

        public HomeController(
            ILogger<HomeController> logger,
            IProductRepository productRepository,
            ICategoryRepository categoryRepository)
        {
            _logger = logger;
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Menu()
        {
            ViewBag.Categories = _categoryRepository.GetAll();
            var products = _productRepository.GetAllWithImages();
            return View(products.OrderBy(p => p.Category?.Name).ThenBy(p => p.Name));
        }

        public IActionResult Order()
        {
            return View();
        }

        public IActionResult Booking()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
