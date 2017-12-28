using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;

namespace StudentManager
{
    [DataContract]
    public class Lecturer : Person
    {
        /// <summary>
        /// Höchster Abschluss eines Dozenten. Überprüfung der Eingabe
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

        public Lecturer(
            DBManager manager,
            string firstName,
            string lastName,
            DateTime birthdate,
            Degree degree,
            string street,
            string houseNumber,
            int zip,
            string city) : base(firstName,lastName,birthdate,street,houseNumber,zip,city)
        {
            ID = manager.Lecturers.Count;
            Degree = degree;
        }
    }
}
