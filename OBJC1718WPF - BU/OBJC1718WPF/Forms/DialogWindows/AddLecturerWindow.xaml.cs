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

        public AddLecturerWindow(DBManager dBManager)
        {
            this.dBManager = dBManager;
            InitializeComponent();
            DegreeComboBox.ItemsSource = Enum.GetValues(typeof(Degree));
            CourseListbox.ItemsSource = tempData.CourseTempCollection;
            CourseComboBox.ItemsSource = dBManager.Courses.Except(tempData.CourseTempCollection);
            BirthdateDatepicker.DisplayDateStart = DateTime.Today.AddYears(-14);
        }

        private void ConfirmationButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                dBManager.AddLecturer(
                FirstnameTextbox.Text,
                LastnameTextbox.Text,
                BirthdateDatepicker.SelectedDate,
                (Degree)DegreeComboBox.SelectedItem,
                StreetTextbox.Text,
                HouseNumberTextbox.Text,
                ZIPTextbox.Text,
                CityTextbox.Text);
            }
            catch (Exception)
            {
                throw;
            }

            dBManager.JoinLecturerAndCourse(dBManager.Lecturers.Last(), tempData.CourseTempCollection);

            Close();
        }

        private void CourseComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!CourseListbox.Items.Contains((Course)CourseComboBox.SelectedItem))
                tempData.CourseTempCollection.Add((Course)CourseComboBox.SelectedItem);
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            tempData.CourseTempCollection.Remove((Course)CourseListbox.SelectedItem);
        }
    }
}
