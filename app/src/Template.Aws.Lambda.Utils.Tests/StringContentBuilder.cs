using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Mime;
using System.Text;

namespace Template.Aws.Lambda.Utils.Tests
{
    public class StringContentBuilder
    {
        private StringContentBuilder() { }

        public static StringContent Build(object content = null,
            JsonSerializerSettings jsonSettings = null) =>
            new StringContent(
                JsonConvert.SerializeObject(content, jsonSettings),
                Encoding.UTF8,
                MediaTypeNames.Application.Json);
    }
}
