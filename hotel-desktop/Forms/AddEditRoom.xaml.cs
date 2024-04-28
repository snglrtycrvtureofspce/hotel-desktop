using System.Windows;
using System.Data.SqlClient;

namespace snglrtycrvtureofspce.Hotels.Desktop
{
    /// <summary>
    /// Interaction logic for AddEditEmployee.xaml
    /// </summary>
    public partial class AddEditRoom : Window
    {
        private readonly string _connectionString = System.Configuration.ConfigurationManager.ConnectionStrings
            ["connectionString"].ConnectionString;
        private readonly string id = "";
        public AddEditRoom()
        {
            InitializeComponent();
            SqlConnection connection = new SqlConnection(_connectionString);
            lblHeader.Content = "Добавить номер";
            btnEdit.Content = "Добавить";

            connection.Open();
            SqlDataReader rdr = null;
            SqlCommand cmd = new SqlCommand("SELECT TOP 1 RoomID FROM tblRooms ORDER BY RoomID DESC", connection);
            rdr = cmd.ExecuteReader();
            string RoomID = "";
            while (rdr.Read())
            {
                RoomID = rdr["RoomID"].ToString();
            }
            int id = int.Parse(RoomID.Substring(1)) + 1;
            RoomID = "U" + id.ToString().PadLeft(2, '0'); ;
            txtID.Text = RoomID;
            if (rdr != null)
            {
                rdr.Close();
            }
            if (connection != null)
            {
                connection.Close();
            }
            txtID.Focus();
        }
        public AddEditRoom(string id)
        {
            InitializeComponent();
            SqlConnection connection = new SqlConnection(_connectionString);
            lblHeader.Content = "Edit Employee";
            btnEdit.Content = "Update";
            this.id = id;
            txtID.Text = id;
            connection.Open();
            SqlCommand reservation = new SqlCommand("select * from tblEmployee where employeeID =  '" + txtID.Text + "'", connection);
            SqlDataReader rdr = reservation.ExecuteReader();

            while (rdr.Read())
            {
                cnvRoom.Visibility = Visibility.Visible;
                txtID.Text = rdr["RoomID"].ToString();
                txtType.Text = rdr["RoomTypeID"].ToString();
                txtStatusID.Text = rdr["StatusID"].ToString();
                txtCost.Text = rdr["Cost"].ToString();
                txtFloor.Text = rdr["RoomFloor"].ToString();
            }
            connection.Close();
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            if (id != "")
            {
                if (txtID.Text == "" || txtType.Text == "" || txtStatusID.Text == "" || txtCost.Text == "" || txtFloor.Text == "")
                {
                    MessageBox.Show("Все поля должны быть заполнены.");
                }
                else
                {
                    connection.Open();
                    SqlCommand update = new SqlCommand("UPDATE tblRooms SET RoomTypeID='" + txtType.Text + "' ,StatusID='" + txtStatusID.Text + "' ,Cost='" + txtCost.Text + "',RoomFloor='" + txtFloor.Text + "'", connection);
                    int r = update.ExecuteNonQuery();
                    if (r > 0)
                    {
                        MessageBox.Show("Номер обновлен успешно!");
                        this.Close();
                    }
                    connection.Close();
                }
            }
            else
            {
                if (txtID.Text == "" || txtType.Text == "" || txtStatusID.Text == "" || txtCost.Text == "" || txtFloor.Text == "")
                {
                    MessageBox.Show("Все поля должны быть заполнены");
                }
                else
                {
                    connection.Open();
                    SqlCommand insertReservation = new SqlCommand("INSERT INTO tblRooms (RoomID, RoomTypeID, StatusID, Cost, RoomFloor) VALUES (@id, @type, @statusid, @cost, @floor)", connection);
                    insertReservation.Parameters.Add(new SqlParameter("id", txtID.Text));
                    insertReservation.Parameters.Add(new SqlParameter("type", txtType.Text));
                    insertReservation.Parameters.Add(new SqlParameter("statusid", txtStatusID.Text));
                    insertReservation.Parameters.Add(new SqlParameter("cost", txtCost.Text));
                    insertReservation.Parameters.Add(new SqlParameter("floor", txtFloor.Text));

                    int r = insertReservation.ExecuteNonQuery();
                    if (r == 0)
                    {
                        MessageBox.Show("Невозможно обновить информацию");
                    }
                    else
                    {
                        MessageBox.Show("Номер успешно добавлен");
                        this.Close();
                    }
                }
            }
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
