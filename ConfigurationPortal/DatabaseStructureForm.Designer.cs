namespace ConfigurationPortal
{
    partial class DatabaseStructureForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.list_table = new System.Windows.Forms.ListBox();
            this.tb_table_name = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btn_add_table = new System.Windows.Forms.Button();
            this.btn_add_col = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.tb_col_name = new System.Windows.Forms.TextBox();
            this.list_col = new System.Windows.Forms.ListBox();
            this.btn_next = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.lbl_table_name = new System.Windows.Forms.Label();
            this.btn_save_col = new System.Windows.Forms.Button();
            this.tb_json = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(34, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(203, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Enter your Database Tables and Columns";
            // 
            // list_table
            // 
            this.list_table.AllowDrop = true;
            this.list_table.FormattingEnabled = true;
            this.list_table.HorizontalScrollbar = true;
            this.list_table.Location = new System.Drawing.Point(60, 202);
            this.list_table.Name = "list_table";
            this.list_table.Size = new System.Drawing.Size(245, 186);
            this.list_table.TabIndex = 1;
            this.list_table.SelectedIndexChanged += new System.EventHandler(this.List_table_SelectedIndexChanged);
            // 
            // tb_table_name
            // 
            this.tb_table_name.Location = new System.Drawing.Point(162, 109);
            this.tb_table_name.Name = "tb_table_name";
            this.tb_table_name.Size = new System.Drawing.Size(137, 20);
            this.tb_table_name.TabIndex = 2;
            this.tb_table_name.TextChanged += new System.EventHandler(this.Tb_table_name_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(57, 109);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Table Name";
            // 
            // btn_add_table
            // 
            this.btn_add_table.Location = new System.Drawing.Point(224, 157);
            this.btn_add_table.Name = "btn_add_table";
            this.btn_add_table.Size = new System.Drawing.Size(75, 23);
            this.btn_add_table.TabIndex = 6;
            this.btn_add_table.Text = "Add Table";
            this.btn_add_table.UseVisualStyleBackColor = true;
            this.btn_add_table.Click += new System.EventHandler(this.Btn_add_table_Click);
            // 
            // btn_add_col
            // 
            this.btn_add_col.Enabled = false;
            this.btn_add_col.Location = new System.Drawing.Point(639, 157);
            this.btn_add_col.Name = "btn_add_col";
            this.btn_add_col.Size = new System.Drawing.Size(75, 23);
            this.btn_add_col.TabIndex = 11;
            this.btn_add_col.Text = "Add column";
            this.btn_add_col.UseVisualStyleBackColor = true;
            this.btn_add_col.Click += new System.EventHandler(this.Button1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Enabled = false;
            this.label2.Location = new System.Drawing.Point(472, 109);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(73, 13);
            this.label2.TabIndex = 10;
            this.label2.Text = "Column Name";
            // 
            // tb_col_name
            // 
            this.tb_col_name.Enabled = false;
            this.tb_col_name.Location = new System.Drawing.Point(577, 109);
            this.tb_col_name.Name = "tb_col_name";
            this.tb_col_name.Size = new System.Drawing.Size(137, 20);
            this.tb_col_name.TabIndex = 9;
            // 
            // list_col
            // 
            this.list_col.AllowDrop = true;
            this.list_col.Enabled = false;
            this.list_col.FormattingEnabled = true;
            this.list_col.HorizontalScrollbar = true;
            this.list_col.Location = new System.Drawing.Point(475, 202);
            this.list_col.Name = "list_col";
            this.list_col.Size = new System.Drawing.Size(245, 186);
            this.list_col.TabIndex = 12;
            // 
            // btn_next
            // 
            this.btn_next.Location = new System.Drawing.Point(990, 469);
            this.btn_next.Name = "btn_next";
            this.btn_next.Size = new System.Drawing.Size(75, 23);
            this.btn_next.TabIndex = 13;
            this.btn_next.Text = "Next >";
            this.btn_next.UseVisualStyleBackColor = true;
            this.btn_next.Click += new System.EventHandler(this.Btn_next_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(149, 391);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(84, 13);
            this.label4.TabIndex = 14;
            this.label4.Text = "Table Name List";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Enabled = false;
            this.label5.Location = new System.Drawing.Point(565, 391);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(92, 13);
            this.label5.TabIndex = 15;
            this.label5.Text = "Column Name List";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Enabled = false;
            this.label6.Location = new System.Drawing.Point(472, 74);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(113, 13);
            this.label6.TabIndex = 16;
            this.label6.Text = "Add Columns of table :";
            // 
            // lbl_table_name
            // 
            this.lbl_table_name.AutoSize = true;
            this.lbl_table_name.Enabled = false;
            this.lbl_table_name.Location = new System.Drawing.Point(591, 74);
            this.lbl_table_name.Name = "lbl_table_name";
            this.lbl_table_name.Size = new System.Drawing.Size(96, 13);
            this.lbl_table_name.TabIndex = 17;
            this.lbl_table_name.Text = "No Table Selected";
            // 
            // btn_save_col
            // 
            this.btn_save_col.Location = new System.Drawing.Point(736, 365);
            this.btn_save_col.Name = "btn_save_col";
            this.btn_save_col.Size = new System.Drawing.Size(88, 23);
            this.btn_save_col.TabIndex = 18;
            this.btn_save_col.Text = "Save Columns";
            this.btn_save_col.UseVisualStyleBackColor = true;
            this.btn_save_col.Click += new System.EventHandler(this.Btn_save_col_Click);
            // 
            // tb_json
            // 
            this.tb_json.Location = new System.Drawing.Point(858, 202);
            this.tb_json.Multiline = true;
            this.tb_json.Name = "tb_json";
            this.tb_json.Size = new System.Drawing.Size(207, 186);
            this.tb_json.TabIndex = 19;
            // 
            // DatabaseStructure
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1093, 504);
            this.Controls.Add(this.tb_json);
            this.Controls.Add(this.btn_save_col);
            this.Controls.Add(this.lbl_table_name);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.btn_next);
            this.Controls.Add(this.list_col);
            this.Controls.Add(this.btn_add_col);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tb_col_name);
            this.Controls.Add(this.btn_add_table);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tb_table_name);
            this.Controls.Add(this.list_table);
            this.Controls.Add(this.label1);
            this.Name = "DatabaseStructure";
            this.Text = "Database Structure";
            this.Load += new System.EventHandler(this.DatabaseStructure_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox list_table;
        private System.Windows.Forms.TextBox tb_table_name;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btn_add_table;
        private System.Windows.Forms.Button btn_add_col;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tb_col_name;
        private System.Windows.Forms.ListBox list_col;
        private System.Windows.Forms.Button btn_next;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label lbl_table_name;
        private System.Windows.Forms.Button btn_save_col;
        private System.Windows.Forms.TextBox tb_json;
    }
}