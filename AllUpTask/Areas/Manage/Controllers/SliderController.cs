using AllUpTask.DAL;
using AllUpTask.Models;
using Microsoft.AspNetCore.Mvc;
using System.IO;

namespace AllUpTask.Areas.Manage.Controllers
{
    [Area("manage")]
    public class SliderController : Controller
    {
      
        private readonly AppDbContext _context;

        public SliderController(AppDbContext context)
        {
            _context = context;    
        }
       
        public IActionResult Index()
        {
            List<Slider> sliders = _context.Sliders.ToList();
            return View(sliders);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Slider slider)
        {
            string fileName = slider.ImageUrl.FileName;
            if (slider.ImageUrl.ContentType != "image/jpeg" && slider.ImageUrl.ContentType != "image/png")
            {
                ModelState.AddModelError("ImageUrl", "can only upload .jpeg or .png");
            }

            if (slider.ImageUrl.Length > 1048576)
            {
                ModelState.AddModelError("ImageUrl", "File size must be lower than 1mb");
            }

            if (fileName.Length > 64)
            {
                fileName = fileName.Substring(fileName.Length - 64, 64);
            }
            fileName = Guid.NewGuid().ToString() + fileName;
            string path = "C:\\Users\\II novbe\\Desktop\\AllUpTasks\\AllUpTask\\wwwroot\\uploads\\Slider\\"+fileName;
            using (FileStream fileStream = new FileStream(path, FileMode.Create))
            {
                slider.ImageUrl.CopyTo(fileStream);
            }

            if (!ModelState.IsValid)
            {
                return View(slider);
            }
            slider.Image = fileName;

            _context.Sliders.Add(slider);
            _context.SaveChanges();

            return RedirectToAction("index");

        }
    }
}
