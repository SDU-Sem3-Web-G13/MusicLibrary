using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Backend.Models;
using Frontend.Models;

namespace RazorMusic.Pages;

public class AdminViewModel : PageModel
{
    private readonly ILogger<PrivacyModel> _logger;
    private readonly LoginRegisterModel loginRegisterModel = new LoginRegisterModel();
    private readonly AlbumsModel albumsModel = new AlbumsModel();
    private readonly AdministrationModel administrationModel = new AdministrationModel();

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
        UserList = administrationModel.GetUsers();
        foreach(var user in UserList) {
            AlbumList.AddRange(albumsModel.GetAlbums(user.Id));
        }
    }

    public IActionResult OnGetDeleteAlbum(int albumId) {
        albumsModel.DeleteAlbum(albumId);
        GetUsersAndAlbums();
        return new JsonResult(new { success = true });
    }

    public IActionResult OnGetDeleteUser(int userId) {
        if(loginRegisterModel.IsAdmin(userId)) {
            return new JsonResult(new { success = false, message = "Cannot delete admin user" });
        }
        albumsModel.DeleteAllUserAlbums(userId);
        administrationModel.DeleteUser(userId);
        GetUsersAndAlbums();
        return new JsonResult(new { success = true });
    }

    private void ValidateSessionStorage() {
        if (HttpContext.Session.GetInt32("IsLoggedIn") != 1) {
            Response.Redirect("/Login");
        } 
        var userId = HttpContext.Session.GetInt32("userId");
        if(userId == null || !loginRegisterModel.IsAdmin(userId.Value)) {
            Response.Redirect("/Index");
        }
    }
}

