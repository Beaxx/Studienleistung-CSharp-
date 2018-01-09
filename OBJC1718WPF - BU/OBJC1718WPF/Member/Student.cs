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
        /// <summary>
        /// Semester, in dem sich ein Student befindet.
        /// </summary>
        [DataMember]
        private Semester semester;
        public Semester Semester
        {
            get => semester;
            set
            {
                semester = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Angestrebter Abschluss eines Studenten.
        /// </summary>
        [DataMember]
        private Degree degree;
        public Degree Degree
        {
            get => degree;
            set
            {
                degree = value;
                NotifyPropertyChanged();
            }
        }

        public Student(
            DBManager manager,
            string firstName,
            string lastName,
            DateTime birthdate,
            Degree degree,
            string street,
            string houseNumber,
            string zip,
            string city,
            Semester semester) : base(firstName, lastName, birthdate, street, houseNumber, zip, city)
        {
            ID = manager.Students.Count;
            Degree = degree;
            Semester = semester;
        }

        /// <summary>
        /// Erweiterung der ObjectToText aus der Basisklasse + Semester und Abschluss
        /// </summary>
        /// <returns>Alle Propertys einer Person als verketten String + Erweiterung</returns>
        public override string ObjectToText()
        {
            string output = base.ObjectToText();
            return (output + "." + Semester + "." + Degree).ToLower();
        }
    }
}
