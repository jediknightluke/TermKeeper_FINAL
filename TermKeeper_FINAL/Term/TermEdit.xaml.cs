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
    public partial class TermEdit : ContentPage
    {
        private SQLiteAsyncConnection _conn;
        private Models.Term _term;
        public TermEdit(Models.Term currentTerm)
        {
            InitializeComponent();
            _term = currentTerm;
            _conn = DependencyService.Get<ISQL_DB>().GetConnection();
        }
        protected override async void OnAppearing()
        {
            await _conn.CreateTableAsync<Models.Term>();
            TermTitle.Text = _term.Title;
            startDate.Date = _term.StartDate;
            endDate.Date = _term.EndDate;
            base.OnAppearing();

        }

        private async void BackButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        private async void SubmitButtonClicked(object sender, EventArgs e)
        {
            _term.Title = TermTitle.Text;
            _term.StartDate = startDate.Date;
            _term.EndDate = endDate.Date;

            if (CheckForNull(_term.Title))
            {
                if (_term.StartDate < _term.EndDate)
                {

                    await _conn.UpdateAsync(_term);
                    await Navigation.PopModalAsync();
                }
                else await DisplayAlert("Error.", "Please ensure start date is before end date.", "Ok");
            }
            else await DisplayAlert("Error.", "Please ensure all fields are completed.", "Ok");
        }

        private static bool CheckForNull(string entry)
        {
            if (!String.IsNullOrEmpty(entry))
                return true;
            return false;
        }
    }
}