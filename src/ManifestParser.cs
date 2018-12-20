using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;

namespace SectionNormalization.src
{
    public static class ManifestParser
    {
        public static List<ManifestRecord> ParseManifestFile(string manifestFile)
        {
            if (!File.Exists(manifestFile))
                throw new FileNotFoundException($"File: {manifestFile} was not found.");

            var rawFileContents = File.ReadAllLines(manifestFile)
                                        .ToList()
                                        .Skip(1)
                                        .Select(i => ParseLineItemIntoManifestRecord(i));
            return rawFileContents.ToList();
        }

        private static ManifestRecord ParseLineItemIntoManifestRecord(string lineRecord) {
            var lineItem = lineRecord.Split(',');
            var manifestRecord = new ManifestRecord();

            //sectionId
            if (!string.IsNullOrEmpty(lineItem[0]))
            {
                if (Int32.TryParse(lineItem[0].Trim(), out var sectionId))
                    manifestRecord.SectionId = sectionId;
            }
   
            //section name
            if (!string.IsNullOrEmpty(lineItem[1])) {
                manifestRecord.SectionName = lineItem[1].Trim();
            } else {
                manifestRecord.SectionName = string.Empty;
            }

            //rowId
            if (!string.IsNullOrEmpty(lineItem[2]))
            {
                if (Int32.TryParse(lineItem[2].Trim(), out var rowId))
                {
                    manifestRecord.RowId = rowId;
                }
                else
                    manifestRecord.RowId = null;
            }
            else
            {
                manifestRecord.RowId = null;
            }

            //row name
            if (!string.IsNullOrEmpty(lineItem[3])) {
                manifestRecord.RowName = lineItem[3].Trim();
            }
            else {
                manifestRecord.RowName = string.Empty;
            }

            if (manifestRecord.SectionId > 0 && (!manifestRecord.RowId.HasValue && string.IsNullOrEmpty(manifestRecord.RowName)))
                manifestRecord.IsSuite = true;

            return manifestRecord;
        }
    }
}
