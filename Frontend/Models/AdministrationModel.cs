using Backend.Models;
using Backend.Services;

namespace Frontend.Models
{
    public class AdministrationModel
    {
        private readonly IAdministrationService _administrationService;

        public AdministrationModel(IAdministrationService administrationService)
        {
            _administrationService = administrationService;
        }
        
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