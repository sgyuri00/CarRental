using CarRent.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CarRent.Logic
{
    public interface IHomeLogic
    {
        public Car? ReadFromId(string plateNum);

        public void Cancel(string plateNum);

        public List<Car> Filter(string brand);
    }
}
