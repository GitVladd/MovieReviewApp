using MassTransit;
using ReviewService.Dtos;

namespace ReviewService.AsyncDataClients
{
    public class MovieRequestClient
    {
        private readonly IRequestClient<MovieExistsRequestDto> _client;

        public MovieRequestClient(IRequestClient<MovieExistsRequestDto> client)
        {
            _client = client;
        }

        public async Task<bool> MovieExistsAsync(Guid movieId)
        {
            var response = await _client.GetResponse<MovieExistsResponseDto>(new MovieExistsRequestDto { MovieId = movieId });
            return response.Message.bExists;
        }
    }
}
