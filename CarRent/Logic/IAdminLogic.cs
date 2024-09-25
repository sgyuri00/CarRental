namespace CarRent.Logic
{
    public interface IAdminLogic
    {
        public Task RemoveAdminAsync(string uid);

        public Task GrantAdminAsync(string uid);

        public void DeleteCar(string plateNum);
    }
}
