using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
        [DataMember]
        public int ID { get; set; }

        /// <summary>
        /// Vorname einer Person. Überprüfung des Eingabeparameters
        /// </summary>
        [DataMember]
        private string firstName;
        public string FirstName
        {
            get { return firstName; }
            set
            {
                if (Validation.CheckInputAllLetters(value))
                {
                    firstName = value;
                    NotifyPropertyChanged();
                }

            }
        }

        /// <summary>
        /// Nachname einer Person. Übeprüfung des Eingabeparameter
        /// </summary>
        [DataMember]
        private string lastName;
        public string LastName
        {
            get { return lastName; }
            set
            {
                if (Validation.CheckInputAllLetters(value))
                {
                    lastName = value;
                    NotifyPropertyChanged();
                }
            }
        }


        /// <summary>
        /// Geburtsdatum einer Person. Überprüfung des Eingabeparameters
        /// </summary>
        [DataMember]
        private DateTime birthdate;
        public DateTime Birthdate
        {
            get { return birthdate; }
            set
            {
                if (Validation.CheckMemberBirthdateInput(value))
                {
                    birthdate = (DateTime)value;
                    NotifyPropertyChanged();
                }
                
            }
        }

        // Adresse
        /// <summary>
        /// Wohnstraße einer Person. Überprüfung des Eingabeparameters
        /// </summary>
        [DataMember]
        private string street;
        public string Street
        {
            get { return street; }
            set
            {
                if (Validation.CheckInputAllLetters(value))
                {
                    street = value;
                    NotifyPropertyChanged();
                }

            }
        }

        /// <summary>
        /// Hausnummer einer Person. Überprüfung des Eingabeparameters
        /// </summary>
        [DataMember]
        private string houseNumber;
        public string HouseNumber
        {
            get { return houseNumber; }
            set
            {
                if (Validation.CheckMemberHouseNumberInput(value))
                {
                    houseNumber = value;
                    NotifyPropertyChanged();
                } 
            }
        }

        /// <summary>
        /// Postleitzahl einer Person. Überprüfung erfolgt in Methode die 
        /// den Konstruktor aufruft, da für Überprüfung ein komplexer Datentyp verwendet wird.
        /// </summary>
        [DataMember]
        public int Zip { get; set; }

        /// <summary>
        /// Stadt einer Person. Überprüfung des Eingabeparameters
        /// </summary>
        [DataMember]
        private string city;
        public string City
        {
            get { return city; }
            set
            {
                if (Validation.CheckInputAllLetters(value))
                {
                    city = value;
                    NotifyPropertyChanged();
                }
                    
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
            int zip,
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

        public Person(
            string firstName, 
            string lastName, 
            DateTime birthdate,
            string street, 
            string houseNumber, 
            int zip, 
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

        public override string ToString()
        {
            return FirstName + " " + LastName;
        }

        protected void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

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
    }
}
