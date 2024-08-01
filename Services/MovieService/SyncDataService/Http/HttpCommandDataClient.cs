using MovieService.Dtos.MovieDto;
using System;
using System.Text;
using System.Text.Json;

/// <summary>
/// Summary description for Class1
/// </summary>
public class HttpCommandDataClient : IHttpCommandDataClient
{
	private readonly HttpClient _httpClient;
	private readonly string _commandServiceURL;
	public HttpCommandDataClient(HttpClient httpClient)
	{
		_httpClient = httpClient;
	}

	public async Task SendMovieToReview(MovieGetDto dto)
	{
		try
		{
			var httpContent = new StringContent
			(
				JsonSerializer.Serialize(dto),
				Encoding.UTF8,
				"application/json"
			);

			var response = await _httpClient.PostAsync(_commandServiceURL, httpContent);
			if(response.IsSuccessStatusCode)
			{
				//ok
			}
			else
			{
				//not ok
			}
		}
		catch (Exception ex)
		{

		}
	}
}
