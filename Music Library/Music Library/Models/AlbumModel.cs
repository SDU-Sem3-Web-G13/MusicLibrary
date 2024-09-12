namespace Music_Library.Models
{
    public class AlbumModel
    {
        public string AlbumName { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string Artist { get; set; }
        public AlbumTypes AlbumType { get; set; }
        public string Description { get; set; }
        public string[] Tracks { get; set; } 

        public AlbumModel(
            string albumName,
            DateTime releaseDate,
            string artist,
            AlbumTypes albumType,
            string description,
            string[] tracks
        ) {
            this.AlbumName = albumName;
            this.ReleaseDate = releaseDate;
            this.Artist = artist;
            this.AlbumType = albumType;
            this.Description = description;
            this.Tracks = tracks;
        }
    }
}
