using CarRent.Controllers;
using CarRent.Data;
using CarRent.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarRent.Logic
{
    public class HomeLogic : IHomeLogic
    {
        private readonly UserManager<SiteUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _db;
        private readonly ILogger<HomeController> _logger;

        public HomeLogic(UserManager<SiteUser> userManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext db, ILogger<HomeController> logger)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _db = db;
            _logger = logger;

            AddCitiesToCars();
        }

        public Car? ReadFromId(string plateNum)
        {
            return _db.Cars.FirstOrDefault(t => t.PlateNumber == plateNum);
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

        public void Cancel(string plateNum)
        {
            var car = _db.Cars.FirstOrDefault(c => c.PlateNumber == plateNum);
            if (car != null)
            {
                car.UserId = null;
                car.User = null;

                _db.SaveChanges();
            }
        }
        public List<Car> Filter(string brand)
        { 
            var filteredCars = new List<Car>();

            foreach (var car in _db.Cars)
            {
                if (car.Brand.ToLower() == brand.ToLower())
                {
                    filteredCars.Add(car);
                }
            }
            return filteredCars;
        }
    }
}
