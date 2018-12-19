namespace SectionNormalization
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using CommandLine;

    public class Program {
        public static void Main(string[] args)
        {
            var options = new CommandLineOptions();
            CommandLine.Parser.Default.ParseArguments(args, options);

            //TODO::remove later
            options.Manifest = @"/Users/hwong/dev/seekg/citifield_sections.csv";
            //@C:\Dev\sectionnorm-SGO\sectionnorm-SGO\csharp\citifield_sections.csv;

            var manifest = options.Manifest;
            var normalizer = new Normalizer(manifest);
            normalizer.readManifest();

            //TODO::remove later
            options.Input = @"/Users/hwong/dev/seekg/samples/metstest.csv";
            // @"C:\Dev\sectionnorm-SGO\sectionnorm-SGO\csharp\samples\metstest.csv";


            var input = options.Input;
            var section = options.Section;
            var row = options.Row;

            if (!string.IsNullOrEmpty(input))
            {
                var samples = readCSVInput(input);
                normalizer.Normalize(samples);
                outputSamples(samples);
            }
            else if (section != null && row != null)
            {
                var normalized = normalizer.Normalize(section, row);
                Console.WriteLine($"Input:\n    [section] {section}\t[row] {row}\nOutput:\n    [section_id] {normalized.sectionId}\t[row_id] {normalized.rowId}\nValid?:\n    {normalized.valid}");
            }
        }

        public static void outputSamples(IEnumerable<SampleRecord> samples) {
            foreach (var sample in samples) {
                Console.WriteLine(sample.ToJson());
            }
        }

        public static IEnumerable<SampleRecord> readCSVInput(string filename)
            => File.ReadAllLines(filename).Skip(1).Select(line => {
                    var split = line.Split(',');
                    var record = new SampleRecord
                    {
                        input = new SampleInput
                        {
                            section = split[0],
                            row = split[1]
                        },
                        expected = new SampleExpected
                        {
                            sectionId = int.TryParse(split[2], out var section) ? section : -1,
                            rowId = int.TryParse(split[3], out var row) ? row : -1,
                            valid = bool.Parse(split[4])
                        }
                    };
                    return record;
                });
    }

    class CommandLineOptions
    {
        [Option()]
        public string Manifest { get; set; }

        [Option()]
        public string Input { get; set; }

        [Option()]
        public string Section { get; set; }

        [Option()]
        public string Row { get; set; }
    }
}
