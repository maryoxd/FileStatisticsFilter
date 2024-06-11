using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileStatisticsFilter.CommonLibrary
{
    public class SearchedFiles
    {
        public SearchedFile[] Files { get; set; }

        public SearchedFiles()
        {
            Files = Array.Empty<SearchedFile>();
        }

        public SearchedFiles(IEnumerable<FileInfo> files)
        {
            Files = files.Select(file => new SearchedFile(file)).ToArray();
        }

        public void LoadFromCsv(FileInfo file)
        {
            var lines = File.ReadAllLines(file.FullName);
            var delimeter = '\t';
            Files = lines.Skip(1).Select(line => new SearchedFile(line, delimeter)).ToArray();

        }

        public void SaveToCsv(FileInfo file)
        {
            var delimiter = '\t'; // Predpokladáme, že CSV bude oddeľovaný tabulátorom
            var lines = new List<string> { SearchedFile.ToCsvHeaderLine(delimiter) };
            lines.AddRange(Files.Select(f => f.ToCsvLine(delimiter)));
            File.WriteAllLines(file.FullName, lines);
        }

    }
}
