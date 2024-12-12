using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

// Модель данных
public class MyProgram
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Version { get; set; }
    public string Description { get; set; }
}

// Контекст базы данных
public class ApplicationDbContext : DbContext
{
    public DbSet<MyProgram> Programs { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(@"Server=localhost\MSSQLSERVER01;Database=ProgramsDB;Trusted_Connection=True;Encrypt=True;TrustServerCertificate=True");
    }
}

// Сервис для работы с программами
public class ProgramService
{
    public void CreateProgram(MyProgram program)
    {
        using (var context = new ApplicationDbContext())
        {
            context.Programs.Add(program);
            context.SaveChanges();
        }
    }

    public List<MyProgram> GetAllPrograms()
    {
        using (var context = new ApplicationDbContext())
        {
            return context.Programs.ToList();
        }
    }

    public MyProgram GetProgramById(int id)
    {
        using (var context = new ApplicationDbContext())
        {
            return context.Programs.Find(id);
        }
    }

    public void UpdateProgram(MyProgram program)
    {
        using (var context = new ApplicationDbContext())
        {
            context.Programs.Update(program);
            context.SaveChanges();
        }
    }

    public void DeleteProgram(int id)
    {
        using (var context = new ApplicationDbContext())
        {
            var program = new MyProgram { Id = id };
            context.Programs.Remove(program);
            context.SaveChanges();
        }
    }
}

// Основной класс приложения
class MyProgramApp
{
    static void Main(string[] args)
    {
        ProgramService service = new ProgramService();

        // Пример создания программы
        var newProgram = new MyProgram { Name = "MyApp", Version = "1.0", Description = "Описание приложения" };
        service.CreateProgram(newProgram);
        Console.WriteLine("Программа создана.");

        // Пример получения всех программ
        var programs = service.GetAllPrograms();
        Console.WriteLine("Список программ:");
        foreach (var program in programs)
        {
            Console.WriteLine($"ID: {program.Id}, Name: {program.Name}, Version: {program.Version}, Description: {program.Description}");
        }

        // Пример обновления программы
        if (programs.Count > 0)
        {
            var programToUpdate = programs[0];
            programToUpdate.Version = "1.1";
            service.UpdateProgram(programToUpdate);
            Console.WriteLine("Программа обновлена.");
        }

        // Пример удаления программы
        if (programs.Count > 0)
        {
            service.DeleteProgram(programs[0].Id);
            Console.WriteLine("Программа удалена.");
        }
    }
}

