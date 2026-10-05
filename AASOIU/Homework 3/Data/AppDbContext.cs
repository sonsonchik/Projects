using Microsoft.EntityFrameworkCore;
using Homework_3.Models;

namespace Homework_3.Data;

public class AppDbContext : DbContext
{
    public DbSet<Album> Albums { get; set; }
    public DbSet<Song> Songs { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite("Data Source=app.db");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Album>().HasData(
            new Album { Id = 1, Name = "The Dark Side of the Moon" },
            new Album { Id = 2, Name = "Abbey Road" },
            new Album { Id = 3, Name = "Thriller" },
            new Album { Id = 4, Name = "Back in Black" }
        );

        modelBuilder.Entity<Song>().HasData(
            new Song { Id = 1, AlbumId = 1, Name = "Speak to Me", DurationSec = 90 },
            new Song { Id = 2, AlbumId = 1, Name = "Breathe", DurationSec = 163 },
            new Song { Id = 3, AlbumId = 1, Name = "On the Run", DurationSec = 216 },
            new Song { Id = 4, AlbumId = 1, Name = "Time", DurationSec = 421 },
            new Song { Id = 5, AlbumId = 1, Name = "The Great Gig in the Sky", DurationSec = 276 },
            new Song { Id = 6, AlbumId = 2, Name = "Come Together", DurationSec = 259 },
            new Song { Id = 7, AlbumId = 2, Name = "Something", DurationSec = 182 },
            new Song { Id = 8, AlbumId = 2, Name = "Here Comes the Sun", DurationSec = 185 },
            new Song { Id = 9, AlbumId = 3, Name = "Billie Jean", DurationSec = 294 },
            new Song { Id = 10, AlbumId = 3, Name = "Beat It", DurationSec = 258 },
            new Song { Id = 11, AlbumId = 3, Name = "Thriller", DurationSec = 357 },
            new Song { Id = 12, AlbumId = 4, Name = "Hells Bells", DurationSec = 312 },
            new Song { Id = 13, AlbumId = 4, Name = "Back in Black", DurationSec = 255 },
            new Song { Id = 14, AlbumId = 4, Name = "You Shook Me All Night Long", DurationSec = 210 }
        );
    }
}