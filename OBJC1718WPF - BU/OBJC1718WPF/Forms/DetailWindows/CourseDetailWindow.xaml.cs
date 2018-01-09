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
            private RuntimeTempData tempDataDeleted = new RuntimeTempData();

        public CourseDetailWindow(DBManager dBManager, Course course)
        {

            // TODO: Evtl. Union. operator nutzen für schöneren code?
            // TODO: Bessere möglichkeit für die Interpretation einer Querry Liste, Cast geht nicht? 
            //tempData.StudentTempCollection = courseAttendees;

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

            var courseLecturerList = courseLecturer.ToList();
            tempData.LecturerTempCollection.Add(courseLecturerList.First());

            LecturerComboBox.ItemsSource = dBManager.Lecturers;
            LecturerComboBox.SelectedItem = courseLecturerList.FirstOrDefault(); //Es existiert immer nur ein Dozent.

            //Querry für Kursteilnehmer (Studenten)
            var courseAttendees = from Listens in dBManager.Listens
                                  from Student in dBManager.Students
                                  where (Listens.CourseID == course.ID && Listens.StudentID == Student.ID)
                                  select Student;

            var courseAttendeesList = courseAttendees.ToList();

            //Combobox enthält nur Elemente, die nicht bereits in der Listbox sind.
            foreach (var student in courseAttendeesList)
            {
                tempData.StudentTempCollection.Add(student);
            }

            StudentComboBox.ItemsSource = dBManager.Students.Except(tempData.StudentTempCollection);
            StudentListBox.ItemsSource = tempData.StudentTempCollection;
        }

        private void ConfirmationButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                course.Name = NameTextbox.Text;
                course.Description = DescriptionTextbox.Text;
                course.StartDate = (DateTime)StartdateDatePicker.SelectedDate;
                course.EndDate = (DateTime)EnddateDatePicker.SelectedDate;
                course.Semester = (Semester)SemesterComboBox.SelectedItem;
            }
            catch (Exception)
            {
                throw;
            }

            //Update der Holds und Listens
            //dBManager.JoinLecturerAndCourse(tempData.LecturerTempCollection.First(), course);
            dBManager.JoinStudentsAndCourse(tempData.StudentTempCollection, course);

            Close();
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
