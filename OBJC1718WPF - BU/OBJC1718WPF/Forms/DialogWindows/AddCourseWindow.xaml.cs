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
    /// Interaction logic for AddCourse.xaml
    /// </summary>
    public partial class AddCourseWindow : Window
    {
        private DBManager dBManager;
        private RuntimeTempData tempData = new RuntimeTempData();

        public AddCourseWindow(DBManager dBManager)
        {
            this.dBManager = dBManager;
            
            InitializeComponent();
            SemesterComboBox.ItemsSource = Enum.GetValues(typeof(Semester));
            LecturerComboBox.ItemsSource = dBManager.Lecturers;
            StudentComboBox.ItemsSource = dBManager.Students.Except(tempData.StudentTempCollection);
            StudentListbox.ItemsSource = tempData.StudentTempCollection;
        }

        private void ConfirmationButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                dBManager.AddCourse(
                NameTextbox.Text,
                DescriptionTextbox.Text,
                StartDateDatepicker.SelectedDate,
                EndDateDatepicker.SelectedDate,
                (Semester)SemesterComboBox.SelectedItem);

            }
            catch (Exception)
            {
                throw;
            }



            dBManager.JoinLecturerAndCourse((Lecturer)LecturerComboBox.SelectedItem, dBManager.Courses.Last());

            Close();
            
        }

        private void StudentComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!StudentListbox.Items.Contains((Student)StudentComboBox.SelectedItem))
                tempData.StudentTempCollection.Add((Student)StudentComboBox.SelectedItem);
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            tempData.StudentTempCollection.Remove((Student)StudentListbox.SelectedItem);
        }
    }
}
