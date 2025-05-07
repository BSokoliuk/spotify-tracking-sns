using Models;

namespace Services.Interfaces;

public interface ITokenService
{
    string Generate(User user, List<string> roles);
}