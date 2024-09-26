using AutoMapper;
using MovieService.Dtos.ContentTypeDto;
using MovieService.Enums;
using MovieService.Models;
using MovieService.Repository;
using System.Linq.Expressions;

namespace MovieService.Service
{
    public class ContentTypeService : IContentTypeService
    {
        private readonly IBaseRepository<ContentType> _repository;
        private readonly IMapper _mapper;

        public ContentTypeService(IBaseRepository<ContentType> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ContentTypeGetDto>> GetAsync(
            Expression<Func<ContentType, bool>> predicate = null,
            IEnumerable<Expression<Func<ContentType, object>>> include = null,
            int take = int.MaxValue, int skip = 0,
            IEnumerable<Expression<Func<ContentType, object>>> sortBy = null,
            SortDirection sortDirection = SortDirection.Ascending,
            CancellationToken cancellationToken = default)
        {
            var entities = await _repository.GetAsync(predicate, include, take, skip, sortBy, sortDirection, cancellationToken);
            var result = _mapper.Map<IEnumerable<ContentTypeGetDto>>(entities);
            return result;
        }

        public async Task<ContentTypeGetDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            Expression<Func<ContentType, bool>> predicate = m => m.Id == id;

            var entities = await _repository.GetAsync(predicate: predicate);

            var entity = entities.FirstOrDefault();

            if (entity == null)
            {
                return null;
            }

            var result = _mapper.Map<ContentTypeGetDto>(entity);
            return result;
        }

        public async Task<ContentTypeGetDto> CreateAsync(ContentTypeCreateDto createDto)
        {
            var entity = _mapper.Map<ContentType>(createDto);
            _repository.Create(entity);
            await _repository.SaveAsync();
            var result = _mapper.Map<ContentTypeGetDto>(entity);
            return result;
        }

    }
}