using System.Windows;
using System.Data.SqlClient;
using System.Windows.Controls;

namespace snglrtycrvtureofspce.Hotels.Desktop
{
    /// <summary>
    /// Interaction logic for AddEditGuest.xaml
    /// </summary>
    public partial class EditGuest : Window
    {
        private readonly string _connectionString = System.Configuration.ConfigurationManager.ConnectionStrings
            ["connectionString"].ConnectionString;
        private readonly string id = "";

        public EditGuest()
        {
            InitializeComponent();
        }

        public EditGuest(string id)
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            InitializeComponent();
            this.id = id;
            txtGuestID.Text = id;
            connection.Open();
            SqlCommand reservation = new SqlCommand("select * from tblGuests where guestID =  '" + txtGuestID.Text + "'", connection);
            SqlDataReader rdr = reservation.ExecuteReader();

            while (rdr.Read())
            {
                txtGuestID.Text = rdr["GuestID"].ToString();
                txtFirstName.Text = rdr["FirstName"].ToString();
                txtLastName.Text = rdr["LastName"].ToString();
                txtPhone.Text = rdr["Phone"].ToString();
                txtAddress.Text = rdr["GuestAddress"].ToString();
                txtGender.Text = rdr["Gender"].ToString();
                txtEmail.Text = rdr["EmailAddress"].ToString();
            }
            connection.Close();
        }


        /*private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }*/

        private void BtnUpdateGuest_Click(object sender, RoutedEventArgs e)
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            if (id != "")
            {
                if (txtFirstName.Text == "" || txtLastName.Text == "" || txtEmail.Text == "" || txtPhone.Text == "" || txtGender.Text == "" || txtAddress.Text == "")
                {
                    MessageBox.Show("Поля не заполнены.");
                }
                else
                {
                    connection.Open();

                    string selectedGender = ((TextBlock)txtGender.SelectedItem).Text;

                    SqlCommand update = new SqlCommand("UPDATE tblGuests SET FirstName=@FirstName, LastName=@LastName, GuestAddress=@GuestAddress, Phone=@Phone, Gender=@Gender, EmailAddress=@EmailAddress WHERE GuestID=@GuestID", connection);

                    update.Parameters.AddWithValue("@FirstName", txtFirstName.Text);
                    update.Parameters.AddWithValue("@LastName", txtLastName.Text);
                    update.Parameters.AddWithValue("@GuestAddress", txtAddress.Text);
                    update.Parameters.AddWithValue("@Phone", txtPhone.Text);
                    update.Parameters.AddWithValue("@Gender", selectedGender);
                    update.Parameters.AddWithValue("@EmailAddress", txtEmail.Text);
                    update.Parameters.AddWithValue("@GuestID", txtGuestID.Text);

                    int r = update.ExecuteNonQuery();
                    if (r > 0)
                    {
                        MessageBox.Show("Данные гостя обновлены");
                        this.Close();
                    }
                    connection.Close();
                }
            }
        }

        private void BtnReturn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}