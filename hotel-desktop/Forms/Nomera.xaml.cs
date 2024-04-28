using System;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;

namespace snglrtycrvtureofspce.Hotels.Desktop
{
    /// <summary>
    /// Логика взаимодействия для Nomera.xaml
    /// </summary>
    public partial class Nomera : Window
    {
        private readonly string _connectionString = System.Configuration.ConfigurationManager.ConnectionStrings
            ["connectionString"].ConnectionString;

        public Nomera()
        {
            InitializeComponent();
            SqlConnection connection = new SqlConnection(_connectionString);
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


        private void CmbRoomType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            cmbRoomNumber.Items.Clear();

            if (dpiStartDate.Text != "" && dpiEndDate.Text != "")
            {
                ReloadRoom();
            }
            else
            {
                cmbRoomType.SelectedIndex = -1;
            }
        }

        private void DpiStartDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            dpiEndDate.DisplayDateStart = dpiStartDate.SelectedDate;
            dpiEndDate.Text = "";

        }

        private void DpiEndDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {

            if (cmbRoomType.SelectedIndex != -1)
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
                if (cmbRoomType.SelectedValue != null)
                {
                    SqlCommand cmdGetRoomTypeID = new SqlCommand("SELECT RoomTypeID FROM tblRoomType WHERE TypeDescription = '" + cmbRoomType.SelectedValue.ToString() + "'", connection);
                    reader = cmdGetRoomTypeID.ExecuteReader();

                    while (reader.Read())
                    {
                        type = reader["RoomTypeID"].ToString();
                    }
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

        protected int CheckRoomStatus(string roomID)
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            int output = 0;
            try
            {
                connection.Open();
                SqlCommand check = new SqlCommand("select statusid from tblRooms where roomID = '" + roomID + "'", connection);
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
            int status = CheckRoomStatus(cmbRoomNumber.SelectedValue.ToString());
            if (status == 2)
            {
               
            }
            else if (status == 3)
            {
            }
        }

        private void MainMenu_Click(object sender, RoutedEventArgs e)
        {
            MainWindow form = new MainWindow();
            form.Show();
            this.Close();
        }
    }
}
