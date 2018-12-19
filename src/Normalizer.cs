namespace SectionNormalization
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using SectionNormalization.src;

    public class Normalizer
    {
        private readonly string manifestPath;
        private List<ManifestRecord> _ManifestRecords;
        private Dictionary<int, string> _StadiumSectionIdsToSectionNameMapping;

        public Normalizer(string manifestPath)
        {
            this.manifestPath = manifestPath;
        }

        /**
        * reads a manifest file
        * manifest should be a CSV containing the following columns
        * * section_id
        * * section_name
        * * row_id
        * * row_name
        */
        public void readManifest() {
            Console.WriteLine("Reading from " + manifestPath);
            // TODO your code goes here
            _ManifestRecords = ManifestParser.ParseManifestFile(manifestPath).ToList<ManifestRecord>();

            //TODO::populate a dictionary 
            //_StadiumSectionIds = _ManifestRecords
            //                        .Select(i => i.SectionId)
            //                        .Distinct()
            //                        .ToList();
        }

        private string LookUpSectionName(string sectionInput) {
            bool isNumericId = int.TryParse(sectionInput, out var sectionName);
            if (isNumericId)
                return sectionName.ToString();
            else {
                //check to see if the string contains any digits that we can extract out as the sectionId
                if (sectionInput.Any(c => char.IsDigit(c)))
                {
                    var id = new StringBuilder();
                    foreach (var chararacter in sectionInput.ToCharArray()) {
                        if (char.IsDigit(chararacter))
                            id.Append(chararacter);
                    }
                    sectionName = int.Parse(id.ToString());
                    return sectionName.ToString();
                }
            }

            return sectionInput;
        }

        private bool IsSectionASuite(ManifestRecord manifestRecord) =>
              (manifestRecord.SectionId > 0
                        && !string.IsNullOrEmpty(manifestRecord.SectionName)
                        && manifestRecord.RowId == null
                        && string.IsNullOrEmpty(manifestRecord.RowName));


        private List<ManifestRecord> GetManifestRecordsForSectionId(int sectionId) => _ManifestRecords.Where(i => i.SectionId == sectionId).ToList();

        private NormalizationResult InvalidateRecordWithNoMatch() => new NormalizationResult() { valid = false };

        private NormalizationResult InvalidateRecordWithMatchingSectionId(int sectionId) => new NormalizationResult() { valid = false, sectionId = sectionId };

        private NormalizationResult ValidMatchingRecord(int sectionId, int rowId = 0) => new NormalizationResult() { valid = true, sectionId = sectionId, rowId = rowId };

        /**
        * normalize a single (section, row) input
        * Given a (Section, Row) input, returns (section_id, row_id, valid)
        * where
        * section_id = int or None
        * row_id = int or None
        * valid = True or False
        * Arguments:
        * section {[type]} -- [description]
        * row {[type]} -- [description]
        */
        public NormalizationResult Normalize(string section, string row)
        {
            // initialize return data structure
            NormalizationResult r = null;

            //TODO::add your code here

            var sectionName = LookUpSectionName(section);

            //look for section id based off the provided section name
            var bFoundSectionName = _ManifestRecords.Any(i => i.SectionName.ToLower().Equals(sectionName));
            if (bFoundSectionName)
            {
                //locate the sectionId
                var sectionId = _ManifestRecords
                                        .Where(i => i.SectionName.ToLower().Equals(sectionName))
                                        .Select(j => j.SectionId)
                                        .FirstOrDefault();

                var manifestRecordsForSectionId = GetManifestRecordsForSectionId(sectionId);

                //if the row is empty or contains the word suite, check to see if it is a suite
                if (string.IsNullOrEmpty(row) || row.ToLower().Trim().Contains("suite"))
                {
                    foreach (var rec in manifestRecordsForSectionId)
                    {
                        if (IsSectionASuite(rec)) {
                            r = ValidMatchingRecord(sectionId);
                            break;
                        }
                    }
                }
                else
                {
                    //do the row analysis
                    var bHasRows = manifestRecordsForSectionId.Any(i => i.RowName.ToLower().Equals(row.Trim().ToLower()));
                    if (bHasRows)
                    {
                        var rowsInSectionId = manifestRecordsForSectionId
                                     .Where(k => k.RowName.ToLower().Equals(row.Trim().ToLower()));
                           
                        var rowId = rowsInSectionId.FirstOrDefault().RowId;
                        if (rowId.HasValue) {
                            r = ValidMatchingRecord(sectionId, rowId.Value);
                        }
                        else {
                            r = InvalidateRecordWithMatchingSectionId(sectionId);
                        }
                    } else {
                        r = InvalidateRecordWithMatchingSectionId(sectionId);
                    }
                }
            }
            else {
                r = InvalidateRecordWithNoMatch();
            }

            return r;
        }

        public void Normalize(IEnumerable<SampleRecord> samples) {
            foreach (var sample in samples)
            {
                var result = this.Normalize(sample.input.section, sample.input.row);
                sample.output.sectionId = result.sectionId;
                sample.output.rowId = result.rowId;
                sample.output.valid = result.valid;
            }
        }
    }
}
