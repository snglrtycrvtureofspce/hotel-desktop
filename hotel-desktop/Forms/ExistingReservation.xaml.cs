using System.Windows;
using System.Windows.Controls;
using System.Data.SqlClient;

namespace snglrtycrvtureofspce.Hotels.Desktop
{
    /// <summary>
    /// Interaction logic for ExistingReservation.xaml
    /// </summary>
    public partial class ExistingReservation : Window
    {
        private readonly string _connectionString = System.Configuration.ConfigurationManager.ConnectionStrings
            ["connectionString"].ConnectionString;

        public ExistingReservation()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            MainWindow form = new MainWindow();
            form.Show();
            this.Close();
        }

        private void btnFind_Click(object sender, RoutedEventArgs e)
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            if (txtFind.Text !="")
            {
                connection.Open();
                SqlCommand reservation = new SqlCommand("SELECT * FROM tblReservations INNER JOIN tblGuests ON tblReservations.GuestID = tblGuests.GuestID WHERE reservationid =  '" + txtFind.Text + "'", connection);
                SqlDataReader rdr = reservation.ExecuteReader();
                bool found = false;
                while(rdr.Read())
                {
                    cnvResult.Visibility = Visibility.Visible;
                    txtGuestID.Text = rdr["GuestID"].ToString();
                    lblStartDate.Content = rdr["ReservationStartDate"];
                    lblEndDate.Content = rdr["ReservationEndDate"];
                    lblRoomNumber.Content = rdr["RoomID"].ToString();
                    txtFirstName.Text = rdr["FirstName"].ToString();
                    txtLastName.Text = rdr["LastName"].ToString();
                    txtPhone.Text = rdr["Phone"].ToString();
                    txtAddress.Text = rdr["GuestAddress"].ToString();
                    txtEmail.Text = rdr["EmailAddress"].ToString();
                    txtPostalCode.Text = rdr["Gender"].ToString();
                    found = true;
                }

                if(found == false)
                {
                    cnvResult.Visibility = Visibility.Collapsed;
                    MessageBox.Show("Бронирование не найдено!");
                }
                rdr.Close();
                connection.Close();
            }
            else
            {
                MessageBox.Show("Введите айди бронирования");
                txtFind.Focus();
            }
        }
        private void txtGuestID_LostFocus(object sender, RoutedEventArgs e)
        {

        }

        private void dpiStartDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void dpiEndDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
           
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            Window choose = new ChangeRoom(txtGuestID.Text );
            var result = choose.ShowDialog();
            if(result == true)
            {
                lblStartDate.Content = ChangeRoom.startDate;
                lblEndDate.Content = ChangeRoom.endDate;
                lblRoomNumber.Content = ChangeRoom.roomNumber;
            }
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            if (txtFirstName.Text=="" || txtLastName.Text =="" || txtAddress.Text=="" || txtPhone.Text =="" ||txtEmail.Text==""||txtPostalCode.Text=="")
            {
                MessageBox.Show("Все поля должны быть заполнены");
            }
            else
            {
                connection.Open();
                SqlCommand update = new SqlCommand("UPDATE tblGuests SET FirstName='" + txtFirstName.Text + "' ,LastName='" + txtLastName.Text + "' ,GuestAddress='" + txtAddress.Text + "' ,Gender='" + txtPostalCode.Text + "', Phone='" + txtPhone.Text + "' ,EmailAddress = '" + txtEmail.Text + "' where GuestID = '"+txtGuestID.Text+"'", connection);
                int r = update.ExecuteNonQuery();
                if(r > 0)
                {
                    SqlCommand updateR = new SqlCommand("UPDATE tblReservations SET RoomID='" + lblRoomNumber.Content + "', ReservationStartDate='" + lblStartDate.Content + "', ReservationEndDate='" + lblEndDate.Content + "' WHERE GuestID = '" + txtGuestID.Text + "'", connection);
                    int s = updateR.ExecuteNonQuery();
                    if(s > 0)
                    {
                        MessageBox.Show("Бронирование обновлено!");
                        this.Close();
                    }
                }

                connection.Close();
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            connection.Open();
            SqlCommand update = new SqlCommand("DELETE FROM tblReservations where reservationID = '" + txtFind.Text + "'", connection);
            int r = update.ExecuteNonQuery();
            if(r > 0)
            {
                MessageBox.Show("Бронирование удалено успешно!");
                this.Close();
            }
        }
    }
}
