using System.Windows;
using System.Data.SqlClient;

namespace snglrtycrvtureofspce.Hotels.Desktop
{
    /// <summary>
    /// Interaction logic for AddEditEmployee.xaml
    /// </summary>
    public partial class AddEditEmployee : Window
    {
        private readonly string _connectionString = System.Configuration.ConfigurationManager.ConnectionStrings
            ["connectionString"].ConnectionString;
        private readonly string id = "";

        public AddEditEmployee()
        {
            InitializeComponent();
            SqlConnection connection = new SqlConnection(_connectionString);

            connection.Open();
            SqlCommand cmd = new SqlCommand("SELECT TOP 1 EmployeeID FROM tblEmployee ORDER BY EmployeeID DESC", connection);
            SqlDataReader rdr = cmd.ExecuteReader();
            string EmployeeID = "";
            while (rdr.Read())
            {
                EmployeeID = rdr["EmployeeID"].ToString();
            }

            int id = int.Parse(EmployeeID.Substring(1)) + 1;
            EmployeeID = "E" + id.ToString().PadLeft(5, '0'); ;
            txtID.Text = EmployeeID;
            if (rdr != null)
            {
                rdr.Close();
            }
            if (connection != null)
            {
                connection.Close();
            }
            txtFirstName.Focus();
        }
        public AddEditEmployee(string id)
        {
            InitializeComponent();
            SqlConnection connection = new SqlConnection(_connectionString);
            lblHeader.Content = "Изменить работника";
            btnEdit.Content = "Обновить";
            this.id = id;
            txtID.Text = id;
            connection.Open();
            SqlCommand reservation = new SqlCommand("SELECT * FROM tblEmployee WHERE employeeID =  '" + txtID.Text + "'", connection);
            SqlDataReader rdr = reservation.ExecuteReader();

            while (rdr.Read())
            {
                cnvEmployee.Visibility = Visibility.Visible;
                txtID.Text = rdr["EmployeeID"].ToString();
                txtFirstName.Text = rdr["FirstName"].ToString();
                txtLastName.Text = rdr["LastName"].ToString();
                txtPhone.Text = rdr["Phone"].ToString();
                txtAddress.Text = rdr["EmployeeAddress"].ToString();
                txtRate.Text = rdr["PayRate"].ToString();
                txtPosition.Text = rdr["Position"].ToString();
                txtType.Text = rdr["EmployeeType"].ToString();
            }
            connection.Close();
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            if (id != "")
            {
                if (txtFirstName.Text == "" || txtLastName.Text == "" || txtPosition.Text == "" || txtPhone.Text == "" || txtType.Text == "" || txtAddress.Text == "" || txtRate.Text == "")
                {
                    MessageBox.Show("Все поля должны быть заполнены");
                }
                else
                {
                    connection.Open();
                    SqlCommand update = new SqlCommand("UPDATE tblEmployee SET FirstName='" + txtFirstName.Text + "' ,LastName='" + txtLastName.Text + "' ,EmployeeAddress='" + txtAddress.Text + "' ,EmployeeType='" + txtType.Text + "', Phone='" + txtPhone.Text + "', Position='" + txtPosition.Text + "' ,PayRate = '" + txtRate.Text + "' where EmployeeID = '" + txtID.Text + "'", connection);
                    int r = update.ExecuteNonQuery();
                    if (r > 0)
                    {
                        MessageBox.Show("Информация обновлена успешно!");
                        this.Close();
                    }
                    connection.Close();
                }
            }
            else
            {
                if (txtFirstName.Text == "" || txtLastName.Text == "" || txtPosition.Text == "" || txtPhone.Text == "" || txtType.Text == "" || txtAddress.Text == "" || txtRate.Text == "")
                {
                    MessageBox.Show("Все поля должны быть заполнены");
                }
                else
                {
                    connection.Open();
                    SqlCommand insertReservation = new SqlCommand("INSERT INTO tblEmployee (EmployeeID,FirstName,LastName,EmployeeAddress,Phone,EmployeeType,Position,PayRate) VALUES (@id,@first,@last,@add,@phone,@type,@pos,@rate)", connection);
                    insertReservation.Parameters.Add(new SqlParameter("id", txtID.Text));
                    insertReservation.Parameters.Add(new SqlParameter("first", txtFirstName.Text));
                    insertReservation.Parameters.Add(new SqlParameter("last", txtLastName.Text));
                    insertReservation.Parameters.Add(new SqlParameter("add", txtAddress.Text));
                    insertReservation.Parameters.Add(new SqlParameter("phone", txtPhone.Text));
                    insertReservation.Parameters.Add(new SqlParameter("type", txtType.Text));
                    insertReservation.Parameters.Add(new SqlParameter("pos", txtPosition.Text));
                    insertReservation.Parameters.Add(new SqlParameter("rate", txtRate.Text));

                    int r = insertReservation.ExecuteNonQuery();
                    if (r == 0)
                    {
                        MessageBox.Show("Невозможно обновить информацию");
                    }
                    else
                    {
                        MessageBox.Show("Работник успешно добавлен");                        
                        this.Close();
                    }
                }
            }
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            MainWindow form = new MainWindow();
            form.Show();
            this.Close();
        }
    }
}
