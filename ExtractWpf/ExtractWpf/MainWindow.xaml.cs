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
                return;
            if (!File.Exists(FileOrFolderPath) && !Directory.Exists(FileOrFolderPath))
                return;
            
            //good to go!!;
            if (Directory.Exists(FileOrFolderPath))
            {
                var zipFileName = Path.Combine(FileOrFolderPath, $"{Path.GetDirectoryName(FileOrFolderPath)}.zip");
                if (File.Exists(zipFileName))
                {
                    //Delete file
                    File.Delete(zipFileName);
                }
                ZipFile.CreateFromDirectory(FileOrFolderPath, zipFileName);
            }
            else
            {
                //Create folder with same name if it doesn't exist
                //if it exists, remove it
                var parentFolder = Path.GetDirectoryName(FileOrFolderPath);
                if (string.IsNullOrEmpty(parentFolder)) return;
                var folderName = Path.Combine(parentFolder, $"{Path.GetFileNameWithoutExtension(FileOrFolderPath)}");
                if(Directory.Exists(folderName))
                    Directory.Delete(folderName, true);
                Directory.CreateDirectory(folderName);
                //copy file into folder
                File.Copy(FileOrFolderPath, Path.Combine(folderName, Path.GetFileName(FileOrFolderPath) ?? "filename"));
                //Zip folder
                var zipFileName = Path.Combine(parentFolder, $"{Path.GetFileNameWithoutExtension(FileOrFolderPath)}.zip");
                ZipFile.CreateFromDirectory(folderName, zipFileName);
                //Delete folder
                Directory.Delete(folderName, true);
            }
        }

        private void UnzipClicked(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(FileOrFolderPath))
                return;
            var fileExtension = Path.GetExtension(FileOrFolderPath);
            if (string.IsNullOrEmpty(fileExtension))
                return;
            if (!fileExtension.ToLower().Equals(".zip"))
                return;
            //good to go...
            var parentFolder = Path.GetDirectoryName(FileOrFolderPath);
            if (string.IsNullOrEmpty(parentFolder))
                return;
            var filenameWithoutExtension = Path.GetFileNameWithoutExtension(FileOrFolderPath);
            if (string.IsNullOrEmpty(filenameWithoutExtension))
                return;
            var folderName = Path.Combine(parentFolder, filenameWithoutExtension);
            if (Directory.Exists(folderName))
            {
                Directory.Delete(folderName, true);
            }
            ZipFile.ExtractToDirectory(FileOrFolderPath, folderName);
        }
    }
}