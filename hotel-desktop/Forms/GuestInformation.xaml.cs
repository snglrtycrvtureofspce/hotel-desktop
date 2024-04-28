using System.Windows;
using System.Data.SqlClient;

namespace snglrtycrvtureofspce.Hotels.Desktop
{
    /// <summary>
    /// Interaction logic for GuestInformation.xaml
    /// </summary>
    public partial class GuestInformation : Window
    {
        private readonly string _connectionString = System.Configuration.ConfigurationManager.ConnectionStrings
            ["connectionString"].ConnectionString;

        public GuestInformation()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnView_Click(object sender, RoutedEventArgs e)
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            if (txtGuestID.Text != "")
            {
                connection.Open();
                SqlCommand guest = new SqlCommand("SELECT * FROM tblGuests WHERE guestID =  '" + txtGuestID.Text + "'", connection);
                SqlDataReader rdr = null;
                rdr = guest.ExecuteReader();
                bool found = false;
                while (rdr.Read())
                {
                    cnvGuest.Visibility = Visibility.Visible;
                    lblID.Content = rdr["GuestID"].ToString();
                    lblFirstName.Content = rdr["FirstName"].ToString();
                    lblLastName.Content = rdr["LastName"];
                    lblPhone.Content = rdr["Phone"];
                    lblAddress.Content = rdr["GuestAddress"].ToString();
                    lblEmail.Content = rdr["EmailAddress"].ToString();
                    lblPostal.Content = rdr["Gender"].ToString();                  
                    found = true;
                }

                if (found == false)
                {
                    cnvGuest.Visibility = Visibility.Collapsed;
                    MessageBox.Show("Гость не найден!");
                }
                rdr.Close();
                connection.Close();
            }
            else
            {
                MessageBox.Show("Введите айди бронирования");
                txtGuestID.Focus();
            }
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            EditGuest add = new EditGuest(txtGuestID.Text);
            add.ShowDialog();
            
            btnView_Click(sender,e);
            
        }

        private void btnViewCopy_Click(object sender, RoutedEventArgs e)
        {
            Guest form = new Guest();
            form.ShowDialog();
            this.Close();
        }
    }
}
