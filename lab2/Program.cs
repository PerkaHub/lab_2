﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

public class Student
{
    public string Name { get; set; } 
}

public class Teacher
{
    public string Email { get; set; }
    public string Name { get; set; }
    public int Age { get; set; }
    public List<Student> Students { get; set; }


    internal Teacher(string email, string name, int age, List<Student> students) 
    {
        Email = email;
        Name = name;
        Age = age;
        Students = students;
    }

}

// Класс для валидации
public class TeacherCreator
{
    private const string LogFilePath = "teacher_creation_log.txt";

    public Teacher CreateTeacher(string email, string name, int age, List<Student> students)
    {
        List<string> errors = new List<string>();

        // Валидация почты
        if (!IsValidEmail(email))
        {
            errors.Add($"Неверный формат почты: {email}");
        }

        // Валидация имени
        if (!IsValidName(name))
        {
            errors.Add($"Неверный формат имени: {name}");
        }


        // Валидация возраста
        if (age <= 0 || age > 120)
        {
            errors.Add($"Неверный возраст: {age}");
        }

        if (errors.Count > 0)
        {
            LogErrors(errors);
            return null;
        }

        return new Teacher(email, name, age, students);
    }


    private bool IsValidEmail(string email)
    {
        // Регулярное выражение для проверки почты
        return Regex.IsMatch(email, @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$");
    }

    private bool IsValidName(string name)
    {
        // Регулярное выражение для проверки имени
         return Regex.IsMatch(name, @"^[а-яА-Я\s]+$");
    }

    private void LogErrors(List<string> errors)
    {
        try
        {
            using (StreamWriter writer = new StreamWriter(LogFilePath, true)) 
            {
                writer.WriteLine($"[{DateTime.Now}] Ошибка создания объекта Teacher:");
                foreach (string error in errors)
                {
                    writer.WriteLine($"- {error}");
                }
                writer.WriteLine();
            }
        }
        catch (Exception ex) 
        {
            Console.WriteLine($"Ошибка записи в лог-файл: {ex.Message}");
            
        }
    }


}


// Пример использования
public class Example
{
    public static void Main(string[] args)
    {
        TeacherCreator creator = new TeacherCreator();
        List<Student> students = new List<Student>(); 

        Teacher teacher1 = creator.CreateTeacher("test@example.com", "Иван Иванов", 30, students);
        Teacher teacher2 = creator.CreateTeacher("invalid-email", "John Doe", -5, students);

        if (teacher1 != null)
        {
            Console.WriteLine("Учитель teacher1 создан успешно.");
        }

         if (teacher2 != null)
        {
            Console.WriteLine("Учитель teacher2 создан успешно.");
        }
    }

}