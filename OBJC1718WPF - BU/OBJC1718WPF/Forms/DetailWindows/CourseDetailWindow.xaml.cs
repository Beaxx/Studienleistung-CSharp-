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
            var courseLecturerHold = from Holds in dBManager.Holds
                                     where (course.ID == Holds.CourseID)
                                     select Holds;

            var courseLecturerList = courseLecturerHold.ToList();
            if (courseLecturerList.FirstOrDefault() != null) //Handling, falls noch kein Dozent zugewiesen ist.
            {
                Holds hold = (Holds) courseLecturerList.First();
                var courseLecturer = from Lecturer in dBManager.Lecturers
                                     where (Lecturer.ID == hold.LecturerID)
                                     select Lecturer;

                var lecturer = courseLecturer.ToList();
                tempData.LecturerTempCollection.Add(lecturer.First());
            }
           
            LecturerComboBox.ItemsSource = dBManager.Lecturers;
            LecturerComboBox.SelectedItem = tempData.LecturerTempCollection.FirstOrDefault(); //Es existiert immer nur ein Dozent.

            // TODO: LINQ mit join statt 2 from
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

            //Entfernen der gelöschten Studentenverbindung aus der Datenbank
            var studentQuery = from Student in tempDataDeleted.StudentTempCollection
                        join Listens in dBManager.Listens on course.ID equals Listens.CourseID
                        where (Student.ID == Listens.StudentID)
                        select Listens;

            List<Listens> studentToRemove = studentQuery.ToList();
            foreach (Listens item in studentToRemove)
            {
                dBManager.Listens.Remove(item);
            }

            //Überschreiben des Dozenten
            if (LecturerComboBox.SelectedItem != null) // Handling, falls noch kein Dozent zugewiesen war.
            {
                Lecturer chosenLecturer = (Lecturer)LecturerComboBox.SelectedItem;
                if (tempData.LecturerTempCollection.FirstOrDefault() != null) 
                {
                    var lecturerQuery = from Holds in dBManager.Holds
                                        where (tempData.LecturerTempCollection.First().ID == Holds.LecturerID)
                                        select Holds;

                    List<Holds> lecturerToRemove = lecturerQuery.ToList();
                    if (lecturerToRemove.FirstOrDefault() != null)
                    { 
                        dBManager.Holds.Remove(lecturerToRemove.First());
                    }
                }
                dBManager.Holds.Add(new Holds(chosenLecturer.ID, course.ID));
            }

            //Hinzufügen der neuen Studentenverbindungen in die Datenbank
            dBManager.JoinStudentsAndCourse(tempData.StudentTempCollection, course);

            Close();
        }

        private void StudentComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!StudentListBox.Items.Contains(StudentComboBox.SelectedItem))
                tempData.StudentTempCollection.Add((Student)StudentComboBox.SelectedItem);
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            tempDataDeleted.StudentTempCollection.Add((Student)StudentListBox.SelectedItem);
            tempData.StudentTempCollection.Remove((Student) StudentListBox.SelectedItem);
        }
    }
}
