using System;
using System.Collections.Generic;
using System.Text;

namespace SectionNormalization.src
{
    public class RawManifestRecord
    {
        public string Section { get; set; }
        public string Row { get; set; }
        public int SectionId { get; set; }
        public int RowId { get; set}
        public bool Valid { get; set; }
    }
}
