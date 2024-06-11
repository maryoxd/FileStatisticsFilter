using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using FileStatisticsFilter.CommonLibrary;
using Microsoft.Win32;
using Path = System.IO.Path;

namespace Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private SearchedFiles sf;

        public MainWindow()
        {
            InitializeComponent();
            sf = new SearchedFiles();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog()
            {
                Filter = "CSV Files (*.csv)|*.csv|All Files (*.*)|*.*",
                FilterIndex = 1,
                InitialDirectory = @"C:\",
                RestoreDirectory = true
            };

            bool? result = open.ShowDialog();
            if (result == true)
            {
                string filePath = open.FileName;
                sf.LoadFromCsv(new FileInfo(filePath));
                FilesListView.ItemsSource = sf.Files;
                UpdateStatistics();
                LoadDirectories(Path.GetDirectoryName(filePath));
            }
        }

        private void DirectoryComboBox_SelectionChanged(object sender,
            System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (DirectoryComboBox.SelectedItem != null)
            {
                string selectedDirectory = DirectoryComboBox.SelectedItem.ToString();
                LoadFilesFromDirectory(selectedDirectory, IncludeSubdirectoriesCheckBox.IsChecked == true);
            }
        }

        private void IncludeSubdirectoriesCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (DirectoryComboBox.SelectedItem != null)
            {
                string selectedDirectory = DirectoryComboBox.SelectedItem.ToString();
                LoadFilesFromDirectory(selectedDirectory, true);
            }
        }

        private void IncludeSubdirectoriesCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            if (DirectoryComboBox.SelectedItem != null)
            {
                string selectedDirectory = DirectoryComboBox.SelectedItem.ToString();
                LoadFilesFromDirectory(selectedDirectory, false);
            }
        }

        private void LoadDirectories(string rootDirectory)
        {
            try
            {
                DirectoryComboBox.Items.Clear();
                DirectoryComboBox.Items.Add(rootDirectory);
                foreach (var directory in Directory.EnumerateDirectories(rootDirectory, "*",
                             SearchOption.TopDirectoryOnly))
                {
                    DirectoryComboBox.Items.Add(directory);
                }

                DirectoryComboBox.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading directories: {ex.Message}", "Error", MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void LoadFilesFromDirectory(string directory, bool includeSubdirectories)
        {
            try
            {
                var searchOption = includeSubdirectories ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
                var files = Directory.EnumerateFiles(directory, "*", searchOption)
                    .Select(f => new FileInfo(f));
                sf = new SearchedFiles(files);
                FilesListView.ItemsSource = sf.Files;
                UpdateStatistics();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading files: {ex.Message}", "Error", MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void UpdateStatistics()
        {
            try
            {
                var totalFiles = sf.Files.Length;
                var totalSize = sf.Files.Sum(file => file.Size);
                var totalDirectories = sf.Files.Select(file => file.Directory).Distinct().Count();
                var readOnlyFiles = sf.Files.Count(file => file.IsReadOnly);
                var createdNewest = sf.Files.Max(file => file.CreatedTime);
                var createdOldest = sf.Files.Min(file => file.CreatedTime);
                var modifiedNewest = sf.Files.Max(file => file.ModifiedTime);
                var modifiedOldest = sf.Files.Min(file => file.ModifiedTime);

                FilesTextBlock.Text = $"Files: {totalFiles} / {totalFiles}";
                DirectoriesTextBlock.Text = $"Directories: {totalDirectories} / {totalDirectories}";
                TotalSizeTextBlock.Text = $"Total size: {GetReadableSize(totalSize)}";
                CreatedNewestTextBlock.Text = $"Created time (newest): {createdNewest}";
                CreatedOldestTextBlock.Text = $"Created time (oldest): {createdOldest}";
                ModifiedNewestTextBlock.Text = $"Modified time (newest): {modifiedNewest}";
                ModifiedOldestTextBlock.Text = $"Modified time (oldest): {modifiedOldest}";
                ReadOnlyFilesTextBlock.Text = $"Readonly files: {readOnlyFiles}";

                var extensionStats = sf.Files.GroupBy(file => file.Extension.ToLower())
                    .Select(g => new
                    {
                        Extension = g.Key,
                        Count = g.Count(),
                        TotalSize = GetReadableSize(g.Sum(file => file.Size))
                    })
                    .OrderByDescending(stat => stat.Count)
                    .ToList();

                ExtensionListView.ItemsSource = extensionStats;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating statistics: {ex.Message}", "Error", MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private string GetReadableSize(long size)
        {
            string[] sizes = { "B", "KB", "MB", "GB", "TB" };
            double len = size;
            int order = 0;
            while (len >= 1024 && order < sizes.Length - 1)
            {
                order++;
                len /= 1024;
            }
            return $"{len:0.##} {sizes[order]}";
        }
    }
    
}
