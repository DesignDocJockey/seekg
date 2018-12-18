using System;
using System.Collections.Generic;
using System.Text;

namespace SectionNormalization.src
{
    public class ManifestRecord
    {
        public int? SectionId { get; set; }
        public string Section { get; set; }
        public int? RowId { get; set; }
        public string RowName { get; set; }
        public bool Valid { get; set; }

        //public bool IsValid() => (!string.IsNullOrEmpty(this.Section)) &&
                                 //(!string.IsNullOrEmpty(this.RowName));
             //&&
                                 //(this.SectionId.HasValue) && 
                                 //(this.RowId.HasValue);
       
    }
}
