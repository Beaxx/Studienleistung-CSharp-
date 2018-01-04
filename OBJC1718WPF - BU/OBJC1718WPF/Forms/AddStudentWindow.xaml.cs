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

        public AddStudentWindow(DBManager dBManager)
        {
            this.dBManager = dBManager;
            InitializeComponent();
            SemesterComboBox.ItemsSource = Enum.GetValues(typeof(Semester));
            DegreeComboBox.ItemsSource = Enum.GetValues(typeof(Degree));
            CourseComboBox.ItemsSource = dBManager.Courses;
            CourseListbox.ItemsSource = tempData.CourseTempCollection;
        }

        private void ConfirmationButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                dBManager.AddStudent(
                FirstnameTextbox.Text,
                LastnameTextbox.Text,
                BirthdateDatepicker.SelectedDate,
                (Degree)DegreeComboBox.SelectedItem,
                StreetTextbox.Text,
                HouseNumberTextbox.Text,
                ZIPTextbox.Text,
                CityTextbox.Text,
                (Semester)SemesterComboBox.SelectedItem);
            }
            catch (Exception)
            {

                throw;
            }
            Close();
            
        }

        private void CourseComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            tempData.CourseTempCollection.Add((Course)CourseComboBox.SelectedItem);
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            tempData.CourseTempCollection.Remove((Course)CourseListbox.SelectedItem);
        }
    }
}
