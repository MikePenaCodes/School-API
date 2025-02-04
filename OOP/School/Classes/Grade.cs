using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace School.Classes
{
    class Grade
    {
        public Subject Subject { get; set; }
        public Student Student { get; set; }
        public int? GradeNumber { get; set; }

        public Grade(Subject subject, Student student)
        {
            Subject = subject;
            Student = student;
        }
        public static void AssignGrade(Student student, List<Grade> gradeslist, Teacher teacher, Subject subject)
        {
            string prompt = "Enter the grade";
            string message = "Grade";
            int gradenumber = Validation.Validationint(prompt, message);

            Grade existingGrade = gradeslist.FirstOrDefault(g => g.Student == student && g.Subject == subject);
            if (existingGrade != null)
            {
                existingGrade.GradeNumber = gradenumber;
                Console.WriteLine($"Grade has been updated.");
            }
            else
            {
                Console.WriteLine("No matching grade found.");
            }
        }

        public static void DisplayGradeInfoStudent(Student studentaccount, List<Grade> gradeslist)
        {
            if (studentaccount.subjectlist.Count == 0)
            {
                Console.WriteLine("No classes assigned");
                Console.ReadLine();
                return;
            }
            else
            {
                foreach (Grade grade in gradeslist)
                {
                    if (grade.Student == studentaccount)
                    {
                        if (grade.GradeNumber == null)
                        {
                            Console.WriteLine($"{grade.Subject.SubjectName}: X");
                           
                        }
                        else
                        {
                            Console.WriteLine($"{grade.Subject.SubjectName}: {grade.GradeNumber}");
                           
                        }
                    }
                }
                Console.ReadLine();
            }
        }
        public static void HandleGradeClasses(Teacher teacheraccount, List<Student> studentlist, List<Grade> gradeslist)
        {
            string subjectchoices = Teacher.GetSubjectChoices(teacheraccount);
            Console.WriteLine(subjectchoices);
            Console.ReadLine();

            bool studentinclass = true;
            string prompt1 = "Choose which class";
            string message1 = "class";
            int chosensubject = Validation.Validationint(prompt1, message1);
            Subject subjectchosen = null;
            try {
            subjectchosen = teacheraccount.SubjectSpecialization[chosensubject];
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
                Console.ReadLine();
                return;
            }

            string students = Subject.GetStudentsInSubject(subjectchosen);
            Console.WriteLine(students);

            Console.WriteLine("Enter the name of the student");
            Student studentaccount = null;
            while (studentaccount == null)
            {
                string answername = Console.ReadLine();
                studentaccount = Student.FindCurrentStudent(answername, studentlist);
            };

            if (studentaccount.subjectlist.Count == 0)
            {
                Console.WriteLine("Student is not enrolled in any classes.");
                Console.ReadLine();
                return;
            }

            foreach (Subject subject in studentaccount.subjectlist)
            {
                if (subject == subjectchosen)
                {
                    Grade.AssignGrade(studentaccount, gradeslist, teacheraccount, subject);
                    studentinclass = false;
                }
            }

            if (studentinclass)
                Console.WriteLine("Student is not enrolled in this class.");
            Console.ReadLine();
        }
    }
}


