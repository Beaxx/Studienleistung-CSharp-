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
    /// Interaction logic for DetailsWindow.xaml
    /// </summary>
    public partial class StudentDetailWindow : Window
    {
        private DBManager dBManager;
        private Student student;
        private RuntimeTempData tempData = new RuntimeTempData();
        private RuntimeTempData tempDataDeleted = new RuntimeTempData();

        public StudentDetailWindow(DBManager dBManager, Student member)
        {
            this.dBManager = dBManager;
            student = member;
            DataContext = member;

            InitializeComponent();

            Title = member.ToString();
            FirstnameTextbox.Text = member.FirstName;
            LastnameTextbox.Text = member.LastName;

            BirthdateDatepPicker.DisplayDateStart = DateTime.Today.AddYears(-14);
            BirthdateDatepPicker.SelectedDate = member.Birthdate;

            StreetTextbox.Text = member.Street;
            HouseNumberTextbox.Text = member.HouseNumber;
            ZIPTextbox.Text = member.Zip.ToString();
            CityTextbox.Text = member.City;

            DegreeComboBox.ItemsSource = Enum.GetValues(typeof(Degree));
            DegreeComboBox.SelectedItem = member.Degree;

            SemesterComboBox.ItemsSource = Enum.GetValues(typeof(Semester));
            SemesterComboBox.SelectedItem = member.Semester;

            //Querry für Kursliste
            var enrolledInCourses = from Listens in dBManager.Listens
                                    from Course in dBManager.Courses
                                    where (Listens.StudentID == student.ID && Listens.CourseID == Course.ID)
                                    select Course;

            var enrolledInCoursesList = enrolledInCourses.ToList();
            foreach (var course in enrolledInCoursesList)
            {
                tempData.CourseTempCollection.Add(course);
            }

            //Combobox enthält nur Elemente, die nicht bereits in der Listbox sind.
            CourseComboBox.ItemsSource = dBManager.Courses.Except(tempData.CourseTempCollection);
            CourseListbox.ItemsSource = tempData.CourseTempCollection;
        }

        private void ConfirmationButton_Click(object sender, RoutedEventArgs e)
        {
            // Übertragen der Änderungen am Studenten in die Datenbank
            try
            {
                student.FirstName = FirstnameTextbox.Text;
                student.LastName = LastnameTextbox.Text;
                student.Birthdate = (DateTime)BirthdateDatepPicker.SelectedDate;
                student.Degree = (Degree)DegreeComboBox.SelectedItem;
                student.Street = StreetTextbox.Text;
                student.HouseNumber = HouseNumberTextbox.Text;
                student.Zip = new DBManager.ZIP(ZIPTextbox.Text).Number;
                student.City = CityTextbox.Text;
                student.Semester = (Semester)SemesterComboBox.SelectedItem;
            }
            catch (Exception)
            {
                throw;
            }

            //Entfernen der gelöschten Kurseverbindungen aus der Datenbank
            var query = from Course in tempDataDeleted.CourseTempCollection
                        join Listens in dBManager.Listens on student.ID equals Listens.StudentID
                        where (Course.ID == Listens.CourseID)
                        select Listens;

            List<Listens> toRemove = query.ToList();
            foreach (Listens item in toRemove)
            {
                dBManager.Listens.Remove(item);
            }

            //Hinzufügen der neuen Kursverbindungen in die Datenbank
            dBManager.JoinStudentsAndCourse(student, tempData.CourseTempCollection);

            Close();
        }

        private void CourseComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!CourseListbox.Items.Contains(CourseComboBox.SelectedItem))
                tempData.CourseTempCollection.Add((Course)CourseComboBox.SelectedItem);
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            tempDataDeleted.CourseTempCollection.Add((Course)CourseListbox.SelectedItem);
            tempData.CourseTempCollection.Remove((Course)CourseListbox.SelectedItem);
        }
    }
}
