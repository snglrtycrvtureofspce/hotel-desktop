using System.Windows;
using System.Windows.Controls;
using System.Data.SqlClient;


namespace snglrtycrvtureofspce.Hotels.Desktop
{
    /// <summary>
    /// Interaction logic for RoomServices.xaml
    /// </summary>
    public partial class EditTransaction : Window
    {
        private readonly string _connectionString = System.Configuration.ConfigurationManager.ConnectionStrings
            ["connectionString"].ConnectionString;
        public static int transactionid = 0;
        public EditTransaction()
        {
            InitializeComponent();
            SqlConnection connection = new SqlConnection(_connectionString);
            cnvButton.Visibility = Visibility.Hidden;
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

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            MainWindow form = new MainWindow();
            form.Show();
            this.Close();
        }

        private void rdbRestaurant_Checked(object sender, RoutedEventArgs e)
        {
            cnvService.Visibility = Visibility.Hidden;
            cnvButton.Visibility = Visibility.Hidden;
            cnvButton.Margin = new Thickness(502, 420, 485.2, 334.2);
        }

        private void rdbService_Checked(object sender, RoutedEventArgs e)
        {
            cnvService.Visibility = Visibility.Hidden;
            cnvButton.Visibility = Visibility.Hidden;
            cnvButton.Margin = new Thickness(502, 501, 485.2, 415.2);
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            if (txtRoomID.Text == "" || txtAmount.Text=="")
            {
                MessageBox.Show("Заполните всю информацию");
            }
            else
            {
                if (!checkReservationID())
                {
                    MessageBox.Show("Бронирование не найдено!");
                }
                else
                {
                    connection.Open();
                    if (rdbRestaurant.IsChecked == true)
                    {
                        SqlCommand insert = new SqlCommand("UPDATE tblRestaurantTransactions SET COST='" + txtAmount.Text + "' WHERE  OrderID = '" + transactionid + "'", connection);
                        int r = insert.ExecuteNonQuery();
                        if(r > 0)
                        {
                            MessageBox.Show("Транзакция прошла успешно!");
                            MainWindow n = new MainWindow();
                            n.Show();
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("Транзакция не прошла!");
                            MainWindow n = new MainWindow();
                            n.Show();
                            this.Close();
                        }
                    }
                    else
                    {
                            int service = cmbService.SelectedIndex + 1;
                            SqlCommand insert = new SqlCommand("UPDATE tblServicesTransactions SET Amount = '" + txtAmount.Text + "' , ServiceID='"+ service +"' WHERE ServiceTransactionID = '" + transactionid+ "'", connection);

                            int r = insert.ExecuteNonQuery();
                            if (r > 0)
                            {
                                MessageBox.Show("Транзакция прошла успешно!");
                            }
                            else
                            {
                                MessageBox.Show("Транзакция не прошла!");
                            }
                       
                    }
                    connection.Close();
                }
            }
        }

        private void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            int serviceId = cmbService.SelectedIndex + 1;
            if(connection == null || connection.State != System.Data.ConnectionState.Open)
            {
                connection.Open();
            }
            
            SqlCommand cost = new SqlCommand("select Cost from tblServices where ServiceID = '" + serviceId + "'", connection);
            var result = cost.ExecuteScalar();
            double price = 0;
            if(result!=null)
            {
                price = double.Parse(result.ToString());
            }
            txtAmount.Text = price.ToString("#.##");

            connection.Close();
        }

        private void btnTransactions_Click(object sender, RoutedEventArgs e)
        {
            int sid;
            if (txtRoomID.Text !="")
            {
                if (int.TryParse(txtRoomID.Text, out sid))
                {
                    if (checkReservationID())
                    {
                        if (rdbRestaurant.IsChecked == true)
                        {

                            Transactions tr = new Transactions(txtRoomID.Text, "r");
                            tr.ShowDialog();
                       
                            if(transactionid!=0)
                            {
                                rdbRestaurant_Checked(sender, e);
                                loadForm(transactionid);
                            }
                        }
                        else if (rdbService.IsChecked == true)
                        {
                            Transactions tr = new Transactions(txtRoomID.Text, "s");
                            tr.ShowDialog();
                        
                            if (transactionid != 0)
                            {
                                rdbService_Checked(sender, e);
                                loadForm(transactionid);
                            }
                        }
                        else
                        {
                            MessageBox.Show("Выберите другое бронирование либо услугу");
                            rdbRestaurant.Focus();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Бронирования не найден!");
                        txtRoomID.SelectAll();
                        txtRoomID.Focus();
                    }
                }
                else
                {
                    MessageBox.Show("Айди бронирования может содержать только цифры!");
                    txtRoomID.SelectAll();
                    txtRoomID.Focus();
                }

            }
            else
            {
                MessageBox.Show("Индетификатор бронирования не может быть пуст");
                txtRoomID.Focus();
            }
        }

        private void loadForm(int id)
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            int index = -1;
            string amount = "";
            connection.Open();
            if (rdbRestaurant.IsChecked == true)
            {
                cnvButton.Visibility = Visibility.Visible;
                cnvService.Visibility = Visibility.Collapsed;
                SqlCommand select = new SqlCommand("select cost from tblRestaurantTransactions where orderID = '" + transactionid + "'", connection);
                var a = select.ExecuteScalar();
                if(a != null)
                {
                    txtAmount.Text = a.ToString();
                }
            }
            else if (rdbService.IsChecked == true)
            {
                cnvButton.Visibility = Visibility.Visible;
                cnvService.Visibility = Visibility.Visible;
                SqlCommand select = new SqlCommand("SELECT serviceID, amount FROM tblServicesTransactions WHERE ServiceTransactionID = '" + transactionid + "'", connection);
                SqlDataReader rdr = select.ExecuteReader();
                while (rdr.Read())
                {
                    amount = rdr["Amount"].ToString();
                   index = int.Parse(rdr["serviceID"].ToString()) - 1;
                }
                rdr.Close();
                cmbService.SelectedIndex = index;
                txtAmount.Text = amount;
            }
            connection.Close();
        }

        private bool checkReservationID()
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            bool o = false;

            if (connection == null || connection.State == System.Data.ConnectionState.Closed)
            {
                connection.Open();
            }
                SqlCommand find = new SqlCommand("Select ReservationID from tblReservations where ReservationID = '" + txtRoomID.Text + "'", connection);
                SqlDataReader rdr = find.ExecuteReader();
                while (rdr.Read())
                {
                    o = true;
                }

            connection.Close();
           
            return o;
        }
    }
}
