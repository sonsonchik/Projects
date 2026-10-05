namespace Homework_3.Models;

public class Song
{
    public int Id { get; set; }
    public int AlbumId { get; set; }
    public string Name { get; set; } = "";
    public int DurationSec { get; set; }
    
    public Album? Album { get; set; }
}