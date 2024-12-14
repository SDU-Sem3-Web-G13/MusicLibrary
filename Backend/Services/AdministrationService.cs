using Backend.DataAccess;
using Backend.DataAccess.Interfaces;
using Backend.Services.ServiceDtos;
using BCrypt.Net;

namespace Backend.Services
{
    public interface IAdministrationService
    {
        public List<IUserServiceDto> GetUsers();
        public void DeleteUser(int id);

    }

    public class AdministrationService: IAdministrationService
    {
        private readonly ILoginRegisterService _loginRegisterService;

        private readonly IUserRepository _userRepository;

        public AdministrationService(IUserRepository userRepository, ILoginRegisterService loginRegisterService)
        {
            _userRepository = userRepository;
            _loginRegisterService = loginRegisterService;
        }

        public List<IUserServiceDto> GetUsers() 
        {
            return _userRepository.GetUsers().Select(user => new UserServiceDto
            (
                user.Id,
                user.Name,
                user.Mail,
                user.IsAdmin
            )).ToList<IUserServiceDto>();
        }

        public void DeleteUser(int id) 
        {
            var mail = _userRepository.GetSingleUser(id).Mail;
            var fixedSalt = _loginRegisterService.GetFixedSalt();
            string emailHash = BCrypt.Net.BCrypt.HashPassword(mail, fixedSalt);
            byte[] emailHashBytes = System.Text.Encoding.UTF8.GetBytes(emailHash);
            _userRepository.DeleteUserCredentials(emailHashBytes);
            _userRepository.DeleteUser(id);
        }
    }
}