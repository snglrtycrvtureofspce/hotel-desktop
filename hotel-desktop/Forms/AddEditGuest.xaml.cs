using System.Windows;
using System.Data.SqlClient;

namespace snglrtycrvtureofspce.Hotels.Desktop
{
    /// <summary>
    /// Interaction logic for AddEditGuest.xaml
    /// </summary>
    public partial class AddEditGuest : Window
    {
        private readonly string _connectionString = System.Configuration.ConfigurationManager.ConnectionStrings
            ["connectionString"].ConnectionString;
        private readonly string id = "";

        public AddEditGuest()
        {
            InitializeComponent();
            SqlConnection connection = new SqlConnection(_connectionString);
            connection.Open();
            SqlCommand cmd = new SqlCommand("SELECT TOP 1 GuestID FROM tblGuests ORDER BY GuestID DESC", connection);
            SqlDataReader rdr = cmd.ExecuteReader();
            string guestid = "";
            while (rdr.Read())
            {
                guestid = rdr["GuestID"].ToString();
            }

            int id = int.Parse(guestid.Substring(1)) + 1;
            guestid = "G" + id.ToString().PadLeft(5, '0'); ;
            txtGuestID.Text = guestid;
            if (rdr != null)
            {
                rdr.Close();
            }
            if (connection != null)
            {
                connection.Close();
            }
            txtGuestID.Focus();
        }
        private void rdbNewGuest_Checked(object sender, RoutedEventArgs e)
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            connection.Open();
            SqlDataReader rdr = null;
            SqlCommand cmd = new SqlCommand("SELECT TOP 1 GuestID FROM tblGuests ORDER BY GuestID DESC", connection);
            rdr = cmd.ExecuteReader();
            string guestid = "";
            while (rdr.Read())
            {
                guestid = rdr["GuestID"].ToString();
            }

            int id = int.Parse(guestid.Substring(1)) + 1;
            guestid = "G" + id.ToString().PadLeft(5, '0'); ;
            txtGuestID.Text = guestid;
            if (rdr != null)
            {
                rdr.Close();
            }
            if (connection != null)
            {
                connection.Close();
            }
            txtGuestID.Focus();
        }

        public AddEditGuest(string id)
        {
            InitializeComponent();
            SqlConnection connection = new SqlConnection(_connectionString);
            this.id = id;
            txtGuestID.Text = id;
            connection.Open();
            SqlCommand reservation = new SqlCommand("SELECT * FROM tblGuests WHERE guestID =  '" + txtGuestID.Text + "'", connection);
            SqlDataReader rdr = reservation.ExecuteReader();

            while (rdr.Read())
            {
                txtGuestID.Text = rdr["GuestID"].ToString();
                txtFirstName.Text = rdr["FirstName"].ToString();
                txtLastName.Text = rdr["LastName"].ToString();
                txtPhone.Text = rdr["Phone"].ToString();
                txtAddress.Text = rdr["GuestAddress"].ToString();
                pol.Text = rdr["Gender"].ToString();
                txtEmail.Text = rdr["EmailAddress"].ToString();
            }
            connection.Close();
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            if (id != "")
            {
                if (txtFirstName.Text == "" || txtLastName.Text == "" || txtEmail.Text == "" || txtPhone.Text == "" || pol.Text == "" || txtAddress.Text == "")
                {
                    MessageBox.Show("Заполните все поля");
                }

                else
                {
                    connection.Open();
                    SqlCommand update = new SqlCommand("UPDATE tblGuests SET FirstName='" + txtFirstName.Text + "' ,LastName='" + txtLastName.Text + "' ,GuestAddress='" + txtAddress.Text + "', Phone='" + txtPhone.Text + "', Gender='" + pol.Text + "' ,EmailAddress = '" + txtEmail.Text + "' where GuestID = '" + txtGuestID.Text + "'", connection);
                    int r = update.ExecuteNonQuery();
                    if (r > 0)
                    {
                        MessageBox.Show("Гость обновлен успешно!");
                        this.Close();
                    }
                    connection.Close();
                }
            }
        }
        private void btnaddguest(object sender, RoutedEventArgs e)
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            if (txtFirstName.Text == "" || txtLastName.Text == "" || txtEmail.Text == "" || txtPhone.Text == "" || pol.Text == "" || txtAddress.Text == "")
            {
                MessageBox.Show("Заполните все поля");
            }
            
            else
            {
                connection.Open();
                SqlCommand insertGuest = new SqlCommand("INSERT INTO tblGuests (GuestID,FirstName,LastName,GuestAddress, Gender,Phone,EmailAddress) VALUES (@guest,@first,@last,@address,@postal,@phone,@email)", connection);

                insertGuest.Parameters.Add(new SqlParameter("guest", txtGuestID.Text));
                insertGuest.Parameters.Add(new SqlParameter("first", txtFirstName.Text));
                insertGuest.Parameters.Add(new SqlParameter("last", txtLastName.Text));
                insertGuest.Parameters.Add(new SqlParameter("address", txtAddress.Text));
                insertGuest.Parameters.Add(new SqlParameter("postal", (pol.Text).ToUpper()));
                insertGuest.Parameters.Add(new SqlParameter("phone", txtPhone.Text));
                insertGuest.Parameters.Add(new SqlParameter("email", txtEmail.Text));
                int result = insertGuest.ExecuteNonQuery();
                if (result == 0)
                {
                    MessageBox.Show("Невозможно добавить гостя");
                }
                else
                {
                    MessageBox.Show("Гость успешно добавлен");
                    this.Close();
                }
            }
            connection.Close();
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
