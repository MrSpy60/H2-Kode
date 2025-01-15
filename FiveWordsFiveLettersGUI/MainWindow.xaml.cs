using Microsoft.Win32; // For OpenFileDialog
using System;
using System.Windows;
using System.Windows.Controls;
using FiveWordsFiveLettersCL;
using System.ComponentModel;

namespace FiveWordsFiveLettersGUI
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private string selectedFilePath = string.Empty;
        private FiveWordsFiveLettersCL.FiveWordsFiveLettersCL _fwfl;

        protected void NotifyPropertyChange(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private bool _isIndeterminate;
        public bool IsIndeterminate
        {
            get { return _isIndeterminate; }
            set { _isIndeterminate = value; NotifyPropertyChange("Indeterminate"); }
        }

        private int _percent = 0;
        public int Percent
        {
            get { return _percent; }
            set { _percent = value; NotifyPropertyChange("Percent"); }
        }

        private int _percentMax = 100;
        public int PercentMax
        {
            get { return _percentMax; }
            set { _percentMax = value; NotifyPropertyChange("SearchMax"); }
        }

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OpenFileButton_Click(object sender, RoutedEventArgs e)
        {
            // Open file dialog to select a file
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                // Store the selected file path
                selectedFilePath = openFileDialog.FileName;
                RunLibraryButton.IsEnabled = true;
            }
        }

        private async void RunLibraryButton_Click(object sender, RoutedEventArgs e)
        {

            // Disable all buttons during the task
            SetButtonsEnabled(false);

            IsIndeterminate = true;
            try
            {
                // Assuming you have a class library with a method like Run or Process
                // Example: YourClassLibrary.YourClass.Run(filePath, wordCount, wordLength);
                _fwfl = new FiveWordsFiveLettersCL.FiveWordsFiveLettersCL(selectedFilePath, 5, 5);
                //_fwfl = new FiveWordsFiveLettersCL.FiveWordsFiveLettersCL(
                //    selectedFilePath,               // File path
                //    (int)wordCount.Value,           // Word count slider value
                //    (int)wordLength.Value           // Word length slider value
                //);

                _fwfl.SearchIndex += Fwfl_SearchIndex;
                _fwfl.SearchMaxFound += Fwfl_SearchMaxFound;

                await _fwfl.DoWork();

                // Optionally, update the progress bars
                // Adjust based on what the method returns
                MessageBox.Show($"Run completed. Found {_fwfl._countSolutions} Solutions");
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while running the library: " + ex.Message);
            }
            finally
            {
                // Re-enable all buttons once the task is completed or an exception is caught
                SetButtonsEnabled(true);
            }
        }

        private void Fwfl_SearchMaxFound(object? sender, int e)
        {
            PercentMax = e;
        }

        private void Fwfl_SearchIndex(object? sender, int e)
        {
            Percent = e;
        }

        // Update Word Count value display
        private void wordCount_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            // Update the TextBlock displaying Word Count slider value
            if (wordCountValue != null)
            {
                wordCountValue.Text = ((int)wordCount.Value).ToString();
            }
            if (wordLength != null)
            {
                wordLength.Maximum = (int)(26 / wordCount.Value);
            }
            if (ProgressBar2 != null)
            {
                calcLettersBar();
            }
        }

        // Update Word Length value display
        private void wordLength_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            // Update the TextBlock displaying Word Length slider value
            if (wordLengthValue != null)
            {
                wordLengthValue.Text = ((int)wordLength.Value).ToString();
            }
            if (ProgressBar2 != null)
            {
                calcLettersBar();
            }
        }

        private void calcLettersBar()
        {
            if (wordLength != null && wordCount != null)
            {
                ProgressBar2.Value = wordCount.Value * wordLength.Value;
            }
        }

        private void SaveFileButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(selectedFilePath))
            {
                MessageBox.Show("Please select a file first.");
                return;
            }

            // Get the directory path of the selected file
            string directoryPath = System.IO.Path.GetDirectoryName(selectedFilePath);

            // Define the new file name
            string newFileName = System.IO.Path.Combine(directoryPath, "output.txt");

            if (_fwfl == null)
            {
                MessageBox.Show($"Run program before you can save solution");
                return;
            }

            try
            {
                // Sample data to write to the file (you can modify this based on your needs)
                string dataToWrite = "Sample output data goes here.";

                // Write the data to the file
                System.IO.File.WriteAllText(newFileName, dataToWrite);

                MessageBox.Show($"File saved successfully to {newFileName}");
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while saving the file: " + ex.Message);
            }
        }

        private void SetButtonsEnabled(bool isEnabled)
        {
            // Set the buttons' enabled state to false or true
            OpenFileButton.IsEnabled = isEnabled;
            RunLibraryButton.IsEnabled = isEnabled;
            SaveFileButton.IsEnabled = isEnabled;
        }
    }
}
