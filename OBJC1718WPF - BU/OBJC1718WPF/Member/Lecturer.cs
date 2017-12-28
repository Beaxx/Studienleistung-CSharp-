using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManager
{
    public class Lecturer : Person
    {
        private static int counter = 0;
        /// <summary>
        /// Höchster Abschluss eines Dozenten. Überprüfung der Eingabe
        /// </summary>
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
            string firstName,
            string lastName,
            DateTime birthdate,
            Degree degree,
            string street,
            string houseNumber,
            int zip,
            string city) : base(counter++, firstName,lastName,birthdate,street,houseNumber,zip,city)
        {
            Degree = degree;
        }
    }
}
