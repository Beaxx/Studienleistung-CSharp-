using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManager
{
    public class Listens
    {
        private static int counter = 0;
        public int ID { get; set; }
        public Student Student { get; set; }
        public Course Course { get; set; }

    public Listens(Student student, Course course)
        {
            ID = counter++;
            Student = student;
            Course = course;
        }
    }
}
