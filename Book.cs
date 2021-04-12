using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace CalibreView
{
    class Book
    {
        public string Title
        { get; set; }

        public string Author
        { get; set; }

        public Uri Cover
        { get; set; }

        public int ID
        { get; set; }

        public Book(string title, string author, Uri cover, int id)
        {
            Title = title;
            Author = author;
            Cover = cover;
            ID = id;
        }
    }
}
