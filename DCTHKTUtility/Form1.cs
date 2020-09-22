using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
using Microsoft.VisualStudio.Services.Common;
using Microsoft.VisualStudio.Services.WebApi;
using Microsoft.VisualStudio.Services.WebApi.Patch.Json;
using Microsoft.TeamFoundation.Core.WebApi;
using Microsoft.TeamFoundation.Core.WebApi.Types;
using Microsoft.TeamFoundation.SourceControl.WebApi;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
using Microsoft.VisualStudio.Services.WebApi;
using Microsoft.VisualStudio.Services.WebApi.Patch;
using Microsoft.VisualStudio.Services.WebApi.Patch.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace DCTHKTUtility
{
    public partial class Form1 : Form
    {
        private List<Ticket> tickets;
        public List<Ticket1> tickets1 = new List<Ticket1>();
        public List<Ticket1> tickets2 = new List<Ticket1>();
        public List<Ticket1> tickets3 = new List<Ticket1>();
        public List<Ticket1> tickets4 = new List<Ticket1>();
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            GetTFSTickets();
        }

        public void GetTFSTickets()
        {
            Uri orgUrl = new Uri("https://dev.azure.com/DCTTFS/");
            String personalAccessToken = "ndzzurgegqnpmjwrqhwji2c5exr7wffx4bzjhuccdfprfsdjuydq";
            VssConnection connection = new VssConnection(orgUrl, new VssBasicCredential(string.Empty, personalAccessToken));

            WorkItemTrackingHttpClient witClient = connection.GetClient<WorkItemTrackingHttpClient>();

            Wiql wiql = new Wiql();

            wiql.Query = "SELECT [System.Id],[System.Title],[System.State],[Custom._Severity],[Custom._Module],[System.Description] FROM workitems";

            WorkItemQueryResult tasks = witClient.QueryByWiqlAsync(wiql).Result;

            IEnumerable<WorkItemReference> tasksRefs;
            tasksRefs = tasks.WorkItems.OrderBy(x => x.Id);

            List<WorkItem> tasksList = witClient.GetWorkItemsAsync(tasksRefs.Select(wir => wir.Id)).Result;

            tickets = new List<Ticket>();


            foreach (var task in tasksList)
            {

                tickets.Add(new Ticket
                {
                    ID = task.Id,
                    Title = task.Fields["System.Title"].ToString(),
                    State = task.Fields["System.State"].ToString(),
                    Description = task.Fields["System.Description"].ToString(),
                    Severity = task.Fields["Custom._Severity"].ToString(),
                    Module = task.Fields["Custom._Module"].ToString()
                });

            }


            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = "server = tcp:hatchathon.database.windows.net,1433; initial catalog = acrf; persist security info = false; user id = dct; password = Duck@1234; multipleactiveresultsets = false; encrypt = true; trustservercertificate = false; connection timeout = 30;";
                // using the code here...  


                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                SqlTransaction transaction;
                transaction = conn.BeginTransaction();
                cmd.Transaction = transaction;
                cmd.Connection = conn;
                try
                {
                    foreach (var task in tasksList)
                    {
                        string sqlstr = "";
                        //sqlstr = "insert into tbl_employee(empid,managerempid,profile,projectid,password,createdby,createdon) values (@empid,@createdby,@createdon)";
                        sqlstr = "insert into TFSTicketsData(ID,Title,Description,State,Severity,Module) values (@ID,@Title,@Description,@State,@Severity,@Module)";
                        //sqlstr = "insert into tbl_employee(empid,managerempid,profile,projectid,password,status,experties,stream) values (@empid,@managerempid,@profile,@projectid,@password,@status,@experties,@stream)";
                        cmd.CommandText = sqlstr;
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@ID", task.Id);
                        cmd.Parameters.AddWithValue("@Title", task.Fields["System.Title"].ToString());
                        cmd.Parameters.AddWithValue("@Description", task.Fields["System.Description"].ToString());
                        cmd.Parameters.AddWithValue("@State", task.Fields["System.State"].ToString());
                        cmd.Parameters.AddWithValue("@Severity", task.Fields["Custom._Severity"].ToString());
                        cmd.Parameters.AddWithValue("@Module", task.Fields["Custom._Module"].ToString());
                        cmd.ExecuteNonQuery();
                    }

                    transaction.Commit();
                    conn.Close();
                    //result = "employee added successfully!";
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    conn.Close();
                    //global.errorhandlerclass.logerror(ex);
                    //result = ex.message;
                }
            }



            // string displayString = "Type :" + wiType + " Title :" + wiTitle + " State :" + wiState + " Description :" + wiDescription ;


            connection.Disconnect();
        }

        public void InsertDT(DataTable ds)
        {
            SqlConnection con;
            SqlCommand cmd;
            string spName = "USP_Billing_Asrun_Import";
            using (con = new SqlConnection())
            {
                con.ConnectionString = "Server = tcp:hatchathon.database.windows.net,1433; Initial Catalog = ACRF; Persist Security Info = False; User ID = dct; Password = Duck@1234; MultipleActiveResultSets = False; Encrypt = True; TrustServerCertificate = False; Connection Timeout = 30;";
                //con.ConnectionString = ConfigurationManager.ConnectionStrings["BL_Config"].ConnectionString;
                using (cmd = new SqlCommand(spName, con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Details", ds);
                    int Result = cmd.ExecuteNonQuery();
                }
            }
        }
    }

    public class Ticket
    {

        public int? ID { get; set; }

        public string Title { get; set; }

        public string State { get; set; }

        public string Description { get; set; }
        public string Severity { get; set; }

        public string Module { get; set; }
    }

    public class Ticket1
    {

        public string ID { get; set; }

        public string Title { get; set; }

    }
}
