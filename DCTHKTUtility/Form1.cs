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

            wiql.Query = "SELECT [System.Id],[System.Title],[System.State],[Custom._Severity],[Custom._Module],[System.Description],[System.IterationPath],[Custom.Stream] FROM workitems where [System.State] = 'New'";

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
                    Description = "NA",
                    Severity = task.Fields["Custom._Severity"].ToString(),
                    Module = task.Fields["Custom._Module"].ToString(),
                    Sprint = task.Fields["System.IterationPath"].ToString().Split('\\')[1],
                    Stream = task.Fields["Custom.Stream"].ToString()
                }) ;

            }


            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = "server = tcp:hatch.database.windows.net,1433; initial catalog = acrf; persist security info = false; user id = dct; password = Duck@1234; multipleactiveresultsets = false; encrypt = true; trustservercertificate = false; connection timeout = 30;";
                // using the code here...  


                conn.Open();
                //SqlCommand cmd = conn.CreateCommand();
                //SqlTransaction transaction;
                //transaction = conn.BeginTransaction();
                //cmd.Transaction = transaction;
                //cmd.Connection = conn;
                try
                {
                    foreach (var task in tasksList)
                    {
                        string sqlstr = "InsertTFSTickets";
                        //sqlstr = "insert into tbl_employee(empid,managerempid,profile,projectid,password,createdby,createdon) values (@empid,@createdby,@createdon)";
                        //sqlstr = "insert into TFSTicketsData(ID,Title,Description,State,Severity,Module) values (@ID,@Title,@Description,@State,@Severity,@Module)";
                        //sqlstr = "insert into tbl_employee(empid,managerempid,profile,projectid,password,status,experties,stream) values (@empid,@managerempid,@profile,@projectid,@password,@status,@experties,@stream)";


                        SqlCommand cmd = new SqlCommand(sqlstr, conn);
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@ID", task.Id);
                        cmd.Parameters.AddWithValue("@Title", task.Fields["System.Title"].ToString());
                        cmd.Parameters.AddWithValue("@Description", "NA");
                        cmd.Parameters.AddWithValue("@State", task.Fields["System.State"].ToString());
                        cmd.Parameters.AddWithValue("@Severity", task.Fields["Custom._Severity"].ToString());
                        cmd.Parameters.AddWithValue("@Module", task.Fields["Custom._Module"].ToString());
                        cmd.Parameters.AddWithValue("@Sprint", task.Fields["System.IterationPath"].ToString().Split('\\')[1].Replace(" ", "").ToLower());
                        cmd.Parameters.AddWithValue("@Stream", task.Fields["Custom.Stream"].ToString());
                        cmd.ExecuteNonQuery();

                    }

                    //transaction.Commit();
                    conn.Close();
                    //result = "employee added successfully!";
                }
                catch (Exception ex)
                {
                    //transaction.Rollback();
                    conn.Close();
                    //global.errorhandlerclass.logerror(ex);
                    //result = ex.message;
                }
            }

               connection.Disconnect();
        }

        public void GetLatestDeveloper()
        {
            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = "server = tcp:hatch.database.windows.net,1433; initial catalog = acrf; persist security info = false; user id = dct; password = Duck@1234; multipleactiveresultsets = false; encrypt = true; trustservercertificate = false; connection timeout = 30;";
                // using the code here...  
                conn.Close();
                //conn.Open();
                //SqlCommand cmd = conn.CreateCommand();
                //SqlTransaction transaction;
                //transaction = conn.BeginTransaction();
                //cmd.Transaction = transaction;
                //cmd.Connection = conn;
                try
                {
                    string sqlstr = "GetTFSTickets";
                    SqlCommand cmd = new SqlCommand(sqlstr, conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    List<LatestDeveloper> TFSList = new List<LatestDeveloper>();
                    LatestDeveloper rec = null;

                    while (reader.Read())
                    {
                        rec = new LatestDeveloper();
                        rec.ID = Convert.ToInt32(reader["ID"]);
                        rec.Developer = null;
                        rec.Status = null;
                        TFSList.Add(rec);
                    }
                    //transaction.Commit();
                    conn.Close();


                    //TFSList.Add(new LatestDeveloper
                    //{
                    //    ID = 7,
                    //    Developer = ""
                    //}); ;

                    //TFSList.Add(new LatestDeveloper
                    //{
                    //    ID =8,
                    //    Developer = ""
                    //}); ;
                    //TFSList.Add(new LatestDeveloper
                    //{
                    //    ID = 9,
                    //    Developer = ""
                    //}); ;
                    //TFSList.Add(new LatestDeveloper
                    //{
                    //    ID = 10,
                    //    Developer = ""
                    //}); ;


                     string INParam = string.Join(",", TFSList.Select(x => x.ID));

                    
                    Uri orgUrl = new Uri("https://dev.azure.com/DCTTFS/");
                    String personalAccessToken = "ndzzurgegqnpmjwrqhwji2c5exr7wffx4bzjhuccdfprfsdjuydq";
                    VssConnection connection = new VssConnection(orgUrl, new VssBasicCredential(string.Empty, personalAccessToken));

                    WorkItemTrackingHttpClient witClient = connection.GetClient<WorkItemTrackingHttpClient>();

                    Wiql wiql = new Wiql();

                    wiql.Query = "SELECT [System.Id],[Custom.Developer],[System.State] FROM workitems where [System.Id] IN (" + INParam + ") ";

                    WorkItemQueryResult tasks = witClient.QueryByWiqlAsync(wiql).Result;

                    IEnumerable<WorkItemReference> tasksRefs;
                    tasksRefs = tasks.WorkItems.OrderBy(x => x.Id);

                    List<WorkItem> tasksList = witClient.GetWorkItemsAsync(tasksRefs.Select(wir => wir.Id)).Result;

                    string sqlstrmulti = "";

                    foreach (var task in tasksList)
                    {

                        sqlstrmulti= sqlstrmulti += "Update TFSTicketsData set Developer='" + task.Fields["Custom.Developer"].ToString() + "' ,State='" + task.Fields["System.State"].ToString() + "'  where ID=" + task.Id + " ;" ;

                    }
                   
                    conn.Open();
                    SqlCommand cmd1 = new SqlCommand(sqlstrmulti, conn);
                    cmd1.CommandType = System.Data.CommandType.Text;
                    SqlDataReader sdr = cmd1.ExecuteReader();
                    conn.Close();
                }
                catch (Exception ex)
                {
                    //transaction.Rollback();
                    conn.Close();
                    //global.errorhandlerclass.logerror(ex);
                    //result = ex.message;
                }
                conn.Close();
            }
        }
        public void UpdateTickets()
        {
            Uri orgUrl = new Uri("https://dev.azure.com/DCTTFS/");
            String personalAccessToken = "ndzzurgegqnpmjwrqhwji2c5exr7wffx4bzjhuccdfprfsdjuydq";
            JsonPatchDocument patchDocument = new JsonPatchDocument();

            VssConnection connection = new VssConnection(orgUrl, new VssBasicCredential(string.Empty, personalAccessToken));

            WorkItemTrackingHttpClient witClient = connection.GetClient<WorkItemTrackingHttpClient>();

            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = "server = tcp:hatch.database.windows.net,1433; initial catalog = acrf; persist security info = false; user id = dct; password = Duck@1234; multipleactiveresultsets = false; encrypt = true; trustservercertificate = false; connection timeout = 30;";
                // using the code here...  
                conn.Close();
                //conn.Open();
                //SqlCommand cmd = conn.CreateCommand();
                //SqlTransaction transaction;
                //transaction = conn.BeginTransaction();
                //cmd.Transaction = transaction;
                //cmd.Connection = conn;
                try
                {
                    string sqlstr = "GetRecommendations";
                    SqlCommand cmd = new SqlCommand(sqlstr, conn);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Recommendation> TFSList = new List<Recommendation>();
                    Recommendation rec = null;

                    while (reader.Read())
                    {
                        rec = new Recommendation();
                        rec.ID = Convert.ToInt32(reader["ID"]);
                        rec.Developer = reader["Developer"].ToString();
                        TFSList.Add(rec);
                    }
                    //transaction.Commit();
                    conn.Close();
                    //result = "employee added successfully!";

                    foreach (var task in TFSList)
                    {
                        patchDocument.Clear();

                        patchDocument.Add(
                                          new JsonPatchOperation()
                                          {
                                              Operation = Operation.Add,
                                              Path = "/fields/Custom.Developer",
                                              Value = task.Developer
                                          }
                                      );

                        WorkItem result = witClient.UpdateWorkItemAsync(patchDocument, task.ID).Result;
                    }                                       
                }
                catch (Exception ex)
                {
                    //transaction.Rollback();
                    conn.Close();
                    //global.errorhandlerclass.logerror(ex);
                    //result = ex.message;
                }
                conn.Close();
            }

          
        }

        public void CallFlaskAPI()
        { 
        
        }

        private void UpdateTFS_Click(object sender, EventArgs e)
        {
            UpdateTickets();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            GetLatestDeveloper();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //JiraConnect jira = new JiraConnect();

//            jira.GetJiraTicket();
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

        public string Sprint { get; set; }
        public string Stream { get; set; }
    }

    public class Ticket1
    {

        public string ID { get; set; }

        public string Title { get; set; }

    }

    public class Recommendation
    {

        public int ID { get; set; }

        public string Developer { get; set; }

    }

    public class LatestDeveloper
    {

        public int ID { get; set; }

        public string Developer { get; set; }

        public string Status { get; set; }



    }
}
