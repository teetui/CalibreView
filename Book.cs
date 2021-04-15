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

        public string SeriesName
        { get; set; }

        public string SeriesIndex
        { get; set; }

        public string AuthorName
        { get; set; }

        public Uri Cover
        { get; set; }

        public Book(string title, string series_name, string series_index, string author_name, Uri cover)
        {
            Title = title;
            SeriesName = series_name;
            SeriesIndex = series_index;
            AuthorName = author_name;
            Cover = cover;
        }
    }
}
