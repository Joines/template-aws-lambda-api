using System.Text.Json.Serialization;

namespace Template.Aws.Lambda.Domain.Results.Bases
{
    public class Result<TData>
    {
        public TData Data { get; set; }
        public Result() { }
        public Result(TData data) =>
            Data = data;
    }
}
