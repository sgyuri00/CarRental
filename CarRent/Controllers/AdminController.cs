using CarRent.Data;
using CarRent.Logic;
using CarRent.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace CarRent.Controllers
{
    public class AdminController : Controller
    {
        private readonly IHomeLogic _homeLogic;
        private readonly IAdminLogic _adminLogic;
        private readonly UserManager<SiteUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _db;

        public AdminController(IHomeLogic homeLogic, IAdminLogic adminLogic, UserManager<SiteUser> userManager, RoleManager<IdentityRole> roleManager, ILogger<HomeController> logger, ApplicationDbContext db)
        {
            _homeLogic = homeLogic;
            _adminLogic = adminLogic;
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;
            _db = db;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Add()
        {
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Users()
        {
            return View(_userManager.Users);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RemoveAdmin(string uid)
        {
            await _adminLogic.RemoveAdminAsync(uid);
            return RedirectToAction(nameof(Users));
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GrantAdmin(string uid)
        {
            await _adminLogic.GrantAdminAsync(uid);
            return RedirectToAction(nameof(Users));
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Remove(string plateNum)
        {
            _adminLogic.DeleteCar(plateNum);
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Add(Car car, IFormFile picturedata)
        {
            using (var stream = picturedata.OpenReadStream())
            {
                byte[] buffer = new byte[picturedata.Length];
                stream.Read(buffer, 0, (int)picturedata.Length);
                string filename = car.PlateNumber + "." + picturedata.FileName.Split('.')[1];
                car.ImageFileName = filename;
                //fájl módszer
                //System.IO.File.WriteAllBytes(Path.Combine("wwwroot", "images", filename), buffer);
                //db módszer
                car.Data = buffer;
                car.ContentType = picturedata.ContentType;
            }
            if (!ModelState.IsValid)
            {
                return View(car);
            }
            _db.Cars.Add(car);
            _db.SaveChanges();

            return RedirectToAction("Index", "Home");
        }


    }
}
