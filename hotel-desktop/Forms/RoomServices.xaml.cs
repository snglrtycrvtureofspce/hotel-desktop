using System.Windows;
using System.Windows.Controls;
using System.Data.SqlClient;


namespace snglrtycrvtureofspce.Hotels.Desktop
{
    /// <summary>
    /// Interaction logic for RoomServices.xaml
    /// </summary>
    public partial class RoomServices : Window
    {
        private readonly string _connectionString = System.Configuration.ConfigurationManager.ConnectionStrings
            ["connectionString"].ConnectionString;

        public RoomServices()
        {
            InitializeComponent();
            SqlConnection connection = new SqlConnection(_connectionString);
            rdbRestaurant.IsChecked = true;
            SqlDataReader rdr = null;
            try
            {
                connection.Open();
                SqlCommand service = new SqlCommand("SELECT ServiceDescription FROM tblServices", connection);
                rdr = service.ExecuteReader();
                while (rdr.Read())
                {
                    cmbService.Items.Add(rdr["ServiceDescription"].ToString());
                }
            }
            finally
            {
                if (rdr != null)
                {
                    rdr.Close();
                }
                if (connection != null)
                {
                    connection.Close();
                }
            }
        }

        private void RdbRestaurant_Checked(object sender, RoutedEventArgs e)
        {
            cnvService.Visibility = Visibility.Hidden;
            cnvButton.Margin = new Thickness(364, 340, 623.2, 414.2);
        }

        private void RdbService_Checked(object sender, RoutedEventArgs e)
        {
            cnvService.Visibility = Visibility.Visible;
            cnvButton.Margin = new Thickness(364, 376, 623.2, 378.2);
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            int serviceId = cmbService.SelectedIndex + 1;
            connection.Open();

            SqlCommand cost = new SqlCommand("SELECT Cost FROM tblServices WHERE ServiceId = '" + serviceId + "'", connection);
            var result = cost.ExecuteScalar();
            double price = 0;
            if (result != null)
            {
                price = double.Parse(result.ToString());
            }
            txtAmount.Text = price.ToString("#.##" + " " + "руб.");

            connection.Close();
        }

        private void BtnChange_Click(object sender, RoutedEventArgs e)
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            if (txtRoomID.Text == "" || txtAmount.Text=="")
            {
                MessageBox.Show("Заполните всю информацию");
            }
            else
            {
                connection.Open();
                SqlCommand id = new SqlCommand("SELECT ReservationID FROM tblReservations WHERE RoomID = '" + txtRoomID.Text + "' AND ReservationEndDate > CONVERT (date, SYSDATETIME()) AND ReservationStartDate <= CONVERT (date, SYSDATETIME())", connection);
                var rid = id.ExecuteScalar();
                if(rid == null)
                {
                    MessageBox.Show("Номер - " + txtRoomID.Text + " Никем не забронирован. Выберите другой номер.");
                }
                else
                {
                    if(rdbRestaurant.IsChecked == true)
                    {
                        SqlCommand insert = new SqlCommand("INSERT INTO tblRestaurantTransactions (ReservationID,Cost,TransactionDate) VALUES ('" + rid.ToString() + "','" + txtAmount.Text + "',CONVERT (DATE,SYSDATETIME()))", connection);
                        int r = insert.ExecuteNonQuery();
                        if(r > 0)
                        {
                            MessageBox.Show("Транзакция прошла!");
                            MainWindow n = new MainWindow();
                            n.Show();
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("Транзакция не прошла!");
                        }
                    }
                    else
                    {
                            int service = cmbService.SelectedIndex;
                            SqlCommand insert = new SqlCommand("INSERT INTO tblServicesTransactions (ReservationID,ServiceID,ServiceDate) VALUES ('" + rid.ToString() + "','" + service++ + "',CONVERT (DATE,SYSDATETIME()))", connection);

                            int r = insert.ExecuteNonQuery();
                            if (r > 0)
                            {
                                MessageBox.Show("Транзакция прошла!");
                            }
                            else
                            {
                                MessageBox.Show("Транзакция не прошла!");
                            }
                    }
                }
                connection.Close();
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            MainWindow form = new MainWindow();
            form.Show();
            this.Close();
        }
    }
}
