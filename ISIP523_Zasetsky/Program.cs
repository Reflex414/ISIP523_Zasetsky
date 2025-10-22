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

    static void AddStudent()
    {
        Console.WriteLine("\n=== ДОБАВЛЕНИЕ СТУДЕНТА ===");
        Console.Write("ФИО: ");
        string fio = Console.ReadLine();
        Console.Write("Возраст: ");
        int age = int.Parse(Console.ReadLine());
        Console.Write("Дата рождения (гггг-мм-дд): ");
        DateOnly birthday = DateOnly.Parse(Console.ReadLine());
        Console.Write("Пол: ");
        string gender = Console.ReadLine();

        Student student = new Student(fio, age, birthday, gender, nextStudentId);
        students.Add(student);
        nextStudentId++;
        Console.WriteLine($"Студент успешно добавлен! ID: {student.StudentID}");
    }


    static void AddTeacher()
    {
        Console.WriteLine("\n=== ДОБАВЛЕНИЕ ПРЕПОДАВАТЕЛЯ ===");
        Console.Write("ФИО: ");
        string fio = Console.ReadLine();
        Console.Write("Возраст: ");
        int age = int.Parse(Console.ReadLine());
        Console.Write("Дата рождения (гггг-мм-дд): ");
        DateOnly birthday = DateOnly.Parse(Console.ReadLine());
        Console.Write("Пол: ");
        string gender = Console.ReadLine();
        Console.Write("Стаж (лет): ");
        int expYear = int.Parse(Console.ReadLine());

        Teacher teacher = new Teacher(fio, age, birthday, gender, nextTeacherId, expYear);
        teachers.Add(teacher);
        nextTeacherId++;
        Console.WriteLine($"Преподаватель успешно добавлен! ID: {teacher.TeacherID}");
    }

    static void AddCourse()
    {
        Console.WriteLine("\n=== СОЗДАНИЕ КУРСА ===");
        Console.Write("Название курса: ");
        string courseName = Console.ReadLine();
        Console.Write("Год курса: ");
        int courseYear = int.Parse(Console.ReadLine());

        Course course = new Course(nextCourseId, courseName, courseYear);
        courses.Add(course);
        nextCourseId++;
        Console.WriteLine($"Курс успешно создан! ID: {course.CourseID}");
    }
    static void ShowAllStudents()
    {
        Console.WriteLine("\n=== ВСЕ СТУДЕНТЫ ===");
        if (students.Count == 0)
        {
            Console.WriteLine("Студентов нет");
            return;
        }
        foreach (var student in students)
        {
            Console.WriteLine(student.GetInfo());
        }
    }

    static void ShowAllTeachers()
    {
        Console.WriteLine("\n=== ВСЕ ПРЕПОДАВАТЕЛИ ===");
        if (teachers.Count == 0)
        {
            Console.WriteLine("Преподавателей нет");
            return;
        }
        foreach (var teacher in teachers)
        {
            Console.WriteLine(teacher.GetInfo());
        }
    }

    static void ShowAllCourses()
    {
        Console.WriteLine("\n=== ВСЕ КУРСЫ ===");
        if (courses.Count == 0)
        {
            Console.WriteLine("Курсов нет");
            return;
        }
        foreach (var course in courses)
        {
            Console.WriteLine(course.GetInfo());
        }
    }

}
