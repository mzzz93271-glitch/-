//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Data;
//
//
//namespace ConsoleApp2
//{
//    internal class Program
//    {
//        static void Main(string[] args)
//        {
//           DataSet dataSet = new DataSet();
//            DataTable groups = new DataTable();

//            groups.Columns.Add("id", typeof(int));
//            groups.Columns.Add("name", typeof(string));
//            groups.PrimaryKey = new[]
//            {
//                    groups.Columns["id"]
//            };
//            DataTable students = new DataTable("students");
//            students.Columns.Add("id", typeof(int));
//            students.Columns.Add ("name", typeof(string));
//            students.Columns.Add( "Groupid", typeof(int));
//            students.PrimaryKey = new[]
//            {
//             students.Columns["id"]
//            };
//            dataSet.Tables.Add(groups);
//            dataSet.Tables.Add(students);
//        }
//    }
//}

using System;
using System.Data;

namespace ConsoleApp2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            DataSet dataSet = new DataSet();

            // Инициализация таблиц и ПК
            DataTable groups = new DataTable("groups");
            groups.Columns.Add("id", typeof(int));
            groups.Columns.Add("name", typeof(string));
            groups.PrimaryKey = new[] { groups.Columns["id"] };

            DataTable students = new DataTable("students");
            students.Columns.Add("id", typeof(int));
            students.Columns.Add("name", typeof(string));
            students.Columns.Add("age", typeof(int));
            students.Columns.Add("Groupid", typeof(int));
            students.PrimaryKey = new[] { students.Columns["id"] };

            dataSet.Tables.Add(groups);
            dataSet.Tables.Add(students);

            // Заполнение групп
            groups.Rows.Add(1, "Программисты");
            groups.Rows.Add(2, "Дизайнеры");
            groups.Rows.Add(3, "Аналитики");

            // Списки данных для студентов
            string[] names = { "Иван", "Мария", "Алексей", "Ольга", "Дмитрий", "Елена", "Павел", "Анна", "Сергей", "Ирина" };
            Random rand = new Random();

            // Заполнение студентов 
            int studentId = 1;
            foreach (DataRow groupRow in groups.Rows)
            {
                int groupId = (int)groupRow["id"];
                for (int j = 0; j < 10; j++)
                {
                    string studentName = $"{names[j]} ({groupRow["name"]})";
                    int age = rand.Next(18, 25); // Случайный возраст от 18 до 24
                    students.Rows.Add(studentId++, studentName, age, groupId);
                }
            }

            // Создание связи
            DataRelation relation = new DataRelation("GroupStudents", groups.Columns["id"], students.Columns["Groupid"]);
            dataSet.Relations.Add(relation);

            // Вывод списка доступных групп
            Console.WriteLine("Доступные группы:");
            foreach (DataRow row in groups.Rows)
            {
                Console.WriteLine($"{row["id"]} - {row["name"]}");
            }

            // Выбор группы пользователем
            Console.Write("\nВведите ID группы для просмотра студентов: ");
            if (int.TryParse(Console.ReadLine(), out int selectedGroupId))
            {
                DataRow selectedGroupRow = groups.Rows.Find(selectedGroupId);

                if (selectedGroupRow != null)
                {
                    Console.WriteLine($"\nСтуденты группы: {selectedGroupRow["name"]}");
                    Console.WriteLine("---------------------------------------------");

                    // Получение связанных строк через DataRelation
                    DataRow[] childRows = selectedGroupRow.GetChildRows(relation);

                    foreach (DataRow studentRow in childRows)
                    {
                        Console.WriteLine($"ID: {studentRow["id"]} | Имя: {studentRow["name"],-25} | Возраст: {studentRow["age"]}");
                    }
                }
                else
                {
                    Console.WriteLine("Группа с таким ID не найдена.");
                }
            }
            else
            {
                Console.WriteLine("Некорректный ввод ID.");
            }

            Console.ReadLine();
        }
    }
}
