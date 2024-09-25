using CarRent.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace CarRent.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<Car> Cars { get; set; }
        public DbSet<City> City { get; set; }
        public DbSet<SiteUser> Users { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<IdentityRole>().HasData(
                new IdentityRole { Id = "1", Name = "Admin", NormalizedName = "ADMIN" },
                new IdentityRole { Id = "2", Name = "User", NormalizedName = "USER" }
            );

            builder.Entity<City>().HasData(
                new City { Id = 1, CityName = "Budapest", County = "Budapest", ZipCode = 1131 },
                new City { Id = 2, CityName = "Szolnok", County = "Jász-Nagykun-Szolnok", ZipCode = 5008 },
                new City { Id = 3, CityName = "Veszprém", County = "Veszprém", ZipCode = 8200},
                new City { Id = 4, CityName = "Esztergom", County = "Komárom-Esztergom", ZipCode = 2500 }
                );

            builder.Entity<Car>().HasData(
               new Car { PlateNumber = "ABC123", Brand = "Toyota", Fuel = "Petrol", Year = 2009, CityId = 1, Price = 79 },
               new Car { PlateNumber = "CBA123", Brand = "Ford", Fuel = "Petrol", Year = 2012, CityId = 2, Price = 99 },
               new Car { PlateNumber = "QWE789", Brand = "BMW", Fuel = "Electric", Year = 2020, CityId = 4, Price = 149 },
               new Car { PlateNumber = "LKJ246", Brand = "Suzuki", Fuel = "Petrol", Year = 2008, CityId = 2, Price = 59 },
               new Car { PlateNumber = "PRT728", Brand = "Skoda", Fuel = "Petrol", Year = 2011, CityId = 3, Price = 119 },
               new Car { PlateNumber = "LVG093", Brand = "Opel", Fuel = "Diesel", Year = 2012, CityId = 1, Price = 129 }
               );

            var hasher = new PasswordHasher<SiteUser>();

            builder.Entity<SiteUser>().HasData(
                new SiteUser
                {
                    Id = "1",
                    UserName = "Gyuri",
                    NormalizedUserName = "GYURI.SEBESTYEN@FREEMAIL.HU",
                    Email = "gyuri.sebestyen@freemail.hu",
                    NormalizedEmail = "GYURI.SEBESTYEN@FREEMAIL.HU",
                    EmailConfirmed = true,
                    PasswordHash = hasher.HashPassword(null, "pirosalmafa"),
                    FirstName = "György",
                    LastName = "Sebestyén"
                }
            );

            builder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string> { UserId = "1", RoleId = "1" } // Assign Admin role
            );

        }
    }
}