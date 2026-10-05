using System;
using System.Windows.Forms;

namespace Homework_3;

public partial class MainForm : Form
{
    public MainForm()
    {
        InitializeComponent();
        SetupUI();
    }

    private void SetupUI()
    {
        this.Text = "Музыкальная библиотека";
        this.Size = new System.Drawing.Size(400, 350);
        this.StartPosition = FormStartPosition.CenterScreen;

        Label title = new Label
        {
            Text = "Управление музыкальной библиотекой",
            Font = new System.Drawing.Font("Segoe UI", 14),
            Location = new System.Drawing.Point(50, 30),
            Size = new System.Drawing.Size(300, 40),
            TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        };

        Button albumsBtn = new Button
        {
            Text = "🎵 Управление альбомами",
            Location = new System.Drawing.Point(100, 100),
            Size = new System.Drawing.Size(200, 50)
        };
        albumsBtn.Click += (s, e) => { new AlbumsForm().ShowDialog(); };

        Button songsBtn = new Button
        {
            Text = "🎶 Управление песнями",
            Location = new System.Drawing.Point(100, 170),
            Size = new System.Drawing.Size(200, 50)
        };
        songsBtn.Click += (s, e) => { new SongsForm().ShowDialog(); };

        Button reportBtn = new Button
        {
            Text = "📊 Отчёты",
            Location = new System.Drawing.Point(100, 240),
            Size = new System.Drawing.Size(200, 50)
        };
        reportBtn.Click += (s, e) => { new ReportForm().ShowDialog(); };

        this.Controls.Add(title);
        this.Controls.Add(albumsBtn);
        this.Controls.Add(songsBtn);
        this.Controls.Add(reportBtn);
    }

    private void InitializeComponent()
    {
        this.AutoScaleMode = AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(400, 350);
    }
}