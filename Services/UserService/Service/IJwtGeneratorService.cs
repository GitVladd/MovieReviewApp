using UserService.Models;

namespace UserService.Service
{
    public interface IJwtGeneratorService
    {
        Task<string> GenerateJwtTokenAsync(User user, IList<string> userRoles);
    }
}
