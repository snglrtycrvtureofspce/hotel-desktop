using System.Windows;
using System.Data.SqlClient;

namespace snglrtycrvtureofspce.Hotels.Desktop
{
    /// <summary>
    /// Interaction logic for EmployeeInformation.xaml
    /// </summary>
    public partial class EmployeeInformation : Window
    {
        private readonly string _connectionString = System.Configuration.ConfigurationManager.ConnectionStrings
            ["connectionString"].ConnectionString;

        public EmployeeInformation()
        {
            InitializeComponent();
            txtEmployeeID.Focus();
        }

        private void BtnView_Click(object sender, RoutedEventArgs e)
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            if (txtEmployeeID.Text != "")
            {
                connection.Open();
                SqlCommand reservation = new SqlCommand("SELECT * FROM tblEmployee WHERE employeeID =  '" + txtEmployeeID.Text + "'", connection);
                SqlDataReader rdr = reservation.ExecuteReader();
                bool found = false;
                while (rdr.Read())
                {
                    cnvEmployee.Visibility = Visibility.Visible;
                    lblID.Content = rdr["EmployeeID"].ToString();
                    lblFirstName.Content = rdr["FirstName"].ToString();
                    lblLastName.Content = rdr["LastName"];
                    lblPhone.Content = rdr["Phone"];
                    lblAddress.Content = rdr["EmployeeAddress"].ToString();
                    lblRate.Content = rdr["PayRate"].ToString();
                    lblPosition.Content = rdr["Position"].ToString();
                    lblType.Content = rdr["EmployeeType"].ToString();
                    found = true;
                }

                if (found == false)
                {
                    cnvEmployee.Visibility = Visibility.Collapsed;
                    MessageBox.Show("Работник не найден!");
                }
                rdr.Close();
                connection.Close();
            }
            else
            {
                MessageBox.Show("Введите id бронирования для более детальных результатов");
                txtEmployeeID.Focus();
            }
        }
        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            AddEditEmployee add = new AddEditEmployee();
            add.ShowDialog();
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            AddEditEmployee addEditEmployee = new AddEditEmployee(txtEmployeeID.Text);
            addEditEmployee.ShowDialog();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            MainWindow form = new MainWindow();
            form.Show();
            this.Close();
        }
    }
}
