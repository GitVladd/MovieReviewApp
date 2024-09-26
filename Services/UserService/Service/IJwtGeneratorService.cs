using UserService.Models;

namespace UserService.Service
{
    public interface IJwtGeneratorService
    {
        string GenerateJwtToken(User user, IList<string> userRoles);
    }
}
