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
    /// Interaktionslogik des Student-Bearbeiten Dialogs
    /// </summary>
    public partial class StudentDetailWindow : Window
    {
        // TODO: Code in DBManager
        private DBManager dBManager;
        private Student student;
        private RuntimeTempData tempData = new RuntimeTempData();
        private RuntimeTempData tempDataDeleted = new RuntimeTempData();

        /// <summary>
        /// Konstruktor des "Student Bearbeiten" Dialogs
        /// Initialisiert XAML Bindungen
        /// </summary>
        /// <param name="dBManager">Eine Instanz des DBManagers</param>
        /// <param name="member">Der zu bearbeitende Student</param>
        public StudentDetailWindow(DBManager dBManager, Student member)
        {
            this.dBManager = dBManager;
            student = member;
            DataContext = member;

            InitializeComponent();

            Title = member.ToString();
            FirstnameTextbox.Text = member.FirstName;
            LastnameTextbox.Text = member.LastName;

            BirthdateDatepPicker.DisplayDateStart = DateTime.Today.AddYears(-14);
            BirthdateDatepPicker.SelectedDate = member.Birthdate;

            StreetTextbox.Text = member.Street;
            HouseNumberTextbox.Text = member.HouseNumber;
            ZIPTextbox.Text = member.Zip.ToString();
            CityTextbox.Text = member.City;

            DegreeComboBox.ItemsSource = Enum.GetValues(typeof(Degree));
            DegreeComboBox.SelectedItem = member.Degree;

            SemesterComboBox.ItemsSource = Enum.GetValues(typeof(Semester));
            SemesterComboBox.SelectedItem = member.Semester;

            //Querry für Kursliste
            var enrolledInCourses = from Listens in dBManager.Listens
                                    from Course in dBManager.Courses
                                    where (Listens.StudentID == student.ID && Listens.CourseID == Course.ID)
                                    select Course;

            var enrolledInCoursesList = enrolledInCourses.ToList();
            foreach (var course in enrolledInCoursesList)
            {
                tempData.CourseTempCollection.Add(course);
            }

            //Combobox enthält nur Elemente, die nicht bereits in der Listbox sind.
            CourseComboBox.ItemsSource = dBManager.Courses.Except(tempData.CourseTempCollection);
            CourseListbox.ItemsSource = tempData.CourseTempCollection;
        }

        /// <summary>
        /// Event für den Klick des "OK"-Buttons. Fügt die Änderungen am Studenten der Datenbank hinzu. 
        /// Ändert die Bindungen zwischen Studenten und Kurs, wenn es Änderungen gab.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ConfirmationButton_Click(object sender, RoutedEventArgs e)
        {
            
            // Übertragen der Änderungen am Studenten in die Datenbank
            student.FirstName = FirstnameTextbox.Text;
            student.LastName = LastnameTextbox.Text;
            student.Birthdate = (DateTime)BirthdateDatepPicker.SelectedDate;
            student.Degree = (Degree)DegreeComboBox.SelectedItem;
            student.Street = StreetTextbox.Text;
            student.HouseNumber = HouseNumberTextbox.Text;
            student.Zip = new DBManager.ZIP(ZIPTextbox.Text).Number;
            student.City = CityTextbox.Text;
            student.Semester = (Semester) SemesterComboBox.SelectedItem;

            //Entfernen der gelöschten Kurseverbindungen aus der Datenbank
            var query = from Course in tempDataDeleted.CourseTempCollection
                        join Listens in dBManager.Listens on student.ID equals Listens.StudentID
                        where (Course.ID == Listens.CourseID)
                        select Listens;

            List<Listens> toRemove = query.ToList();
            foreach (Listens item in toRemove)
            {
                dBManager.Listens.Remove(item);
            }

            //Hinzufügen der neuen Kursverbindungen in die Datenbank
            dBManager.JoinStudentsAndCourse(student, tempData.CourseTempCollection);

            Close();
        }

        /// <summary>
        /// Event für das Auswählen eines Kurses aus der Kurs-Kombobox.
        /// Übernimmt den Kurs in die temporären Daten.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CourseComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!CourseListbox.Items.Contains(CourseComboBox.SelectedItem))
                tempData.CourseTempCollection.Add((Course)CourseComboBox.SelectedItem);
        }

        /// <summary>
        /// Event für das Klicken des X-Buttons, löscht den ausgewählten Listeneintrag.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            tempDataDeleted.CourseTempCollection.Add((Course)CourseListbox.SelectedItem);
            tempData.CourseTempCollection.Remove((Course)CourseListbox.SelectedItem);
        }
    }
}
