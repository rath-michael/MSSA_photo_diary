using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Week8Project
{
    public partial class UploadWindow : Window
    {
        private UserImage uploadedImage;
        public UploadWindow()
        {
            InitializeComponent();
        }
        // Window loaded event
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Add list of states to state combobox
            cboState.DisplayMemberPath = "StateName";
            try
            {
                cboState.ItemsSource = ImageRepository.GetStates();
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred");
                Console.WriteLine("UploadWindow.xaml.cs: WindowLoaded error: " + ex.Message);
            }
            uploadedImage = null;
        }
        // Title textbox focus on event
        private void txtTitle_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox txtTitle = sender as TextBox;
            if (txtTitle.Text == "Enter title")
            {
                txtTitle.Text = "";

                SolidColorBrush brush = new SolidColorBrush();
                brush.Color = Color.FromRgb(255, 255, 255);
                txtTitle.Foreground = brush;
            }
        }
        // Title textbox focus off event
        private void txtTitle_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox txtTitle = sender as TextBox;
            if (txtTitle.Text == "")
            {
                txtTitle.Text = "Enter title";

                SolidColorBrush brush = new SolidColorBrush();
                brush.Color = Color.FromRgb(115, 115, 115);
                txtTitle.Foreground = brush;
            }
        }
        // Tags textbox focus on event
        private void txtTags_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox txtTags = sender as TextBox;
            if (txtTags.Text == "Enter tags seperated by comma")
            {
                txtTags.Text = "";

                SolidColorBrush brush = new SolidColorBrush();
                brush.Color = Color.FromRgb(255, 255, 255);
                txtTags.Foreground = brush;
            }
        }
        // Tags textbox focus off event
        private void txtTags_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox txtTags = sender as TextBox;
            if (txtTags.Text == "")
            {
                txtTags.Text = "Enter tags seperated by comma";

                SolidColorBrush brush = new SolidColorBrush();
                brush.Color = Color.FromRgb(115, 115, 115);
                txtTags.Foreground = brush;
            }
        }
        // Description textbox focus on event
        private void txtDescription_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox txtDescription = sender as TextBox;
            if (txtDescription.Text == "Enter description")
            {
                txtDescription.Text = "";

                SolidColorBrush brush = new SolidColorBrush();
                brush.Color = Color.FromRgb(255, 255, 255);
                txtDescription.Foreground = brush;
            }
        }
        // Description textbox focus off event
        private void txtDescription_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox txtDescription = sender as TextBox;
            if (txtDescription.Text == "")
            {
                txtDescription.Text = "Enter description";

                SolidColorBrush brush = new SolidColorBrush();
                brush.Color = Color.FromRgb(115, 115, 115);
                txtDescription.Foreground = brush;
            }
        }
        // Save button click event
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            bool imageSuccess = false;

            if (Validate())
            {
                try
                {
                    // Create new userimage object and save properties
                    UserImage newImage = new UserImage();
                    State state = (State)cboState.SelectedItem;
                    newImage.StateID = ImageRepository.GetStateID(state.StateName);
                    newImage.ImageID = ImageRepository.GetMostRecentImageID() + 1;

                    newImage.ImageByte = uploadedImage.ImageByte;
                    //newImage.ImageByte = CompressionTool.Compress(uploadedImage.ImageByte);

                    newImage.Title = txtTitle.Text.ToString().Trim();
                    newImage.Description = txtDescription.Text.ToString().Trim();
                    newImage.DateTaken = dtpTaken.SelectedDate.Value;
                    // Convert string input to list of tags
                    List<string> tags = TagConverter.StringToTags(txtTags.Text);
                    // Save list of tags to new image
                    foreach (string tag in tags)
                    {
                        ImageTag newTag = new ImageTag()
                        {
                            TagID = ImageRepository.GetMostRecentTagID() + 1,
                            UserImageID = newImage.ImageID,
                            Tag = tag
                        };
                        newImage.ImageTags.Add(newTag);
                    }

                    // Save new image to DB
                    imageSuccess = ImageRepository.AddImage(newImage);
                    MessageBox.Show("Added photo");
                    Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("UploadWindow SaveImageClick: " + ex.Message);
                    MessageBox.Show("An error occured");
                }
            }
            else
            {
                MessageBox.Show("Please enter all necessary information");
            }
        }
        // Validate input text boxes
        private bool Validate()
        {
            bool canSave = true;
            // if newImage imagebyte is not empty
            if (imgNewImage.Source == null)
            {
                canSave = false;
            }
            // if title is not empty
            if (txtTitle.Text == "Enter title")
            {
                canSave = false;
            }
            // if description is not empty
            if (txtDescription.Text == "Enter description")
            {
                canSave = false;
            }
            // if datetaken is not empty
            if (!dtpTaken.SelectedDate.HasValue)
            {
                canSave = false;
            }
            // if tags are not empty
            if (txtTags.Text == "Enter tags seperated by comma")
            {
                canSave = false;
            }
            // if state is not empty
            if (cboState.SelectedIndex == -1)
            {
                canSave = false;
            }
            return canSave;
        }
        // Select image button click
        private void btnSelectImage_Click(object sender, RoutedEventArgs e)
        {
            // Open file dialog with image extensions
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Title = "Select a picture";
            dialog.Filter = "All supported graphics|*.jpg;*.jpeg;*.jfif;*.png|" +
              "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
              "JFIF (*.jfif;*.jfif)|*.jfif;*.jfif|" +
              "Portable Network Graphic (*.png)|*.png";
            // If dialog valid
            if (dialog.ShowDialog() == true)
            {
                // Save image to UploadImage
                uploadedImage = new UserImage()
                {
                    ImageByte = File.ReadAllBytes(dialog.FileName)
                };
                // Add UploadImage to image display context
                imgNewImage.DataContext = null;
                imgNewImage.DataContext = uploadedImage;
            }
        }
        // Window closed event
        private void On_Closed(object sender, EventArgs e)
        {
            // Verify can navigate to previous windw
            if (NavigationService.CanNavigateBack())
            {
                // Navigate to previous window
                NavigationService.NavigateBack();
                try
                {
                    // Get instance of previous window and reload list with image added
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
        // Cancel button click event
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
