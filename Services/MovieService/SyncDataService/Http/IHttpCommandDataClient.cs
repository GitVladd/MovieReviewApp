using MovieService.Dtos.MovieDto;
using System;

/// <summary>
/// Summary description for Class1
/// </summary>
interface IHttpCommandDataClient
{
	Task SendMovieToReview(MovieGetDto dto);
}
