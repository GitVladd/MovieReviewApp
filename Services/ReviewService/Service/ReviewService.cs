using AutoMapper;
using ReviewService.AsyncDataClients;
using ReviewService.Dtos;
using ReviewService.Enums;
using ReviewService.Exceptions;
using ReviewService.Models;
using ReviewService.Repository;
using System.Linq.Expressions;

namespace ReviewService.Service
{
    public class ReviewService : IReviewService
    {
        private readonly IBaseRepository<Review> _repository;
        private readonly IMapper _mapper;
        private readonly MovieRequestClient _client;

        public ReviewService(IBaseRepository<Review> repository,
                             IMapper mapper,
                             MovieRequestClient client)
        {
            _repository = repository;
            _mapper = mapper;
            _client = client;
        }

        public async Task<IEnumerable<ReviewGetDto>> GetAsync(
            Expression<Func<Review, bool>> predicate = null,
            IEnumerable<Expression<Func<Review, object>>> include = null,
            int take = int.MaxValue, int skip = 0,
            IEnumerable<Expression<Func<Review, object>>> sortBy = null,
            SortDirection sortDirection = SortDirection.Ascending,
            CancellationToken cancellationToken = default)
        {
            var entities = await _repository.GetAsync(predicate, include, take, skip, sortBy, sortDirection, cancellationToken);

            if (!entities.Any()) return null;

            var result = _mapper.Map<IEnumerable<ReviewGetDto>>(entities);
            return result;
        }

        public async Task<ReviewGetDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            Expression<Func<Review, bool>> predicate = m => m.Id == id;

            var entities = await _repository.GetAsync(predicate: predicate);

            var entity = entities.FirstOrDefault();

            if (entity == null)
            {
                return null;
            }

            return _mapper.Map<ReviewGetDto>(entity);
        }

        public async Task<List<ReviewGetDto>> GetReviewsByMovieIdAsync(Guid movieId)
        {
            var entities = await _repository.GetAsync(predicate: r => r.MovieId == movieId);

            if (entities == null || !entities.Any())
            {
                return null;
            }

            return _mapper.Map<List<ReviewGetDto>>(entities);
        }
        public async Task<ReviewGetDto> CreateAsync(ReviewCreateDto createDto, Guid UserId)
        {
            //check if userid and movieid exists
            bool bExist = await _client.MovieExistsAsync(createDto.MovieId);

            if (!bExist) throw new EntityNotFoundException($"Movie not found. id:{createDto.MovieId}");
            var entity = _mapper.Map<Review>(createDto);
            _repository.Create(entity);
            await _repository.SaveAsync();
            var result = _mapper.Map<ReviewGetDto>(entity);
            return result;
        }

        public async Task<ReviewGetDto> UpdateAsync(Guid reviewId, ReviewUpdateDto updateDto, Guid UserId)
        {
            var entities = await _repository.GetAsync(m => reviewId == m.Id);
            if (entities == null || entities.Count == 0)
                return null;

            var entity = _mapper.Map(updateDto, entities.First());

            if (entity.UserId != UserId) throw new UnauthorizedAccessException("You are not authorized to update this review.");

            _repository.Update(entity);
            await _repository.SaveAsync();

            var movieGetDto = _mapper.Map<ReviewGetDto>(entity);
            return movieGetDto;
        }

        public async Task DeleteAsync(Guid reviewId, Guid UserId)
        {
            var entities = await _repository.GetAsync(m => reviewId == m.Id);
            if (entities == null || entities.Count == 0)
                throw new EntityNotFoundException($"Review not found. id:{reviewId}");

            var entity = entities.First();

            if (entity.UserId != UserId) throw new UnauthorizedAccessException("You are not authorized to delete this review.");

            _repository.Delete(entity);
            await _repository.SaveAsync();
        }
    }
}
