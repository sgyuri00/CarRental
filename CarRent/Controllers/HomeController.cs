using CarRent.Data;
using CarRent.Logic;
using CarRent.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Core.Types;
using System.Diagnostics;

namespace CarRent.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHomeLogic _homeLogic;
        private readonly UserManager<SiteUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _db;

        public HomeController(IHomeLogic homeLogic, UserManager<SiteUser> userManager, RoleManager<IdentityRole> identityManager, ILogger<HomeController> logger, ApplicationDbContext db)
        {
            _homeLogic = homeLogic;
            _userManager = userManager;
            _roleManager = identityManager;
            _logger = logger;
            _db = db;
        }

        public async Task<IActionResult> DelegateAdmin() 
        {
            var principal = this.User;
            var user = await _userManager.GetUserAsync(principal);
            var role = new IdentityRole()
            {
                Name = "Admin"
            };
            if (!await _roleManager.RoleExistsAsync("Admin"))
            {
                await _roleManager.CreateAsync(role);
            }
            await _userManager.AddToRoleAsync(user, "Admin");
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Index()
        {
            return View(_db.Cars);
        }

        public IActionResult GetImage(string plateNum)
        {
            var car = _homeLogic.ReadFromId(plateNum);
            if (car.ContentType.Length > 3)
            {
                return new FileContentResult(car.Data, car.ContentType);
            }
            else
            {
                return BadRequest();
            }

        }

        public async Task<IActionResult> Cancel(string plateNum)
        {
            _homeLogic.Cancel(plateNum);
            var user = await _userManager.GetUserAsync(User);
            user.PlateNum = null;
            user.EndDate = default;
            user.StartDate = default;
            await _db.SaveChangesAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Reserve(SiteUser user)
        {
            var user2 = await _userManager.GetUserAsync(User);
            user2.EndDate = user.EndDate;
            user2.StartDate = user.StartDate;
            await _db.SaveChangesAsync();
            var car = _db.Cars.FirstOrDefault(c => c.PlateNumber == user.PlateNum);
            if (car != null)
            {
                car.UserId = user2.Id;
                await _userManager.UpdateAsync(user);
                await _db.SaveChangesAsync();

            }
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> ReserveAsync(string plateNum)
        {
            var user = await _userManager.GetUserAsync(User);
            user.PlateNum = plateNum;
            await _db.SaveChangesAsync();
            return View();
        }

        [HttpGet]
        public IActionResult Filter(string brand)
        {
            var filteredCars = _homeLogic.Filter(brand);
            
            return View("Index", filteredCars);
        }
    }
}