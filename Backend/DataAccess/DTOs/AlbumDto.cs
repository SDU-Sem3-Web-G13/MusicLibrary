using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.DataAccess.Dtos
{
    public interface IAlbumDto : IDto
    {
        int? Id { get; set; }
        int? OwnerId { get; set; }
        byte[]? CoverImage { get; set; }
        string AlbumName { get; set; }
        DateTime? ReleaseDate { get; set; }
        string Artist { get; set; }
        string AlbumType { get; set; }
        string Description { get; set; }
        string[] Tracks { get; set; } 
    }

    public class AlbumDto: IAlbumDto
    {
        public int? Id { get; set; }
        public int? OwnerId { get; set; }
        public byte[]? CoverImage { get; set; }
        public string AlbumName { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public string Artist { get; set; }
        public string AlbumType { get; set; }
        public string Description { get; set; }
        public string[] Tracks { get; set; } 

        public AlbumDto(
            string albumName,
            DateTime? releaseDate,
            string artist,
            string albumType,
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