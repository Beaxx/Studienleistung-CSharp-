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
        /// Höchster Abschluss eines Dozenten.
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

        public Lecturer(
            DBManager manager,
            string firstName,
            string lastName,
            DateTime birthdate,
            Degree degree,
            string street,
            string houseNumber,
            string zip,
            string city) : base(firstName, lastName, birthdate, street, houseNumber, zip, city)
        {
            ID = manager.Lecturers.Count;
            Degree = degree;
        }

        /// <summary>
        /// Erweiterung der ObjectToText aus der Basisklasse + Abschluss
        /// </summary>
        /// <returns>Alle Propertys einer Person als verketten String + Erweiterung</returns>
        public override string ObjectToText()
        {
            string output = base.ObjectToText();
            return (output + "." + degree).ToLower();
        }
    }
}
