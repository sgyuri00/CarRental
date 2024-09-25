using CarRent.Controllers;
using CarRent.Data;
using CarRent.Models;
using Microsoft.AspNetCore.Identity;

namespace CarRent.Logic
{
    public class AdminLogic : IAdminLogic
    {
        private readonly UserManager<SiteUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _db;
        private readonly ILogger<HomeController> _logger;

        public AdminLogic(UserManager<SiteUser> userManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext db, ILogger<HomeController> logger)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _db = db;
            _logger = logger;

            AddCitiesToCars();
        }
        public void AddCitiesToCars()
        {
            foreach (var car in _db.Cars)
            {
                var users = _userManager.Users;
                foreach (var user in users)
                {
                    if (car.UserId == user.Id)
                    {
                        car.User = user;
                    }
                }
                var city = _db.City.FirstOrDefault(c => c.Id == car.CityId);
                if (city != null)
                {
                    car.City = city;
                }
            }
            _db.SaveChanges();
        }

        public async Task RemoveAdminAsync(string uid)
        {
            var user = _userManager.Users.FirstOrDefault(t => t.Id == uid);
            var x = await _userManager.RemoveFromRoleAsync(user, "Admin");
        }

        public async Task GrantAdminAsync(string uid)
        {
            var user = _userManager.Users.FirstOrDefault(t => t.Id == uid);
            await _userManager.AddToRoleAsync(user, "Admin");
        }

        public void DeleteCar(string plateNum)
        {
            var car = _db.Cars.FirstOrDefault(c => c.PlateNumber == plateNum);
            if (car != null)
            {
                _db.Cars.Remove(car);
                _db.SaveChanges();
            }
        }

    }
}
