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
        private int courseIDs;
        public int CourseIDs
        {
            get { return courseIDs; }
            set { courseIDs = value; }
        }

        public Listens(int studentID, int courseIDs)
        {
            StudentID = studentID;
            CourseIDs = courseIDs;
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
        private int courseIDs;
        public int CourseIDs
        {
            get { return courseIDs; }
            set { courseIDs = value; }
        }

        public Holds(int lecturerID, int courseIDs)
        {
            LecturerID = LecturerID;
            CourseIDs = courseIDs;
        }
    }

}

