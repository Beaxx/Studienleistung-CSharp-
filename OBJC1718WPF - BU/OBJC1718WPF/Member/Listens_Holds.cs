using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Converters;

namespace StudentManager
{
    /// <summary>
    /// Listens stellt die Verknüpfung zwischen Kurs und Student dar, in dem die IDs der 
    /// beiden Objekte gekoppelt werden. So werden Studenten und Kurse selbst von ihren Zugehörigkeiten entkoppelt.
    /// </summary>
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

        public override bool Equals(object obj)
        {
            if (obj is Listens temp)
            {
                return CourseID == temp.CourseID && StudentID == temp.StudentID;
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    /// <summary>
    /// Holds stellt die Verknüpfung zwischen Kurs und Dozent dar, in dem die IDs der 
    /// beiden Objekte gekoppelt werden. So werden Dozenten und Kurse selbst von ihren Zugehörigkeiten entkoppelt.
    /// </summary>
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
            LecturerID = lecturerID;
            CourseID = courseID;
        }

        public override bool Equals(object obj)
        {
            if (obj is Holds temp)
            {
                return CourseID == temp.CourseID && LecturerID == temp.LecturerID;
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

}

