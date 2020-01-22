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
    public partial class DatabaseStructureForm : Form
    {
        static public JObject db_structure_json = new JObject();
        public DatabaseStructureForm()
        {
            InitializeComponent();
        }

        private void DatabaseStructure_Load(object sender, EventArgs e)
        {
            
        }

        private void Btn_add_table_Click(object sender, EventArgs e)
        {
            if (!list_table.Items.Contains(tb_table_name.Text))
            {
                list_table.Items.Add(tb_table_name.Text);
            }
            else
            {
                MessageBox.Show("Table Already Inserted !");
            }
            tb_table_name.Focus();
            tb_table_name.Clear();
        }

        private void Btn_start_col_Click(object sender, EventArgs e)
        {

        }

        private void Button1_Click(object sender, EventArgs e)
        {
            if (!list_col.Items.Contains(tb_col_name.Text))
            {
                list_col.Items.Add(tb_col_name.Text);
            }
            else
            {
                MessageBox.Show("Column Already Inserted !");
            }
            tb_col_name.Focus();
            tb_col_name.Clear();
        }

        private void Tb_table_name_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void List_table_SelectedIndexChanged(object sender, EventArgs e)
        {
            //MessageBox.Show(list_table.SelectedItem.ToString());
            label2.Enabled = true;
            label5.Enabled = true;
            tb_col_name.Enabled = true;
            btn_add_col.Enabled = true;
            list_col.Enabled = true;
            lbl_table_name.Enabled = true;
            label6.Enabled = true;

            lbl_table_name.Text = list_table.SelectedItem.ToString();

        }

        private void Btn_save_col_Click(object sender, EventArgs e)
        {
            string cur_tb = list_table.SelectedItem.ToString();
            JArray cur_cols = new JArray();
            foreach(string col in list_col.Items)
            {
                cur_cols.Add(col);
            }
            try
            {
            db_structure_json.Add(cur_tb, cur_cols);
            }
            catch (Exception)
            {
                //
            }
            list_col.Items.Clear();
            tb_json.Text = db_structure_json.ToString();
        }

        private void Btn_next_Click(object sender, EventArgs e)
        {
            string path = Environment.CurrentDirectory + "/OutputFiles/" + "db_structure_json.json";
            File.WriteAllText(path, db_structure_json.ToString());
            MessageBox.Show("File saved Succesfully");

            MappingForm mappingForm = new MappingForm();
            this.Hide();
            mappingForm.Show();
        }
    }
}
