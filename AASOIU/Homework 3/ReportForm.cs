using System;
using System.Linq;
using System.Windows.Forms;
using Microsoft.EntityFrameworkCore;
using Homework_3.Data;

namespace Homework_3;

public partial class ReportForm : Form
{
    private DataGridView grid1, grid2, grid3;
    private TabControl tabControl;

    public ReportForm()
    {
        InitializeComponent();
        SetupUI();
        LoadReports();
    }

    private void SetupUI()
    {
        this.Text = "Отчёты";
        this.Size = new System.Drawing.Size(800, 600);
        this.StartPosition = FormStartPosition.CenterScreen;

        tabControl = new TabControl
        {
            Location = new System.Drawing.Point(10, 10),
            Size = new System.Drawing.Size(760, 540)
        };

        TabPage page1 = new TabPage("Все песни");
        TabPage page2 = new TabPage("Количество по альбомам");
        TabPage page3 = new TabPage("Средняя длительность");

        grid1 = new DataGridView { Dock = DockStyle.Fill, ReadOnly = true, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill };
        grid2 = new DataGridView { Dock = DockStyle.Fill, ReadOnly = true, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill };
        grid3 = new DataGridView { Dock = DockStyle.Fill, ReadOnly = true, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill };

        page1.Controls.Add(grid1);
        page2.Controls.Add(grid2);
        page3.Controls.Add(grid3);

        tabControl.TabPages.Add(page1);
        tabControl.TabPages.Add(page2);
        tabControl.TabPages.Add(page3);

        this.Controls.Add(tabControl);
    }

    private void LoadReports()
    {
        using var context = new AppDbContext();
        context.Database.EnsureCreated();

        var report1 = context.Songs
            .Include(s => s.Album)
            .OrderBy(s => s.Name)
            .Select(s => new { Песня = s.Name, Альбом = s.Album != null ? s.Album.Name : "", Длительность = s.DurationSec })
            .ToList();
        grid1.DataSource = report1;

        var report2 = context.Songs
            .GroupBy(s => s.Album != null ? s.Album.Name : "Без альбома")
            .Select(g => new { Альбом = g.Key, Количество = g.Count() })
            .OrderBy(r => r.Альбом)
            .ToList();
        grid2.DataSource = report2;

        var report3 = context.Songs
            .GroupBy(s => s.Album != null ? s.Album.Name : "Без альбома")
            .Select(g => new { Альбом = g.Key, СредняяДлительность = Math.Round(g.Average(s => s.DurationSec), 1) })
            .OrderByDescending(r => r.СредняяДлительность)
            .ToList();
        grid3.DataSource = report3;
    }

    private void InitializeComponent()
    {
        this.AutoScaleMode = AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(800, 600);
    }
}