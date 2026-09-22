namespace TechStore.Model.DTOs.Authentication
{
    public class RefreshTokenRotationResult
    {
        public required string AccessToken { get; set; }
        public required string RefreshToken { get; set; }
        public required DateTimeOffset AccessTokenExpiresAt { get; set; }
        public required DateTimeOffset RefreshTokenExpiresAt { get; set; }
    }
}
