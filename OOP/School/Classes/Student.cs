using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace School.Classes
{
    class Student : Person
    {
        public string StudentID { get; set; }
       
        public List<Subject> subjectlist = new List<Subject>();
        public double GPA { get; set; }

        public Student(string name, int age, string address, string studentID, double gpa) : base(name, age, address)
        {
            StudentID = studentID;
            GPA = gpa;
        }
       
        public static void PrintStudentsSubjects(Student studentaccount)
        {
            foreach (Subject i in studentaccount.subjectlist)
            {
                Console.WriteLine(i.SubjectName);
            }
        }
        public void EnrollinSubject(Subject subject, Student studentaccount, List<Grade> gradeslist)
        {
            
            if (!studentaccount.subjectlist.Contains(subject))
            {
                subjectlist.Add(subject);
                subject.StudentList.Add(studentaccount);



                gradeslist.Add(new Grade(subject, studentaccount));




                Console.WriteLine($"{Name} has been enrolled in {subject.SubjectName}.");
            }
            else
            {
                Console.WriteLine($"{Name} is already enrolled in {subject.SubjectName}.");
                Console.ReadLine();
            }
            
            
        }

        public override void DisplayInfo()
        {
            Console.WriteLine($"Name: {Name}");
            Console.WriteLine($"Age: {Age}");
            Console.WriteLine($"Address: {Address}");
            Console.WriteLine($"Student ID: {StudentID}");
            //Console.WriteLine($"GradeLeve: {GradeLevel}");
            Console.WriteLine($"GPA: {GPA}");
            Console.ReadLine();
        }

        public static Student? FindCurrentStudent( string id, List<Student> studentlist)
        {
           
            foreach (Student student in studentlist)
            {
                if (id == student.StudentID.ToString() || id == student.Name.ToString())
                {
                    Console.Clear();
                    return student;
                }
                
            }

            Console.WriteLine("Student not found.");
            Console.ReadLine();
            
            return null;
        }
        
           
    }
}
