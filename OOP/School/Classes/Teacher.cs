using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace School.Classes
{
    class Teacher : Person
    {
        public string TeacherID { get; set; }
        public List<Subject> SubjectSpecialization { get; set; }
        //This needs to become a list.
        public int YearsofExperience { get; set; }

        public Teacher(string name, int age, string address, string teacherId, int yearsofExperience) : base(name, age, address)
        {
            TeacherID = teacherId;
            YearsofExperience = yearsofExperience;
            SubjectSpecialization = new List<Subject>();
        }

        public static void AssignSubject(List<Teacher> teacherlist, Teacher teacheraccount, List<Subject> subjectlist)
        {
            teacheraccount.SubjectSpecialization.Clear();
            foreach (Subject i in subjectlist)
            {
                if (teacheraccount == i.Teacher)
                {
                    teacheraccount.SubjectSpecialization.Add(i);
                }
            }
        }

        public static void CreateSubject(List<Subject> subjectlist, Teacher teacheraccount, List<List<Student>> studentlist)
        {
            bool notexistingsubject = true;
            Console.WriteLine("Please insert the subject you wish to add.");
            string newsubject = Console.ReadLine();

            foreach (Subject i in subjectlist)
            {
                if (i.SubjectName == newsubject)
                {
                    Console.WriteLine("Sorry this subject already exists.");
                    Console.ReadLine();
                    notexistingsubject = false;
                    break;
                }
            }

            if (notexistingsubject)
            {
                List<Student> studentSubjectList = new List<Student>();
                subjectlist.Add(new Subject(newsubject, teacheraccount, studentSubjectList));
                Console.WriteLine("Subject added successfully.");
                Console.ReadLine();
            }
        }

        public override void DisplayInfo()
        {
            Console.WriteLine($"Name: {Name}");
            Console.WriteLine($"Age: {Age}");
            Console.WriteLine($"Address: {Address}");
            Console.WriteLine($"Teacher ID: {TeacherID}");
            Console.WriteLine($"Subject Specialization: ");
            foreach(Subject i in SubjectSpecialization)
            {
                Console.WriteLine(i.SubjectName);
            }
            Console.WriteLine($"Years of Experience: {YearsofExperience}");
            Console.ReadLine();

        }

        public static Teacher FindCurrentTeacher(string id, List<Teacher> teacherlist)
        {

            foreach (Teacher teacher in teacherlist)
            {
                if (id == teacher.TeacherID.ToString())
                {
                    Console.Clear();
                    
                    return teacher;
                }
            }

            Console.WriteLine("No Student not found.");
            return null;


        }

        public static string GetSubjectChoices(Teacher teacheraccount)
        {
            string subjectchoices = null;
            for (int index = 0; index < teacheraccount.SubjectSpecialization.Count; index++)
            {
                string add = ($"{index}: {teacheraccount.SubjectSpecialization[index].SubjectName}\n");
                subjectchoices = subjectchoices + add;
            }
            return subjectchoices;
        }


    }
}
