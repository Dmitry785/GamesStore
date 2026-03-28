using ASPNET.Domain.Models;

namespace ASPNET.Application.Logic.Games.Models
{
    public abstract class DataTransferObject;
    public class ShortGameInfo : DataTransferObject
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }
        public string Genre { get; set; }
        public string? Poster { get; set; }
    }
}
