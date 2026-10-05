using System.Collections.Generic;

namespace Homework_3.Models;

public class Album
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    
    public ICollection<Song> Songs { get; set; } = new List<Song>();
}