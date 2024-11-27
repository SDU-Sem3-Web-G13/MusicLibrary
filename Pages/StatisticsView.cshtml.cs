using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models.DataAccess;

namespace RazorMusic.Pages
{
    public class StatisticsModel : PageModel
    {
        private readonly ILogger<StatisticsModel> _logger;
        private readonly DbAccess _dbAccess;

        public StatisticsModel(ILogger<StatisticsModel> logger, DbAccess dbAccess)
        {
            _logger = logger;
            _dbAccess = dbAccess;
        }

        public int NumberOfAlbums { get; set; }
        public int NumberOfSongs { get; set; }
        public int NumberOfArtists { get; set; }
        public int NumberOfGenres { get; set; }

        public void OnGet()
        {
            GetNumberOfAlbums();
            GetNumberOfSongs();
            GetNumberOfArtists();
            GetNumberOfGenres();
        }

        public void GetNumberOfAlbums()
        {
            string sql = "SELECT COUNT(*) FROM albums"; // Replace 'albums' with the actual table name
            NumberOfAlbums = _dbAccess.ExecuteScalar(sql);
        }

        public void GetNumberOfSongs()
        {
            string sql = "SELECT COUNT(*) FROM songs"; // Replace 'songs' with the actual table name
            NumberOfSongs = _dbAccess.ExecuteScalar(sql);
        }

        public void GetNumberOfArtists()
        {
            string sql = "SELECT COUNT(*) FROM artists"; // Replace 'artists' with the actual table name
            NumberOfArtists = _dbAccess.ExecuteScalar(sql);
        }

        public void GetNumberOfGenres()
        {
            string sql = "SELECT COUNT(*) FROM genres"; // Replace 'genres' with the actual table name
            NumberOfGenres = _dbAccess.ExecuteScalar(sql);
        }
    }
}
