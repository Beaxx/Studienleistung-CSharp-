using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace StudentManager
{
    [DataContract]
    public class Listens
    {
        [DataMember]
        private int studentID;
        public int StudentID
        {
            get { return studentID; }
            set { studentID = value; }
        }

        [DataMember]
        private int courseID;
        public int CourseID
        {
            get { return courseID; }
            set { courseID = value; }
        }

        public Listens(int studentID, int courseID)
        {
            StudentID = studentID;
            CourseID = courseID;
        }
    }

    [DataContract]
    public class Holds
    {
        [DataMember]
        private int lecturerID;
        public int LecturerID
        {
            get { return lecturerID; }
            set { lecturerID = value; }
        }

        [DataMember]
        private int courseID;
        public int CourseID
        {
            get { return courseID; }
            set { courseID = value; }
        }

        public Holds(int lecturerID, int courseID)
        {
            LecturerID = LecturerID;
            CourseID = courseID;
        }
    }

}

