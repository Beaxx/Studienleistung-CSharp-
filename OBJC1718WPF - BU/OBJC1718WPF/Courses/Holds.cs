using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManager
{
    public class Holds
    {
        private static int counter = 0;
        public int ID { get; set; }
        public Lecturer Lecturer { get; set; }
        public Course Course { get; set; }

    public Holds(Lecturer lecturer, Course course)
        {
            ID = counter++;
            Lecturer = lecturer;
            Course = course;
        }
    }
}
