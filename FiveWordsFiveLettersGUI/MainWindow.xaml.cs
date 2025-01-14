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
                MessageBox.Show("File selected: " + selectedFilePath);
            }
        }

        private async void RunLibraryButton_Click(object sender, RoutedEventArgs e)
        {
            
            // Ensure a file is selected
            if (string.IsNullOrEmpty(selectedFilePath))
            {
                MessageBox.Show("Please select a file first.");
                return;
            }
            IsIndeterminate = true;
            try
            {
                // Assuming you have a class library with a method like Run or Process
                // Example: YourClassLibrary.YourClass.Run(filePath, wordCount, wordLength);
                var fwfl = new FiveWordsFiveLettersCL.FiveWordsFiveLettersCL(selectedFilePath, 5, 5);
                //var fwfl = new FiveWordsFiveLettersCL.FiveWordsFiveLettersCL(
                //    selectedFilePath,               // File path
                //    (int)wordCount.Value,           // Word count slider value
                //    (int)wordLength.Value           // Word length slider value
                //);

                fwfl.SearchIndex += Fwfl_SearchIndex;
                fwfl.SearchMaxFound += Fwfl_SearchMaxFound;

                await fwfl.DoWork();

                // Optionally, update the progress bars
                // Adjust based on what the method returns

                MessageBox.Show($"Run completed. Found {fwfl._countSolutions} Solutions");
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while running the library: " + ex.Message);
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
        private void wordCount_ValueChanged(object sender, RoutedPropertyChangedEventArgs<int> e)
        {
            // Update the TextBlock displaying Word Count slider value
            if (wordCountValue != null)
            {
                wordCountValue.Text = ((int)wordCount.Value).ToString();
            }
        }

        // Update Word Length value display
        private void wordLength_ValueChanged(object sender, RoutedPropertyChangedEventArgs<int> e)
        {
            // Update the TextBlock displaying Word Length slider value
            if (wordLengthValue != null)
            {
                wordLengthValue.Text = ((int)wordLength.Value).ToString();
            }
        }
    }
}
