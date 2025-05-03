using System.Text.Json.Serialization;

namespace WebApp.Models;

public class CookieConsent
{
    [JsonPropertyName("essential")]
    public bool Essential { get; set; }

    [JsonPropertyName("preferences")]
    public bool Functional { get; set; }

    [JsonPropertyName("statistics")]
    public bool Analytics { get; set; }

    [JsonPropertyName("marketing")]
    public bool Marketing { get; set; }
}
