using System;
using System.Collections.Generic;
using System.Linq; // <-- добавлено

namespace UniversityManagement
{
    // ====== АБСТРАКТНЫЙ КЛАСС ЧЕЛОВЕКА ======
    abstract class Person
    {
        private string name;
        private int age;
        private string contact;

        public string Name { get => name; protected set => name = value; }
        public int Age { get => age; protected set => age = value; }
        public string Contact { get => contact; protected set => contact = value; }

        public Person(string name, int age, string contact)
        {
            Name = name;
            Age = age;
            Contact = contact;
        }

        public abstract void PrintInfo();
    }

    // ====== СТУДЕНТ ======
    class Student : Person
    {
        private List<Course> courses = new List<Course>();

        public IReadOnlyList<Course> Courses => courses;

        public Student(string name, int age, string contact)
            : base(name, age, contact) { }

        public void Enroll(Course course)
        {
            if (!courses.Contains(course))
                courses.Add(course);
        }

        public override void PrintInfo()
        {
            Console.WriteLine($"[Студент] {Name}, {Age} лет, Контакт: {Contact}");
            // Используем LINQ Select вместо ConvertAll
            var titles = Courses.Any() ? string.Join(", ", Courses.Select(c => c.Title)) : "нет";
            Console.WriteLine("Курсы: " + titles);
        }
    }

    // ====== ПРЕПОДАВАТЕЛЬ ======
    class Teacher : Person
    {
        public Teacher(string name, int age, string contact)
            : base(name, age, contact) { }

        public override void PrintInfo()
        {
            Console.WriteLine($"[Преподаватель] {Name}, {Age} лет, Контакт: {Contact}");
        }
    }

    // ====== КУРС ======
    class Course
    {
        private List<Student> students = new List<Student>();

        public string Title { get; private set; }
        public Teacher Teacher { get; private set; }
        public IReadOnlyList<Student> Students => students;

        public Course(string title, Teacher teacher)
        {
            Title = title;
            Teacher = teacher;
        }

        public void AddStudent(Student student)
        {
            if (!students.Contains(student))
                students.Add(student);
        }

        public void PrintInfo()
        {
            Console.WriteLine($"Курс: {Title}");
            Console.WriteLine($"Преподаватель: {Teacher.Name}");
            Console.WriteLine("Студенты:");

            if (Students.Count == 0)
                Console.WriteLine("  Нет записанных студентов.");
            else
                foreach (var s in Students)
                    Console.WriteLine($"  - {s.Name}");
        }
    }

    // ====== ГЛАВНАЯ ПРОГРАММА ======
    class Program
    {
        static List<Student> students = new List<Student>();
        static List<Teacher> teachers = new List<Teacher>();
        static List<Course> courses = new List<Course>();

        static void Main()
        {
            while (true)
            {
                Console.WriteLine("\n===== МЕНЮ УНИВЕРСИТЕТА =====");
                Console.WriteLine("1. Добавить студента");
                Console.WriteLine("2. Показать всех студентов");
                Console.WriteLine("3. Записать студента на курс");
                Console.WriteLine("4. Добавить преподавателя");
                Console.WriteLine("5. Показать всех преподавателей");
                Console.WriteLine("6. Создать курс");
                Console.WriteLine("7. Показать все курсы");
                Console.WriteLine("8. Просмотр информации о курсе");
                Console.WriteLine("0. Выход");
                Console.Write("Выберите пункт: ");

                switch (Console.ReadLine())
                {
                    case "1": AddStudent(); break;
                    case "2": ShowStudents(); break;
                    case "3": EnrollStudent(); break;
                    case "4": AddTeacher(); break;
                    case "5": ShowTeachers(); break;
                    case "6": AddCourse(); break;
                    case "7": ShowCourses(); break;
                    case "8": ShowCourseInfo(); break;
                    case "0": return;
                    default: Console.WriteLine("Неверный выбор!"); break;
                }
            }
        }

        static void AddStudent()
        {
            Console.Write("Имя студента: ");
            string name = Console.ReadLine();
            Console.Write("Возраст: ");
            if (!int.TryParse(Console.ReadLine(), out int age))
            {
                Console.WriteLine("Неверный возраст. Операция отменена.");
                return;
            }
            Console.Write("Контакт: ");
            string contact = Console.ReadLine();

            students.Add(new Student(name, age, contact));
            Console.WriteLine("Студент добавлен!");
        }

        static void ShowStudents()
        {
            if (students.Count == 0)
                Console.WriteLine("Студентов нет.");
            else
                foreach (var s in students)
                    s.PrintInfo();
        }

        static void EnrollStudent()
        {
            if (students.Count == 0 || courses.Count == 0)
            {
                Console.WriteLine("Нет студентов или курсов.");
                return;
            }

            Console.WriteLine("Выберите студента:");
            for (int i = 0; i < students.Count; i++)
                Console.WriteLine($"{i + 1}. {students[i].Name}");

            if (!int.TryParse(Console.ReadLine(), out int sIndex) || sIndex < 1 || sIndex > students.Count)
            {
                Console.WriteLine("Неверный выбор студента.");
                return;
            }
            sIndex--;

            Console.WriteLine("Выберите курс:");
            for (int i = 0; i < courses.Count; i++)
                Console.WriteLine($"{i + 1}. {courses[i].Title}");

            if (!int.TryParse(Console.ReadLine(), out int cIndex) || cIndex < 1 || cIndex > courses.Count)
            {
                Console.WriteLine("Неверный выбор курса.");
                return;
            }
            cIndex--;

            students[sIndex].Enroll(courses[cIndex]);
            courses[cIndex].AddStudent(students[sIndex]);

            Console.WriteLine("Студент записан на курс!");
        }

        static void AddTeacher()
        {
            Console.Write("Имя преподавателя: ");
            string name = Console.ReadLine();
            Console.Write("Возраст: ");
            if (!int.TryParse(Console.ReadLine(), out int age))
            {
                Console.WriteLine("Неверный возраст. Операция отменена.");
                return;
            }
            Console.Write("Контакт: ");
            string contact = Console.ReadLine();

            teachers.Add(new Teacher(name, age, contact));
            Console.WriteLine("Преподаватель добавлен!");
        }

        static void ShowTeachers()
        {
            if (teachers.Count == 0)
                Console.WriteLine("Преподавателей нет.");
            else
                foreach (var t in teachers)
                    t.PrintInfo();
        }

        static void AddCourse()
        {
            if (teachers.Count == 0)
            {
                Console.WriteLine("Сначала добавьте преподавателя!");
                return;
            }

            Console.Write("Введите название курса: ");
            string title = Console.ReadLine();

            Console.WriteLine("Выберите преподавателя:");

            for (int i = 0; i < teachers.Count; i++)
                Console.WriteLine($"{i + 1}. {teachers[i].Name}");

            if (!int.TryParse(Console.ReadLine(), out int index) || index < 1 || index > teachers.Count)
            {
                Console.WriteLine("Неверный выбор преподавателя.");
                return;
            }
            index--;

            courses.Add(new Course(title, teachers[index]));
            Console.WriteLine("Курс создан!");
        }

        static void ShowCourses()
        {
            if (courses.Count == 0)
                Console.WriteLine("Курсов нет.");
            else
                foreach (var c in courses)
                    Console.WriteLine($"Курс: {c.Title}, преподаватель: {c.Teacher.Name}");
        }

        static void ShowCourseInfo()
        {
            if (courses.Count == 0)
            {
                Console.WriteLine("Курсов нет.");
                return;
            }

            Console.WriteLine("Выберите курс:");

            for (int i = 0; i < courses.Count; i++)
                Console.WriteLine($"{i + 1}. {courses[i].Title}");

            if (!int.TryParse(Console.ReadLine(), out int index) || index < 1 || index > courses.Count)
            {
                Console.WriteLine("Неверный выбор курса.");
                return;
            }
            index--;

            courses[index].PrintInfo();
        }
    }
}
