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
    public partial class AddLecturerWindow : Window
    {
        private DBManager dBManager;
        private RuntimeTempData tempData = new RuntimeTempData();

        /// <summary>
        /// Konstruktor des "Dozent Hinzufügen" Dialogs
        /// Initialisiert XAML Bindungen.
        /// </summary>
        public AddLecturerWindow(DBManager dBManager)
        {
            this.dBManager = dBManager;
            InitializeComponent();
            DegreeComboBox.ItemsSource = Enum.GetValues(typeof(Degree));
            CourseListbox.ItemsSource = tempData.CourseTempCollection;
            CourseComboBox.ItemsSource = dBManager.Courses.Except(tempData.CourseTempCollection);
            BirthdateDatepicker.DisplayDateEnd = DateTime.Today.AddYears(-14);
        }

        /// <summary>
        /// Event für den Klick des "OK"-Buttons. Fügt den Dozenten der Datenbank hinzu und 
        /// richtet die Bindungen ziwschen Dozent und Kursen ein, wenn diese erstellt wurden
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ConfirmationButton_Click(object sender, RoutedEventArgs e)
        {
            dBManager.AddLecturer(
                FirstnameTextbox.Text,
                LastnameTextbox.Text,
                BirthdateDatepicker.SelectedDate,
                (Degree) DegreeComboBox.SelectedItem,
                StreetTextbox.Text,
                HouseNumberTextbox.Text,
                ZIPTextbox.Text,
                CityTextbox.Text);

            dBManager.JoinLecturerAndCourse(dBManager.Lecturers.Last(), tempData.CourseTempCollection);

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
