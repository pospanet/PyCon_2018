using System.Collections.Generic;
using CommandLine;

namespace ImageGenerator
{
    public interface IOptions
    {
        [Option('c', "columns", Default = 5, Required = false, HelpText = "Number of tiles column.")]
        int Columns { get; set; }
        [Option('r', "rows", Default = 5, Required = false, HelpText = "Number of tiles rows.")]
        int Rows { get; set; }
        [Option('p', "padding", Default = 10, Required = false, HelpText = "Image padding.")]
        int Padding { get; set; }
        [Option('o', "output", Default = ".", Required = false, HelpText = "Output folder.")]
        string OutputPath { get; set; }
        [Option('u', "url", Required = false, HelpText = "Input URL(s) to be processed.")]
        // ReSharper disable InconsistentNaming
        IEnumerable<string> InputURLs { get; set; }
        // ReSharper restore InconsistentNaming
        [Option('f', "files", Required = true, HelpText = "Input file(s) to be processed.")]
        IEnumerable<string> Files { get; set; }
        [Option('k', "cntk", Default = true, Required = false, HelpText = "Generate CNTK meta files.")]
        bool Cntk { get; set; }
    }
}