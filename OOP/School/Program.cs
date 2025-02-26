using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime;
using System.Xml.Linq;
using School.Classes;

Student studentaccount = null;
Teacher teacheraccount = null;

const string mainmenu = @"
---------------------
Main Menu
---------------------
1. View personal info
2. View Subjects
3. View Grades
4. Enroll in Class
e. Exit
---------------------
";
const string mainmenu2 = @$"
---------------------
Main Menu - 
---------------------
1. View personal info
2. Grade classes
3. Create Subject
e. Exit
---------------------
";

List<Student> studentlist = new List<Student>
                {
                    new Student("Mike", 23, "1246 Canyon Creek Circle", "1001", 3.5),
                    new Student("Alice", 21, "763 Pinewood Avenue", "1002", 3.8),
                    new Student("John", 25, "455 Elm Street", "1003", 3.2),
                    new Student("Sophia", 22, "982 Oak Lane", "1004", 3.9),
                    new Student("James", 24, "327 Maple Road", "1005", 2.8),
                    new Student("Emma", 20, "1067 Willow Drive", "1006", 3.7),
                    new Student("Lucas", 23, "453 Birch Avenue", "1007", 3.6),
                    new Student("Olivia", 22, "889 Cedar Circle", "1008", 3.4),
                    new Student("Benjamin", 26, "674 Aspen Street", "1009", 3.5),
                    new Student("Charlotte", 19, "333 Oakwood Drive", "1010", 3.8),
                    new Student("Liam", 27, "254 Pine Avenue", "1011", 3.1),
                    new Student("Amelia", 22, "1457 Maplewood Lane", "1012", 4.0),
                    new Student("Henry", 24, "632 River Road", "1013", 3.2),
                    new Student("Isabella", 21, "789 Birchwood Lane", "1014", 3.6),
                    new Student("Daniel", 23, "1020 Willow Road", "1015", 3.9),
                    new Student("Mason", 23, "455 Cedar Road", "1016", 3.3),
                    new Student("Ava", 21, "356 Oakwood Drive", "1017", 3.7),
                    new Student("Elijah", 22, "896 Birch Street", "1018", 3.5),
                    new Student("Zoe", 20, "1135 Pine Grove", "1019", 3.9),
                    new Student("Nathan", 24, "877 Riverwood Lane", "1020", 3.2),
                    new Student("Harper", 21, "223 Maple Avenue", "1021", 3.6),
                    new Student("Logan", 23, "1022 Willow Lane", "1022", 3.4),
                    new Student("Ella", 19, "454 Oak Road", "1023", 3.8),
                    new Student("Jack", 25, "678 Birch Lane", "1024", 2.9),
                    new Student("Grace", 22, "1187 Pinehill Avenue", "1025", 3.7),
                    new Student("Samuel", 23, "547 Cedarwood Street", "1026", 3.5),
                    new Student("Chloe", 24, "982 River Avenue", "1027", 3.6),
                    new Student("Lucas", 23, "864 Oak Grove", "1028", 3.4),
                    new Student("Sophia", 21, "675 Elmwood Drive", "1029", 3.9),
                    new Student("William", 27, "1023 Maplewood Avenue", "1030", 3.0),
                    new Student("Leah", 22, "832 Birchwood Avenue", "1031", 3.8),
                    new Student("Isaiah", 25, "1122 Cedar Road", "1032", 3.3),
                    new Student("Lily", 20, "246 Pine Avenue", "1033", 4.0),
                    new Student("Gabriel", 24, "654 Willow Street", "1034", 3.6),
                    new Student("Maya", 23, "981 Elm Avenue", "1035", 3.8),
                    new Student("Elena", 22, "439 Oak Lane", "1036", 3.5),
                    new Student("Sebastian", 21, "320 Maple Street", "1037", 3.7),
                    new Student("Leo", 23, "765 Pinewood Road", "1038", 3.2),
                    new Student("Victoria", 26, "543 Birch Lane", "1039", 3.4),
                    new Student("Matthew", 20, "1325 River Drive", "1040", 3.9),
                    new Student("Scarlett", 22, "478 Willow Road", "1041", 3.7),
                    new Student("Jameson", 25, "564 Oak Avenue", "1042", 3.1),
                    new Student("Nora", 23, "387 Pine Ridge", "1043", 3.6),
                    new Student("Owen", 21, "204 Birchwood Street", "1044", 3.3),
                    new Student("Addison", 22, "765 Cedar Lane", "1045", 3.8),
                    new Student("Benjamin", 24, "342 Elmwood Street", "1046", 3.4),
                    new Student("Mila", 19, "651 Maple Street", "1047", 3.9),
                    new Student("Jaxon", 23, "1025 Pine Circle", "1048", 3.5),
                    new Student("Madeline", 20, "1200 Oakwood Road", "1049", 3.6),
                    new Student("Daniel", 21, "453 River Lane", "1050", 3.8),
                    new Student("Brooklyn", 22, "572 Birch Drive", "1051", 3.3),
                    new Student("Henry", 23, "698 Willow Avenue", "1052", 3.5),
                    new Student("Grace", 25, "1324 Elm Lane", "1053", 3.7),
                    new Student("Aiden", 24, "401 Oak Lane", "1054", 3.4),
                    new Student("Sophie", 20, "1156 Birchwood Road", "1055", 3.9),
                    new Student("Oliver", 21, "905 Pinehill Avenue", "1056", 3.6),
                    new Student("Harper", 22, "776 Maple Road", "1057", 3.3),
                    new Student("Matthew", 23, "283 Cedar Avenue", "1058", 3.8),
                    new Student("Carter", 24, "497 Riverwood Drive", "1059", 3.5),
                    new Student("Lena", 23, "863 Oak Street", "1060", 3.7),
                    new Student("Jackson", 22, "1032 Pine Road", "1061", 3.4),
                    new Student("Amos", 25, "497 Birch Lane", "1062", 3.1),
                    new Student("Emily", 21, "540 Maple Lane", "1063", 3.8),
                    new Student("Ethan", 22, "791 Oak Avenue", "1064", 3.6),
                    new Student("Megan", 23, "246 Pinewood Road", "1065", 3.7),
                    new Student("Andrew", 24, "1286 River Road", "1066", 3.4),
                    new Student("Zoe", 22, "839 Maplewood Avenue", "1067", 3.9),
                    new Student("Ryan", 23, "764 Cedar Avenue", "1068", 3.5),
                    new Student("Natalie", 21, "457 Birch Avenue", "1069", 3.6),
                    new Student("Tyler", 22, "908 Oakwood Lane", "1070", 3.7),
                    new Student("Samuel", 23, "634 Pine Drive", "1071", 3.4)
                };

List<Student> mathlist = new List<Student>();
List<Student> sciencelist = new List<Student>();
List<Student> historylist = new List<Student>();
List<Student> englishlist = new List<Student>();
List<Student> geographylist = new List<Student>();
List<Student> physicslist = new List<Student>();
List<Student> chemistrylist = new List<Student>();
List<Student> artlist = new List<Student>();
List<Student> musiclist = new List<Student>();
List<Student> peList = new List<Student>();
List<Student> economicslist = new List<Student>();

List<List<Student>> studentsubjectlist = new List<List<Student>>
{
    mathlist,
    sciencelist,
    historylist,
    englishlist,
    geographylist,
    physicslist,
    chemistrylist,
    artlist,
    musiclist,
    peList,
    economicslist
};

List<Teacher> teacherlist = new List<Teacher>
 {
    new Teacher("Juan", 22, "838 Mineral Wells Lane", "2001", 5),
    new Teacher("Sophia", 30, "245 Riverfront Street", "2002", 7),
    new Teacher("Michael", 35, "762 Birchwood Drive", "2003", 10),
    new Teacher("Emily", 28, "123 Pine Street", "2004", 4),
    new Teacher("David", 40, "1020 Cedar Lane", "2005", 12),
    new Teacher("Alice", 33, "123 Maple Street", "2006", 8),
    new Teacher("Bob", 45, "456 Pine Avenue", "2007", 15),
    new Teacher("Clara", 29, "789 Birch Lane", "2008", 6),
    new Teacher("Daniel", 38, "321 Elm Street", "2009", 9),
    new Teacher("Grace", 41, "654 Cedar Blvd", "2010", 18),
    new Teacher("Oliver", 33, "111 Oak Blvd", "2011", 14)
 };

List<Subject> subjectlist = new List<Subject>
{
    new Subject("Math", teacherlist[0], studentsubjectlist[0]),
    new Subject("Science", teacherlist[1], studentsubjectlist[1]),
    new Subject("History", teacherlist[2], studentsubjectlist[2]),
    new Subject("English", teacherlist[3], studentsubjectlist[3]),
    new Subject("Geography", teacherlist[4],studentsubjectlist[4]),
    new Subject("Physics", teacherlist[5], studentsubjectlist[5]),
    new Subject("Chemistry", teacherlist[6], studentsubjectlist[6]),
    new Subject("Art", teacherlist[7],studentsubjectlist[7]),
    new Subject("Music", teacherlist[8], studentsubjectlist[8]),
    new Subject("PE", teacherlist[9], studentsubjectlist[9]),
    new Subject("Economics", teacherlist[10], studentsubjectlist[10])
};

List<Grade> gradeslist = new List<Grade> { };


bool accountfound = false;
string studentorprof = @"
Welcome to the School Project!
-------------------------------
[Student] or [Professor]
    1             2       
";

while (!accountfound)
{
    int choice = Validation.Validationint(studentorprof, "choice");

    switch (choice)
    {
        case 1:
            string studentid = new Validation().ValidationId();
            studentaccount = Student.FindCurrentStudent(studentid, studentlist);
            break;
        case 2:
            string teacherid = new Validation().ValidationId();
            teacheraccount = Teacher.FindCurrentTeacher(teacherid, teacherlist);
            break;
    }

    while (studentaccount != null)
    {
        Console.Clear();
        Console.WriteLine($"{studentaccount.Name}!");
        Console.Write(mainmenu);

        var answer = Console.ReadLine();

        switch (answer)
        {
            case "1":
                Console.Clear();
                studentaccount.DisplayInfo();
                break;

            case "2":
                Console.Clear();
                Student.PrintStudentsSubjects(studentaccount);
                Console.ReadLine();
                break;
            case "3":
                Console.Clear();
                Grade.DisplayGradeInfoStudent(studentaccount, gradeslist);
                break;
            case "4":
                string subjectchoices = Subject.GetSubjectChoices(subjectlist);

                int newsubject = Validation.Validationint(subjectchoices, "integer");
                try { 
                studentaccount.EnrollinSubject(subjectlist[newsubject], studentaccount, gradeslist);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    Console.ReadLine();
                }
                break;
            case "e":
                studentaccount = null;
                break;
            default:
                Console.WriteLine("Invalid option. Please try again.");
                break;
        }
    }

    while (teacheraccount != null)
    {
        Console.Clear();
        Console.WriteLine($"{teacheraccount.Name}!");
        Teacher.AssignSubject(teacherlist, teacheraccount, subjectlist);
        Console.WriteLine(mainmenu2);
        var answer = Console.ReadLine();
        switch (answer)
        {
            case "1":
                teacheraccount.DisplayInfo();
                break;
            case "2":
                Grade.HandleGradeClasses(teacheraccount, studentlist, gradeslist);
                break;
            case "3":
                Teacher.CreateSubject(subjectlist, teacheraccount, studentsubjectlist);
                break;
            case "e":
                teacheraccount = null;
                break;
            default:
                Console.WriteLine("Invalid option. Please try again.");
                break;
        }
    }
}


