using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Data.SqlClient;
using System.Data;

namespace snglrtycrvtureofspce.Hotels.Desktop
{
    /// <summary>
    /// Interaction logic for Transactions.xaml
    /// </summary>
    public partial class Transactions : Window
    {
        private readonly string _connectionString = System.Configuration.ConfigurationManager.ConnectionStrings
            ["connectionString"].ConnectionString;
        private readonly string id = "";
        private readonly string type = "";
        private readonly string sql = "";

        public Transactions()
        {
            InitializeComponent();
        }
        public Transactions(string id,string type)
        {
            InitializeComponent();
            SqlConnection connection = new SqlConnection(_connectionString);
            this.type = type;
            if(type == "s")
            {
                sql = "SELECT serviceTransactionID AS 'Transaction ID', servicedescription AS 'Description', servicedate AS Date,amount AS Amount FROM tblServicesTransactions INNER JOIN tblServices ON tblServicesTransactions.ServiceID = tblServices.ServiceId WHERE ReservationID = '" + id + "'";
            }
            else
            {
                sql = "SELECT orderID AS 'Transaction ID', Transactiondate AS Date,Cost AS Amount FROM tblRestaurantTransactions WHERE ReservationID = '" + id + "'";
            }
            
            this.id = id;
            SqlCommand tr = new SqlCommand(sql, connection);
            SqlDataAdapter dataAdapter = new SqlDataAdapter(tr); //c.con is the connection string

            DataTable dtRecord = new DataTable();
            dataAdapter.Fill(dtRecord);
            dtgTransactions.ItemsSource = dtRecord.DefaultView;
        }

        private void dtgTransactions_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }
        private void Edit(object sender, RoutedEventArgs e)
        {
            int id = getID(sender, e);
            if(id > 0)
            {
                EditTransaction.transactionid = id;
                this.Close();
            }
        }

        private int getID(object sender, RoutedEventArgs e)
        {
            int s = 0;
            for (var vis = sender as Visual; vis != null; vis = VisualTreeHelper.GetParent(vis) as Visual)
            {
                if (vis is DataGridRow)
                {
                    var row = (DataGridRow)vis;
                    int i = row.GetIndex();
                    DataRowView v = (DataRowView)dtgTransactions.Items[i];  // this give you access to the row
                    s = (int)v[0];
                    break;
                }
            }
            return s;
        }
        private void Delete(object sender, RoutedEventArgs e)
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            int id = getID(sender, e);
            MessageBoxResult result = MessageBox.Show("Are you sure about deleting transaction?", "Deleting Transaction", MessageBoxButton.YesNo);
            connection.Open();
            if(result == MessageBoxResult.Yes)
            {
                if (id > 0)
                {
                    SqlCommand delete;
                    if (type == "s")
                    {
                        delete = new SqlCommand("DELETE FROM tblServicesTransactions WHERE serviceTransactionID = '" + id + "'", connection);
                    }
                    else
                    {
                        delete = new SqlCommand("DELETE FROM tblRestaurantTransactions WHERE OrderID = '" + id + "'", connection);
                    }

                    int r = delete.ExecuteNonQuery();
                    if (r > 0)
                    {
                        MessageBox.Show("Transaction Deleted Successfully!");
                       
                        this.Close();
                    }
                }
            }
            connection.Close();
        }
    }
}
