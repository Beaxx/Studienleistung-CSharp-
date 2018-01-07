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

            //Querry für Lecturer Combobox
            var courseLecturer = from Holds in dBManager.Holds
                                 from Lecturer in dBManager.Lecturers
                                 where (Holds.CourseID == course.ID && Holds.LecturerID == Lecturer.ID)
                                 select Lecturer;

            LecturerComboBox.ItemsSource = dBManager.Lecturers;
            // TODO: Kombobox bleibt trotz zugewiesenem Wert Null.
            LecturerComboBox.SelectedItem = courseLecturer;

            //Querry für Kursteilnehmer (Studenten)
            // TODO: Evtl. Union. operator nutzen für schöneren code?
            var courseAttendees = from Listens in dBManager.Listens
                                  from Student in dBManager.Students
                                  where (Listens.CourseID == course.ID && Listens.StudentID == Student.ID)
                                  select Student;

            //Combobox enthält nur Elemente, die nicht bereits in der Listbox sind.
            StudentListBox.ItemsSource = courseAttendees;
            StudentComboBox.ItemsSource = dBManager.Students.Except(courseAttendees);

            // TODO: Bessere möglichkeit für die Interpretation einer Querry Liste, Cast geht nicht? 
            //tempData.StudentTempCollection = courseAttendees;

            foreach (var student in courseAttendees)
            {
                tempData.StudentTempCollection.Add(student);
            }
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

        private void StudentComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!StudentListBox.Items.Contains((Student)StudentComboBox.SelectedItem))
                tempData.StudentTempCollection.Add((Student)StudentComboBox.SelectedItem);
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            tempData.StudentTempCollection.Remove((Student)StudentListBox.SelectedItem);
        }
    }
}
