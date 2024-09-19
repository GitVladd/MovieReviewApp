namespace MovieService.HealthCheck
{
    public interface IDatabaseHealthCheck
    {
        Task<bool> CheckConnectionAsync();
    }
}
