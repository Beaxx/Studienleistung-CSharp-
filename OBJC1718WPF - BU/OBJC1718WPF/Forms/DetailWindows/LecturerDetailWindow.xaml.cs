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
    /// Interaktionslogik des Dozent-Bearbeiten Dialogs
    /// </summary>
    public partial class LecturerDetailWindow : Window
    {
        // TODO: Code in DBManager
        private DBManager dBManager;
        private Lecturer lecturer;
        private RuntimeTempData tempData = new RuntimeTempData();
        private RuntimeTempData tempDataDeleted = new RuntimeTempData();

        /// <summary>
        /// Konstruktor des "Dozent Bearbeiten" Dialogs
        /// Initialisiert XAML Bindungen
        /// </summary>
        /// <param name="dBManager">Eine Instanz des DBManagers</param>
        /// <param name="member">Der zu bearbeitende Dozent</param>
        public LecturerDetailWindow(DBManager dBManager, Lecturer member)
        {
            this.dBManager = dBManager;
            lecturer = member;
            DataContext = member;

            InitializeComponent();

            Title = member.ToString();
            FirstnameTextbox.Text = member.FirstName;
            LastnameTextbox.Text = member.LastName;

            BirthdateDatepicker.DisplayDateEnd = DateTime.Today.AddYears(-14);
            BirthdateDatepicker.SelectedDate = member.Birthdate;

            StreetTextbox.Text = member.Street;
            HouseNumberTextbox.Text = member.HouseNumber;
            ZIPTextbox.Text = member.Zip.ToString();
            CityTextbox.Text = member.City;

            DegreeComboBox.ItemsSource = Enum.GetValues(typeof(Degree));
            DegreeComboBox.SelectedItem = member.Degree;

            //Querry für Kursliste
            var holdingCourses = from Holds in dBManager.Holds
                                 from Course in dBManager.Courses
                                 where (Holds.LecturerID == lecturer.ID && Holds.CourseID == Course.ID)
                                 select Course;

            var holdingCoursesList = holdingCourses.ToList();
            foreach (var course in holdingCoursesList)
            {
                tempData.CourseTempCollection.Add(course);
            }

            //Combobox enthält nur Elemente, die nicht bereits in der Listbox sind.
            CourseListbox.ItemsSource = tempData.CourseTempCollection;
            CourseComboBox.ItemsSource = dBManager.Courses.Except(tempData.CourseTempCollection);
        }

        /// <summary>
        /// Event für den Klick des "OK"-Buttons. Fügt die Änderungen am Dozenten der Datenbank hinzu. 
        /// Ändert die Bindungen zwischen Dozenten und Kurs, wenn es Änderungen gab.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ConfirmationButton_Click(object sender, RoutedEventArgs e)
        {
            // Übertragen der Änderungen am Dozenten in die Datenbank
            lecturer.FirstName = FirstnameTextbox.Text;
            lecturer.LastName = LastnameTextbox.Text;
            lecturer.Birthdate = (DateTime)BirthdateDatepicker.SelectedDate;
            lecturer.Degree = (Degree)DegreeComboBox.SelectedItem;
            lecturer.Street = StreetTextbox.Text;
            lecturer.HouseNumber = HouseNumberTextbox.Text;
            lecturer.Zip = ZIPTextbox.Text;
            lecturer.City = CityTextbox.Text;

            //Entfernen der gelöschten Kurseverbindungen aus der Datenbank
            var query = from Course in tempDataDeleted.CourseTempCollection
                        join Holds in dBManager.Holds on lecturer.ID equals Holds.LecturerID
                        where (Course.ID == Holds.CourseID)
                        select Holds;

            List<Holds> toRemove = query.ToList();
            foreach (Holds item in toRemove)
            {
                dBManager.Holds.Remove(item);
            }

            //Hinzufügen der neuen Kursverbindungen in die Datenbank
            dBManager.JoinLecturerAndCourse(lecturer, tempData.CourseTempCollection);

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

