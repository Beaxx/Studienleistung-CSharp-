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

        public AddCourseWindow(DBManager dBManager)
        {
            this.dBManager = dBManager;
            InitializeComponent();
            SemesterComboBox.ItemsSource = Enum.GetValues(typeof(Semester));
            //CourseComboBox.ItemsSource = dBManager.Courses;
        }

        private void ConfirmationButton_Click(object sender, RoutedEventArgs e)
        {
            //TODO
        }
    }
}
