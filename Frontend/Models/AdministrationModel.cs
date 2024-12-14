using Frontend.Objects;
using Backend.Services;

namespace Frontend.Models
{
    public interface IAdministrationModel
    {
        List<UserModel> GetUsers();
        void DeleteUser(int id);
    }

    public class AdministrationModel: IAdministrationModel
    {
        private readonly IAdministrationService _administrationService;

        public AdministrationModel(IAdministrationService administrationService)
        {
            _administrationService = administrationService;
        }
        
        public List<UserModel> GetUsers() 
        {
            return _administrationService.GetUsers().Select(dto => new UserModel
            (
                dto.Id,
                dto.Name,
                dto.Mail,
                dto.IsAdmin
            )).ToList();
        }

        public void DeleteUser(int id) 
        {
            _administrationService.DeleteUser(id);
        }
    }
}