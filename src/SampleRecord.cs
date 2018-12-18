namespace SectionNormalization
{
    using Newtonsoft.Json;

    public sealed class SampleRecord
    {
        public SampleInput input = new SampleInput();

        public SampleExpected expected = new SampleExpected();

        public SampleOutput output = new SampleOutput();

        public string ToJson() => JsonConvert.SerializeObject(this);
    }

    public class SampleInput {

        public string section;

        public string row;
    }

    public class SampleExpected {

        [JsonProperty(PropertyName="section_id")]
        public int sectionId;

        [JsonProperty(PropertyName="row_id")]
        public int rowId;

        public bool valid;
    }

    public class SampleOutput {

        [JsonProperty(PropertyName="section_id")]
        public int sectionId = -1;

        [JsonProperty(PropertyName="row_id")]
        public int rowId = -1;

        public bool valid;
    }
}
