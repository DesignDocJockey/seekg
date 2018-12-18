using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;

namespace SectionNormalization.src
{
    public static class ManifestParser
    {
        public static IEnumerable<RawManifestRecord> ParseManifestFile(string manifestFile)
        {
            if (!File.Exists(manifestFile))
                throw new FileNotFoundException($"File: {manifestFile} was not found.");

            var rawFileContents = File.ReadAllLines(manifestFile)
                                        .ToList()
                                        .Select(i => ParseRawManifestRecord(i));
            return rawFileContents;
        }

        private static RawManifestRecord ParseRawManifestRecord(string rawRecord) {
            var lineItem = rawRecord.Split(',');
            //TODO::continue here
        }
    }
}
