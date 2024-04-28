using System.Windows;
using System.Windows.Controls;
using System.Data.SqlClient;

namespace snglrtycrvtureofspce.Hotels.Desktop
{
    /// <summary>
    /// Interaction logic for RoomPricing.xaml
    /// </summary>
    public partial class RoomPricing : Window
    {
        private readonly string _connectionString = System.Configuration.ConfigurationManager.ConnectionStrings
            ["connectionString"].ConnectionString;

        public RoomPricing()
        {
            InitializeComponent();
            SqlConnection connection = new SqlConnection(_connectionString);
            SqlDataReader reader = null;
            try
            {
                connection.Open();
                SqlCommand cmdType = new SqlCommand("SELECT * FROM tblRoomType", connection);

                reader = cmdType.ExecuteReader();

                while (reader.Read())
                {
                    cmbRoomType.Items.Add(reader["TypeDescription"].ToString());
                }
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
                if (connection != null)
                {
                    connection.Close();
                }
            }
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

        private void CmbRoomType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            connection.Open();
            if (cmbRoomType.SelectedValue.ToString() == "Suite")
            {
                txtPrice.Text = "";
                cmbRoomNumber.Items.Clear();
                cnvRoomNumber.Visibility = Visibility.Visible;

                SqlCommand number = new SqlCommand("SELECT RoomID FROM tblRooms INNER JOIN tblRoomType ON tblRooms.RoomTypeID = tblRoomType.RoomTypeID WHERE TypeDescription ='Suite'", connection);
                SqlDataReader reader = number.ExecuteReader();

                while (reader.Read())
                {
                    cmbRoomNumber.Items.Add(reader["RoomID"].ToString());
                }
                reader.Close();
            }
            else
            {
                double money = 0;
                cnvRoomNumber.Visibility = Visibility.Collapsed;
                SqlCommand type = new SqlCommand("SELECT TOP 1 Cost FROM tblRooms INNER JOIN tblRoomType ON tblRooms.RoomTypeID = tblRoomType.RoomTypeID WHERE TypeDescription ='" + cmbRoomType.SelectedValue.ToString() + "'", connection);
                var cost = type.ExecuteScalar();
                if (cost != null)
                {
                    money = double.Parse(cost.ToString());
                }
                txtPrice.Text = money.ToString("#.##" + " " + "руб.");
            }
           
            connection.Close();
        }

        private void CmbRoomNumber_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            if (cmbRoomNumber.SelectedIndex != -1)
            {
                connection.Open();
                SqlCommand num = new SqlCommand("SELECT Cost FROM tblRooms WHERE RoomID = '" + cmbRoomNumber.SelectedValue.ToString() + "'", connection);
                double result = 0;
                var output = num.ExecuteScalar();
                if (output != null)
                {
                    result = double.Parse(output.ToString());
                }
                txtPrice.Text = result.ToString("#.##");
                connection.Close();
            }
            else
            {
            }
        }

        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            double price;
            if (double.TryParse(txtPrice.Text,out price) && price > 0)
            {
                connection.Open();

                if (cmbRoomType.SelectedValue.ToString() == "Suite")
                {
                    SqlCommand update = new SqlCommand("UPDATE tblRooms SET Cost='" + txtPrice.Text + "' WHERE RoomID = '" + cmbRoomNumber.Text + "'", connection);
                    int r = update.ExecuteNonQuery();
                    if (r > 0)
                    {
                        MessageBox.Show("Reservation Updated successfully!");
                        MainWindow main = new MainWindow();
                        main.Show();
                        this.Close();
                    }
                }
                else
                {
                    SqlCommand update = new SqlCommand("UPDATE tblrooms SET cost = '" + txtPrice.Text + "' WHERE RoomTypeID = ( SELECT top 1 tblrooms.roomtypeid FROM tblrooms INNER JOIN tblroomtype ON tblrooms.RoomTypeID = tblroomtype.RoomTypeID WHERE Typedescription = '" + cmbRoomType.Text + "')", connection);
                    int r = update.ExecuteNonQuery();
                    if (r > 0)
                    {
                        MessageBox.Show("Reservation Updated successfully!");
                        MainWindow main = new MainWindow();
                        main.Show();
                        this.Close();
                    }
                }
            }
            else
            {
                MessageBox.Show("Enter numbers for price and greater than 0");
            }
        }

        private void TxtPrice_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void CmbService_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            int serviceId = cmbService.SelectedIndex + 1;
            connection.Open();

            SqlCommand cost = new SqlCommand("SELECT Cost FROM tblServices WHERE ServiceID = '" + serviceId + "'", connection);
            var result = cost.ExecuteScalar();
            double price = 0;
            if (result != null)
            {
                price = double.Parse(result.ToString());
            }
            txtAmount.Text = price.ToString("#.##" + " " + "руб.");

            connection.Close();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            MainWindow form = new MainWindow();
            form.Show();
            this.Close();
        }
    }
}
