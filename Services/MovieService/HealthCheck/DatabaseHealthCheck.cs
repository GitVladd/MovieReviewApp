using MovieService.Data;

namespace MovieService.HealthCheck
{
    public class DatabaseHealthCheck : IDatabaseHealthCheck
    {
        private readonly ApplicationDbContext _context;

        public DatabaseHealthCheck(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CheckConnectionAsync()
        {
            try
            {
                await _context.Database.CanConnectAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
