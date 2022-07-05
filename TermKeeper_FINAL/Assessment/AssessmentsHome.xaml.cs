using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TermKeeper_FINAL.Interface;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TermKeeper_FINAL
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AssessmentsHome : ContentPage
    {
        private Models.Course _currentCourse;
        private SQLiteAsyncConnection _conn;
        private ObservableCollection<Models.Assessment> _assessmentList;
        public AssessmentsHome(Models.Course currentCourse)
        {
            InitializeComponent();
            CourseName.Text = currentCourse.CourseName;
            _currentCourse = currentCourse;
            _conn = DependencyService.Get<ISQL_DB>().GetConnection();
            assessmentListView.ItemTapped += new EventHandler<ItemTappedEventArgs>(Assessment_Tapped);

        }

        protected override async void OnAppearing()
        {
            CourseName.Text = _currentCourse.CourseName;
            await _conn.CreateTableAsync<Models.Assessment>();
            var assessmentList = await _conn.QueryAsync<Models.Assessment>($"Select * From Assessments Where Course = '{_currentCourse.Id}'");
            _assessmentList = new ObservableCollection<Models.Assessment>(assessmentList);
            assessmentListView.ItemsSource = _assessmentList;
            base.OnAppearing();
        }

        private async void OnButtonClick(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        private async void Add_Assessment_Click(object sender, EventArgs e)
        {
            await _conn.CreateTableAsync<Models.Assessment>();
            var assessmentCount = await _conn.QueryAsync<Models.Assessment>($"Select Type From Assessments Where Course = '{_currentCourse.Id}'");
            if (assessmentCount.Count == 2)
            {
                await DisplayAlert("Alert", "You cannot add more than two exams. Please remove an exam and try again", "Ok");
            }
            else await Navigation.PushModalAsync(new AssessmentAdd(_currentCourse));
        }
        private async void Assessment_Tapped(object sender, ItemTappedEventArgs e)
        {
            Models.Assessment assessment = (Models.Assessment)e.Item;
            await Navigation.PushModalAsync(new DetailAssessment(assessment));
        }
    }
}