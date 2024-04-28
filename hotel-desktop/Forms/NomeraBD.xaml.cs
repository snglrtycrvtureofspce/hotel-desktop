using snglrtycrvtureofspce.Hotels.Desktop.Model;
using System;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Diagnostics;

namespace snglrtycrvtureofspce.Hotels.Desktop
{
    /// <summary>
    /// Логика взаимодействия для NomeraBD.xaml
    /// </summary>
    public partial class NomeraBD : Window
    {
        private readonly string _connectionString = System.Configuration.ConfigurationManager.ConnectionStrings
            ["connectionString"].ConnectionString;
        private readonly string id = "";
        private readonly string sql = "";
        public NomeraBD()
        {
            InitializeComponent();
        }
        
        public NomeraBD(string id)
        {
            InitializeComponent();
            SqlConnection connection = new SqlConnection(_connectionString);
            sql = "SELECT serviceTransactionID AS 'Transaction ID', servicedescription AS 'Description', servicedate AS Date,amount AS Amount FROM tblServicesTransactions INNER JOIN tblServices ON tblServicesTransactions.ServiceID = tblServices.ServiceId WHERE ReservationID = '" + 1 + "'";

            //sql = "select orderID as'Transaction ID', Transactiondate as Date,Cost as Amount from tblRestaurantTransactions where ReservationID = '" + 1 + "'";
            this.id = id;
            SqlCommand tr = new SqlCommand(sql, connection);
            SqlDataAdapter dataAdapter = new SqlDataAdapter(tr);

            DataTable dtRecord = new DataTable();
            dataAdapter.Fill(dtRecord);
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            RoomGrid.ItemsSource = AppData.db.tblRooms.ToList();
        }
        private void Poisk_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                RoomGrid.ItemsSource = AppData.db.tblRooms.Where(item => item.RoomID.Contains(Poisk.Text)).ToList();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Check_click(object sender, RoutedEventArgs e)
        {
            Nomera form = new Nomera();
            form.ShowDialog();
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            RoomGrid.ItemsSource = AppData.db.tblRooms.Where(item => item.RoomTypeID.Contains("S")).ToList();
        }

        private void CheckBox_Checked_1(object sender, RoutedEventArgs e)
        {
            RoomGrid.ItemsSource = AppData.db.tblRooms.Where(item => item.RoomTypeID.Contains("D")).ToList();
        }

        private void CheckBox_Checked_2(object sender, RoutedEventArgs e)
        {
            RoomGrid.ItemsSource = AppData.db.tblRooms.Where(item => item.RoomTypeID.Contains("L")).ToList();
        }

        private void CheckBox_Checked_3(object sender, RoutedEventArgs e)
        {
            RoomGrid.ItemsSource = AppData.db.tblRooms.Where(item => item.RoomTypeID.Contains("J")).ToList();
        }

        private void CheckBox_Checked_4(object sender, RoutedEventArgs e)
        {
            RoomGrid.ItemsSource = AppData.db.tblRooms.Where(item => item.RoomTypeID.Contains("C")).ToList();
        }

        private void CheckBox_Checked_5(object sender, RoutedEventArgs e)
        {
            RoomGrid.ItemsSource = AppData.db.tblRooms.Where(item => item.RoomTypeID.Contains("R")).ToList();
        }

        private void CheckBox_Checked_6(object sender, RoutedEventArgs e)
        {
            RoomGrid.ItemsSource = AppData.db.tblRooms.Where(item => item.RoomTypeID.Contains("F")).ToList();
        }

        private void CheckBox_Checked_7(object sender, RoutedEventArgs e)
        {
            RoomGrid.ItemsSource = AppData.db.tblRooms.Where(item => item.RoomTypeID.Contains("P")).ToList();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            RoomGrid.ItemsSource = AppData.db.tblRooms.ToList();
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            RoomGrid.ItemsSource = AppData.db.tblRooms.ToList();
        }

        private void Report_Click(object sender, RoutedEventArgs e)
        {
            string filePath = @"C:\Users\Asus\Desktop\КоличествоБронирований.docx";

            // запускаем приложение Word и открываем файл
            Process.Start("WINWORD.EXE", filePath);
        }

        private void MainMenu_Click(object sender, RoutedEventArgs e)
        {
            MainWindow form = new MainWindow();
            form.Show();
            this.Close();
        }
    }
}
