using System;
using System.Linq;
using System.Windows.Forms;
using Microsoft.EntityFrameworkCore;
using Homework_3.Data;
using Homework_3.Models;

namespace Homework_3;

public partial class AlbumsForm : Form
{
    private DataGridView dataGridView;
    private TextBox nameBox;
    private Button addBtn, editBtn, deleteBtn;

    public AlbumsForm()
    {
        InitializeComponent();
        SetupUI();
        LoadData();
    }

    private void SetupUI()
    {
        this.Text = "Управление альбомами";
        this.Size = new System.Drawing.Size(600, 500);
        this.StartPosition = FormStartPosition.CenterScreen;

        dataGridView = new DataGridView
        {
            Location = new System.Drawing.Point(20, 20),
            Size = new System.Drawing.Size(540, 250),
            ReadOnly = true,
            SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            MultiSelect = false,
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        };

        Label nameLabel = new Label
        {
            Text = "Название альбома:",
            Location = new System.Drawing.Point(20, 290),
            Size = new System.Drawing.Size(120, 30)
        };

        nameBox = new TextBox
        {
            Location = new System.Drawing.Point(150, 290),
            Size = new System.Drawing.Size(200, 30)
        };

        addBtn = new Button
        {
            Text = "Добавить",
            Location = new System.Drawing.Point(370, 285),
            Size = new System.Drawing.Size(90, 35)
        };
        addBtn.Click += AddBtn_Click;

        editBtn = new Button
        {
            Text = "Редактировать",
            Location = new System.Drawing.Point(470, 285),
            Size = new System.Drawing.Size(90, 35)
        };
        editBtn.Click += EditBtn_Click;

        deleteBtn = new Button
        {
            Text = "Удалить",
            Location = new System.Drawing.Point(370, 330),
            Size = new System.Drawing.Size(90, 35)
        };
        deleteBtn.Click += DeleteBtn_Click;

        this.Controls.Add(dataGridView);
        this.Controls.Add(nameLabel);
        this.Controls.Add(nameBox);
        this.Controls.Add(addBtn);
        this.Controls.Add(editBtn);
        this.Controls.Add(deleteBtn);
    }

    private void LoadData()
    {
        using var context = new AppDbContext();
        context.Database.EnsureCreated();
        var albums = context.Albums.OrderBy(a => a.Name).ToList();
        dataGridView.DataSource = albums.Select(a => new { a.Id, a.Name }).ToList();
        
        if (dataGridView.Columns.Contains("Id"))
            dataGridView.Columns["Id"].Visible = false;
    }

    private void AddBtn_Click(object? sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(nameBox.Text))
        {
            MessageBox.Show("Введите название альбома!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        using var context = new AppDbContext();
        var album = new Album { Name = nameBox.Text };
        context.Albums.Add(album);
        context.SaveChanges();
        nameBox.Clear();
        LoadData();
        MessageBox.Show("Альбом добавлен!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    private void EditBtn_Click(object? sender, EventArgs e)
    {
        if (dataGridView.SelectedRows.Count == 0)
        {
            MessageBox.Show("Выберите альбом для редактирования!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        int id = (int)dataGridView.SelectedRows[0].Cells["Id"].Value;
        using var context = new AppDbContext();
        var album = context.Albums.Find(id);
        if (album == null) return;

        string newName = Microsoft.VisualBasic.Interaction.InputBox("Введите новое название:", "Редактирование", album.Name);
        if (!string.IsNullOrWhiteSpace(newName))
        {
            album.Name = newName;
            context.SaveChanges();
            LoadData();
            MessageBox.Show("Альбом обновлён!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }

    private void DeleteBtn_Click(object? sender, EventArgs e)
    {
        if (dataGridView.SelectedRows.Count == 0)
        {
            MessageBox.Show("Выберите альбом для удаления!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        int id = (int)dataGridView.SelectedRows[0].Cells["Id"].Value;
        using var context = new AppDbContext();
        var album = context.Albums.Include(a => a.Songs).FirstOrDefault(a => a.Id == id);
        if (album == null) return;

        if (album.Songs.Any())
        {
            MessageBox.Show("Нельзя удалить альбом, в котором есть песни!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (MessageBox.Show($"Удалить альбом \"{album.Name}\"?", "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
        {
            context.Albums.Remove(album);
            context.SaveChanges();
            LoadData();
            MessageBox.Show("Альбом удалён!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }

    private void InitializeComponent()
    {
        this.AutoScaleMode = AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(600, 400);
    }
}