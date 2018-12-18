using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;

namespace SectionNormalization.src
{
    public static class ManifestParser
    {
        public static IEnumerable<ManifestRecord> ParseManifestFile(string manifestFile)
        {
            if (!File.Exists(manifestFile))
                throw new FileNotFoundException($"File: {manifestFile} was not found.");

            var rawFileContents = File.ReadAllLines(manifestFile)
                                        .ToList()
                                        .Skip(1)
                                        .Select(i => ParseLineItemIntoManifestRecord(i));
                                        //.Where(rec => rec.IsValid());

            return rawFileContents;
        }

        private static ManifestRecord ParseLineItemIntoManifestRecord(string lineRecord) {
            //section,row,n_section_id,n_row_id,valid
            //311PL,G,160,6,True
            var lineItem = lineRecord.Split(',');
            var manifestRecord = new ManifestRecord();

            if (lineItem.Length != 5)
                return manifestRecord;

            //section name
            if (!string.IsNullOrEmpty(lineItem[0])) {
                manifestRecord.Section = lineItem[0].Trim();
            } else {
                manifestRecord.Section = string.Empty;
            }

            //row name
            if (!string.IsNullOrEmpty(lineItem[1])) {
                manifestRecord.RowName = lineItem[1].Trim();
            }
            else {
                manifestRecord.RowName = string.Empty;
            }

            //sectionId
            if (!string.IsNullOrEmpty(lineItem[2])) {
                if (Int32.TryParse(lineItem[2].Trim(), out var sectionId))
                    manifestRecord.SectionId = sectionId;
                else
                    manifestRecord.SectionId = null;
            }
            else
                manifestRecord.SectionId = null;

            //rowId
            if (!string.IsNullOrEmpty(lineItem[3])) 
            {
                if (Int32.TryParse(lineItem[3].Trim(), out var rowId)) {
                    manifestRecord.RowId = rowId;
                }
                else
                    manifestRecord.RowId = null;
            }
            else
                manifestRecord.RowId = null;

            //valid
            if (!string.IsNullOrEmpty(lineItem[4])) {
                if (Boolean.TryParse(lineItem[4].Trim(), out var validFlag))
                    manifestRecord.Valid = validFlag;
                else
                    manifestRecord.Valid = false;
            }
            else {
                manifestRecord.Valid = false;
            }

            return manifestRecord;
        }
    }
}
