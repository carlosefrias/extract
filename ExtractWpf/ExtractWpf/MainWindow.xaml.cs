using System;
using System.ComponentModel;
using System.IO;
using System.IO.Compression;
using System.Runtime.CompilerServices;
using System.Windows;
using ExtractWpf.Annotations;
using FolderBrowserForWPF;
using Microsoft.Win32;


namespace ExtractWpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public sealed partial class MainWindow: INotifyPropertyChanged
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private string _fileOrFolderPath;
        public string FileOrFolderPath
        {
            get => _fileOrFolderPath;
            set
            {
                if (string.Equals(value, _fileOrFolderPath))
                    return;
                _fileOrFolderPath = value;
                OnPropertyChanged(nameof(FileOrFolderPath));
            }
        }
        private void BrowseFolderClicked(object sender, RoutedEventArgs e)
        {
            var folderBrowser = new Dialog();
            if(folderBrowser.ShowDialog() == true)
            {
                FileOrFolderPath = folderBrowser.FileName;
            }
        }

        private void BrowseFileClicked(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog();
            if(openFileDialog.ShowDialog() == true)
                FileOrFolderPath = openFileDialog.FileName;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void ZipClicked(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(FileOrFolderPath))
            {
                throw new ApplicationException("No file/folder selected");
            }
            if (!File.Exists(FileOrFolderPath) && !Directory.Exists(FileOrFolderPath))
            {
                throw new ApplicationException("File/Folder doesn't exist");
            }
            //good to go!!;
            if (Directory.Exists(FileOrFolderPath))
            {
                ZipFile.CreateFromDirectory(FileOrFolderPath, Path.Combine(FileOrFolderPath, $"{Path.GetDirectoryName(FileOrFolderPath)}.zip"));
            }
            //TODO file...
        }

        private void UnzipClicked(object sender, RoutedEventArgs e)
        {
            throw new System.NotImplementedException();
        }
    }
}