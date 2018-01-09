using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;

namespace StudentManager
{
    [DataContract]
    public abstract class Person : INotifyPropertyChanged
    {
        /// <summary>
        /// ID einer Person.
        /// </summary>
        [DataMember]
        public int ID { get; set; }

        /// <summary>
        /// Vorname einer Person.
        /// </summary>
        [DataMember]
        private string firstName;
        public string FirstName
        {
            get => firstName;

            set
            {
                firstName = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Nachname einer Person.
        /// </summary>
        [DataMember]
        private string lastName;
        public string LastName
        {
            get => lastName;
            set
            {
                lastName = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Geburtsdatum einer Person.
        /// </summary>
        [DataMember]
        private DateTime birthdate;
        public DateTime Birthdate
        {
            get => birthdate;
            set
            {
                birthdate = (DateTime)value;
                NotifyPropertyChanged();  
            }
        }

        // Adresse
        /// <summary>
        /// Wohnstraße einer Person.
        /// </summary>
        [DataMember]
        private string street;
        public string Street
        {
            get => street;
            set
            { 
                street = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Hausnummer einer Person.
        /// </summary>
        [DataMember]
        private string houseNumber;
        public string HouseNumber
        {
            get => houseNumber;
            set
            {
                houseNumber = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Postleitzahl einer Person
        /// </summary>
        [DataMember]
        private string zip;
        public string Zip
        {
            get => zip;

            set
            {
                zip = value;
                NotifyPropertyChanged();
            }
        }

    /// <summary>
    /// Stadt einer Person.
    /// </summary>
    [DataMember]
        private string city;
        public string City
        {
            get => city;
            set
            {
                city = value;
                NotifyPropertyChanged();                   
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public Person(
            int id,
            string firstName,
            string lastName,
            DateTime birthdate,
            string street,
            string houseNumber,
            string zip,
            string city)
        {
            ID = id;
            FirstName = firstName;
            LastName = lastName;
            Birthdate = birthdate;
            Street = street;
            HouseNumber = houseNumber;
            Zip = zip;
            City = city;
        }

        // TODO: Überflüssig?
        public Person(
            string firstName, 
            string lastName, 
            DateTime birthdate,
            string street, 
            string houseNumber,
            string zip, 
            string city)
        {
            FirstName = firstName;
            LastName = lastName;
            Birthdate = birthdate;
            Street = street;
            HouseNumber = houseNumber;
            Zip = zip;
            City = city;
        }

        /// <summary>
        /// Tostring Überladung zur Darstellung einer Person mit Vor- und Nachnamen
        /// </summary>
        /// <returns>Vor und Namname einer Person</returns>
        public override string ToString()
        {
            return FirstName + " " + LastName;
        }

        /// <summary>
        /// String-Serialisierung einer Person, um diese für die Suchfunktion durchsuchbar zu machen.
        /// </summary>
        /// <returns>Alle Propertys einer Person als verketten String</returns>
        public virtual string ObjectToText()
        {
            string output = 
                ID + "." +
                firstName + "." +
                lastName + "." +
                Birthdate + "." +
                street + "." +
                houseNumber + "." +
                Zip + "." +
                city;

            return output.ToLower();
        }

        protected void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
