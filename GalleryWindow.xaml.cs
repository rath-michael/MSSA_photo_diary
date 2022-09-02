using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Week8Project
{
    public delegate List<UserImage> GalleryDelegate(string input);
    public partial class GalleryWindow : Window
    {
        private GalleryDelegate galleryDel;
        private string searchTerm;
        private List<UserImage> images;

        public GalleryWindow(GalleryDelegate del, string input)
        {
            InitializeComponent();
            galleryDel = del;
            searchTerm = input;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            images = galleryDel(searchTerm);
            ImageList.ItemsSource = null;
            ImageList.ItemsSource = images;
            LoadImages();
        }

        public void LoadImages()
        {
            images = galleryDel(searchTerm);
            ImageList.ItemsSource = null;
            ImageList.ItemsSource = images;
        }

        private void Image_MouseEnter(object sender, MouseEventArgs e)
        {
            Image image = sender as Image;
            image.Cursor = Cursors.Hand;
        }

        private void Image_MouseLeave(object sender, MouseEventArgs e)
        {
            Image image = sender as Image;
            image.Cursor = Cursors.Arrow;
        }

        private void Image_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Image image = sender as Image;
            UserImage selectedImage = image.DataContext as UserImage;
            if (selectedImage != null)
            {
                NavigationService.NavigateTo(new DetailsWindow(images, selectedImage.ImageID));
            }
            else
            {
                MessageBox.Show("An error occurred");
                Console.WriteLine("GalleryWindow.xaml.cs: Image_MouseLeftButtonUp error");
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            if (NavigationService.CanNavigateBack())
            {
                NavigationService.NavigateBack();
            }
        }
    }
}
