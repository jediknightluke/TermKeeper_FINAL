using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TermKeeper_FINAL.Interface;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TermKeeper_FINAL
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DetailCourse : ContentPage
    {
        private SQLiteAsyncConnection _conn;
        private Models.Course _currentCourse;
        public DetailCourse(Models.Course course)
        {
            InitializeComponent();
            _currentCourse = course;
            _conn = DependencyService.Get<ISQL_DB>().GetConnection();
        }
        protected override void OnAppearing()
        {


            courseName.Text = _currentCourse.CourseName;

            StartDate.Text = _currentCourse.StartDate.ToString("MM/dd/yy");
            EndDate.Text = _currentCourse.EndDate.ToString("MM/dd/yy");


            InstructorName.Text = _currentCourse.InstructorName;
            InstructorPhone.Text = _currentCourse.InstructorPhone;
            InstructorEmail.Text = _currentCourse.InstructorEmail;
            Notes.Text = _currentCourse.Notes;
            NotificationsEnabled.Text = _currentCourse.NotificationEnabled == 1 ? "Yes" : "No";
            Status.Text = _currentCourse.Status;


            base.OnAppearing();


        }

        private async void BackButtonClick(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        private async void EditCourseClick(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new CourseEdit(_currentCourse));
        }

        private async void DropCourseClick(object sender, EventArgs e)
        {
            var confirmation = await DisplayAlert("Alert", "Are you sure you want to drop this course?", "Yes", "No");
            if (confirmation)
            {
                await _conn.DeleteAsync(_currentCourse);
                await Navigation.PopModalAsync();
            }
        }

        private async void AssessmentsClick(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new AssessmentsHome(_currentCourse));
        }
    }
}