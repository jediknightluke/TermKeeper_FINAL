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
    public partial class TermAdd : ContentPage
    {
        public MainPage main_Page;
        private SQLiteAsyncConnection _conn;

        public TermAdd(MainPage mainPage)
        {
            InitializeComponent();
            main_Page = mainPage;
            _conn = DependencyService.Get<ISQL_DB>().GetConnection();
        }


        private async void SaveButton_Clicked(object sender, EventArgs e)
        {
            var term = new Models.Term();
            term.Title = txtTermTitle.Text;
            term.StartDate = startDatePicker.Date;
            term.EndDate = endDatePicker.Date;

            if (term.StartDate < term.EndDate)
            {
                await _conn.InsertAsync(term);
                main_Page.term_List.Add(term);
                await Navigation.PopModalAsync();
            }

            else await DisplayAlert("Error.", "Please ensure start date is before end date.", "Ok");

        }


        private async void BackButtonClick(object sender, EventArgs e)
        {

            await Navigation.PopModalAsync();
        }


    }
}