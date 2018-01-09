using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaktionslogik des Hinzufügen Dialogs
    /// </summary>
    public partial class AddCourseWindow : Window
    {
        private DBManager dBManager;
        private RuntimeTempData tempData = new RuntimeTempData();

        /// <summary>
        /// Konstruktor des "Kurs Hinzufügen" Dialogs
        /// Initialisiert XAML Bindungen.
        /// </summary>
        public AddCourseWindow(DBManager dBManager)
        {
            this.dBManager = dBManager;
            
            InitializeComponent();
            SemesterComboBox.ItemsSource = Enum.GetValues(typeof(Semester));
            LecturerComboBox.ItemsSource = dBManager.Lecturers;
            StudentComboBox.ItemsSource = dBManager.Students.Except(tempData.StudentTempCollection);
            StudentListbox.ItemsSource = tempData.StudentTempCollection;
        }

        /// <summary>
        /// Event für den Klick des "OK"-Buttons. Fügt den Kurs der Datenbank hinzu und 
        /// richtet die Bindungen ziwschen Dozent und Kursen sowie Studenten und Kurs ein, wenn diese erstellt wurden.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ConfirmationButton_Click(object sender, RoutedEventArgs e)
        {
                dBManager.AddCourse(
                NameTextbox.Text,
                DescriptionTextbox.Text,
                StartDateDatepicker.SelectedDate,
                EndDateDatepicker.SelectedDate,
                (Semester)SemesterComboBox.SelectedItem);
            
            dBManager.JoinLecturerAndCourse((Lecturer)LecturerComboBox.SelectedItem, dBManager.Courses.Last());
            dBManager.JoinStudentsAndCourse((Student)StudentComboBox.SelectedItem, dBManager.Courses.Last());

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
            if (!StudentListbox.Items.Contains(StudentComboBox.SelectedItem))
                tempData.StudentTempCollection.Add((Student)StudentComboBox.SelectedItem);
        }

        /// <summary>
        /// Event für das Klicken des X-Buttons, löscht den ausgewählten Listeneintrag.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            tempData.StudentTempCollection.Remove((Student)StudentListbox.SelectedItem);
        }
    }
}
