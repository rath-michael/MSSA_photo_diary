using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Week8Project
{
    public partial class DetailsWindow : Window
    {
        private List<UserImage> imageList;
        private UserImage currentImage;
        private int selectedID;
        public int listIndex;
        private double currentScale = 1.0;
        public DetailsWindow(List<UserImage> images, int id)
        {
            InitializeComponent();
            currentImage = null;
            imageList = images;
            selectedID = id;
        }
        // Window loaded event
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Get list index of current image
            listIndex = 0;
            foreach (UserImage image in imageList)
            {
                if (image.ImageID == selectedID)
                {
                    break;
                }
                listIndex++;
            }
            // Display current image
            UpdateDisplay(listIndex);
        }
        // Previous button click event
        private void btnPrev_Click(object sender, RoutedEventArgs e)
        {
            // If current list index above 0, show previous image
            if (listIndex > 0)
            {
                UpdateDisplay(--listIndex);
            }
            else
            {
                btnPrev.Visibility = Visibility.Hidden;
            }
        }
        // Next button click event
        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            // If current list index less than list length, show next image
            if (listIndex < imageList.Count-1)
            {
                UpdateDisplay(++listIndex);
            }
            else
            {
                btnNext.Visibility = Visibility.Hidden;
            }
        }
        // Update display method
        public void UpdateDisplay(int index)
        {
            // Get current image
            currentImage = imageList[index];
            // Reset and hide details context menu
            panelDetails.Visibility = Visibility.Hidden;
            imgDisplay.Opacity = 1;
            btnDetails.Header = "Show Details";
            // Reset image context details
            imgDisplay.DataContext = null;
            panelDetails.DataContext = null;
            imgDisplay.DataContext = currentImage;
            panelDetails.DataContext = currentImage;

            // Reset zoom back to default and image to center screen
            currentScale = 1.0;
            imgDisplay.RenderTransform = new ScaleTransform(currentScale, currentScale, 600, 370);

            // Reset previous and next buttons based on list length
            if (imageList.Count <= 1)
            {
                btnPrev.Visibility = Visibility.Hidden;
                btnNext.Visibility = Visibility.Hidden;
            }
            else
            {
                // Reset previous and next buttons based on index position
                if (index == 0)
                {
                    btnPrev.Visibility = Visibility.Hidden;
                    btnNext.Visibility = Visibility.Visible;
                }
                else if (index == imageList.Count - 1)
                {
                    btnPrev.Visibility = Visibility.Visible;
                    btnNext.Visibility = Visibility.Hidden;
                }
                else
                {
                    btnPrev.Visibility = Visibility.Visible;
                    btnNext.Visibility = Visibility.Visible;
                }
            }   
        }
        // Navigate to EditWindow with current image
        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.NavigateTo(new EditWindow(currentImage));
        }
        // Confirm and delete current image
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            // Confirm intention to delete
            var result = MessageBox.Show("Are you sure?", "Confirm", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                // Delete and send confirmation message
                bool res = ImageRepository.DeleteImage(currentImage);
                if (res) MessageBox.Show("Photo deleted", "Removed", MessageBoxButton.OK);
                else MessageBox.Show("Couldn't delete", "Error", MessageBoxButton.OK);
                Close();
            }
        }
        // Image details context menu
        private void btnDetails_Click(object sender, RoutedEventArgs e)
        {
            // Show context menu
            if (panelDetails.Visibility == Visibility.Hidden)
            {
                panelDetails.Visibility = Visibility.Visible;
                imgDisplay.Opacity = 0.4;
                btnDetails.Header = "Hide Details";
            }
            // Hide context menu
            else
            {
                panelDetails.Visibility = Visibility.Hidden;
                imgDisplay.Opacity = 1;
                btnDetails.Header = "Show Details";
            }
        }
        // Window closed event
        private void Window_Closed(object sender, EventArgs e)
        {
            // Verify can navigate to previous windw
            if (NavigationService.CanNavigateBack())
            {
                // Navigate to previous window
                NavigationService.NavigateBack();
                // Get instance of previous window and reload list with image removed
                try
                {
                    ImageGallery window = NavigationService.PeekStack() as ImageGallery;
                    if (window != null && window.Name == "Image_Gallery")
                    {
                        window.startPos = 0;
                        window.endPos = 5;
                        window.ShowImages(0, 5);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("DetailsWindow.xaml.cs Window_Closed: " + ex.Message);
                }
            }
        }

        // Source user: mmm8  https://stackoverflow.com/questions/43629642/zoom-on-image-in-wpf
        private void Image_MouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
        {
            var position = e.MouseDevice.GetPosition(imgDisplay);

            if (e.Delta > 0)
            {
                currentScale += 0.1;
            }
            else if (e.Delta < 0)
            {
                currentScale -= 0.1;
                if (currentScale < 1.0)
                    currentScale = 1.0;
            }

            imgDisplay.RenderTransform = new ScaleTransform(currentScale, currentScale,
                position.X, position.Y);
        }
        // Back button click event
        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
