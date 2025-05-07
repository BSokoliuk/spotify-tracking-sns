namespace Settings;

public class CookieSettings
{
    public string Name { get; set; } = "X-Access-Token";
    public bool HttpOnly { get; set; } = true;
    public string SameSite { get; set; } = "Strict";
    public int ExpiresInDays { get; set; } = 7;
}