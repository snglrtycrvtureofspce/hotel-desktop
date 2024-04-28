using System.Windows;
using System.Data.SqlClient;
using System.Data;

namespace snglrtycrvtureofspce.Hotels.Desktop
{
    /// <summary>
    /// Interaction logic for CheckPreviousRoom.xaml
    /// </summary>
    public partial class CheckPreviousRoom : Window
    {
        private readonly string _connectionString = System.Configuration.ConfigurationManager.ConnectionStrings
            ["connectionString"].ConnectionString;

        public CheckPreviousRoom()
        {
            InitializeComponent();
        }

        public CheckPreviousRoom(string value)
        {
            InitializeComponent();
            SqlConnection connection = new SqlConnection(_connectionString);
            connection.Open();
            string sql = "SELECT RoomID, CONVERT(VARCHAR(11), DateMade, 106) AS Date FROM tblReservations WHERE GuestID = @id ORDER BY DateMade DESC";
            SqlCommand com = new SqlCommand(sql, connection);
            com.Parameters.AddWithValue("@id", value);

            using (SqlDataAdapter adapter = new SqlDataAdapter(com))
            {
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                dtgCheck.ItemsSource = dt.DefaultView;
            }
        }
    }
}
