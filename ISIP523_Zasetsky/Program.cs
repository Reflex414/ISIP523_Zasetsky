using System;
using System.Collections.Generic;

class Program
{
    static List<Student> students = new List<Student>();
    static List<Teacher> teachers = new List<Teacher>();
    static List<Course> courses = new List<Course>();
    static int nextStudentId = 1;
    static int nextTeacherId = 1;
    static int nextCourseId = 1;

    static void Main(string[] args)
    {

        AddTestData();

        bool exit = false;
        while (!exit)
        {
            Console.Clear();
            Console.WriteLine("=== СИСТЕМА УПРАВЛЕНИЯ УНИВЕРСИТЕТОМ ===");
            Console.WriteLine("1. Показать всех студентов");
            Console.WriteLine("2. Показать всех преподавателей");
            Console.WriteLine("3. Показать все курсы");
            Console.WriteLine("4. Добавить студента");
            Console.WriteLine("5. Добавить преподавателя");
            Console.WriteLine("6. Создать курс");
            Console.WriteLine("7. Записать студента на курс");
            Console.WriteLine("8. Назначить преподавателя на курс");
            Console.WriteLine("9. Показать курсы студента");
            Console.WriteLine("10. Показать студентов курса");
            Console.WriteLine("11. Показать курсы преподавателя");
            Console.WriteLine("0. Выход");
            Console.Write("Выберите опцию: ");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    ShowAllStudents();
                    break;
                case "2":
                    ShowAllTeachers();
                    break;
                case "3":
                    ShowAllCourses();
                    break;
                case "4":
                    AddStudent();
                    break;
                case "5":
                    AddTeacher();
                    break;
                case "6":
                    AddCourse();
                    break;
                case "7":
                    EnrollStudentInCourse();
                    break;
                case "8":
                    AssignTeacherToCourse();
                    break;
                case "9":
                    ShowStudentCourses();
                    break;
                case "10":
                    ShowCourseStudents();
                    break;
                case "11":
                    ShowTeacherCourses();
                    break;
                case "0":
                    exit = true;
                    Console.WriteLine("Выход из системы...");
                    break;
                default:
                    Console.WriteLine("Неверный выбор!");
                    break;
            }

            if (!exit)
            {
                Console.WriteLine("\nНажмите любую клавишу для продолжения...");
                Console.ReadKey();
            }
        }
    }

    
}
