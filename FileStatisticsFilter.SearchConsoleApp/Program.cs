using FileStatisticsFilter.CommonLibrary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FileStatisticsFilter.SearchConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            string inputDirectory = null;
            string outputFile = null;
            bool recursive = false;
            
            // Parsing command line arguments
            for (int i = 0; i < args.Length; i++)
            {
                Console.WriteLine(args[i]);
                if (args[i].Equals("--input"))
                {
                    inputDirectory = args[i + 1];
                }
                else if (args[i].Equals("--output"))
                {
                    outputFile = args[i + 1];
                }
                else if (args[i] == "--recursive")
                {
                    recursive = true;
                }
            }

            Console.WriteLine(inputDirectory);
            // Validate input parameters
            if (string.IsNullOrEmpty(inputDirectory) || string.IsNullOrEmpty(outputFile))
            {
                Console.Error.WriteLine("Error: Both input and output parameters are required.");
                return;
            }

            if (!Directory.Exists(inputDirectory))
            {
                Console.Error.WriteLine($"Error: The directory '{inputDirectory}' does not exist.");
                return;
            }

            try
            {
                // Search files
                var searchOption = recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
                var files = Directory.EnumerateFiles(inputDirectory, "*", searchOption)
                                     .Select(f => new FileInfo(f));

                // Create SearchedFiles instance
                var searchedFiles = new SearchedFiles(files);

                // Save to CSV
                var outputFileInfo = new FileInfo(outputFile);
                searchedFiles.SaveToCsv(outputFileInfo);

                Console.WriteLine($"Files have been successfully saved to {outputFile}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}