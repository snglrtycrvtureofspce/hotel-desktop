using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Data.SqlClient;
using System.Data;
using snglrtycrvtureofspce.Hotels.Desktop.Model;

namespace snglrtycrvtureofspce.Hotels.Desktop
{
    /// <summary>
    /// Логика взаимодействия для Reserv.xaml
    /// </summary>
    public partial class Reserv : Window
    {
        private readonly string _connectionString = System.Configuration.ConfigurationManager.ConnectionStrings
            ["connectionString"].ConnectionString;
        private readonly string id = "";
        private readonly string type = "";
        private readonly string sql = "";
        
        public Reserv()
        {
            InitializeComponent();
        }
        public Reserv(string id, string type)
        {
            InitializeComponent();
            sql = "SELECT serviceTransactionID AS 'Transaction ID', servicedescription AS 'Description', servicedate AS Date,amount AS Amount FROM tblServicesTransactions INNER JOIN tblServices ON tblServicesTransactions.ServiceID = tblServices.ServiceId WHERE ReservationID = '" + 1 + "'";
            //sql = "select orderID as'Transaction ID', Transactiondate as Date,Cost as Amount from tblRestaurantTransactions where ReservationID = '" + 1 + "'";
            SqlConnection connection = new SqlConnection(_connectionString);
            this.id = id;
            SqlCommand tr = new SqlCommand(sql, connection);
            SqlDataAdapter dataAdapter = new SqlDataAdapter(tr); //c.con is the connection string

            DataTable dtRecord = new DataTable();
            dataAdapter.Fill(dtRecord);
        }

        private void Windows_Loaded(object sender, RoutedEventArgs e)
        {
            GuestGrid.ItemsSource = AppData.db.tblReservations.ToList();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ExistingReservation form = new ExistingReservation();
            form.Show();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            MainWindow form = new MainWindow();
            form.Show();
            this.Close();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                GuestGrid.ItemsSource = AppData.db.tblReservations.Where(item => item.RoomID.Contains(Poisk.Text)).ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void TextBox_TextChanged_1(object sender, TextChangedEventArgs e)
        {
            GuestGrid.ItemsSource = AppData.db.tblReservations.Where(item => item.RoomID.Contains(Poisk.Text)).ToList();
        }

        private void dated_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            // Фильтруйте данные по выбранной дате и отобразите их
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            string dates = "25/05/2023";
            
            GuestGrid.ItemsSource = AppData.db.tblReservations.Where(item => item.ReservationStartDate.Equals(dates)).ToList();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            GuestGrid.ItemsSource = AppData.db.tblReservations.ToList();
        }
    }
}
