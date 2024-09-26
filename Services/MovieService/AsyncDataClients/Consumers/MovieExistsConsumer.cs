using MassTransit;
using MovieService.Dtos;
using MovieService.Service;

namespace MovieService.AsyncDataClients.Conusmers
{
    public class MovieExistsConsumer : IConsumer<MovieExistsRequestDto>
    {
        private readonly IMovieService _movieService;

        public MovieExistsConsumer(IMovieService movieService)
        {
            _movieService = movieService;
        }

        public async Task Consume(ConsumeContext<MovieExistsRequestDto> context)
        {
            var movieId = context.Message.MovieId;
            bool exists = await _movieService.MovieExistsAsync(movieId);

            // Отправить ответ
            await context.RespondAsync(new MovieExistsResponseDto { bExists = exists });
        }
    }
}
