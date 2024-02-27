namespace Translation.API.Models.Configurations
{
    public class JwtConfig
    {
        public required string SeceretKey { get; set; }
        public required string Issuer { get; set; }
        public required string Audience { get; set; }
    }
}
