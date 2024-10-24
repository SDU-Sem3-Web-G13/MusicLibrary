using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    internal class Program
    {
        static void Main()
        {
            UserRepository user = new UserRepository();
            AlbumRepository album = new AlbumRepository();

            //Adding users works
            //user.AddUser("John Doe", "test@student.sdu.dk");

            //Console.WriteLine("added your quries");

            //user.AddUser("John Troy", "test@gmail.dk");

            //Thread.Sleep(5000);

            //user.DeleteUser("John Troy");

            //Console.WriteLine("deleted album");

            //Adding albums works
            //album.AddAlbum(2, new byte[] { 0x01, 0x02, 0x03 }, "Test Album", DateTime.Now, "Test Artist", "Test Type", "Test Description", new string[] { "Track 1", "Track 2", "Track 3" });

            //album.AddAlbum(2, new byte[] { 0x01, 0x02, 0x03 }, "Test Album 2", DateTime.Now, "Test Artist", "Test Type", "Test Description", new string[] { "Track 1", "Track 2", "Track 3" });

            //Console.WriteLine("added your quries");

            //Thread.Sleep(5000);

            //album.DeleteAlbum("Test Album 2");

            //Console.WriteLine("deleted album");

            //Modifying albums works
            //album.ModifyAlbum(3, 2, new byte[] { 0x01, 0x02, 0x03 }, "Test Album 5", DateTime.Now, "Test Artist", "Test Type", "Test Description", new string[] { "Track 1", "Track 2", "Track 3" });
        }
    }
}
