using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using System.Drawing.Printing;
using System.Drawing;

namespace snglrtycrvtureofspce.Hotels.Desktop
{
    /// <summary>
    /// Логика взаимодействия для Checks.xaml
    /// </summary>
    public partial class Checks : Window
    {
        private readonly string _connectionString = System.Configuration.ConfigurationManager.ConnectionStrings
            ["connectionString"].ConnectionString;
        private readonly string result = "";

        public Checks()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NewReservation form = new NewReservation();
            this.Close();
            form.ShowDialog();
        }
        
        void PrintPageHandler(object sender, PrintPageEventArgs e)
        {
            e.Graphics.DrawString(result, new Font("Arial", 14), Brushes.Black, 0, 0);
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            SqlConnection connection = new SqlConnection(_connectionString);
            connection.Open();
            SqlCommand id = new SqlCommand("SELECT ReservationID FROM tblReservations ORDER BY ReservationID DESC", connection);
            var result = id.ExecuteScalar();
            string price = "0";
            if (result != null)
            {
                price = result.ToString();
            }
            vivods.Content = price.ToString();

            connection.Close();

            connection.Open();
            SqlCommand room = new SqlCommand("SELECT RoomID FROM tblReservations ORDER BY ReservationID DESC", connection);
            var results = room.ExecuteScalar();
            string roomid = "0";
            if (result != null)
            {
                roomid = results.ToString();
            }
            nomer.Content = roomid.ToString();

            connection.Close();

            connection.Open();
            SqlCommand datef = new SqlCommand("SELECT ReservationStartDate FROM tblReservations ORDER BY ReservationID DESC", connection);
            var rdate = datef.ExecuteScalar();
            string datefs = "0";
            if (rdate != null)
            {
                datefs = rdate.ToString();
            }
            first.Content = datefs.ToString();

            connection.Close();

            connection.Open();
            SqlCommand dates = new SqlCommand("SELECT ReservationEndDate FROM tblReservations ORDER BY ReservationID DESC", connection);
            var sdate = dates.ExecuteScalar();
            string datese = "0";
            if (sdate != null)
            {
                datese = sdate.ToString();
            }
            second.Content = datese.ToString();

            connection.Close();

            connection.Open();
            SqlCommand priceday = new SqlCommand("SELECT DATEDIFF(day, ReservationStartDate, ReservationEndDate) AS days_diff FROM tblReservations ORDER BY ReservationID DESC", connection);
            var daypri = priceday.ExecuteScalar();
            int dayric = 0;
            if (daypri != null)
            {
                dayric = int.Parse(daypri.ToString());
            }
            third.Content = (dayric * 150 + " " + "руб. " + " " + "за" + " " + dayric + " " + "дней");

            connection.Close();

            thirds.Visibility = Visibility.Visible;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            nazad.Visibility = Visibility.Hidden;
            ce.Visibility = Visibility.Hidden;
            PrintDialog p = new PrintDialog();
            if(p.ShowDialog() == true)
            {
                p.PrintVisual(checkgrid, "Печать");
            }
        }
    }
}
