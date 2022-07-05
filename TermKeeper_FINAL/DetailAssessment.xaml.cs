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
    public partial class DetailAssessment : ContentPage
    {
        private Models.Assessment _assessment;
        private SQLiteAsyncConnection _conn;
        public DetailAssessment(Models.Assessment assessment)
        {
            InitializeComponent();
            _assessment = assessment;
            _conn = DependencyService.Get<ISQL_DB>().GetConnection();
        }

        protected override void OnAppearing()
        {
            AssessmentName.Text = _assessment.Title;
            StartDate.Text = _assessment.StartDate.ToString("MM/dd/yy");
            EndDate.Text = _assessment.EndDate.ToString("MM/dd/yy");
            AssessmentType.Text = _assessment.Type;
            NotificationsEnabled.Text = _assessment.NotificationEnabled == 1 ? "Yes" : "No";
            base.OnAppearing();
        }
        private async void OnButtonClick(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        private async void Edit_Assessment_Click(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new AssessmentEdit(_assessment));
        }

        private async void Drop_Assessment_Click(object sender, EventArgs e)
        {
            var confirmation = await DisplayAlert("Alert", "Are you sure you want to delete this assessment?", "Yes", "No");
            if (confirmation)
            {
                await _conn.DeleteAsync(_assessment);
                await Navigation.PopModalAsync();
            }
        }
    }
}