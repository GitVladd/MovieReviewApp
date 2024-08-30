using AutoMapper;
using MovieReviewApp.Common.Enums;
using MovieReviewApp.Common.Exceptions;
using MovieReviewApp.Common.Repository;
using ReviewService.Dtos;
using ReviewService.Models;
using System.Linq.Expressions;

namespace ReviewService.Service
{
    public class ReviewService : IReviewService
    {

        private readonly IBaseRepository<Review> _repository;
        private readonly IMapper _mapper;

        public ReviewService(IBaseRepository<Review> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
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

            var result = _mapper.Map<ReviewGetDto>(entity);
            return result;
        }

        public async Task<ReviewGetDto> CreateAsync(ReviewCreateDto createDto)
        {
            throw new NotImplementedException();
            //check if userid and movieid exists
            var entity = _mapper.Map<Review>(createDto);
            _repository.Create(entity);
            await _repository.SaveAsync();
            var result = _mapper.Map<ReviewGetDto>(entity);
            return result;
        }

        public async Task<ReviewGetDto> UpdateAsync(Guid id, ReviewUpdateDto updateDto)
        {
            throw new NotImplementedException();
            /*
            var entity = await _repository.GetByIdWithDetailsAsync(id);
            if (entity == null)
                return null;

            if (entity.ContentType.Id != updateDto.ContentTypeId)
            {
                var contentType = await _contentTypeService.GetByIdAsync(updateDto.ContentTypeId);
                if (contentType == null)
                {
                    throw new EntityNotFoundException($"Content type not found. id:{updateDto.ContentTypeId}");

                }
                entity.ContentType = _mapper.Map<ContentType>(contentType);
            }

            var categories = await _categoryService.GetAsync(c => updateDto.CategoryIds.Contains(c.Id));
            var missingCategoryIds = updateDto.CategoryIds
                .Where(categoryId => !categories.Select(c => c.Id).Contains(categoryId))
                .ToList();
            if (missingCategoryIds.Any())
            {
                throw new EntityNotFoundException($"Category not found. id:{string.Join(", id:", missingCategoryIds)}");

            }

            entity = _mapper.Map(updateDto, entity);

            _repository.Update(entity);
            await _repository.SaveAsync();

            var movieGetDto = _mapper.Map<ReviewGetDto>(entity);
            return movieGetDto;
            */
        }

        public async Task DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
            /*
            var entity = await _repository.GetByIdWithDetailsAsync(id);
            if (entity == null)
            {
                throw new EntityNotFoundException($"Movie type not found. id:{id}");
            }

            _repository.Delete(entity);
            await _repository.SaveAsync();
             */
        }
    }
}
