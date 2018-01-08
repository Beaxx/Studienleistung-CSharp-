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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data;

namespace StudentManager
{
    ////Delegates
    //public delegate bool CheckFieldInputAllLetter(string input);
    //public delegate bool CheckFieldInputAllNumber(string input);
    //public delegate bool CheckFieldInputNumberOrLetterShort(string input);
    //public delegate bool CheckFieldInputDate(string input);
    //public delegate bool CheckFieldInput(string input);

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// DBManager Instanzierung zum Programstart
        /// </summary>
        public DBManager dBManager = DBManager.LoadFromDatabase();
        private RuntimeTempData tempData = new RuntimeTempData();

        public MainWindow()
        {
            InitializeComponent();
            StudentTab.DataContext = dBManager.Students;
            LecturerTab.DataContext = dBManager.Lecturers;
            CourseTab.DataContext = dBManager.Courses;
        }

        //Detail Button
        private void DetailButton_Click(object sender, RoutedEventArgs e)
        {
            var rowMember = ((FrameworkElement)sender).DataContext as dynamic;

            if (rowMember is Student)
            {
                StudentDetailWindow detailsWindow = new StudentDetailWindow(dBManager, rowMember);
                detailsWindow.ShowDialog();
            }

            if (rowMember is Lecturer)
            {
                LecturerDetailWindow detailsWindow = new LecturerDetailWindow(dBManager, rowMember);
                detailsWindow.ShowDialog();
            }

            if (rowMember is Course)
            {
                CourseDetailWindow detailsWindow = new CourseDetailWindow(dBManager, rowMember);
                detailsWindow.ShowDialog();
            }
        }

        //Delete Button
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var rowMember = ((FrameworkElement)sender).DataContext as dynamic;

            if (rowMember is Student)
            {
                MessageBoxResult messageBoxResult = MessageBox.Show(String.Format("Sind Sie sicher, dass Sie {0} löschen wollen?", rowMember),"Bestätigen", MessageBoxButton.OKCancel, MessageBoxImage.Exclamation, MessageBoxResult.Cancel);
                if (messageBoxResult == MessageBoxResult.OK)
                {
                    var toRemove = dBManager.Listens.Where(studentListens => rowMember.ID == studentListens.StudentID);
                    for (int i = toRemove.Count() - 1; i >= 0; i--)
                    {
                        dBManager.Listens.Remove(toRemove.First());
                    }
                    dBManager.RemovePersonOrCourse(rowMember);
                }            
            }

            if (rowMember is Lecturer)
            {
                MessageBoxResult messageBoxResult = MessageBox.Show(String.Format("Sind Sie sicher, dass Sie {0} löschen wollen?", rowMember), "Bestätigen", MessageBoxButton.OKCancel, MessageBoxImage.Exclamation, MessageBoxResult.Cancel);
                if (messageBoxResult == MessageBoxResult.OK)
                {
                    var toRemove = dBManager.Holds.Where(lecturerHolds => rowMember.ID == lecturerHolds.LecturerID);
                    for (int i = toRemove.Count() - 1; i >= 0; i--)
                    {
                        dBManager.Holds.Remove(toRemove.First());
                    }
                    dBManager.RemovePersonOrCourse(rowMember);
                }
            }

            if (rowMember is Course)
            {
                MessageBoxResult messageBoxResult = MessageBox.Show(String.Format("Sind Sie sicher, dass Sie {0} löschen wollen?", rowMember), "Bestätigen", MessageBoxButton.OKCancel, MessageBoxImage.Exclamation, MessageBoxResult.Cancel);
                if (messageBoxResult == MessageBoxResult.OK)
                {
                    var toRemove1 = dBManager.Holds.Where(lecturerOfCourse => rowMember.ID == lecturerOfCourse.LecturerID);
                    var toRemove2 = from Listens in dBManager.Listens
                                    where (Listens.CourseID == rowMember.ID)
                                    select Listens;

                    for (int i = toRemove2.Count() - 1; i >= 0; i--)
                    {
                        dBManager.Listens.Remove(toRemove2.Last());
                    }

                    //Unproblematisch, da Kurse nur einen dozenten haben.
                    dBManager.Holds.Remove(toRemove1.First());
                    dBManager.RemovePersonOrCourse(rowMember);
                }
            }
        }


        //Studenten Inhalte
        private void AddStudentMenuButton_Click(object sender, RoutedEventArgs e)
        {
            AddStudentWindow addStudentWindow = new AddStudentWindow(dBManager);
            addStudentWindow.ShowDialog();
        }

        //Dozenten Inhalte
        private void AddLecturerMenuButton_Click(object sender, RoutedEventArgs e)
        {
            AddLecturerWindow addLecturer = new AddLecturerWindow(dBManager);
            addLecturer.ShowDialog();
        }

        //Kursinhalte
        private void AddCourseMenuButton_Click(object sender, RoutedEventArgs e)
        {
            AddCourseWindow addCourseWindow = new AddCourseWindow(dBManager);
            addCourseWindow.ShowDialog();
        }

        //Fenster Schließen
        private void ExitApplicationMenuButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            dBManager.SaveToDatabase();
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            StudentTab.DataContext = dBManager.Students.Where(student => student.ObjectToText().Contains(SearchTextBox.Text));
            LecturerTab.DataContext = dBManager.Lecturers.Where(lecturer => lecturer.ObjectToText().Contains(SearchTextBox.Text));
            CourseTab.DataContext = dBManager.Courses.Where(course => course.ObjectToText().Contains(SearchTextBox.Text));
        }
    }
}
