using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace StudentManager
{
    [DataContract]
    public class Student : Person
    {
        private static int counter = 0;
        /// <summary>
        /// Semester, in dem sich ein Student befindet. Überprüfung der Eingabe.
        /// </summary>
        [DataMember]
        private Semester semester;
        public Semester Semester
        {
            get { return semester; }
            set
            {
                if (Validation.CheckSemesterComboboxInput(value))
                {
                    semester = value;
                    NotifyPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Angestrebter Abschluss eines Studenten. Überprüfung der Eingabe.
        /// </summary>
        [DataMember]
        private Degree degree;
        public Degree Degree
        {
            get { return degree; }
            set
            {
                if (Validation.CheckDegreeComboboxInput(value))
                {
                    degree = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public Student(
            string firstName,
            string lastName,
            DateTime birthdate,
            Degree degree,
            string street,
            string houseNumber,
            int zip,
            string city,
            Semester semester) : base(counter++, firstName, lastName, birthdate, street, houseNumber, zip, city)
        {
            Degree = degree;
            Semester = semester;
        }
    }
}
