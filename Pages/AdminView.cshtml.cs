using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models;
using Models.DataAccess;

namespace RazorMusic.Pages;

public class AdminViewModel : PageModel
{
    private readonly ILogger<PrivacyModel> _logger;
    private readonly UserRepository userRepository = new UserRepository();
    private readonly AlbumRepository albumRepository = new AlbumRepository();

    public List<UserModel> UserList = new List<UserModel>();
    public List<AlbumModel> AlbumList = new List<AlbumModel>();

    public AdminViewModel(ILogger<PrivacyModel> logger)
    {
        _logger = logger;
    }

    public void OnGet()
    {
        ValidateSessionStorage();
        GetUsersAndAlbums();
    }

    private void GetUsersAndAlbums() {
        UserList.Clear();
        AlbumList.Clear();
        UserList = userRepository.GetUsers();
        foreach(var user in UserList) {
            AlbumList.AddRange(albumRepository.GetAlbums(user.Id));
        }
    }

    public IActionResult OnGetDeleteAlbum(int albumId) {
        albumRepository.DeleteAlbum(albumId);
        GetUsersAndAlbums();
        return new JsonResult(new { success = true });
    }

    public IActionResult OnGetDeleteUser(int userId) {
        if(userRepository.IsAdmin(userId)) {
            return new JsonResult(new { success = false, message = "Cannot delete admin user" });
        }
        albumRepository.DeleteAllUserAlbums(userId);
        userRepository.DeleteUser(userId);
        GetUsersAndAlbums();
        return new JsonResult(new { success = true });
    }

    private void ValidateSessionStorage() {
        if (HttpContext.Session.GetInt32("IsLoggedIn") != 1) {
            Response.Redirect("/Login");
        } 
        var userId = HttpContext.Session.GetInt32("userId");
        if(userId == null || !userRepository.IsAdmin(userId.Value)) {
            Response.Redirect("/Index");
        }
    }
}

