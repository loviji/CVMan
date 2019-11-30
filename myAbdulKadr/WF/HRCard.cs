using Microsoft.Reporting.WinForms;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Windows.Forms;

namespace PersonMotion.WF
{
    public partial class HRCard : Form
    {
        public string personID = string.Empty;
        private string s = @"data source=.\sqlexpress;initial catalog=people;integrated security=True;";

        public HRCard()
        {
            InitializeComponent();
        }

        private void ResizeForm()
        {
            Screen scrn = Screen.FromControl(this);
            if (scrn == null)
            {
                scrn = Screen.PrimaryScreen;
            }
            int Height = scrn.WorkingArea.Height;

            this.StartPosition = FormStartPosition.CenterScreen;
            this.Height = Height;
            this.CenterToScreen();
        }

        private void HRCard_Load(object sender, EventArgs e)
        {

            ResizeForm();
            string saheKeep = string.Empty;

            SqlConnection con = new SqlConnection(s);
            string selectSQL = @"SELECT  [id]
           ,[soyadi]
      ,[adi]
      ,[ata_adi]
      ,[tevelludu]
      ,[doguld_yer]
      ,[milliyyeti]
      ,[partiya]
      ,[herbi_rutb]
      ,[tehsili]
      ,[bittehmues]
      ,[dipuzixt]
      ,[ev_unvani]
      ,[telefon]
      ,[mobil]
      ,[aile_veziy]
      ,[idare]
      ,[sexshobe]
      ,[komqer]
      ,[vezifesi]
      ,[teytar]
      ,[iscixtar]
      ,[emekfealiy]
      ,[atestar]
      ,[sebeb]
      ,[oklad]
      ,[sahe]
      ,[tehnovu]
      ,[bitvaxt]
,[photo]
  FROM [people].[dbo].[EMP_CV] ";

            string whereSQL = @"WHERE ID=" + personID;
            string TOPSQL = selectSQL + whereSQL;

            SqlDataAdapter da = new SqlDataAdapter(TOPSQL, con);
            con.Open();
            DataTable dtr = new DataTable();
            da.Fill(dtr);
                                 
            reportViewer1.LocalReport.DataSources.Clear();
            reportViewer1.LocalReport.EnableExternalImages = true;
            ReportDataSource rds = new ReportDataSource();
            rds.Name = "DataSet1";
            rds.Value = dtr;

            reportViewer1.LocalReport.DataSources.Add(rds);
            reportViewer1.RefreshReport();
        }

        private void HRCard_FormClosing(object sender, FormClosingEventArgs e)
        {
            reportViewer1.Dispose();
        }
    }
}
