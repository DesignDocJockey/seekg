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
            NormalizationResult r = new NormalizationResult();
            //r.sectionId = 112312;
            //r.rowId = 112;
            //r.valid = true;

            //TODO::add your code here

            var sectionName = LookUpSectionName(section);

            //look for section id based off the section name
            var bFoundSectionName = _ManifestRecords.Any(i => i.SectionName.ToLower().Equals(sectionName));
            if (bFoundSectionName)
            {
                var sectionId = _ManifestRecords
                                        .Where(i => i.SectionName.ToLower().Equals(sectionName))
                                        .Select(j => j.SectionId)
                                        .FirstOrDefault();

                //TODO::do the row analysis
                var rowInfo = _ManifestRecords
                                        .Where(i => i.SectionId == sectionId)
                                        .Select(j => new {
                                            j.RowId,
                                            j.RowName
                                        }).ToList();

                //if no rowInfo check if it is a suite?


                if (rowInfo.Any())
                {
      
                    var rowId = rowInfo.Where(k => k.RowName.ToLower().Equals(row.Trim().ToLower()))
                                       .Select(l => l.RowId)
                                       .FirstOrDefault();
                }
               

                //TODO::do some analysis on the provided row


                r.rowId = rowId.Value;
                r.sectionId = sectionId;
                r.valid = true;
            }
            else {
                r.valid = false;
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
