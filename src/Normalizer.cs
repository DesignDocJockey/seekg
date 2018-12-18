namespace SectionNormalization
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using SectionNormalization.src;

    public class Normalizer
    {
        private readonly string manifestPath;
        private List<ManifestRecord> _ManifestRecords;

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
            // TODO your code goes here
            Console.WriteLine("Reading from " + manifestPath);
            _ManifestRecords = ManifestParser.ParseManifestFile(manifestPath).ToList<ManifestRecord>();
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

            // TODO your code goes here
            var bFound = _ManifestRecords.Any(i => i.SectionName.ToLower().Contains(section.ToLower())
                            && i.RowName.ToLower().Contains(row.ToLower()));
            if(bFound) 
            {
                var result = _ManifestRecords
                                    .Where(i => i.SectionName.ToLower().Contains(section.ToLower())
                                            && i.RowName.ToLower().Contains(row.ToLower()))
                                    .FirstOrDefault();
                r.rowId = result.RowId.Value;
                r.sectionId = result.SectionId.Value;
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
