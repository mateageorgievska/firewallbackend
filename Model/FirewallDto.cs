namespace Models
{
    public class FirewallDto
    {
        public int? id { get; set; }
        public string? name { get; set; }
        public Dictionary<string, string>? labels { get; set; }
        public DateTime? created { get; set; }
    }
}
