using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Week8Project
{
    public partial class ImageGallery : Window
    {
        private GalleryDelegate galleryDel;
        private string searchTerm;

        private List<UserImage> images;
        private List<Image> imageBoxes;

        public int startPos, endPos;
        // Initiliaze necessary variables
        public ImageGallery(GalleryDelegate del, string input)
        {
            InitializeComponent();
            galleryDel = del;
            searchTerm = input;
            startPos = 0;
            endPos = 5;
        }
        // Window loaded event
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Add image controls to list
            imageBoxes = new List<Image>()
            {
                ImageOne, ImageTwo, ImageThree, ImageFour, ImageFive, ImageSix
            };
            // Show current images
            ShowImages(startPos, endPos);
        }
        // Mouse enter image control event
        private void Image_MouseEnter(object sender, MouseEventArgs e)
        {
            Image image = sender as Image;
            image.Width *= 1.1;
            image.Height *= 1.1;
            image.Cursor = Cursors.Hand;
        }
        // Mouse leave image control event
        private void Image_MouseLeave(object sender, MouseEventArgs e)
        {
            Image image = sender as Image;
            image.Width = 260;
            image.Height = 175;
            image.Cursor = Cursors.Arrow;
        }
        // Display images method
        public void ShowImages(int start, int end)
        {
            // Get list of images based on delegate and input term
            images = galleryDel(searchTerm);
            lblDisplay.Content = images.Count + " Photos";
            // Set range of images to image contols
            for (int i = start, j = 0; i <= end; i++, j++)
            {
                if (i < images.Count)
                {
                    imageBoxes[j].DataContext = images[i];
                }
                else
                {
                    imageBoxes[j].DataContext = null;
                }
            }
            // By default, hide previous and next buttons
            btnPrev.Visibility = Visibility.Hidden;
            btnNext.Visibility = Visibility.Hidden;
            // Display previous next and buttons based on starnd and end position
            if (start > 5)
            {
                btnPrev.Visibility = Visibility.Visible;
            }
            if (end < images.Count - 1)
            {
                btnNext.Visibility = Visibility.Visible;
            }
        }
        // Previous button click event
        private void btnPrev_Click(object sender, RoutedEventArgs e)
        {
            if (startPos > 5)
            {
                startPos -= 6;
                endPos -= 6;
                ShowImages(startPos, endPos);
            }
            else
            {
                Console.WriteLine("Outside lower bound of list");
            }
        }
        // Next button click event
        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            if (endPos < images.Count - 1)
            {
                startPos += 6;
                endPos += 6;
                ShowImages(startPos, endPos);
            }
            else
            {
                Console.WriteLine("Outside upper bound of list");
            }
        }
        // Image control click event
        private void Image_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            // Get UserImage item from image control data context
            Image image = sender as Image;
            UserImage selectedImage = image.DataContext as UserImage;
            // If valid image, navigate to details window
            if (selectedImage != null)
            {
                NavigationService.NavigateTo(new DetailsWindow(images, selectedImage.ImageID));
            }
            else
            {
                MessageBox.Show("An error occurred");
                Console.WriteLine("ImageGallery.xaml.cs: Image_MouseLeftButtonUp error");
            }
        }
        // Upload button click event
        private void btnUpload_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.NavigateTo(new UploadWindow());
        }
        // Back button click event
        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        // Window closed event
        private void Window_Closed(object sender, EventArgs e)
        {
            // Verify can navigate to previous windw
            if (NavigationService.CanNavigateBack())
            {
                NavigationService.NavigateBack();
            }
        }
    }
}
