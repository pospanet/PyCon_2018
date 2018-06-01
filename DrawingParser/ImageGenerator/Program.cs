using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using CommandLine;
using CommandLine.Text;
using Newtonsoft.Json;

namespace ImageGenerator
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            ParserResult<Options> result = Parser.Default.ParseArguments<Options>(args)
                .WithParsed(RunOptionsAndReturnExitCode)
                .WithNotParsed(HandleParseError);
        }
    
        private static void HandleParseError(IEnumerable<Error> errors)
        {
            //ToDo
        }

        private static void RunOptionsAndReturnExitCode(Options options)
        {
            IEnumerable<Task<string>> fileTasks = options.InputURLs.Select(DownloadFile);
            fileTasks = fileTasks as Task<string>[] ?? fileTasks.ToArray();
            Task.WaitAll(fileTasks.ToArray<Task>());
            IEnumerable<string> dataFiles = fileTasks.Select(ft => ft.Result).Union(options.Files).ToArray();
            List<Drawing> drawings = new List<Drawing>();
            List<string> dataFileList = new List<string>();
            foreach (string dataFile in dataFiles)
            {
                string directory = Path.GetDirectoryName(dataFile);
                string file = Path.GetFileName(dataFile);
                dataFileList.AddRange(Directory.EnumerateFiles(directory, file));
            }
            foreach (string file in dataFileList.Distinct())
            {
                drawings.AddRange(ParseDrawingsFromFile(file).Where(d => d.Recognized));
            }

            Drawing[] recognizedDrawings = drawings.Randomize().ToArray();
            int itemsPerPage = options.Columns * options.Rows;
            for (int page = 0; page < drawings.Count / itemsPerPage; page++)
            {
                Bitmap bmp = new Bitmap(
                    256 * options.Columns + (options.Columns - 1) * options.Padding,
                    256 * options.Rows + (options.Rows - 1) * options.Padding);
                List<Tuple<string, int, int>> labels = new List<Tuple<string, int, int>>();
                for (int i = 0; i < itemsPerPage; i++)
                {
                    Drawing drawing = recognizedDrawings[page * itemsPerPage + i];
                    int xPosition = i / options.Columns * (256 + options.Padding);
                    int yPosition = i % options.Columns * (256 + options.Padding);
                    GenerateImage(bmp, xPosition, yPosition, drawing.Lines);
                    labels.Add(new Tuple<string, int, int>(drawing.Word, xPosition, yPosition));
                }

                string imagePath = Path.Combine(Path.GetFullPath(options.OutputPath),
                    string.Concat("Page_", page, ".jpg"));
                string labelsPath = Path.Combine(Path.GetFullPath(options.OutputPath),
                    string.Concat("Page_", page, ".txt"));
                if (options.Cntk)
                {
                    string cntkBboxesPath = Path.Combine(Path.GetFullPath(options.OutputPath),
                        string.Concat("Page_", page, ".bboxes.tsv"));
                    string cntkBboxLabelsPath = Path.Combine(Path.GetFullPath(options.OutputPath),
                        string.Concat("Page_", page, ".bboxes.labels.tsv"));
                    using (StreamWriter sw = File.CreateText(cntkBboxLabelsPath))
                    {
                        string tabularizedLabels =
                            labels.Select(l => l.Item1)
                                .Aggregate((p, n) => string.Concat(p, Environment.NewLine, n));
                        sw.WriteLine(tabularizedLabels);
                        sw.Flush();
                    }
                    using (StreamWriter sw = File.CreateText(cntkBboxesPath))
                    {
                        string tabularizedLabels =
                            labels.Select(l => string.Concat(
                                    l.Item2, '\t',
                                    l.Item3, '\t',
                                    l.Item2 + 255, '\t',
                                    l.Item3 + 255))
                                .Aggregate((p, n) => string.Concat(p, Environment.NewLine, n));
                        sw.WriteLine(tabularizedLabels);
                        sw.Flush();
                    }
                }

                using (StreamWriter sw = File.CreateText(labelsPath))
                {
                    string tabularizedLabels =
                        labels.Select(l => string.Concat(
                                l.Item1, '\t',
                                l.Item2, '\t',
                                l.Item3, '\t',
                                l.Item2 + 255, '\t',
                                l.Item3 + 255))
                            .Aggregate((p, n) => string.Concat(p, Environment.NewLine, n));
                    sw.WriteLine(tabularizedLabels);
                    sw.Flush();
                }

                bmp.Save(imagePath, ImageFormat.Jpeg);
            }
        }

        private static IEnumerable<Drawing> ParseDrawingsFromFile(string dataFile)
        {
            using (StreamReader sr = File.OpenText(dataFile))
            {
                while (!sr.EndOfStream)
                {
                    string dataLine = sr.ReadLine();
                    yield return JsonConvert.DeserializeObject<Drawing>(dataLine);
                }
            }
        }

        private static async Task<string> DownloadFile(string url)
        {
            string fileName = Path.GetTempFileName();
            using (HttpClient client = new HttpClient())
            using (HttpResponseMessage result = await client.GetAsync(url))
            using (FileStream fileStream = File.Create(fileName))
            {
                if (result.IsSuccessStatusCode)
                {
                    await result.Content.CopyToAsync(fileStream);
                }

                await fileStream.FlushAsync();
            }

            return fileName;
        }

        private static void GenerateImage(Bitmap bmp, int xPosition, int yPosition, IEnumerable<int[][]> drawingLines)
        {
            foreach (int[][] drawingLine in drawingLines)
            {
                for (int i = 0; i < drawingLine[0].Length - 1; i++)
                {
                    BresenhamGraphicsHelper.PlotLine(
                        bmp,
                        xPosition + drawingLine[0][i],
                        yPosition + drawingLine[1][i],
                        xPosition + drawingLine[0][i + 1],
                        yPosition + drawingLine[1][i + 1],
                        Color.White);
                }
            }
        }
    }
}