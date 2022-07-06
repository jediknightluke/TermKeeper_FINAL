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
    public partial class CourseAdd : ContentPage
    {
        private SQLiteAsyncConnection _conn;
        private Models.Term _term;
        public CourseAdd(Models.Term term)
        {
            InitializeComponent();

            _conn = DependencyService.Get<ISQL_DB>().GetConnection();
            _term = term;

        }


        private async void Save_Course(object sender, EventArgs e)
        {
            var course = new Models.Course
            {

                CourseName = CourseName.Text,

                StartDate = startDate.Date,
                EndDate = endDate.Date,

                Status = (string)CourseStatus.SelectedItem,
                InstructorName = InstructorName.Text,
                InstructorPhone = InstructorPhone.Text,
                InstructorEmail = InstructorEmail.Text,
                NotificationEnabled = EnableNotifications.On == true ? 1 : 0,
                Notes = Notes.Text,
                Term = _term.Id
            };

            if (CheckForNull(CourseName.Text) &&
                CheckForNull(InstructorName.Text) &&
                CheckForNull(InstructorPhone.Text))
            {
                if (CheckForValidEmail(InstructorEmail.Text))
                {
                    if (course.StartDate < course.EndDate)
                    {

                        await _conn.InsertAsync(course);

                        await Navigation.PopModalAsync();
                    }
                    else await DisplayAlert("Error.", "Please ensure start date is before end date.", "Ok");
                }
                else await DisplayAlert("Error.", "Please ensure all fields are completed.", "Ok");
            }
            else await DisplayAlert("Error.", "Please provide a valid email address", "Ok");
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

        private async void BackButtonClick(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

    }
}