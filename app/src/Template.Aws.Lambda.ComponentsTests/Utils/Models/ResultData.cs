using System.Text.Json.Serialization;

namespace Template.Aws.Lambda.ComponentsTests.Utils.Models
{
    public class ResultData<TResult> 
        where TResult : class
    {
        [JsonPropertyName("date")]
        public TResult Data { get; set; }
    }
}
