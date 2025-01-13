using Microsoft.Win32; // For OpenFileDialog
using System;
using System.Windows;
using System.Windows.Controls;

namespace FiveWordsFiveLettersGUI
{
    public partial class MainWindow : Window
    {
        private string selectedFilePath = string.Empty;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void OpenFileButton_Click(object sender, RoutedEventArgs e)
        {
            // Open file dialog to select a file
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                // Store the selected file path
                selectedFilePath = openFileDialog.FileName;
                MessageBox.Show("File selected: " + selectedFilePath);
            }
        }

        private void RunLibraryButton_Click(object sender, RoutedEventArgs e)
        {
            // Ensure a file is selected
            if (string.IsNullOrEmpty(selectedFilePath))
            {
                MessageBox.Show("Please select a file first.");
                return;
            }

            try
            {
                // Assuming you have a class library with a method like Run or Process
                // Example: YourClassLibrary.YourClass.Run(filePath, wordCount, wordLength);

                var result = YourClassLibrary.YourClass.Run(
                    selectedFilePath,          // File path
                    wordCount.Value,           // Word count slider value
                    wordLength.Value           // Word length slider value
                );

                // Optionally, update the progress bars
                ProgressBar.Value = result; // Adjust based on what the method returns
                ProgressBar2.Value = (wordCount.Value + wordLength.Value) / 2; // Example logic for updating ProgressBar2

                MessageBox.Show("Library run completed.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while running the library: " + ex.Message);
            }
        }

        // Update Word Count value display
        private void wordCount_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            // Update the TextBlock displaying Word Count slider value
            wordCountValue.Text = ((int)wordCount.Value).ToString();
        }

        // Update Word Length value display
        private void wordLength_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            // Update the TextBlock displaying Word Length slider value
            wordLengthValue.Text = ((int)wordLength.Value).ToString();
        }
    }
}
