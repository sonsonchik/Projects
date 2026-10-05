using System;
using System.Windows.Forms;
using Homework_3.Data;

namespace Homework_3;

static class Program
{
    [STAThread]
    static void Main()
    {
        using var context = new AppDbContext();
        context.Database.EnsureCreated();
        
        ApplicationConfiguration.Initialize();
        Application.Run(new MainForm());
    }
}