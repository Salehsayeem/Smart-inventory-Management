namespace Sims.Api.Dto
{
    public class CommonDdlDto
    {
        public long Value { get; set; }
        public string Label { get; set; } = null!;
        public object? AdditionalData { get; set; }
    }
}
