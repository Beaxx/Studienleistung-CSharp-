using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace StudentManager
{
    /// <summary>
    /// Interaktionslogik des Kurs-Bearbeiten Dialogs
    /// </summary>
    public partial class CourseDetailWindow : Window
    {
        // TODO: Code in DBManager
        private DBManager dBManager;
        private Course course;
        private RuntimeTempData tempData = new RuntimeTempData();
        private RuntimeTempData tempDataDeleted = new RuntimeTempData();

        /// <summary>
        /// Konstruktor des "Dozent Bearbeiten" Dialogs
        /// Initialisiert XAML Bindungen
        /// </summary>
        /// <param name="dBManager">Eine Instanz des DBManagers</param>
        /// <param name="member">Der zu bearbeitende Kurs</param>
        public CourseDetailWindow(DBManager dBManager, Course course)
        {
            this.dBManager = dBManager;
            this.course = course;
            DataContext = course;

            InitializeComponent();

            Title = course.ToString();
            NameTextbox.Text = course.Name;
            DescriptionTextbox.Text = course.Description;
            StartdateDatePicker.SelectedDate = course.StartDate;
            EnddateDatePicker.SelectedDate = course.EndDate;

            SemesterComboBox.ItemsSource = Enum.GetValues(typeof(Semester));
            SemesterComboBox.SelectedItem = course.Semester;

            //Querry für Lecturer Combobox
            var courseLecturerHold = from Holds in dBManager.Holds
                                     where (course.ID == Holds.CourseID)
                                     select Holds;

            var courseLecturerList = courseLecturerHold.ToList();
            if (courseLecturerList.FirstOrDefault() != null) //Handling, falls noch kein Dozent zugewiesen ist.
            {
                Holds hold = (Holds) courseLecturerList.First();
                var courseLecturer = from Lecturer in dBManager.Lecturers
                                     where (Lecturer.ID == hold.LecturerID)
                                     select Lecturer;

                var lecturer = courseLecturer.ToList();
                tempData.LecturerTempCollection.Add(lecturer.First());
            }
           
            LecturerComboBox.ItemsSource = dBManager.Lecturers;
            LecturerComboBox.SelectedItem = tempData.LecturerTempCollection.FirstOrDefault(); //Es existiert immer nur ein Dozent.

            //Querry für Kursteilnehmer (Studenten)
            var courseAttendees = from Listens in dBManager.Listens
                                  from Student in dBManager.Students
                                  where (Listens.CourseID == course.ID && Listens.StudentID == Student.ID)
                                  select Student;

            var courseAttendeesList = courseAttendees.ToList();

            //Combobox enthält nur Elemente, die nicht bereits in der Listbox sind.
            foreach (var student in courseAttendeesList)
            {
                tempData.StudentTempCollection.Add(student);
            }

            StudentComboBox.ItemsSource = dBManager.Students.Except(tempData.StudentTempCollection);
            StudentListBox.ItemsSource = tempData.StudentTempCollection;
        }

        /// <summary>
        /// Event für den Klick des "OK"-Buttons. Fügt die Änderungen am Kurs der Datenbank hinzu. 
        /// Ändert die Bindungen zwischen Dozenten und Kurs sowie zwischen Studenten und Kurs, wenn es Änderungen gab.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ConfirmationButton_Click(object sender, RoutedEventArgs e)
        {
            course.Name = NameTextbox.Text;
            course.Description = DescriptionTextbox.Text;
            course.StartDate = (DateTime)StartdateDatePicker.SelectedDate;
            course.EndDate = (DateTime)EnddateDatePicker.SelectedDate;
            course.Semester = (Semester) SemesterComboBox.SelectedItem;

            //Entfernen der gelöschten Studentenverbindung aus der Datenbank
            var studentQuery = from Student in tempDataDeleted.StudentTempCollection
                        join Listens in dBManager.Listens on course.ID equals Listens.CourseID
                        where (Student.ID == Listens.StudentID)
                        select Listens;

            List<Listens> studentToRemove = studentQuery.ToList();
            foreach (Listens item in studentToRemove)
            {
                dBManager.Listens.Remove(item);
            }

            //Überschreiben des Dozenten
            if (LecturerComboBox.SelectedItem != null) // Handling, falls noch kein Dozent zugewiesen war.
            {
                Lecturer chosenLecturer = (Lecturer)LecturerComboBox.SelectedItem;
                if (tempData.LecturerTempCollection.FirstOrDefault() != null) 
                {
                    var lecturerQuery = from Holds in dBManager.Holds
                                        where (tempData.LecturerTempCollection.First().ID == Holds.LecturerID)
                                        select Holds;

                    List<Holds> lecturerToRemove = lecturerQuery.ToList();
                    if (lecturerToRemove.FirstOrDefault() != null)
                    { 
                        dBManager.Holds.Remove(lecturerToRemove.First());
                    }
                }
                dBManager.Holds.Add(new Holds(chosenLecturer.ID, course.ID));
            }

            //Hinzufügen der neuen Studentenverbindungen in die Datenbank
            dBManager.JoinStudentAndCourse(tempData.StudentTempCollection, course);

            Close();
        }

        /// <summary>
        /// Event für das Auswählen eines Studenten aus der Studenten-Kombobox.
        /// Übernimmt den Studenten in die temporären Daten.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StudentComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!StudentListBox.Items.Contains(StudentComboBox.SelectedItem))
                tempData.StudentTempCollection.Add((Student)StudentComboBox.SelectedItem);
        }

        /// <summary>
        /// Event für das Klicken des X-Buttons, löscht den ausgewählten Listeneintrag.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            tempDataDeleted.StudentTempCollection.Add((Student)StudentListBox.SelectedItem);
            tempData.StudentTempCollection.Remove((Student) StudentListBox.SelectedItem);
        }
    }
}
