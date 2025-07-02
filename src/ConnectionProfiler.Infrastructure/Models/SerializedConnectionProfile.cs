namespace ConnectionProfiler.Infrastructure.Models
{
    public class SerializedConnectionProfile
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Environment { get; set; }
        public required string Type { get; set; }
        public required string PayloadJson { get; set; }
    }
}
