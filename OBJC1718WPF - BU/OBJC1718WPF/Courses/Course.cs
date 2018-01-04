using System;
using System.Collections.Generic;
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
    public class Course: INotifyPropertyChanged
    {
        [DataMember]
        public int ID { get; set; }

        /// <summary>
        /// Name eines Kurses. Überprüfung des Eingabeparameters
        /// </summary>
        [DataMember]
        private string name;
        public string Name
        {
            get { return name; }
            private set
            {
                if (Validation.CheckInputAllLetters(value))
                {
                    name = value;
                    NotifyPropertyChanged();
                }

            }
        }

        /// <summary>
        /// Beschreibung eines Kurses. Überprüfung des Eingabeparameters
        /// </summary>
        [DataMember]
        private string description;
        public string Description
        {
            get { return description; }
            private set
            {
                if (value.Length < 5)
                {
                    throw new ArgumentException("Bitte geben Sie eine längere Beschreibung ein");
                }
                else
                {
                    description = value;
                    NotifyPropertyChanged();
                }

            }
        }

        /// <summary>
        /// Semester, in dem der Kurs stattfindet. Überprüfung der Eingabe.
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

        [DataMember]
        private DateTime startDate;
        public DateTime StartDate
        {
            get { return startDate; }
            set
            {
                startDate = value;
                NotifyPropertyChanged();
            }
        }

        [DataMember]
        private DateTime endDate;
        public DateTime EndDate
        {
            get { return endDate; }
            set
            {
                endDate = value;
                NotifyPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public Course(DBManager manager, string name, string description, Semester semester, DateTime startDate, DateTime endDate)
        {
            ID = manager.Courses.Count;
            Name = name;
            Description = description;
            Semester = semester;
            StartDate = startDate;
            EndDate = endDate;
        }

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
