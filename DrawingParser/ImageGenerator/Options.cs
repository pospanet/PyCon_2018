using System.Collections.Generic;

namespace ImageGenerator
{
    public class Options : IOptions
    {
        public int Columns { get; set; }
        public int Rows { get; set; }
        public int Padding { get; set; }
        public string OutputPath { get; set; }
        public IEnumerable<string> InputURLs { get; set; }
        public IEnumerable<string> Files { get; set; }
        public bool Cntk { get; set; }
    }
}