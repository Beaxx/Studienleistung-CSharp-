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
    /// Interaction logic for LecturerDetailWindow.xaml
    /// </summary>
    public partial class CourseDetailWindow : Window
    {
            private DBManager dBManager;
            private Course course;
            private RuntimeTempData tempData = new RuntimeTempData();

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

            LecturerComboBox.ItemsSource = dBManager.Lecturers;
            // TODO: Dozent-Kurs Verbindung aus Holds suchen.
            //DegreeComboBox.SelectedItem = course
        }

        private void ConfirmationButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                course.Name = NameTextbox.Text;
                course.Description = DescriptionTextbox.Text;
                course.StartDate = (DateTime)StartdateDatePicker.SelectedDate;
                course.EndDate = (DateTime)EnddateDatePicker.SelectedDate;

                // TODO: Siehe oben -> Dozent- Kurs binding für Combobox

                course.Semester = (Semester)SemesterComboBox.SelectedItem;

                // TODO: Update der Listens
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
