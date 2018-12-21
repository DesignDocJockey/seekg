using System;
using System.Collections.Generic;
using System.Text;

namespace SectionNormalization.src
{
    public class ManifestRecord
    {
        public int SectionId { get; set; }
        public string SectionName { get; set; }
        public int? RowId { get; set; }
        public string RowName { get; set; }
        public bool IsSuite { get; set; } = false;
    }
}
