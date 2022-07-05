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
    public partial class AssessmentEdit : ContentPage
    {
        private Models.Assessment _assessment;
        private SQLiteAsyncConnection _conn;

        public AssessmentEdit(Models.Assessment assessment)
        {
            InitializeComponent();
            _assessment = assessment;
            _conn = DependencyService.Get<ISQL_DB>().GetConnection();
        }

        protected async override void OnAppearing()
        {
            await _conn.CreateTableAsync<Models.Assessment>();

            AssessmentName.Text = _assessment.Title;
            StartDate.Date = _assessment.StartDate;
            EndDate.Date = _assessment.EndDate;

            if (_assessment.NotificationEnabled == 1)
            {
                EnableNotifications.On = true;
            }
            base.OnAppearing();
        }
        private async void Button_Clicked(object sender, EventArgs e)
        {
            _assessment.Title = AssessmentName.Text;
            _assessment.StartDate = StartDate.Date;
            _assessment.EndDate = EndDate.Date;
            _assessment.NotificationEnabled = EnableNotifications.On == true ? 1 : 0;

            if (CheckForNull(AssessmentName.Text))
            {
                if (_assessment.StartDate < _assessment.EndDate)
                {
                    await _conn.UpdateAsync(_assessment);
                    await Navigation.PopModalAsync();
                }
                else await DisplayAlert("Error.", "Please ensure start date is before end date.", "Ok");
            }
            else await DisplayAlert("Error.", "Please ensure all fields are completed.", "Ok");
        }


        private async void OnButtonClick(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        private static bool CheckForNull(string entry)
        {
            if (!String.IsNullOrEmpty(entry))
                return true;
            return false;
        }
    }
}