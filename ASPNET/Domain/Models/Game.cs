namespace ASPNET.Domain.Models
{
    public class GameInfo
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }
        public int ReleaseYear { get; set; }
        public string Genre { get; set; }
        public string? Poster { get; set; }
        public GameInfo()
        {

        }
        public GameInfo(Guid id, string name, string author, int year, string genre, string? poster)
        {
            Id = id;
            Name = name;
            Author = author;
            ReleaseYear = year;
            Genre = genre;
            Poster = poster;
        }
    }
}
