using System;
using System.Linq;
using System.Windows.Forms;
using Microsoft.EntityFrameworkCore;
using Homework_3.Data;
using Homework_3.Models;

namespace Homework_3;

public partial class SongsForm : Form
{
    private DataGridView dataGridView;
    private ComboBox albumCombo;
    private TextBox nameBox, durationBox;
    private Button addBtn, editBtn, deleteBtn;
    private int? editingId = null;

    public SongsForm()
    {
        InitializeComponent();
        SetupUI();
        LoadData();
    }

    private void SetupUI()
    {
        this.Text = "Управление песнями";
        this.Size = new System.Drawing.Size(800, 600);
        this.StartPosition = FormStartPosition.CenterScreen;

        dataGridView = new DataGridView
        {
            Location = new System.Drawing.Point(20, 20),
            Size = new System.Drawing.Size(740, 300),
            ReadOnly = true,
            SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            MultiSelect = false,
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        };

        int y = 340;
        Label albumLabel = new Label { Text = "Альбом:", Location = new System.Drawing.Point(20, y), Size = new System.Drawing.Size(100, 30) };
        albumCombo = new ComboBox { Location = new System.Drawing.Point(130, y), Size = new System.Drawing.Size(200, 30), DropDownStyle = ComboBoxStyle.DropDownList };

        Label nameLabel = new Label { Text = "Название:", Location = new System.Drawing.Point(20, y + 40), Size = new System.Drawing.Size(100, 30) };
        nameBox = new TextBox { Location = new System.Drawing.Point(130, y + 40), Size = new System.Drawing.Size(200, 30) };

        Label durationLabel = new Label { Text = "Длительность (сек):", Location = new System.Drawing.Point(20, y + 80), Size = new System.Drawing.Size(120, 30) };
        durationBox = new TextBox { Location = new System.Drawing.Point(150, y + 80), Size = new System.Drawing.Size(180, 30) };

        addBtn = new Button { Text = "Добавить", Location = new System.Drawing.Point(370, y), Size = new System.Drawing.Size(100, 35) };
        addBtn.Click += AddBtn_Click;

        editBtn = new Button { Text = "Редактировать", Location = new System.Drawing.Point(370, y + 45), Size = new System.Drawing.Size(100, 35) };
        editBtn.Click += EditBtn_Click;

        deleteBtn = new Button { Text = "Удалить", Location = new System.Drawing.Point(370, y + 90), Size = new System.Drawing.Size(100, 35) };
        deleteBtn.Click += DeleteBtn_Click;

        this.Controls.Add(dataGridView);
        this.Controls.Add(albumLabel);
        this.Controls.Add(albumCombo);
        this.Controls.Add(nameLabel);
        this.Controls.Add(nameBox);
        this.Controls.Add(durationLabel);
        this.Controls.Add(durationBox);
        this.Controls.Add(addBtn);
        this.Controls.Add(editBtn);
        this.Controls.Add(deleteBtn);
    }

    private void LoadAlbums()
    {
        using var context = new AppDbContext();
        var albums = context.Albums.OrderBy(a => a.Name).ToList();
        albumCombo.DataSource = albums;
        albumCombo.DisplayMember = "Name";
        albumCombo.ValueMember = "Id";
    }

    private void LoadData()
    {
        using var context = new AppDbContext();
        context.Database.EnsureCreated();
        LoadAlbums();
        
        var songs = context.Songs.Include(s => s.Album).OrderBy(s => s.Name).ToList();
        dataGridView.DataSource = songs.Select(s => new { s.Id, Альбом = s.Album?.Name ?? "", Название = s.Name, Длительность = s.DurationSec }).ToList();
        
        if (dataGridView.Columns.Contains("Id"))
            dataGridView.Columns["Id"].Visible = false;
    }

    private void ClearForm()
    {
        editingId = null;
        albumCombo.SelectedIndex = -1;
        nameBox.Clear();
        durationBox.Clear();
        addBtn.Text = "Добавить";
    }

    private void AddBtn_Click(object? sender, EventArgs e)
    {
        if (albumCombo.SelectedItem == null || string.IsNullOrWhiteSpace(nameBox.Text))
        {
            MessageBox.Show("Заполните все поля!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (!int.TryParse(durationBox.Text, out int duration) || duration < 0)
        {
            MessageBox.Show("Длительность должна быть целым неотрицательным числом!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        using var context = new AppDbContext();
        var song = new Song
        {
            AlbumId = (int)albumCombo.SelectedValue,
            Name = nameBox.Text,
            DurationSec = duration
        };
        context.Songs.Add(song);
        context.SaveChanges();
        ClearForm();
        LoadData();
        MessageBox.Show("Песня добавлена!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    private void EditBtn_Click(object? sender, EventArgs e)
    {
        if (dataGridView.SelectedRows.Count == 0)
        {
            MessageBox.Show("Выберите песню для редактирования!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        int id = (int)dataGridView.SelectedRows[0].Cells["Id"].Value;
        using var context = new AppDbContext();
        var song = context.Songs.Find(id);
        if (song == null) return;

        editingId = id;
        albumCombo.SelectedValue = song.AlbumId;
        nameBox.Text = song.Name;
        durationBox.Text = song.DurationSec.ToString();
        addBtn.Text = "Сохранить";
        
        addBtn.Click -= AddBtn_Click;
        addBtn.Click += SaveEdit_Click;
    }

    private void SaveEdit_Click(object? sender, EventArgs e)
    {
        if (!editingId.HasValue) return;

        if (albumCombo.SelectedItem == null || string.IsNullOrWhiteSpace(nameBox.Text))
        {
            MessageBox.Show("Заполните все поля!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (!int.TryParse(durationBox.Text, out int duration) || duration < 0)
        {
            MessageBox.Show("Длительность должна быть целым неотрицательным числом!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        using var context = new AppDbContext();
        var song = context.Songs.Find(editingId.Value);
        if (song == null) return;

        song.AlbumId = (int)albumCombo.SelectedValue;
        song.Name = nameBox.Text;
        song.DurationSec = duration;
        context.SaveChanges();
        
        ClearForm();
        addBtn.Click -= SaveEdit_Click;
        addBtn.Click += AddBtn_Click;
        LoadData();
        MessageBox.Show("Песня обновлена!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    private void DeleteBtn_Click(object? sender, EventArgs e)
    {
        if (dataGridView.SelectedRows.Count == 0)
        {
            MessageBox.Show("Выберите песню для удаления!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        int id = (int)dataGridView.SelectedRows[0].Cells["Id"].Value;
        string name = dataGridView.SelectedRows[0].Cells["Название"].Value?.ToString() ?? "";

        if (MessageBox.Show($"Удалить песню \"{name}\"?", "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
        {
            using var context = new AppDbContext();
            var song = context.Songs.Find(id);
            if (song != null)
            {
                context.Songs.Remove(song);
                context.SaveChanges();
                LoadData();
                MessageBox.Show("Песня удалена!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }

    private void InitializeComponent()
    {
        this.AutoScaleMode = AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(800, 550);
    }
}