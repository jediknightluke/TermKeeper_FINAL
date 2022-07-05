using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Plugin.LocalNotifications;
using System.Runtime.InteropServices;
using TermKeeper_FINAL.Interface;
using SQLitePCL;

[assembly: Xamarin.Forms.Dependency(typeof(TermKeeper_FINAL.Interface.ISQL_DB))]
namespace TermKeeper_FINAL
{

    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        private bool pushNotification = true;
        private SQLiteAsyncConnection _conn;
        public ObservableCollection<Models.Term> term_List;
        public MainPage()
        {
            InitializeComponent();
            _conn = DependencyService.Get<ISQL_DB>().GetConnection();
            termListView.ItemTapped += new EventHandler<ItemTappedEventArgs>(Term_Click);
        }

        private async void ButtonClicked(object sender, EventArgs e)
        {

            await Navigation.PushModalAsync(new TermAdd(this));

        }
        async private void Term_Click(object sender, ItemTappedEventArgs e)
        {
            Models.Term term = (Models.Term)e.Item;
            await Navigation.PushModalAsync(new DetailTerm(term));
        }

        protected override async void OnAppearing()
        {
            await _conn.CreateTableAsync<Models.Term>();
            await _conn.CreateTableAsync<Models.Course>();
            await _conn.CreateTableAsync<Models.Assessment>();

            var termList = await _conn.Table<Models.Term>().ToListAsync();
            var courseList = await _conn.Table<Models.Course>().ToListAsync();
            var assessmentList = await _conn.Table<Models.Assessment>().ToListAsync();

            //populate app with dummy data if not data exists.
            if (!termList.Any())
            {
                var dummyTerm = new Models.Term();
                dummyTerm.Title = "Term 1";
                dummyTerm.StartDate = new DateTime(2022, 03, 01);
                dummyTerm.EndDate = new DateTime(2022, 08, 31);
                await _conn.InsertAsync(dummyTerm);
                termList.Add(dummyTerm);

                var dummyCourse = new Models.Course();
                dummyCourse.CourseName = "Mobile Application";
                dummyCourse.StartDate = new DateTime(2022, 09, 01);
                dummyCourse.StartDate = new DateTime(2023, 02, 28);
                dummyCourse.Status = "In-Progress";
                dummyCourse.InstructorName = "Lauren Provost";
                dummyCourse.InstructorPhone = "(385) 428-8921";
                dummyCourse.InstructorEmail = "lauren.provost@wgu.edu";
                dummyCourse.NotificationEnabled = 1;
                dummyCourse.Notes = "Xamirin Forms, C#, Mobile Development";
                dummyCourse.Term = dummyTerm.Id;
                await _conn.InsertAsync(dummyCourse);

                var dummyObjectiveAssessment = new Models.Assessment();
                dummyObjectiveAssessment.Title = "Example Assessment 1";
                dummyObjectiveAssessment.StartDate = new DateTime(2022, 09, 01);
                dummyObjectiveAssessment.EndDate = new DateTime(2023, 02, 28);
                dummyObjectiveAssessment.Course = dummyCourse.Id;
                dummyObjectiveAssessment.Type = "Objective";
                dummyObjectiveAssessment.NotificationEnabled = 1;
                await _conn.InsertAsync(dummyObjectiveAssessment);

                var dummyPerformanceAssessment = new Models.Assessment();
                dummyPerformanceAssessment.Title = "Example Assessment 2";
                dummyPerformanceAssessment.StartDate = new DateTime(2022, 09, 01);
                dummyPerformanceAssessment.EndDate = new DateTime(2023, 02, 28);
                dummyPerformanceAssessment.Course = dummyCourse.Id;
                dummyPerformanceAssessment.Type = "Performance";
                dummyPerformanceAssessment.NotificationEnabled = 1;
                await _conn.InsertAsync(dummyPerformanceAssessment);
            }

            //notification handling
            if (pushNotification == true)
            {
                pushNotification = false;
                int courseId = 0;
                foreach (Models.Course course in courseList)
                {
                    courseId++;
                    if (course.NotificationEnabled == 1)
                    {
                        if (course.StartDate == DateTime.Today)
                            CrossLocalNotifications.Current.Show("Reminder", $"course: {course.CourseName} begins today!", courseId);
                        if (course.EndDate == DateTime.Today)
                            CrossLocalNotifications.Current.Show("Reminder", $"course: {course.CourseName} ends today!", courseId);
                    }
                }

                int assessmentId = courseId;
                foreach (Models.Assessment assessment in assessmentList)
                {
                    assessmentId++;
                    if (assessment.NotificationEnabled == 1)
                    {
                        if (assessment.StartDate == DateTime.Today)
                            CrossLocalNotifications.Current.Show("Reminder", $"{assessment.Title} begins today!", assessmentId);
                        if (assessment.EndDate == DateTime.Today)
                            CrossLocalNotifications.Current.Show("Reminder", $"{assessment.Title} ends today!", assessmentId);
                    }
                }
            }

            term_List = new ObservableCollection<Models.Term>(termList);
            termListView.ItemsSource = term_List;
            base.OnAppearing();
        }
    }
}
