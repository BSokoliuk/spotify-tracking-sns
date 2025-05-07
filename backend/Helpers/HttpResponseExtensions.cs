using Settings;

namespace Helpers;

public static class HttpResponseExtensions
{
  public static void AppendJwtCookie(this HttpResponse response, string token, CookieSettings opts)
  {
    // Parse SameSite
    var sameSite = opts.SameSite switch
    {
      "None" => SameSiteMode.None,
      "Lax"  => SameSiteMode.Lax,
      _      => SameSiteMode.Strict
    };

    response.Cookies.Append(
      opts.Name,
      token,
      new CookieOptions
      {
        HttpOnly    = opts.HttpOnly,
        SameSite    = sameSite,
        Expires     = DateTimeOffset.UtcNow.AddDays(opts.ExpiresInDays),
        IsEssential = true
      }
    );
  }

  public static void DeleteJwtCookie(this HttpResponse response, CookieSettings opts)
  {
    response.Cookies.Delete(opts.Name);
  }
}