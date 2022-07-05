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
    public partial class AssessmentAdd : ContentPage
    {
        private SQLiteAsyncConnection _conn;
        private Models.Course _course;
        public AssessmentAdd(Models.Course course)
        {
            InitializeComponent();
            _course = course;
            _conn = DependencyService.Get<ISQL_DB>().GetConnection();
        }

        protected override async void OnAppearing()
        {

            await _conn.CreateTableAsync<Models.Assessment>();
            var objectiveCount = await _conn.QueryAsync<Models.Assessment>($"Select Type From Assessments Where Course = '{_course.Id}' And Type = 'Objective'");
            var performanceCount = await _conn.QueryAsync<Models.Assessment>($"Select Type From Assessments Where Course = '{_course.Id}' And Type = 'Performance'");
            if (objectiveCount.Count == 0)
            {
                AssessmentType.Items.Add("Objective");
            }
            if (performanceCount.Count == 0)
            {
                AssessmentType.Items.Add("Performance");
            }
            if (objectiveCount.Count == 1)
            {
                AssessmentType.Items.Remove("Objective");
            }
            if (performanceCount.Count == 1)
            {
                AssessmentType.Items.Remove("Performance");
            }
            base.OnAppearing();
        }

        private async void OnButtonClick(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        private async void Add_Assessment_Clicked(object sender, EventArgs e)
        {
            // Assessment assessment = new Assessment();
            var assessment = new Models.Assessment();
            assessment.Title = AssessmentName.Text;
            assessment.StartDate = StartDate.Date;
            assessment.EndDate = EndDate.Date;
            assessment.Course = _course.Id;
            assessment.Type = (string)AssessmentType.SelectedItem;

            if (CheckForNull(AssessmentName.Text))
            {
                if (assessment.StartDate < assessment.EndDate)
                {
                    await _conn.InsertAsync(assessment);
                    await Navigation.PopModalAsync();
                }
                else await DisplayAlert("Error", "Please Make sure the start date is before end date.", "Ok");
            }
            else await DisplayAlert("Error", "Please make sure that all fields are valid.", "Ok");

        }
        private static bool CheckForNull(string entry)
        {
            if (!String.IsNullOrEmpty(entry))
                return true;
            return false;
        }
    }
}