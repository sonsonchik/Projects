using System;
using System.IO;
using System.Text;

class Program
{
    static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.InputEncoding = Encoding.UTF8;

        string dbPath = "music.db";
        string albumsCsv = Path.Combine(AppContext.BaseDirectory, "albums.csv");
        string songsCsv = Path.Combine(AppContext.BaseDirectory, "songs.csv");

        var db = new DatabaseManager(dbPath);
        db.InitializeDatabase(albumsCsv, songsCsv);

        string choice;
        do
        {
            Console.WriteLine("\n=== УПРАВЛЕНИЕ ПЕСНЯМИ ===");
            Console.WriteLine("1 — Показать все альбомы");
            Console.WriteLine("2 — Показать все песни");
            Console.WriteLine("3 — Добавить песню");
            Console.WriteLine("4 — Редактировать песню");
            Console.WriteLine("5 — Удалить песню");
            Console.WriteLine("6 — Отчёты");
            Console.WriteLine("0 — Выход");
            Console.Write("Ваш выбор: ");

            choice = Console.ReadLine()?.Trim() ?? "";

            switch (choice)
            {
                case "1": ShowAlbums(db); break;
                case "2": ShowSongs(db); break;
                case "3": AddSong(db); break;
                case "4": EditSong(db); break;
                case "5": DeleteSong(db); break;
                case "6": ReportsMenu(db); break;
                case "0": Console.WriteLine("До свидания!"); break;
                default: Console.WriteLine("Неверный пункт меню."); break;
            }
        } while (choice != "0");
    }

    static void ShowAlbums(DatabaseManager db)
    {
        Console.WriteLine("\n--- Все альбомы ---");
        foreach (var a in db.GetAllAlbums())
            Console.WriteLine(" " + a);
    }

    static void ShowSongs(DatabaseManager db)
    {
        Console.WriteLine("\n--- Все песни ---");
        foreach (var s in db.GetAllSongs())
            Console.WriteLine(" " + s);
    }

    static void AddSong(DatabaseManager db)
    {
        Console.WriteLine("\n--- Добавление песни ---");

        Console.WriteLine("Доступные альбомы:");
        foreach (var a in db.GetAllAlbums())
            Console.WriteLine(" " + a);

        Console.Write("ID альбома: ");
        if (!int.TryParse(Console.ReadLine(), out int albumId)) { Console.WriteLine("Ошибка ввода"); return; }

        Console.Write("Название песни: ");
        string name = Console.ReadLine()?.Trim() ?? "";
        if (name == "") { Console.WriteLine("Ошибка: имя не может быть пустым"); return; }

        Console.Write("Длительность (сек): ");
        if (!int.TryParse(Console.ReadLine(), out int duration)) { Console.WriteLine("Ошибка ввода"); return; }

        try
        {
            db.AddSong(new Song(0, albumId, name, duration));
            Console.WriteLine("Песня добавлена.");
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }
    }

    static void EditSong(DatabaseManager db)
    {
        Console.WriteLine("\n--- Редактирование песни ---");
        Console.Write("Введите ID песни: ");
        if (!int.TryParse(Console.ReadLine(), out int id)) { Console.WriteLine("Ошибка ввода"); return; }

        var song = db.GetSongById(id);
        if (song == null) { Console.WriteLine("Песня не найдена"); return; }

        Console.WriteLine($"Текущие данные: {song}");
        Console.WriteLine("(Нажмите Enter, чтобы оставить без изменений)");

        Console.Write($"Название [{song.Name}]: ");
        string input = Console.ReadLine()?.Trim() ?? "";
        if (input != "") song.Name = input;

        Console.Write($"ID альбома [{song.AlbumId}]: ");
        input = Console.ReadLine()?.Trim() ?? "";
        if (input != "" && int.TryParse(input, out int newAlbumId)) song.AlbumId = newAlbumId;

        Console.Write($"Длительность [{song.DurationSec}]: ");
        input = Console.ReadLine()?.Trim() ?? "";
        if (input != "" && int.TryParse(input, out int newDuration))
        {
            try { song.DurationSec = newDuration; }
            catch (ArgumentException ex) { Console.WriteLine($"Ошибка: {ex.Message}"); return; }
        }

        db.UpdateSong(song);
        Console.WriteLine("Данные обновлены.");
    }

    static void DeleteSong(DatabaseManager db)
    {
        Console.WriteLine("\n--- Удаление песни ---");
        Console.Write("Введите ID песни: ");
        if (!int.TryParse(Console.ReadLine(), out int id)) { Console.WriteLine("Ошибка ввода"); return; }

        var song = db.GetSongById(id);
        if (song == null) { Console.WriteLine("Песня не найдена"); return; }

        Console.Write($"Удалить «{song.Name}»? (да/нет): ");
        if (Console.ReadLine()?.Trim().ToLower() == "да")
        {
            db.DeleteSong(id);
            Console.WriteLine("Песня удалена.");
        }
    }

    static void ReportsMenu(DatabaseManager db)
    {
        string choice;
        do
        {
            Console.WriteLine("\n--- Отчёты ---");
            Console.WriteLine("1 — Все песни с названиями альбомов");
            Console.WriteLine("2 — Количество песен по альбомам");
            Console.WriteLine("3 — Средняя длительность песен по альбомам");
            Console.WriteLine("0 — Назад");
            Console.Write("Ваш выбор: ");

            choice = Console.ReadLine()?.Trim() ?? "";

            switch (choice)
            {
                case "1": Report1_SongsWithAlbums(db); break;
                case "2": Report2_CountByAlbum(db); break;
                case "3": Report3_AvgDurationByAlbum(db); break;
            }
        } while (choice != "0");
    }

    static void Report1_SongsWithAlbums(DatabaseManager db)
    {
        new ReportBuilder(db)
            .Query(@"
                SELECT s.song_name, a.album_name, s.duration_sec
                FROM songs s
                JOIN albums a ON s.album_id = a.album_id
                ORDER BY s.song_name
            ")
            .Title("Песни по альбомам")
            .Header("Название песни", "Альбом", "Длительность (сек)")
            .ColumnWidths(30, 40, 15)
            .Print();
    }


    static void Report2_CountByAlbum(DatabaseManager db)
    {
        new ReportBuilder(db)
            .Query(@"
                SELECT a.album_name, COUNT(*) AS count
                FROM songs s
                JOIN albums a ON s.album_id = a.album_id
                GROUP BY a.album_name
                ORDER BY a.album_name
            ")
            .Title("Количество песен по альбомам")
            .Header("Альбом", "Кол-во песен")
            .ColumnWidths(30, 15)
            .Print();
    }

    static void Report3_AvgDurationByAlbum(DatabaseManager db)
    {
        new ReportBuilder(db)
            .Query(@"
                SELECT a.album_name, ROUND(AVG(s.duration_sec), 1) AS avg_duration
                FROM songs s
                JOIN albums a ON s.album_id = a.album_id
                GROUP BY a.album_name
                ORDER BY avg_duration DESC
            ")
            .Title("Средняя длительность песен по альбомам")
            .Header("Альбом", "Средняя длительность (сек)")
            .ColumnWidths(30, 20)
            .Print();
    }
}