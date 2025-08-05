using System.Text.Json.Serialization;

namespace EmployeePortal.DTO
{
    public class LoginDto
    {
        [JsonPropertyName("username")]
        public string Username { get; set; }

        [JsonPropertyName("password")]
        public string Password { get; set; }

    }
}
