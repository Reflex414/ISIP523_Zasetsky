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
    static void EnrollStudentInCourse()
    {
        Console.WriteLine("\n=== ЗАПИСЬ СТУДЕНТА НА КУРС ===");
        ShowAllStudents();
        ShowAllCourses();

        Console.Write("Введите ID студента: ");
        int studentId = int.Parse(Console.ReadLine());
        Console.Write("Введите ID курса: ");
        int courseId = int.Parse(Console.ReadLine());

        Student student = FindStudentById(studentId);
        Course course = FindCourseById(courseId);

        if (student == null)
        {
            Console.WriteLine("Студент не найден!");
            return;
        }

        if (course == null)
        {
            Console.WriteLine("Курс не найден!");
            return;
        }

        student.EnrollInCourse(course);
        Console.WriteLine($"Студент {student.FIO} записан на курс {course.CourseName}");
    }

    static void AssignTeacherToCourse()
    {
        Console.WriteLine("\n=== НАЗНАЧЕНИЕ ПРЕПОДАВАТЕЛЯ НА КУРС ===");
        ShowAllTeachers();
        ShowAllCourses();

        Console.Write("Введите ID преподавателя: ");
        int teacherId = int.Parse(Console.ReadLine());
        Console.Write("Введите ID курса: ");
        int courseId = int.Parse(Console.ReadLine());

        Teacher teacher = FindTeacherById(teacherId);
        Course course = FindCourseById(courseId);

        if (teacher == null)
        {
            Console.WriteLine("Преподаватель не найден!");
            return;
        }

        if (course == null)
        {
            Console.WriteLine("Курс не найден!");
            return;
        }

        course.AssignTeacher(teacher);
        Console.WriteLine($"Преподаватель {teacher.FIO} назначен на курс {course.CourseName}");
    }
    static void ShowStudentCourses()
    {
        Console.WriteLine("\n=== КУРСЫ СТУДЕНТА ===");
        ShowAllStudents();
        Console.Write("Введите ID студента: ");
        int id = int.Parse(Console.ReadLine());

        Student student = FindStudentById(id);
        if (student != null)
        {
            student.ShowEnrolledCourses();
        }
        else
        {
            Console.WriteLine("Студент не найден!");
        }
    }

    static void ShowCourseStudents()
    {
        Console.WriteLine("\n=== СТУДЕНТЫ КУРСА ===");
        ShowAllCourses();
        Console.Write("Введите ID курса: ");
        int id = int.Parse(Console.ReadLine());

        Course course = FindCourseById(id);
        if (course != null)
        {
            course.ShowEnrolledStudents();
        }
        else
        {
            Console.WriteLine("Курс не найден!");
        }
    }

    static void ShowTeacherCourses()
    {
        Console.WriteLine("\n=== КУРСЫ ПРЕПОДАВАТЕЛЯ ===");
        ShowAllTeachers();
        Console.Write("Введите ID преподавателя: ");
        int id = int.Parse(Console.ReadLine());

        Teacher teacher = FindTeacherById(id);
        if (teacher != null)
        {
            teacher.ShowAssignedCourses();
        }
        else
        {
            Console.WriteLine("Преподаватель не найден!");
        }
    }
    static Student FindStudentById(int id)
    {
        foreach (var student in students)
        {
            if (student.StudentID == id)
            {
                return student;
            }
        }
        return null;
    }

    static Teacher FindTeacherById(int id)
    {
        foreach (var teacher in teachers)
        {
            if (teacher.TeacherID == id)
            {
                return teacher;
            }
        }
        return null;
    }

    static Course FindCourseById(int id)
    {
        foreach (var course in courses)
        {
            if (course.CourseID == id)
            {
                return course;
            }
        }
        return null;
    }

    static void AddTestData()
    {

        students.Add(new Student("Иванов Иван Иванович", 20, new DateOnly(2003, 5, 15), "М", 1));
        students.Add(new Student("Петрова Мария Сергеевна", 21, new DateOnly(2002, 8, 22), "Ж", 2));
        students.Add(new Student("Сидоров Алексей Владимирович", 19, new DateOnly(2004, 3, 10), "М", 3));

        teachers.Add(new Teacher("Смирнов Андрей Петрович", 45, new DateOnly(1978, 1, 15), "М", 1, 20));
        teachers.Add(new Teacher("Козлова Елена Викторовна", 38, new DateOnly(1985, 7, 30), "Ж", 2, 15));


        courses.Add(new Course(1, "Программирование на C#", 2024));
        courses.Add(new Course(2, "Базы данных", 2024));
        courses.Add(new Course(3, "Веб-разработка", 2024));


        courses[0].AssignTeacher(teachers[0]);
        courses[1].AssignTeacher(teachers[1]);
        courses[2].AssignTeacher(teachers[0]);


        students[0].EnrollInCourse(courses[0]);
        students[0].EnrollInCourse(courses[1]);
        students[1].EnrollInCourse(courses[0]);
        students[2].EnrollInCourse(courses[2]);

        nextStudentId = 4;
        nextTeacherId = 3;
        nextCourseId = 4;
    }
}

}
