using System;
using System.Windows;
using System.Windows.Controls;
using System.Data.SqlClient;

namespace snglrtycrvtureofspce.Hotels.Desktop
{
    /// <summary>
    /// Interaction logic for ChangeRoom.xaml
    /// </summary>
    public partial class ChangeRoom : Window
    {
        private readonly string _connectionString = System.Configuration.ConfigurationManager.ConnectionStrings
            ["connectionString"].ConnectionString;
        public static string startDate { get; set; }
        public static string endDate { get; set; }
        public static string roomNumber { get; set; }
        private readonly string guest = "";
        public ChangeRoom()
        {
            InitializeComponent();
        }

        public ChangeRoom(string id)
        {
            InitializeComponent();
            SqlConnection connection = new SqlConnection(_connectionString);
            guest = id;
            dpiStartDate.DisplayDateStart = DateTime.Now;
            dpiEndDate.DisplayDateStart = DateTime.Now;
            SqlDataReader reader = null;
            try
            {
                connection.Open();
                SqlCommand cmdType = new SqlCommand("select * from tblRoomType", connection);
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

        private void dpiStartDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            dpiEndDate.DisplayDateStart = dpiStartDate.SelectedDate;
            dpiEndDate.Text = "";
        }

        private void reloadRoom()
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
                SqlCommand cmdRoomNumber = new SqlCommand("SELECT RoomID FROM tblRooms INNER JOIN tblRoomType ON tblRooms.RoomTypeID = tblRoomType.RoomTypeID WHERE tblRooms.RoomTypeID = '" + type + "' AND StatusID = 1 AND (tblRooms.RoomID NOT IN (SELECT RoomID FROM tblReservations WHERE ReservationEndDate >= '" + dpiStartDate.Text + "') OR tblRooms.RoomID NOT IN (SELECT RoomID FROM tblReservations WHERE ReservationStartDate <= '" + dpiEndDate.Text + "'))", connection);
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

        private void dpiEndDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbRoomType.SelectedIndex != -1)
            {
                reloadRoom();
            }
        }

        private void cmbRoomType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            cmbRoomNumber.Items.Clear();
            if (dpiStartDate.Text != "" && dpiEndDate.Text != "")
            {
                reloadRoom();
            }
            else
            {                
                cmbRoomType.SelectedIndex = -1;
            }
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            string error = "";
            if (cmbRoomType.SelectedItem == null || cmbRoomNumber.SelectedItem == null)
            {
                error += "\n Комната должна быть выбрана";
            }
            if (dpiStartDate.Text == "" || dpiEndDate.Text == "")
            {
                error += "\n Дата должна быть выбрана";
            }

            if (error == "")
            {
                startDate = dpiStartDate.Text;
                endDate = dpiEndDate.Text;
                roomNumber = cmbRoomNumber.Text;
                this.DialogResult = true;
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
