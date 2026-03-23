namespace ASPNET
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
        public override bool Equals(object? obj)
        {
            if (obj is not GameInfo gameInfo)
                return false;
            return this.Name == gameInfo.Name &&
                this.Author == gameInfo.Author &&
                this.Poster == gameInfo.Poster &&
                this.ReleaseYear == gameInfo.ReleaseYear &&
                this.Genre == gameInfo.Genre &&
                this.Id.Equals(gameInfo.Id);
        }
    }
}
