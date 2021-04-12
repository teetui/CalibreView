using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.Authorization.AppCapabilityAccess;
using Windows.Storage;
using Windows.Storage.Pickers;

namespace CalibreView
{
    class Calibre
    {
        public List<Book> Books = new List<Book>();

        public async Task GetBooks()
        {
            // Get library from a folder picker
            var folderPicker = new Windows.Storage.Pickers.FolderPicker();
            folderPicker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.Desktop;
            folderPicker.FileTypeFilter.Add("*");

            // Get the metadata.db file
            StorageFile MetadataFile = default;
            StorageFolder LibraryFolder = await folderPicker.PickSingleFolderAsync();
            if (LibraryFolder != null)
            {
                // Application now has read/write access to all contents in the picked folder
                // (including other sub-folder contents)
                Windows.Storage.AccessCache.StorageApplicationPermissions.
                    FutureAccessList.AddOrReplace("PickedFolderToken", LibraryFolder);

                // copy sqlite database file to temporary local folder,
                // because UWP file access permissions don't work in SQLite.
                MetadataFile = await StorageFile.GetFileFromPathAsync(LibraryFolder.Path + "\\metadata.db");
                await MetadataFile.CopyAsync(ApplicationData.Current.LocalFolder, MetadataFile.Name, NameCollisionOption.ReplaceExisting);
            }

            // Read books from Calibre's sqlite database
            using (SqliteConnection sqlite = new SqliteConnection($"Filename={MetadataFile.Name}"))
            {
                try
                {
                    sqlite.Open();

                    SqliteCommand createTable = new SqliteCommand("SELECT title,author_sort,path,id FROM books", sqlite);
                    SqliteDataReader query = createTable.ExecuteReader();

                    while (query.Read())
                    {
                        string title    = query.GetString(0);
                        string author   = query.GetString(1);
                        string path     = query.GetString(2);
                        int id          = query.GetInt32(3);

                        StorageFolder folder = await Windows.Storage.AccessCache.StorageApplicationPermissions.FutureAccessList.
                            GetFolderAsync("PickedFolderToken");
                        Uri cover = new Uri(folder.Path + "\\" + path + "\\cover.jpg");

                        Book book = new Book(
                            title,
                            author,
                            cover,
                            id
                        );

                        Books.Add(book);
                    }

                    sqlite.Close();
                }
                catch (SqliteException e)
                {
                    Debug.WriteLine(e.Message);
                }
            }
        }
    }
}
