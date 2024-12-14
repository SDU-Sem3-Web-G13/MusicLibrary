using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Frontend.Objects;
using Frontend.Models;

namespace RazorMusic.Pages;

public class AdminViewModel : PageModel
{

    private readonly ILoginRegisterModel _loginRegisterModel;
    private readonly IAlbumsModel _albumsModel;
    private readonly IAdministrationModel _administrationModel;

    public List<UserModel> UserList = new List<UserModel>();
    public List<AlbumModel> AlbumList = new List<AlbumModel>();

    public AdminViewModel(ILoginRegisterModel loginRegisterModel, IAlbumsModel albumsModel, IAdministrationModel administrationModel)
        {
            _loginRegisterModel = loginRegisterModel;
            _albumsModel = albumsModel;
            _administrationModel = administrationModel;
        }

    public void OnGet()
    {
        ValidateSessionStorage();
        GetUsersAndAlbums();
    }

    private void GetUsersAndAlbums() {
        UserList.Clear();
        AlbumList.Clear();
        UserList = _administrationModel.GetUsers();
        foreach(var user in UserList) {
            AlbumList.AddRange((IEnumerable<AlbumModel>)_albumsModel.GetAlbums(user.Id));
        }
    }

    public IActionResult OnGetDeleteAlbum(int albumId) {
        _albumsModel.DeleteAlbum(albumId);
        GetUsersAndAlbums();
        return new JsonResult(new { success = true });
    }

    public IActionResult OnGetDeleteUser(int userId) {
        if(_loginRegisterModel.IsAdmin(userId)) {
            return new JsonResult(new { success = false, message = "Cannot delete admin user" });
        }
        _albumsModel.DeleteAllUserAlbums(userId);
        _administrationModel.DeleteUser(userId);
        GetUsersAndAlbums();
        return new JsonResult(new { success = true });
    }

    private void ValidateSessionStorage() {
        if (HttpContext.Session.GetInt32("IsLoggedIn") != 1) {
            Response.Redirect("/Login");
            return;
        } 
        var userId = HttpContext.Session.GetInt32("userId");
        if(userId == null || !_loginRegisterModel.IsAdmin(userId.Value)) {
            Response.Redirect("/Index");
        }
    }
}

