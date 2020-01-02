using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ConfigurationPortal
{
    public partial class MappingForm : Form
    {
        static public JObject db_structure_json = DatabaseStructureForm.db_structure_json;
        static public JObject db_table_mapping_json = new JObject();
        public MappingForm()
        {
            InitializeComponent();
        }

        private void MappingForm_Load(object sender, EventArgs e)
        {
            int i = 0;
            //MessageBox.Show("check");
            //tableLayoutPanel1.Height;
            tableLayoutPanel1.RowCount = db_structure_json.Properties().Count();
            Label label = new Label();

            foreach (var table in db_structure_json)
            {
                Label lbl_db_name = new Label();
                lbl_db_name.TextAlign = ContentAlignment.MiddleCenter;
                lbl_db_name.Text = table.Key.ToString();
                lbl_db_name.Name = "lbl_"+table.Key.ToString();
                tableLayoutPanel1.Controls.Add(lbl_db_name, 0, i);

                TextBox txt_alter_name = new TextBox();
                txt_alter_name.Width = tableLayoutPanel1.Width / 2-2;
                lbl_db_name.Name = "txt_" + table.Key.ToString();
                tableLayoutPanel1.Controls.Add(txt_alter_name, 2, i++);
            }
        }

        private void TableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {
            
            
        }

        private void DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void TableLayoutPanel1_Paint_1(object sender, PaintEventArgs e)
        {

        }

        private void Button1_Click(object sender, EventArgs e)
        {
            //for (int i = 0; i < tableLayoutPanel1.RowCount; i++)
            //{

            //    db_table_mapping_json.Add(tableLayoutPanel1.Controls.GetEnumerator().Current.ToString());
            //}
            string key = "";
            string value = "";
            int i = 0;
            foreach (Control c in tableLayoutPanel1.Controls)
            {
                if (i % 2 == 0)
                {
                    value = c.Text;
                }
                else
                {
                    key = c.Text;
                    db_table_mapping_json.Add(key, value);
                }

                i++;
            }
            string path = Environment.CurrentDirectory + "/OutputFiles/" + "db_table_mapping_json.json";
            File.WriteAllText(path, db_table_mapping_json.ToString());
            MessageBox.Show("File Saved Successfully");

            i = 0;
            foreach (Control c in tableLayoutPanel1.Controls)
            {
                if (i % 2 == 0)
                {
                    MappingColForm mcf = new MappingColForm();
                    MappingColForm.cur_table = c.Text;
                    mcf.Show();

                }
                else
                {
                   
                }

                i++;
            }
            
        }
    }
}
