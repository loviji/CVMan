using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlServerCe;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PersonMotion.WF
{
    public partial class HRCard : Form
    {
        public string personID = string.Empty;
        public string dbid = string.Empty;
        public string name = string.Empty;
        public string orgID = string.Empty;
        Control m_pgv_1 = null;
        Control m_pgv_2 = null;
        MethodInfo m_method_info;
        //private string s = @"Data Source=..\..\App_Data\kadrAzerEnerji.sdf;Max Database Size = 4091; Max Buffer Size = 1024;";
        private string s = @"Data Source=C:\kadrAzerenerjisix\App_Data\kadrAzerEnerji.sdf;Max Database Size = 4091; Max Buffer Size = 1024;";
        private void propertyGrid1_Scroll(object sender, ScrollEventArgs e)
        {
            MessageBox.Show("salam");

            // Set the new scroll position on the neighboring
            // PropertyGridView

        }

        private static Control FindControl(
            Control.ControlCollection controls, string name)
        {
            foreach (Control c in controls)
            {
                if (c.Text == name)
                    return c;
            }

            return null;
        }

        private static MethodInfo FindMethod(Type type, string method)
        {
            foreach (MethodInfo mi in type.GetMethods())
            {
                if (method == mi.Name)
                    return mi;
            }

            return null;
        }

        private static FieldInfo FindField(Type type, string field)
        {
            FieldInfo f = type.GetField(field);

            return f;
        }

        private Dictionary<int, string> photoIndex = new Dictionary<int, string>()
        {
            {1,"Azərenerji"},
            {21,"İnstitut"},
            {22,"Azərenerjikomplektləşdirmə"},
            {23,"EİVŞ"},
            {24,"HMD"},
            {31,"Azərbaycan İES"},
            {32,"Şirvan İES"},
            {33,"Mingəçevir SES Silsiləsi"},
            {34,"Şəmkir SES Silsiləsi"},
            {35,"Füzuli SES"},
            {36,"Bakı İEM"},
            {37,"Bakı ES"},
            {38,"Şimal ES"},
            {39,"Səngəçal ES"},
            {310,"Sumqayıt ES"},
            {311,"Astara ES"},
            {312,"Xaçmaz ES"},
            {313,"Şəki ES"},
            {314,"Şahdağ ES"},
            {315,"Cənub ES"},
            {41,"Sumqayıt YGEŞ"},
            {51,"Abşeron REŞ"},
            {52,"Gəncə REŞ"},
            {53,"Mingəçevir REŞ"},
            {54,"Şirvan REŞ"},
            {55,"İmişli REŞ MMC"},
            {56,"Xaçmaz REŞ"},
            {57,"Sumqayıt REŞ"},
            {58,"Şəki REŞ MMC"},
            {59,"Qəbələ REŞ"}
        };

        public HRCard()
        {
            InitializeComponent();
            // Set the Property Grid Object to something

            // Loop through sub-controlls and find PropertyGridView
            //  m_pgv_1 = this.reportViewer1;


            //  // Reflection trickery to get a private/internal field
            //  // and method, scrollBar and SetScrollOffset in this case
            //  Type type = m_pgv_1.GetType();
            //  FieldInfo f = FindField(type, "IsAccessible");
            // // m_method_info = FindMethod(type, "SetScrollOffset");
            ////  reportViewer1.IsAccessible
            //  // Get the scrollBar for our PropertyGrid and add the event handler
            //  ((ScrollBar)f.GetValue(m_pgv_1)).Scroll +=
            //      new ScrollEventHandler(propertyGrid1_Scroll);


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

            SqlCeConnection con = new SqlCeConnection(s);
            string selectSQL = @"SELECT     id, dbid, soyadi,  adi, ata_adi, CONVERT(nvarchar(30),tevelludu,104) tevelludu, doguld_yer, milliyyeti, partiya, herbi_rutb, tehsili, bittehmues, dipuzixt, ev_unvani, telefon, mobil, aile_veziy, case when sahe like '%ASC%' then sahe+', '+idare else idare end as idare, 
                      sexshobe, komqer, vezifesi, CONVERT(nvarchar(30),teytar,104) teytar, emekfealiy, CONVERT(nvarchar(30),iscixtar,104) iscixtar, CONVERT(nvarchar(30),  atestar,104) as  atestar, sebeb, oklad, sahe, tehnovu,bitvaxt ";
            string fromSQL = @"FROM kadrMain ";
            string whereSQL = @"WHERE ID=" + personID;
            string TOPSQL = selectSQL + fromSQL + whereSQL;

            SqlCeDataAdapter da = new SqlCeDataAdapter(TOPSQL, con);
            con.Open();
            DataTable dtr = new DataTable();
            da.Fill(dtr);


          //  saheKeep = dtr.Rows[0]["sahe"].ToString();
            string prefix = photoIndex.AsQueryable().Where(k => k.Value == saheKeep.Trim()).FirstOrDefault().Key.ToString();



            //   linkLabel1.Text = string.Empty;


            reportViewer1.LocalReport.DataSources.Clear();
            reportViewer1.LocalReport.EnableExternalImages = true;


            ReportDataSource rds = new ReportDataSource();
            rds.Name = "DataSet1";
            rds.Value = dtr;

            reportViewer1.LocalReport.DataSources.Add(rds);

            dbid = dtr.Rows[0]["dbid"].ToString();
            //ReportParameter paramImg = new ReportParameter("s", 18470 + @"\" + 18470);
            //reportViewer1.LocalReport.SetParameters(paramImg);

            //reportViewer1.LocalReport.Refresh();

            name = dtr.Rows[0]["soyadi"].ToString() + " " + dtr.Rows[0]["adi"].ToString() + " " + dtr.Rows[0]["ata_adi"].ToString();
            orgID = prefix;
            reportViewer1.RefreshReport();

            //Warning[] warnings;
            //string[] streamids;
            //string mimeType;
            //string encoding;
            //string extension;

            //string filename = dtr.Rows[0]["soyadi"].ToString() + ' ' + dtr.Rows[0]["adi"].ToString() + '.' + dtr.Rows[0]["ata_adi"].ToString().Substring(0, 1);
            //string sexshobe = dtr.Rows[0]["sexshobe"].ToString();

            //if (string.IsNullOrEmpty(sexshobe))
            //    sexshobe = "boş";
            //string path = @"C:\output\";
            //path = path + dtr.Rows[0]["sexshobe"].ToString();
            //if (!Directory.Exists(path))
            //{
            //    Directory.CreateDirectory(path);
            //}


            //byte[] bytes = reportViewer1.LocalReport.Render(
            //   "Excel", null, out mimeType, out encoding,
            //    out extension,
            //   out streamids, out warnings);

            //FileStream fs = new FileStream(path+"\\"+filename+".xls",
            //   FileMode.Create);
            //fs.Write(bytes, 0, bytes.Length);
            //fs.Close();


        }
        
        private void HRCard_FormClosing(object sender, FormClosingEventArgs e)
        {
            reportViewer1.Dispose();
        }
    }
}
