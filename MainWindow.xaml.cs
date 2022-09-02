using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Week8Project
{
    public partial class MainWindow : Window
    {
        private Dictionary<string, int> dict = new Dictionary<string, int>();
        public MainWindow()
        {
            InitializeComponent();
            //LoadDict();
        }

        private async void LoadDictionary()
        {
            //var res = await ImageRepository.GetCount();
            var res = await ImageRepository.GetPhotoCount();
            dict = new Dictionary<string, int>();
            if (res != null)
            {
                foreach (var item in res)
                {
                    dict.Add(item.Name, item.Count);
                }
            }
        }
        // Path mouse click event
        private void Path_MouseUp(object sender, MouseButtonEventArgs e)
        {
            // Get list of UserImages based on Name of clicked path
            Path path = sender as Path;
            List<UserImage> images = ImageRepository.GetImagesByState(path.Name);
            // If valid images, display images in ImageGallery
            if (images != null)
            {
                GalleryDelegate del = new GalleryDelegate(ImageRepository.GetImagesByState);
                NavigationService.NavigateTo(new ImageGallery(del, path.Name));

                //GalleryDelegate del = new GalleryDelegate(ImageRepository.GetImagesByState);
                //NavigationService.NavigateTo(new GalleryWindow(del, path.Name));
            }
            else
            {
                MessageBox.Show("An error occurred");
                Console.WriteLine("MainWindow.xaml.cs: Path_MouseUp error");
            }
        }
        // Mouse enter path area event
        private void Path_MouseEnter(object sender, MouseEventArgs e)
        {
            Path path = sender as Path;
            SolidColorBrush darkBlue = new SolidColorBrush(Color.FromRgb(51, 92, 122));

            path.Fill = darkBlue;
            path.Cursor = Cursors.Hand;

            if (dict.Keys.Contains(path.Name))
            {
                txtCount.Content = dict[path.Name].ToString() + " Photo(s)";
            }
            else
            {
                txtCount.Content = "No Photos";
            }
        }
        // Mouse leave path area event
        private void Path_MouseLeave(object sender, MouseEventArgs e)
        {
            Path path = sender as Path;
            SolidColorBrush lightBlue = new SolidColorBrush(Color.FromRgb(70, 115, 148));
            
            path.Fill = lightBlue;
            path.Cursor = Cursors.Arrow;

            txtCount.Content = "";
        }
        // Search textbox focus on event
        private void txtSearch_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox txtSeach = sender as TextBox;
            txtSeach.Text = "";

            SolidColorBrush white = new SolidColorBrush(Color.FromRgb(255, 255, 255));

            txtSeach.Foreground = white;
        }
        // Search textbox focus off event
        private void txtSearch_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox txtSeach = sender as TextBox;
            if (txtSeach.Text == "" || txtSeach.Text == string.Empty)
            {
                txtSeach.Text = "Search by location or tag";
                SolidColorBrush gray = new SolidColorBrush(Color.FromRgb(65, 65, 65));

                txtSeach.Foreground = gray;
            }
        }
        // Search button click event
        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            string input = txtSearch.Text.Trim();
            if (input != "" && input != null && input != "Search by location or tag")
            {
                GalleryDelegate del = new GalleryDelegate(ImageRepository.GetImagesByTerm);
                NavigationService.NavigateTo(new ImageGallery(del, input));

                //GalleryDelegate del = new GalleryDelegate(ImageRepository.GetImagesByState);
                //NavigationService.NavigateTo(new GalleryWindow(del, input));
            }
            else
            {
                MessageBox.Show("Enter valid data");
                Console.WriteLine("MainWindow.xaml.cs: btnSearch_Click error");
            }
        }
        // Upload new image button click event
        private void btnUpload_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.NavigateTo(new UploadWindow());
        }

        private void Window_VisibilityChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            Window window = sender as Window;
            if (window.Visibility == Visibility.Visible)
            {
                LoadDictionary();
            }
        }
    }
}
