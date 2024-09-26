namespace MovieService.Dtos
{
    public class PaginationParameters
    {
        private const int MaxPageSize = 50;

        public int Page { get; set; } = 1;

        private int _size = 10;
        public int Size
        {
            get => _size;
            set => _size = (value > MaxPageSize) ? MaxPageSize : value;
        }
    }
}
