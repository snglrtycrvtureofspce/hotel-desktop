using System.Diagnostics;
using System.Windows;

namespace snglrtycrvtureofspce.Hotels.Desktop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void NewReservationForm_Click(object sender, RoutedEventArgs e)
        {
            NewReservation newReservationForm = new NewReservation();
            newReservationForm.Show();
            this.Close();
        }

        private void EditTransactionForm_Copy_Click(object sender, RoutedEventArgs e)
        {
            Transactions Form = new Transactions();
            Form.Show();
            this.Close();
        }

        private void RoomPricingForm_Click(object sender, RoutedEventArgs e)
        {
            RoomPricing form = new RoomPricing();
            form.Show();
            this.Close();
        }

        private void RoomServicesForm_Click(object sender, RoutedEventArgs e)
        {
            RoomServices form = new RoomServices();
            form.Show();
            this.Close();
        }

        private void ReservForm_Click(object sender, RoutedEventArgs e)
        {
            Reserv form = new Reserv();
            form.Show();
            this.Close();
        }

        private void GuestForm_Click(object sender, RoutedEventArgs e)
        {
            Guest form = new Guest();
            this.Close();
            form.ShowDialog();
        }

        private void btnExistingTransaction_Click(object sender, RoutedEventArgs e)
        {
            ExistingReservation form = new ExistingReservation();
            form.ShowDialog();
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Guest form = new Guest();
            form.ShowDialog();
            this.Close();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Reserv form = new Reserv();
            this.Close();
            form.ShowDialog();
        }

        private void btnPricing_Copy_Click(object sender, RoutedEventArgs e)
        {
            NomeraBD form = new NomeraBD();
            this.Close();
            form.ShowDialog();
        }

        private void btnPricing_Copy1_Click(object sender, RoutedEventArgs e)
        {
            NomeraBD form = new NomeraBD();
            this.Close();
            form.ShowDialog();
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            string filePath = @"C:\Users\Asus\Desktop\ПоНомерам.docx";

            // запускаем приложение Word и открываем файл
            Process.Start("WINWORD.EXE", filePath);
        }

        private void EditEmployeeForm_Copy_Click(object sender, RoutedEventArgs e)
        {
            AddEditEmployee Form = new AddEditEmployee();
            Form.Show();
            this.Close();
        }
    }
}
