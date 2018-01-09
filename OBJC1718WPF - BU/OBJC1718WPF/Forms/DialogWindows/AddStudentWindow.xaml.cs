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
    /// Interaktionslogik des Hinzufügen Dialogs
    /// </summary>
    public partial class AddStudentWindow : Window
    {
        private DBManager dBManager;
        private RuntimeTempData tempData = new RuntimeTempData();

        /// <summary>
        /// Konstruktor des "Student Hinzufügen Dialogs
        /// Initialisiert XAML Bindungen.
        /// </summary>
        /// <param name="dBManager"></param>
        public AddStudentWindow(DBManager dBManager)
        {
            this.dBManager = dBManager;
            InitializeComponent();
            SemesterComboBox.ItemsSource = Enum.GetValues(typeof(Semester));
            DegreeComboBox.ItemsSource = Enum.GetValues(typeof(Degree));
            BirthdateDatepicker.DisplayDateEnd = DateTime.Today.AddYears(-14);

            CourseComboBox.ItemsSource = dBManager.Courses;
            CourseListbox.ItemsSource = tempData.CourseTempCollection;
        }

        /// <summary>
        /// Event für den Klick des "OK"-Buttons. Fügt den Studenten der Datenbank hinzu und 
        /// richtet die Bindungen ziwschen Student und Kursen ein, wenn diese erstellt wurden
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ConfirmationButton_Click(object sender, RoutedEventArgs e)
        {
            dBManager.AddStudent(
                FirstnameTextbox.Text,
                LastnameTextbox.Text,
                BirthdateDatepicker.SelectedDate,
                (Degree) DegreeComboBox.SelectedItem,
                StreetTextbox.Text,
                HouseNumberTextbox.Text,
                ZIPTextbox.Text,
                CityTextbox.Text,
                (Semester) SemesterComboBox.SelectedItem);

            dBManager.JoinStudentsAndCourse(dBManager.Students.Last(), tempData.CourseTempCollection);

            Close();
        }

        /// <summary>
        /// Event für das Auswählen eines Kurses aus der Kurs-Kombobox.
        /// Übernimmt Kurs in die temporären Daten.
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
            tempData.CourseTempCollection.Remove((Course)CourseListbox.SelectedItem);
        }
    }
}
