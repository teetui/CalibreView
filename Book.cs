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
        public string TitleName
        { get; set; }

        public string TitleSort
        { get; set; }

        public string SeriesName
        { get; set; }

        public string SeriesIndex
        { get; set; }

        public string AuthorName
        { get; set; }

        public string AuthorSort
        { get; set; }

        public Uri Cover
        { get; set; }

        public Book(
            string title_name, 
            string title_sort,
            string series_name, 
            string series_index, 
            string author_name, 
            string author_sort,
            Uri cover)
        {
            TitleName = title_name;
            TitleSort = title_sort;
            SeriesName = series_name;
            SeriesIndex = series_index;
            AuthorName = author_name;
            AuthorSort = author_sort;
            Cover = cover;
        }
    }
}
