namespace ReviewService.Dtos
{
    public class ReviewGetDto
    {
        public Guid Id { get; set; }
        public Guid MovieId { get; set; }
        public Guid UserId { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    }
}
