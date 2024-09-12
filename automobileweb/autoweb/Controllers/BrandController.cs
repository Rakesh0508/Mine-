using autoweb.Data;
using autoweb.Models;
using Microsoft.AspNetCore.Mvc;

namespace autoweb.Controllers
{
    public class BrandController : Controller
    {

        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public BrandController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {

            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        public IActionResult Index()
        {
            List<Brand> brands = _context.Brand.ToList();
            return View(brands);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Brand brand)
        {
            string webRootPath = _webHostEnvironment.WebRootPath;

            // Retrieve the uploaded files from the form
            var files = HttpContext.Request.Form.Files;

            if (files != null && files.Count > 0)
            {
                // Generate a unique file name
                string newFileName = Guid.NewGuid().ToString();
                var uploadPath = Path.Combine(webRootPath, "images", "brand");

                // Ensure the directory exists
                if (!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);
                }

                // Get the file extension and form the complete path
                var extension = Path.GetExtension(files[0].FileName);
                var fullFilePath = Path.Combine(uploadPath, newFileName + extension);

                // Save the file to the server
                using (var fileStream = new FileStream(fullFilePath, FileMode.Create))
                {
                    files[0].CopyTo(fileStream);
                }

                // Set the BrandLogo property to the relative path
                brand.BrandLogo = @"\images\brand\" + newFileName + extension;
            }

            // Check if the model is valid
            if (ModelState.IsValid)
            {
                _context.Brand.Add(brand);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            return View(brand);
        }

        [HttpGet]
        public IActionResult Details(Guid id)
        {
            Brand brand = _context.Brand.FirstOrDefault(x =>  x.Id == id);

            return View(brand);
        }

    }
}
