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
    public partial class MappingColForm : Form
    {
        static public string cur_table = "";
        static public JObject db_structure_json = DatabaseStructureForm.db_structure_json;
        static public JObject db_column_mapping_json = new JObject();
        static public JObject db_column_mapping_inner_json = new JObject();
        
        public MappingColForm()
        {
            InitializeComponent();
        }

        private void MappingColForm_Load(object sender, EventArgs e)
        {
            MessageBox.Show("Current Table : " + cur_table);
            lbl_cur_table.Text = cur_table;
            int i = 0;
            //MessageBox.Show("check");
            //tableLayoutPanel1.Height;
            tableLayoutPanel1.RowCount = ((JArray)db_structure_json[cur_table]).Count;
            //Label label = new Label();

            foreach (var col in ((JArray)db_structure_json[cur_table]))
            {
                Label lbl_db_name = new Label();
                lbl_db_name.TextAlign = ContentAlignment.MiddleCenter;
                lbl_db_name.Text = col.ToString();
                lbl_db_name.Name = "lbl_" + col.ToString();
                tableLayoutPanel1.Controls.Add(lbl_db_name, 0, i);

                TextBox txt_alter_name = new TextBox();
                txt_alter_name.Width = tableLayoutPanel1.Width / 2 - 2;
                lbl_db_name.Name = "txt_" + col.ToString();
                tableLayoutPanel1.Controls.Add(txt_alter_name, 2, i++);
            }
        }

        private void Btn_save_next_Click(object sender, EventArgs e)
        {
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
                    db_column_mapping_inner_json.Add(key, value);
                }

                i++;
            }
            db_column_mapping_json.Add(cur_table,db_column_mapping_inner_json);
            string path = Environment.CurrentDirectory + "/OutputFiles/" + "db_Column_mapping_json.json";
            File.AppendAllText(path, db_column_mapping_json.ToString());
            MessageBox.Show("File Saved Successfully");
        }
    }
}
