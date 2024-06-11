using System.Globalization;

namespace FileStatisticsFilter.CommonLibrary
{
    public class SearchedFile
    {
        public DateTime CreatedTime { get; set; }
        public string Directory { get; set; }
        public string Extension { get; set; }
        public string FileName { get; }
        public string FileNameWithoutExtension { get; set; }
        public string FullName { get; }
        public bool IsReadOnly { get; set; }
        public DateTime ModifiedTime { get; set; }
        public long Size { get; set; }
        public string SizeReadable { get; }

        public SearchedFile(FileInfo file)
        {
            CreatedTime = file.CreationTime;
            Directory = file.DirectoryName;
            Extension = file.Extension;
            FileName = file.Name;
            FileNameWithoutExtension = Path.GetFileNameWithoutExtension(file.Name);
            FullName = Path.Combine(Directory, FileNameWithoutExtension + Extension);
            IsReadOnly = file.IsReadOnly;
            ModifiedTime = file.LastWriteTime;
            Size = file.Length;
            SizeReadable = GetReadableSize(file.Length);
        }

        public SearchedFile(string csvLine, char delimeter = 't')
        {
            var values = csvLine.Split(delimeter);
            
            Directory = values[0];
            FileNameWithoutExtension = values[1];
            Extension = values[2];
            Size = long.Parse(values[3]);
            CreatedTime = DateTime.Parse(values[4], CultureInfo.InvariantCulture);
            ModifiedTime = DateTime.Parse(values[5], CultureInfo.InvariantCulture);
           
            SizeReadable = GetReadableSize(Size);

            FileName = FileNameWithoutExtension + Extension;
            FullName = Path.Combine(Directory, FileName);
            SizeReadable = GetReadableSize(Size);
        }

        private string GetReadableSize(long size)
        {
            string[] sizes = { "B", "KB", "MB", "GB", "TB" };
            double len = size;
            int order = 0;
            while (len >= 1024 && order < sizes.Length - 1)
            {
                order++;
                len = len / 1024;
            }

            return $"{len:0.##} {sizes[order]}";
        }

        public static string ToCsvHeaderLine(char delimiter = '\t')
        {
            return string.Join(delimiter, new string[]
            {
                "Directory",
                "FileNameWithoutExtension",
                "Extension",
                "Size",
                "CreatedTime",
                "ModifiedTime",
                "IsReadOnly"
            });
        }

        public string ToCsvLine(char delimiter = '\t')
        {
            return string.Join(delimiter, new string[]
                {
                    Directory,
                    FileNameWithoutExtension,
                    Extension,
                    Size.ToString(),
                    CreatedTime.ToString(CultureInfo.InvariantCulture),
                    ModifiedTime.ToString(CultureInfo.InvariantCulture),
                    IsReadOnly.ToString()

                }
            );
        }


    }
}
