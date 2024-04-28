using System;
using System.Data;
using System.Windows.Forms;
using CrystalDecisions.CrystalReports.Engine;
using System.Data.SqlClient;

namespace snglrtycrvtureofspce.Hotels.Desktop
{
    public partial class frmRoomAvailabilityReport : Form
    {
        private readonly string _connectionString = System.Configuration.ConfigurationManager.ConnectionStrings
            ["connectionString"].ConnectionString;
        private readonly string start = "";
        private readonly string end = "";
        private readonly ReportDocument cry = new ReportDocument();
        //Data object declarations
        //dataset object
        //private KingWilliamHotelDBDataSet RoomAvailabilityReportDataset;
        ////table adapter objects
        //private KingWilliamHotelDBDataSetTableAdapters.tblRoomsTableAdapter roomTableAdapter;
        //private KingWilliamHotelDBDataSetTableAdapters.tblRoomStatusesTableAdapter roomStatusTableAdapter;
        public frmRoomAvailabilityReport(string start, string end)
        {
            InitializeComponent();
            this.start = start;
            this.end = end;
        }
        private void frmRoomAvailabilityReport_Load(object sender, EventArgs e)
        {
            ////Declare report object for use at runtime
            //RoomAvailability aRoomAvailabilityReport;
            ////Instantiate the report
            //aRoomAvailabilityReport = new RoomAvailability();

            //try
            //{
            //    //Instantiate the dataset and table adapters
            //    RoomAvailabilityReportDataset = new KingWilliamHotelDBDataSet();
            //    roomTableAdapter = new KingWilliamHotelDBDataSetTableAdapters.tblRoomsTableAdapter();
            //    roomStatusTableAdapter = new KingWilliamHotelDBDataSetTableAdapters.tblRoomStatusesTableAdapter();

            //    //Fill the dataset using the table adapters
            //    //Fill with rooms
            //    roomTableAdapter.Fill(RoomAvailabilityReportDataset.tblRooms);
            //    //Fill with room statuses
            //    roomStatusTableAdapter.Fill(RoomAvailabilityReportDataset.tblRoomStatuses);

            //    //Assign the filled dataset as the datsource for the report
            //    aRoomAvailabilityReport.SetDataSource(RoomAvailabilityReportDataset);

            //    //Set up the report viewer object to show the runtime report object
            //    crystalReportViewer1.ReportSource = aRoomAvailabilityReport;
            //}
            //catch (Exception dataException)
            //{
            //    //Catch any exception thrown
            //    MessageBox.Show("Data Error Encountered: " + dataException.Message);
            //}

            cry.Load(@"E:\Durham\Sem 5\DBAS\KingWilliam2.0\KingWilliamf\KingWilliam\Rooms.rpt");
            SqlConnection connection = new SqlConnection(_connectionString);
            //String sql = "SELECT tblRooms.RoomID, tblRooms.RoomFloor,tblRoomStatuses.StatusDescription FROM tblRooms INNER JOIN tblRoomStatuses ON tblRoomStatuses.StatusID = tblRooms.StatusID WHERE tblRooms.RoomID NOT IN (SELECT RoomID FROM tblReservations WHERE ReservationStartDate < CONVERT (date, SYSDATETIME()) AND ReservationEndDate > CONVERT (date, SYSDATETIME()))";
            SqlDataAdapter sda = new SqlDataAdapter("stpRooms", connection);
            sda.SelectCommand.CommandType = CommandType.StoredProcedure;
            sda.SelectCommand.Parameters.AddWithValue("@start", start);
            sda.SelectCommand.Parameters.AddWithValue("@end", end);
            DataSet st = new DataSet();
            sda.Fill(st, "dtsRooms");
            cry.SetDataSource(st.Tables[0]);
            crystalReportViewer1.ReportSource = cry;
        }
    }
}
