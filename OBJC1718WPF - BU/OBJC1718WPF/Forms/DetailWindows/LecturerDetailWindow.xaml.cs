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
    public partial class LecturerDetailWindow : Window
    {
        private DBManager dBManager;
        private Lecturer lecturer;
        private RuntimeTempData tempData = new RuntimeTempData();
        private RuntimeTempData tempDataDeleted = new RuntimeTempData();

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

        private void ConfirmationButton_Click(object sender, RoutedEventArgs e)
        {
            // Übertragen der Änderungen am Dozenten in die Datenbank
            try
            {
                lecturer.FirstName = FirstnameTextbox.Text;
                lecturer.LastName = LastnameTextbox.Text;
                lecturer.Birthdate = (DateTime)BirthdateDatepicker.SelectedDate;
                lecturer.Degree = (Degree)DegreeComboBox.SelectedItem;
                lecturer.Street = StreetTextbox.Text;
                lecturer.HouseNumber = HouseNumberTextbox.Text;
                lecturer.Zip = new DBManager.ZIP(ZIPTextbox.Text).Number;
                lecturer.City = CityTextbox.Text;
            }
            catch (Exception)
            {
                throw;
            }

            //Update der Listens
            var query = from Course in tempDataDeleted.CourseTempCollection
                        join Holds in dBManager.Listens on lecturer.ID equals Holds.StudentID
                        where (Course.ID == Holds.CourseID)
                        select Holds;

            List<Listens> toRemove = query.ToList();
            foreach (Listens item in toRemove)
            {
                dBManager.Listens.Remove(item);
            }

            //Hinzufügen der neuen Kursverbindungen in die Datenbank
            dBManager.JoinLecturerAndCourse(lecturer, tempData.CourseTempCollection);
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
            tempDataDeleted.CourseTempCollection.Add((Course)CourseListbox.SelectedItem);
        }
    }
}

