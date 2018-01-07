﻿using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;
using System.Collections.Generic;

namespace StudentManager
{
    /// <summary>
    /// Bedeutung für Dozenten:
    /// Höchster erreichter Abschluss.
    /// 
    /// Bedeutung für Studierende:
    /// Angestreber Abschluss.
    /// </summary>
    public enum Degree
    {
        None = 0,
        BachelorOfArts,
        BachelorOfScience,
        MasterOfArts,
        MasterOfScience,
        PHD,
        Professor
    }

    /// <summary>
    /// Mögliche Semesterkombinationen bis 17 Jahre zurück und 13 Jahre in die Zukunft.
    /// </summary>
    public enum Semester
    {
        None = 0,
        SS00, WS0001, SS01, WS0102, SS02, WS0203, SS03, WS0304, SS04, WS0405, SS05, WS0506,
        SS06, WS0607, SS07, WS0708, SS08, WS0809, SS09, WS0910, SS10, WS1011, SS11, WS1112,
        SS12, WS1213, SS13, WS1314, SS14, WS1415, SS15, WS1516, SS16, WS1617, SS17, WS1718,
        SS18, WS1819, SS19, WS1920, SS20, WS2021, SS21, WS2122, SS22, WS2223, SS23, WS2324,
        SS24, WS2425, SS25, WS2526, SS26, WS2627, SS27, WS2728, SS28, WS2829, SS29, WS2939,
        SS30
    }
    
    [DataContract]
    public class DBManager
    {
        // TODO: Enumerations für semester und Degree haben keinen String override.
        // TODO: Enumeration Tostring hat leerzeichen vor semesterkürzel auch in Tostring methode von Kursen
        // TODO: Datepicker mit erstem gültigen Datum beginnen lassen.

        //Enum ToString()
        public static string SemesterToString(Semester semester)
        {
            string enumTemp = Enum.GetName(typeof(Semester), semester);

            if (enumTemp.Length == 6)
            {
                return enumTemp.Substring(0, 2) + " " + enumTemp.Substring(2, 2) + "/" + enumTemp.Substring(4, 2);
            }
            if (enumTemp.Length == 4)
            {
                return enumTemp.Substring(0, 2) + " " + enumTemp.Substring(2, 2);
            }
            else
            {
                return "";
            }
        }

        public static string DegreeToString(Degree degree)
        {
            string enumTemp = Enum.GetName(typeof(Degree), degree);

            switch (degree)
            {
                case Degree.BachelorOfArts:
                    return "Bachelor of Arts";

                case Degree.BachelorOfScience:
                    return "Bachelor of Science";

                case Degree.MasterOfArts:
                    return "Bachelor of Science";

                case Degree.MasterOfScience:
                    return "Master of Science";

                case Degree.PHD:
                    return "Promotion (Dr.)";

                case Degree.Professor:
                    return "Habilitation (Professor)";

                default:
                    return "";
            }
        }
        
        //Collections
        /// <summary>
        /// Collection hält die in der Anwendung dargestellten Studenten und Dozenten. Aktualisierung erfolgt bei
        /// Änderung - wichtig für aktuelle Darstellung in im Applikationsfenster
        /// </summary>
        #region Datacontract Properties
        [DataMember]
        public ObservableCollection<Student> Students { get; set; } = new ObservableCollection<Student>();
        [DataMember]
        public ObservableCollection<Lecturer> Lecturers { get; set; } = new ObservableCollection<Lecturer>();
        [DataMember]
        public ObservableCollection<Holds> Holds { get; set; } = new ObservableCollection<Holds>();
        [DataMember]
        public ObservableCollection<Listens> Listens { get; set; } = new ObservableCollection<Listens>();
        [DataMember]
        public ObservableCollection<Course> Courses { get; set; } = new ObservableCollection<Course>();
        #endregion

        //Konstruktor
        public DBManager()
        {

        }

        /// <summary>
        /// Klasse für komplexen Datentyp ZIP. Dieser wird gebraucht, um die Konvertierung der Benutzereingabe
        /// von ZIP im Form von String zu Interger vorzunehmen, die nicht im Setter der Property erfolgen kann.
        /// </summary>
        public class ZIP
        {
            public int Number { get; private set; }
            public ZIP(string zipInput)
            {
                if (Validation.CheckMemberZIPInput(zipInput))
                    Number = Int32.Parse(zipInput);
            }

            public override string ToString()
            {
                return Number.ToString();
            }
        }

        //Collections Methoden
        public void AddStudent(
            string firstName,
            string lastName,
            DateTime? birthdate,
            Degree degree,
            string street,
            string houseNumber,
            string zip,
            string city,
            Semester semester)
        {
            DateTime castBirthdate = (DateTime)birthdate;
            Students.Add(new Student(this, firstName, lastName, castBirthdate, degree, street, houseNumber, new ZIP(zip).Number, city, semester));
        }

        public void AddLecturer(
            string firstName,
            string lastName,
            DateTime? birthdate,
            Degree degree,
            string street,
            string houseNumber,
            string zip,
            string city)
        {
            DateTime castBirthdate = (DateTime)birthdate;
            Lecturers.Add(new Lecturer(this, firstName, lastName, castBirthdate, degree, street, houseNumber, new ZIP(zip).Number, city));
        }

        public void AddCourse(
            string name,
            string description,
            DateTime? startDate,
            DateTime? endDate,
            Semester semester)
        {
            DateTime castStartDate = (DateTime)startDate;
            DateTime castEndDate = (DateTime)endDate;
            Courses.Add(new Course(this, name, description, semester, castStartDate, castEndDate));
        }

        // TODO: Löschfunktion einbinden
        /// <summary>
        /// Entfernt einee Person aus "Students" oder "Lecturers"
        /// </summary>
        /// <param name="person"></param>
        public void RemovePerson(Person person)
        {
            if (person is Student)
                Students.Remove((Student)person);
            else
                Lecturers.Remove((Lecturer)person);
        }

        public void JoinStudentAndCourse(Student student, ObservableCollection<Course> tempCourses)
        {
            foreach (Course course in tempCourses)
            {
                Listens listensEntry = new Listens(student.ID, course.ID);
                if (student != null && course != null && !Listens.Contains(listensEntry))
                {
                    Listens.Add(listensEntry);
                }
            }
        }

        public void JoinStudentAndCourse(Student student, Course course)
        {
            Listens listensEntry = new Listens(student.ID, course.ID);
            if (student != null && course != null && !Listens.Contains(listensEntry))
                Listens.Add(listensEntry);
        }

        public void JoinLecturerAndCourse(Lecturer lecturer, ObservableCollection<Course> tempCourses)
        {
            foreach (Course course in tempCourses)
            {
                Holds holdsEntry = new Holds(lecturer.ID, course.ID);
                if (lecturer != null && course != null && !Holds.Contains(holdsEntry))
                    Holds.Add(holdsEntry);
            }
        }

        public void JoinLecturerAndCourse(Lecturer lecturer, Course course)
        {
            Holds holdsEntry = new Holds(lecturer.ID, course.ID);
            if (lecturer != null && course != null && !Holds.Contains(holdsEntry))
                Holds.Add(holdsEntry);
        }

        //Datenbank
        /// <summary>
        /// Serialisiert das DBManager Objekt, dass alle Listen enthält und schreibt es in die Datenbank
        /// </summary>
        public void SaveToDatabase()
        {
            MemoryStream stream = new MemoryStream();
            DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(DBManager));
            jsonSerializer.WriteObject(stream, this);
            stream.Position = 0;

            using (FileStream file = new FileStream(@"..\..\Data\Database.json", FileMode.Create, FileAccess.Write))
                stream.CopyTo(file);
        }

        /// <summary>
        /// Deserialisiert den Datenbankinhalt und rekonstruiert das DBManager objekt. Ist keine Datenbank vorhanden,
        /// wir der Konstruktor aufgerufen.
        /// </summary>
        /// <returns>Ein DBManager Objekt</returns>
        public static DBManager LoadFromDatabase()
        {
            MemoryStream stream = new MemoryStream();
            DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(DBManager));
            try
            {
                using (FileStream file = new FileStream(@"..\..\Data\Database.json", FileMode.Open, FileAccess.Read))
                    file.CopyTo(stream);
                stream.Position = 0;
                return (DBManager)jsonSerializer.ReadObject(stream);
            }
            catch (FileNotFoundException)
            {
                return new DBManager();
            }
            
            
        }
    }
}
