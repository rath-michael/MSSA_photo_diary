using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Week8Project
{
    public partial class EditWindow : Window
    {
        private UserImage image;
        public EditWindow(UserImage image)
        {
            InitializeComponent();
            this.image = image;
        }
        // Window loaded event
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Try to load image data into form
            try
            {
                cboState.DisplayMemberPath = "StateName";
                cboState.ItemsSource = ImageRepository.GetStates();

                if (image != null)
                {
                    imgNewImage.DataContext = image;
                    txtTitle.Text = image.Title;
                    cboState.SelectedItem = image.State;
                    dtpTaken.SelectedDate = image.DateTaken;

                    txtTags.Text = TagConverter.TagsToString(image.ImageTags);

                    txtDescription.Text = image.Description;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred");
                Console.WriteLine("EditWindow.xaml.cs: WindowLoaded error: " + ex.Message);
            }
        }
        // Title textbox focus on event
        private void txtTitle_GotFocus(object sender, RoutedEventArgs e)
        {
            // If default text, clear and change color
            TextBox txtTitle = sender as TextBox;
            if (txtTitle.Text == "Enter title")
            {
                txtTitle.Text = "";

                SolidColorBrush white = new SolidColorBrush(Color.FromRgb(255, 255, 255));

                txtTitle.Foreground = white;
            }
        }
        // Title textbox focus off event
        private void txtTitle_LostFocus(object sender, RoutedEventArgs e)
        {
            // If empty text, reset text and color
            TextBox txtTitle = sender as TextBox;
            if (txtTitle.Text == "")
            {
                txtTitle.Text = "Enter title";

                SolidColorBrush gray = new SolidColorBrush(Color.FromRgb(115, 115, 115));

                txtTitle.Foreground = gray;
            }
        }
        // Tags textbox focus on event
        private void txtTags_GotFocus(object sender, RoutedEventArgs e)
        {
            // If default text, clear and change color
            TextBox txtTags = sender as TextBox;
            if (txtTags.Text == "Enter tags seperated by comma")
            {
                txtTags.Text = "";

                SolidColorBrush brush = new SolidColorBrush();
                brush.Color = Color.FromRgb(255, 255, 255);
                txtTags.Foreground = brush;
            }
        }
        // Tags textbox focus on event
        private void txtTags_LostFocus(object sender, RoutedEventArgs e)
        {
            // If empty text, reset text and color
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
            // If default text, clear and change color
            TextBox txtDescription = sender as TextBox;
            if (txtDescription.Text == "Enter description")
            {
                txtDescription.Text = "";

                SolidColorBrush brush = new SolidColorBrush();
                brush.Color = Color.FromRgb(255, 255, 255);
                txtDescription.Foreground = brush;
            }
        }
        // Description textbox focus on event
        private void txtDescription_LostFocus(object sender, RoutedEventArgs e)
        {
            // If empty text, reset text and color
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
            // If all iputs valid
            if (Validate())
            {
                // Try to save changes to current image
                try
                {
                    image.Title = txtTitle.Text.Trim();
                    image.State = cboState.SelectedItem as State;
                    image.DateTaken = dtpTaken.SelectedDate.Value;
                    
                    ImageRepository.RemoveOldTags(image);
                    // Convert text input to list of tags
                    List<string> tags = TagConverter.StringToTags(txtTags.Text);
                    // Add tags to image
                    foreach (string tag in tags)
                    {
                        ImageTag newTag = new ImageTag()
                        {
                            TagID = ImageRepository.GetMostRecentTagID() + 1,
                            UserImageID = image.ImageID,
                            Tag = tag
                        };
                        image.ImageTags.Add(newTag);
                    }
                    image.Description = txtDescription.Text.Trim();
                    ImageRepository.UpdateImage(image);
                    MessageBox.Show("Photo updated");
                    Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("EditWindow SaveImageClick(): " + ex.Message);
                    MessageBox.Show("An error occured");
                }
            }
            else
            {
                Console.WriteLine("Please enter valid information");
            }
        }
        // Validate inputs method
        private bool Validate()
        {
            bool canSave = true;
            // if title is not valid
            if (txtTitle.Text == "Enter title" || txtTitle.Text == String.Empty)
            {
                canSave = false;
            }

            // if state is not valid
            if (cboState.SelectedIndex == -1)
            {
                canSave = false;
            }

            // if datetaken is not empty
            if (!dtpTaken.SelectedDate.HasValue)
            {
                canSave = false;
            }

            // if tags are not empty
            if (txtTags.Text == "Enter tags seperated by comma" || txtTags.Text == String.Empty)
            {
                canSave = false;
            }

            // if description is not empty
            if (txtDescription.Text == "Enter description" || txtDescription.Text == String.Empty)
            {
                canSave = false;
            }
            return canSave;
        }
        // Window closed event
        private void Window_Closed(object sender, EventArgs e)
        {
            // Verify can navigate to previous windw
            if (NavigationService.CanNavigateBack())
            {
                NavigationService.NavigateBack();
                try
                {
                    DetailsWindow window = NavigationService.PeekStack() as DetailsWindow;
                    if (window != null && window.Name == "Details_Window")
                    {
                        window.UpdateDisplay(window.listIndex);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("EditWindow.xaml.cs Window_Closed: " + ex.Message);
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
