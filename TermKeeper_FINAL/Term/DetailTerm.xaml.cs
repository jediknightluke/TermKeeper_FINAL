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
    public partial class DetailTerm : ContentPage
    {
        private SQLiteAsyncConnection _conn;
        private ObservableCollection<Models.Course> _courseList;
        private Models.Term _currentTerm;
        public DetailTerm(Models.Term term)
        {
            InitializeComponent();
            Title = term.Title;
            _currentTerm = term;
            _conn = DependencyService.Get<ISQL_DB>().GetConnection();
            courseListView.ItemTapped += new EventHandler<ItemTappedEventArgs>(Select_Course);
        }


        protected override async void OnAppearing()
        {
            termTitle.Text = _currentTerm.Title;
            TermDetailStart.Text = _currentTerm.StartDate.ToString("MM/dd/yy");
            TermDetailEnd.Text = _currentTerm.EndDate.ToString("MM/dd/yy");
            await _conn.CreateTableAsync<Models.Course>();
            var courseList = await _conn.QueryAsync<Models.Course>($"Select * From Courses Where Term = '{_currentTerm.Id}'");
            _courseList = new ObservableCollection<Models.Course>(courseList);
            courseListView.ItemsSource = _courseList;
            base.OnAppearing();
        }
        private async void DropTerm(object sender, EventArgs e)
        {
            var confirmation = await DisplayAlert("Alert", "Are you sure you want to drop this term?", "Yes", "No");
            if (confirmation)
            {
                await _conn.DeleteAsync(_currentTerm);
                await Navigation.PopModalAsync();
            }
        }

        private async void EditTerm(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new TermEdit(_currentTerm));

        }

        private async void Select_Course(object sender, ItemTappedEventArgs e)
        {
            Models.Course course = (Models.Course)e.Item;
            await Navigation.PushModalAsync(new DetailCourse(course));
        }

        private async void AddCourse(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new CourseAdd(_currentTerm));
        }

        private async void BackButtonClick(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

    }
}