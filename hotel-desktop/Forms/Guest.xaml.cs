using snglrtycrvtureofspce.Hotels.Desktop.Model;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Data.SqlClient;
using System.Data;
using System.Diagnostics;

namespace snglrtycrvtureofspce.Hotels.Desktop
{
    /// <summary>
    /// Логика взаимодействия для Guest2.xaml
    /// </summary>
    public partial class Guest : Window
    {
        private readonly string _connectionString = System.Configuration.ConfigurationManager.ConnectionStrings
            ["connectionString"].ConnectionString;
        private readonly string id = "";
        private readonly string sql = "";

        public Guest()
        {
            InitializeComponent();
        }
        public Guest(string id)
        {
            InitializeComponent();
            SqlConnection connection = new SqlConnection(_connectionString);
            sql = "SELECT serviceTransactionID as'Transaction ID', servicedescription AS 'Description', servicedate AS Date,amount AS Amount FROM tblServicesTransactions INNER JOIN tblServices ON tblServicesTransactions.ServiceID = tblServices.ServiceId WHERE ReservationID = '" + 1 + "'";
            //sql = "select orderID as'Transaction ID', Transactiondate as Date,Cost as Amount from tblRestaurantTransactions where ReservationID = '" + 1 + "'";

            this.id = id;
            SqlCommand tr = new SqlCommand(sql, connection);
            SqlDataAdapter dataAdapter = new SqlDataAdapter(tr);
            DataTable dtRecord = new DataTable();
            dataAdapter.Fill(dtRecord);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            GuestGrid.ItemsSource = AppData.db.tblGuests.ToList();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            AddEditGuest form = new AddEditGuest();
            form.Show();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            GuestGrid.ItemsSource = AppData.db.tblGuests.ToList();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            GuestInformation form = new GuestInformation();
            form.Show();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            MainWindow form = new MainWindow();
            form.Show();
            this.Close();
        }

        private void Poisk_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                GuestGrid.ItemsSource = AppData.db.tblGuests.Where(item => item.FirstName == Poisk.Text || item.FirstName.Contains(Poisk.Text) || item.GuestAddress.Contains(Poisk.Text) || item.LastName.Contains(Poisk.Text)).ToList();
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            GuestGrid.ItemsSource = AppData.db.tblGuests.Where(item => item.Gender.Contains("Мужской")).ToList();
        }

        private void CheckBox_Checked_1(object sender, RoutedEventArgs e)
        {
            GuestGrid.ItemsSource = AppData.db.tblGuests.Where(item => item.Gender.Contains("Женский")).ToList();
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            GuestGrid.ItemsSource = AppData.db.tblGuests.ToList();
        }

        private void CheckBox_Unchecked_1(object sender, RoutedEventArgs e)
        {
            GuestGrid.ItemsSource = AppData.db.tblGuests.ToList();
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            string filePath = @"C:\Users\Asus\Desktop\ПоНомерам.docx";

            // запускаем приложение Word и открываем файл
            Process.Start("WINWORD.EXE", filePath);
        }
    }
}
