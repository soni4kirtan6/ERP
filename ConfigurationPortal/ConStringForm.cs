using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace ConfigurationPortal
{
    public partial class ConStringForm : Form
    {
        public ConStringForm()
        {
            InitializeComponent();
        }

        private void Label1_Click(object sender, EventArgs e)
        {

        }

        private void Btn_next_Click(object sender, EventArgs e)
        {
            string server = txt_server_name.Text;
            string db = txt_database_name.Text;
            string u_id = txt_u_nmae.Text;
            string pwd = txt_pwd.Text;
            string connStr= "Server= "+server+";Database="+db+";Username="+u_id+";Password="+pwd+";";

            Directory.CreateDirectory(Environment.CurrentDirectory + "/OutputFiles");
            string path = Environment.CurrentDirectory + "/OutputFiles/" + "ConnectionString.txt";
            File.WriteAllText(path, connStr);

            DatabaseStructureForm db_form = new DatabaseStructureForm();
            //db_form.Activate();
            this.Hide();
            db_form.Show();
            //Title1.Text =connStr;
        }

        private void ConStringForm_Load(object sender, EventArgs e)
        {

        }
    }
}
