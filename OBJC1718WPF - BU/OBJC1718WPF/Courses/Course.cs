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
        /// <summary>
        /// Die ID eines Kurses
        /// </summary>
        [DataMember]
        public int ID { get; set; }

        /// <summary>
        /// Name eines Kurses.
        /// </summary>
        [DataMember]
        private string name;
        public string Name
        {
            get => name;
            set
            {
                    name = value;
                    NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Beschreibung eines Kurses.
        ///  </summary>
        [DataMember]
        private string description;
        public string Description
        {
            get => description;
            set
            {
                description = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Semester, in dem der Kurs stattfindet.
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
        /// Das Startdatum eines Kurses
        /// </summary>
        [DataMember]
        private DateTime startDate;
        public DateTime StartDate
        {
            get => startDate;
            set
            {
                startDate = value;
                NotifyPropertyChanged();
            }
        }

        /// <summary>
        /// Das Enddatum eines Kurses.
        /// </summary>
        [DataMember]
        private DateTime endDate;
        public DateTime EndDate
        {
            get => endDate;
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

        /// <summary>
        /// String override zur Darstellung des Kurses
        /// </summary>
        /// <returns>Kursname + Semester</returns>
        public override string ToString()
        {
            return Name + " (" + DBManager.SemesterToString(Semester) + ")";
        }

        /// <summary>
        /// String-Serialisierung eines Kurses, um diesen für die Suchfunktion durchsuchbar zu machen.
        /// </summary>
        /// <returns>Alle Propertys eines Kurses als verketten String</returns>
        public virtual string ObjectToText()
        {
            string output =
                ID + "." +
                Name + "." +
                Description + "." +
                Semester + "." +
                StartDate + "." +
                EndDate;

            return output.ToLower();
        }

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
