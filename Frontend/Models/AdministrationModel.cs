using Backend.Models;
using Backend.Services;

namespace Frontend.Models
{
    public class AdministrationModel
    {
        private readonly IAdministrationService _administrationService = new AdministrationService();
        public List<UserModel> GetUsers() 
        {
            return _administrationService.GetUsers();
        }

        public void DeleteUser(int id) 
        {
            _administrationService.DeleteUser(id);
        }
    }
}