using System.Windows;
using System.Data.SqlClient;

namespace snglrtycrvtureofspce.Hotels.Desktop
{
    /// <summary>
    /// Interaction logic for EmployeeInformation.xaml
    /// </summary>
    public partial class RoomInformation : Window
    {
        private readonly string _connectionString = System.Configuration.ConfigurationManager.ConnectionStrings
            ["connectionString"].ConnectionString;

        public RoomInformation()
        {
            InitializeComponent();
            txtRoomID.Focus();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            MainWindow form = new MainWindow();
            form.Show();
            this.Close();
        }

        private void btnView_Click(object sender, RoutedEventArgs e)
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            if (txtRoomID.Text != "")
            {
                connection.Open();
                SqlCommand reservation = new SqlCommand("SELECT * FROM tblRooms WHERE RoomID =  '" + txtRoomID.Text + "'", connection);
                SqlDataReader rdr = null;
                rdr = reservation.ExecuteReader();
                bool found = false;
                while (rdr.Read())
                {
                    cnvRoom.Visibility = Visibility.Visible;
                    lblID.Content = rdr["RoomID"].ToString();
                    lblFirstName.Content = rdr["RoomTypeID"].ToString();

                    lblLastName.Content = rdr["StatusID"];
                    lblPhone.Content = rdr["Cost"];
                    lblAddress.Content = rdr["RoomFloor"].ToString();

                    found = true;
                }

                if (found == false)
                {
                    cnvRoom.Visibility = Visibility.Collapsed;
                    MessageBox.Show("Employee ID not found. Please try again!");
                }
                rdr.Close();
                connection.Close();
            }
            else
            {
                MessageBox.Show("Enter Reservation ID to find Reservation details");
                txtRoomID.Focus();
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            AddEditRoom add = new AddEditRoom();
            add.ShowDialog();
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            AddEditRoom win = new AddEditRoom(txtRoomID.Text);
            win.ShowDialog();
        }
    }
}
