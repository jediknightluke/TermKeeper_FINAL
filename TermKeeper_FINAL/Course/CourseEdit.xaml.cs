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
    public partial class CourseEdit : ContentPage
    {
        private SQLiteAsyncConnection _conn;
        private Models.Course _currentCourse;
        public CourseEdit(Models.Course currentCourse)
        {
            InitializeComponent();
            _currentCourse = currentCourse;
            _conn = DependencyService.Get<ISQL_DB>().GetConnection();
        }
        protected async override void OnAppearing()
        {
            await _conn.CreateTableAsync<Models.Course>();

            CourseName.Text = _currentCourse.CourseName;
            CourseStatus.SelectedItem = _currentCourse.Status;
            StartDate.Date = _currentCourse.StartDate;
            EndDate.Date = _currentCourse.EndDate;
            InstructorName.Text = _currentCourse.InstructorName;
            InstructorPhone.Text = _currentCourse.InstructorPhone;
            InstructorEmail.Text = _currentCourse.InstructorEmail;
            Notes.Text = _currentCourse.Notes;
            if (_currentCourse.NotificationEnabled == 1)
            {
                EnableNotifications.On = true;
            }
            base.OnAppearing();
        }
        private async void SaveCourseUpdate(object sender, EventArgs e)
        {
            _currentCourse.CourseName = CourseName.Text;
            _currentCourse.Status = (string)CourseStatus.SelectedItem;
            _currentCourse.StartDate = StartDate.Date;
            _currentCourse.EndDate = EndDate.Date;
            _currentCourse.InstructorName = InstructorName.Text;
            _currentCourse.InstructorEmail = InstructorEmail.Text;
            _currentCourse.InstructorPhone = InstructorPhone.Text;
            _currentCourse.Notes = Notes.Text;
            _currentCourse.NotificationEnabled = EnableNotifications.On == true ? 1 : 0;

            if (CheckForNull(CourseName.Text) &&
                CheckForNull(InstructorName.Text) &&
                CheckForNull(InstructorPhone.Text))
            {
                if (CheckForValidEmail(InstructorEmail.Text))
                {
                    if (_currentCourse.StartDate < _currentCourse.EndDate)
                    {
                        await _conn.UpdateAsync(_currentCourse);
                        await Navigation.PopModalAsync();
                    }
                    else await DisplayAlert("Error.", "Please ensure start date is before end date.", "Ok");
                }
                else await DisplayAlert("Error.", "Please ensure all fields are completed.", "Ok");
            }
            else await DisplayAlert("Error.", "Please provide a valid email address", "Ok");
        }


        private async void BackButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        private static bool CheckForNull(string entry)
        {
            if (!String.IsNullOrEmpty(entry))
                return true;
            return false;
        }

        private bool CheckForValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            var emailTrim = email.Trim();

            if (emailTrim.EndsWith("."))
            {
                return false;
            }
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == emailTrim;
            }
            catch
            {
                return false;
            }
        }
    }

}