using System;
using System.Windows;
using System.Windows.Controls;
using System.Data.SqlClient;
namespace snglrtycrvtureofspce.Hotels.Desktop
{
    /// <summary>
    /// Interaction logic for NewReservation.xaml
    /// </summary>
    public partial class NewReservation : Window
    {
        private readonly string _connectionString = System.Configuration.ConfigurationManager.ConnectionStrings
            ["connectionString"].ConnectionString;
        //string hotelConnectionString = "Data Source=PARTH-LAPPY;Initial Catalog=KingWilliamHotelDB;Integrated Security=True";

        public NewReservation()
        {
            InitializeComponent();
            SqlConnection connection = new SqlConnection(_connectionString);
            rdbExistingGuest.IsChecked = true;
            SqlDataReader reader = null;
            dpiStartDate.DisplayDateStart = DateTime.Now;
            dpiEndDate.DisplayDateStart = DateTime.Now;
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
        }

        private void RdbNewGuest_Checked(object sender, RoutedEventArgs e)
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            cnvGuest.Visibility = Visibility.Visible;
            btnCheck.Visibility = Visibility.Hidden;
            btnGuest.Visibility = Visibility.Hidden;
            txtGuestID.Visibility = Visibility.Hidden;
            lblGuestID.Visibility = Visibility.Hidden;
            cnvReservation.Margin= new Thickness(366, 450, 432.2, 182.2);
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
            txtGuestID.IsEnabled = false;
        }

        private void RdbExistingGuest_Checked(object sender, RoutedEventArgs e)
        {
            cnvGuest.Visibility = Visibility.Hidden;
            btnCheck.Visibility = Visibility.Visible;
            txtGuestID.Visibility = Visibility.Visible;
            lblGuestID.Visibility = Visibility.Visible;
            btnGuest.Visibility = Visibility.Visible;
            cnvReservation.Margin = new Thickness(366, 300, 432.2, 342.2);
            txtGuestID.Clear();
            txtGuestID.IsEnabled = true;
        }

        private void CmbRoomType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            cmbRoomNumber.Items.Clear();
            lblMessage.Content = "";
            if(dpiStartDate.Text !="" && dpiEndDate.Text !="")
            {
                ReloadRoom();
            }
            else
            {
                lblMessage.Content = "Дата отъезда и дата выезда должны быть выбраны";
                cmbRoomType.SelectedIndex=-1;
                double money = 0;
                SqlCommand type = new SqlCommand("SELECT TOP 1 Cost FROM tblRooms INNER JOIN tblRoomType ON tblRooms.RoomTypeID = tblRoomType.RoomTypeID WHERE TypeDescription ='" + cmbRoomType.SelectedValue.ToString() + "'", connection);
                var cost = type.ExecuteScalar();
                if (cost != null)
                {
                    money = double.Parse(cost.ToString());

                }
                txtPrice.Text = money.ToString("#.##" + " " + "руб. за ночь");
            }
        }


        private void BtnSubmit_Click(object sender, RoutedEventArgs e)
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            if (rdbExistingGuest.IsChecked == true)
            {
                string error = "";
                connection.Open();
                if (txtGuestID.Text != "")
                {
                    if (IsUserExist(txtGuestID.Text) > 0)
                    {
                        if (cmbRoomType.SelectedItem == null || cmbRoomNumber.SelectedItem == null)
                        {
                            error += "\n Нужно выбрать комнату!!";
                        }
                        if (dpiStartDate.Text == "" || dpiEndDate.Text == "")
                        {
                            error += "\n Нужно выбрать дату!!";
                        }

                        if (error == "")
                        {
                            SqlCommand insertReservation = new SqlCommand("INSERT INTO tblReservations (GuestID,RoomID,EmployeeID,DateMade, ReservationStartDate,ReservationEndDate) VALUES (@guest,@room,@emp,CONVERT (date, SYSDATETIME()),@start,@end)", connection);
                            insertReservation.Parameters.Add(new SqlParameter("guest", txtGuestID.Text));
                            insertReservation.Parameters.Add(new SqlParameter("room", cmbRoomNumber.Text));
                            insertReservation.Parameters.Add(new SqlParameter("emp", txtEmployeeID.Text));
                            insertReservation.Parameters.Add(new SqlParameter("start", dpiStartDate.Text));
                            insertReservation.Parameters.Add(new SqlParameter("end", dpiEndDate.Text));
                            int r = insertReservation.ExecuteNonQuery();
                            if (r == 0)
                            {
                                MessageBox.Show("Невозможно забронировать!");
                            }
                            else
                            {
                                MessageBox.Show("Генерация чека...");
                                Checks rs = new Checks();
                                rs.Show();
                                this.Close();
                            }
                        }
                        else
                        {
                            MessageBox.Show(error);
                        }
                        if (connection != null)
                        {
                            connection.Close();
                        }
                    }
                    else
                    {
                        MessageBox.Show("\nГость с ID - " + txtGuestID.Text + " не найден\n");
                        txtGuestID.Focus();
                        txtGuestID.SelectAll();
                    }
                }
                else
                {
                    MessageBox.Show("Такого гостя не существует!");
                }

                connection.Close();
            }
            else if (rdbNewGuest.IsChecked == true)
            {
                string error = "";
                connection.Open();
                if (cmbRoomType.SelectedItem == null || cmbRoomNumber.SelectedItem == null)
                {
                    error += "\n Номер должен быть выбран!";
                }
                if (dpiStartDate.Text == "" || dpiEndDate.Text == "")
                {
                    error += "\n Дата должна быть выбрана!";
                }
                if (txtFirstName.Text == "" || txtLastName.Text == "")
                {
                    error += "\nИмя и Фамилия обязательны к заполнению!";
                }
                if (txtPhone.Text == "")
                {
                    error += "\nВведите номер телефона!";
                }
                if (txtPhone.Text.Length != 13)
                {
                    error += "\nТелефон номер должен состоять из 13 символов!";
                }
                if (txtAddress.Text == "")
                {
                    error += "\nУкажите номер паспорта.";
                }
                if (txtEmail.Text.Contains("@"))
                {

                }
                else
                {
                    error += "\nE-mail адрес введен неверно";
                }

                if (error == "")
                {
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
                        MessageBox.Show("Невозможно добавить гостя!");
                    }
                    else
                    {
                        SqlCommand insertReservation = new SqlCommand("INSERT INTO tblReservations (GuestID,RoomID,EmployeeID,DateMade, ReservationStartDate,ReservationEndDate) VALUES (@guest,@room,@emp,CONVERT (date, SYSDATETIME()),@start,@end)", connection);
                        insertReservation.Parameters.Add(new SqlParameter("guest", txtGuestID.Text));
                        insertReservation.Parameters.Add(new SqlParameter("room", cmbRoomNumber.Text));
                        insertReservation.Parameters.Add(new SqlParameter("emp", txtEmployeeID.Text));
                        insertReservation.Parameters.Add(new SqlParameter("start", dpiStartDate.Text));
                        insertReservation.Parameters.Add(new SqlParameter("end", dpiEndDate.Text));
                        int r = insertReservation.ExecuteNonQuery();
                        if (r == 0)
                        {
                            MessageBox.Show("Невозможно забронировать!");
                        }
                        else
                        {
                            Checks rs = new Checks();
                            rs.Show();
                            this.Close();
                            MessageBox.Show("Генерация чека...");
                        }
                    }
                }
                else
                {
                    MessageBox.Show(error);
                }
                if (connection != null)
                {
                    connection.Close();
                }
            }
        }

        private void DpiStartDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            lblMessage.Content = "";
            dpiEndDate.DisplayDateStart = dpiStartDate.SelectedDate;
            dpiEndDate.Text = "";
        }

        private void DpiEndDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            lblMessage.Content = "";
            if(cmbRoomType.SelectedIndex != -1)
            {
                ReloadRoom();
            }
        }

        private void ReloadRoom()
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            cmbRoomNumber.Items.Clear();
            connection.Open();
            string type = "";
            SqlDataReader reader = null;
            try
            {
                SqlCommand cmdGetRoomTypeID = new SqlCommand("SELECT RoomTypeID FROM tblRoomType WHERE TypeDescription = '" + cmbRoomType.SelectedValue.ToString() + "'", connection);
                reader = cmdGetRoomTypeID.ExecuteReader();

                while (reader.Read())
                {
                    type = reader["RoomTypeID"].ToString();
                }
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }
            SqlDataReader reader2 = null;
            try
            {
                //and StatusID = 1
                SqlCommand cmdRoomNumber = new SqlCommand("SELECT RoomID FROM tblRooms INNER JOIN tblRoomType ON tblRooms.RoomTypeID = tblRoomType.RoomTypeID WHERE tblRooms.RoomTypeID = '" + type + "'  AND (tblRooms.RoomID NOT IN (SELECT RoomID FROM tblReservations WHERE ReservationEndDate >= '" + dpiStartDate.Text + "') OR tblRooms.RoomID NOT IN (SELECT RoomID FROM tblReservations WHERE ReservationStartDate <= '" + dpiEndDate.Text + "'))", connection);
                reader2 = cmdRoomNumber.ExecuteReader();
                while (reader2.Read())
                {
                    cmbRoomNumber.Items.Add(reader2["RoomID"].ToString());
                }
            }
            finally
            {
                if (reader2 != null)
                {
                    reader2.Close();
                }
            }
            if (connection != null)
            {
                connection.Close();
            }
        }

        private int IsUserExist(string id)
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            connection.Open();
            SqlCommand checkUser = new SqlCommand("SELECT COUNT(*) FROM tblGuests WHERE GuestID = '" + id + "'", connection);
            int userExist = (int)checkUser.ExecuteScalar();
            connection.Close();
            return userExist;
        }

        private void BtnCheck_Click(object sender, RoutedEventArgs e)
        {
            Window check = new CheckPreviousRoom(txtGuestID.Text);
            check.ShowDialog();
        }

        protected int CheckRoomStatus(string roomID)
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            int output = 0;
            try
            {
                connection.Open();
                SqlCommand check = new SqlCommand("SELECT StatusID FROM tblRooms WHERE RoomID = '" + roomID + "'", connection);
                var status = check.ExecuteScalar();
                output = int.Parse(status.ToString());

            }
            finally
            {
                if (connection != null)
                {
                    connection.Close();
                }
            }

            return output;
        }

        private void CmbRoomNumber_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            int status = CheckRoomStatus(cmbRoomNumber.SelectedValue.ToString());
            if (status == 2)
            {
                lblMessage.Content = "Комнате " + cmbRoomNumber.SelectedValue.ToString() + "необходима уборка!";
            }
            else if (status == 3)
            {
                lblMessage.Content = "Комнате " + cmbRoomNumber.SelectedValue.ToString() + "необходимо полное обслуживание!";
            }
            else
            {
                connection.Open();
                lblMessage.Content = "";
                double money = 0;
                SqlCommand type = new SqlCommand("SELECT TOP 1 Cost FROM tblRooms INNER JOIN tblRoomType ON tblRooms.RoomTypeID = tblRoomType.RoomTypeID WHERE TypeDescription ='" + cmbRoomType.SelectedValue.ToString() + "'", connection);
                var cost = type.ExecuteScalar();
                if (cost != null)
                {
                    money = double.Parse(cost.ToString());

                }
                txtPrice.Text = money.ToString("#.##" + " " + "руб. за ночь");
                connection.Close();
            } 
        }

        private void BtnGuest_Click(object sender, RoutedEventArgs e)
        {
            Guest form = new Guest();
            form.ShowDialog();
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            double money = 0;
            connection.Open();
            SqlCommand type = new SqlCommand("SELECT TOP 1 Cost FROM tblRooms INNER JOIN tblRoomType ON tblRooms.RoomTypeID = tblRoomType.RoomTypeID WHERE TypeDescription ='" + cmbRoomType.SelectedValue.ToString() + "'", connection);
            var cost = type.ExecuteScalar();
            if (cost != null)
            {
                money = double.Parse(cost.ToString());

            }
            txtPrice.Text = money.ToString("#.##" + " " + "руб. за ночь");
            connection.Close();
        }

        private void TxtPrice_TextChanged(object sender, TextChangedEventArgs e)
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            double money = 0;
            connection.Open();
            SqlCommand type = new SqlCommand("SELECT TOP 1 Cost FROM tblRooms INNER JOIN tblRoomType ON tblRooms.RoomTypeID = tblRoomType.RoomTypeID WHERE TypeDescription ='" + cmbRoomType.SelectedValue.ToString() + "'", connection);
            var cost = type.ExecuteScalar();
            if (cost != null)
            {
                money = double.Parse(cost.ToString());

            }
            txtPrice.Text = money.ToString("#.##" + " " + "руб. за ночь");
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
