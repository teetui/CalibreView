using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace CalibreView
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private async Task Populate(Calibre calibre)
        {
            foreach(Book book in calibre.Books)
            {
                ListViewItem item = new ListViewItem();
                item.Content = new Grid();
                Grid grid = (Grid)item.Content;
                grid.Height = 100;
                grid.Margin = new Thickness(0, 0, 0, 0);

                ColumnDefinition col1 = new ColumnDefinition();
                ColumnDefinition col2 = new ColumnDefinition();
                col1.Width = new GridLength(0, GridUnitType.Auto);
                col2.Width = new GridLength(1, GridUnitType.Star);
                grid.ColumnDefinitions.Add(col1);
                grid.ColumnDefinitions.Add(col2);

                Image image = new Image();
                BitmapImage bitmapImage = new BitmapImage();
                StorageFile imageFile = await StorageFile.GetFileFromPathAsync(book.Cover.ToString());
                using (var stream = await imageFile.OpenAsync(Windows.Storage.FileAccessMode.Read))
                {
                    await bitmapImage.SetSourceAsync(stream);
                    bitmapImage.DecodePixelHeight = 100;
                    image.Margin = new Thickness(0, 0, 5, 0);
                    image.Width = 80;
                    image.Source = bitmapImage;
                }

                RichTextBlock richTextBlock = new RichTextBlock();
                richTextBlock.TextWrapping = TextWrapping.Wrap;
                Paragraph paragraph1 = new Paragraph();
                Paragraph paragraph2 = new Paragraph();
                Run title = new Run();
                title.Text = $"{book.Title}{System.Environment.NewLine}{book.AuthorName}";
                Run series = new Run();
                if (book.SeriesName == "")
                    series.Text = $"";
                else
                    series.Text = $"{book.SeriesName} [{book.SeriesIndex}]";
                title.FontWeight = FontWeights.Bold;
                paragraph1.Inlines.Add(title);
                paragraph2.Inlines.Add(series);
                richTextBlock.Blocks.Add(paragraph1);
                richTextBlock.Blocks.Add(paragraph2);

                grid.Children.Add(image);
                grid.Children.Add(richTextBlock);

                Grid.SetColumn(image, 0);
                Grid.SetColumn(richTextBlock, 1);

                BookList.Items.Add(item);
            }
        }

        private async void BuildPage()
        {
            // Access Calibre SQLite database
            Calibre calibre = new Calibre();
            await calibre.GetBooks();

            // Populate Xaml page
            try
            {
                await Populate(calibre);
            }
            catch(Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }

        public MainPage()
        {
            this.InitializeComponent();

            BuildPage();
        }
    }
}
