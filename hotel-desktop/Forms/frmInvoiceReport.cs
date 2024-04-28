using System;
using System.Data;
using System.Windows.Forms;
using CrystalDecisions.CrystalReports.Engine;
using System.Data.SqlClient;

namespace snglrtycrvtureofspce.Hotels.Desktop
{
    public partial class frmInvoiceReport : Form
    {
        private readonly string _connectionString = System.Configuration.ConfigurationManager.ConnectionStrings
            ["connectionString"].ConnectionString;
        private readonly string reservation = "";
        private readonly ReportDocument cry = new ReportDocument();

        public frmInvoiceReport(string reservation)
        {
            InitializeComponent();
            this.reservation = reservation;
        }

        private void frmInvoiceReport_Load(object sender, EventArgs e)
        {
            cry.Load(@"E:\Durham\Sem 5\DBAS\KingWilliam2.0\KingWilliamf\KingWilliam\Invoice.rpt");
            SqlConnection connection = new SqlConnection(_connectionString);
            SqlDataAdapter sda = new SqlDataAdapter("stpInvoice", connection);
            sda.SelectCommand.CommandType = CommandType.StoredProcedure;
            sda.SelectCommand.Parameters.AddWithValue("@reservation", reservation);
            DataSet st = new DataSet();
            sda.Fill(st, "dtsInvoice");
            cry.SetDataSource(st.Tables[0]);


            SqlDataAdapter sd2 = new SqlDataAdapter("stpRoomInvoice", connection);
            sd2.SelectCommand.CommandType = CommandType.StoredProcedure;
            sd2.SelectCommand.Parameters.AddWithValue("@id", reservation);
            DataSet st2 = new DataSet();
            sd2.Fill(st2, "dtsRoomInvoice");
            cry.OpenSubreport("RoomInvoice").SetDataSource(st2.Tables[0]);

            SqlDataAdapter sd3 = new SqlDataAdapter("stpServiceInvoice", connection);
            sd3.SelectCommand.CommandType = CommandType.StoredProcedure;
            sd3.SelectCommand.Parameters.AddWithValue("@id", reservation);
            DataSet st3 = new DataSet();
            sd3.Fill(st3, "dtsServiceInvoice");
            cry.OpenSubreport("ServiceInvoice").SetDataSource(st3.Tables[0]);

            SqlDataAdapter sd4 = new SqlDataAdapter("stpResInvoice", connection);
            sd4.SelectCommand.CommandType = CommandType.StoredProcedure;
            sd4.SelectCommand.Parameters.AddWithValue("@id", reservation);
            DataSet st4 = new DataSet();
            sd4.Fill(st4, "dtsRestaurantInvoice");
            cry.OpenSubreport("ResInvoice").SetDataSource(st4.Tables[0]);

            crystalReportViewer1.ReportSource = cry;
        }
    }
}
