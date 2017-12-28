﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace StudentManager
{
    public abstract class Person : INotifyPropertyChanged
    {
        public int ID { get; set; }

        /// <summary>
        /// Vorname einer Person. Überprüfung des Eingabeparameters
        /// </summary>
        private string firstName;
        public string FirstName
        {
            get { return firstName; }
            private set
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
        private string lastName;
        public string LastName
        {
            get { return lastName; }
            private set
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
        private DateTime birthdate;
        public DateTime Birthdate
        {
            get { return birthdate; }
            private set
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
        private string street;
        public string Street
        {
            get { return street; }
            private set
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
        private string houseNumber;
        public string HouseNumber
        {
            get { return houseNumber; }
            private set
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
        /// den Konstruktor aufruft
        /// </summary>
        public int Zip { get; set; }

        /// <summary>
        /// Stadt einer Person. Überprüfung des Eingabeparameters
        /// </summary>
        private string city;

        public event PropertyChangedEventHandler PropertyChanged;

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

        public Person(string firstName, string lastName, DateTime birthdate, string street, string houseNumber, int zip, string city)
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
    }
}
