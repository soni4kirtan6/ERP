namespace ConfigurationPortal
{
    partial class ConStringForm
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
            this.Title1 = new System.Windows.Forms.Label();
            this.server_name = new System.Windows.Forms.Label();
            this.db_name = new System.Windows.Forms.Label();
            this.u_name = new System.Windows.Forms.Label();
            this.pwd = new System.Windows.Forms.Label();
            this.txt_server_name = new System.Windows.Forms.TextBox();
            this.txt_database_name = new System.Windows.Forms.TextBox();
            this.txt_pwd = new System.Windows.Forms.TextBox();
            this.txt_u_nmae = new System.Windows.Forms.TextBox();
            this.btn_next = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // Title1
            // 
            this.Title1.AutoSize = true;
            this.Title1.Location = new System.Drawing.Point(25, 34);
            this.Title1.Name = "Title1";
            this.Title1.Size = new System.Drawing.Size(161, 13);
            this.Title1.TabIndex = 11;
            this.Title1.Text = "Enter Your Database Information";
            this.Title1.Click += new System.EventHandler(this.Label1_Click);
            // 
            // server_name
            // 
            this.server_name.AutoSize = true;
            this.server_name.Location = new System.Drawing.Point(64, 78);
            this.server_name.Name = "server_name";
            this.server_name.Size = new System.Drawing.Size(69, 13);
            this.server_name.TabIndex = 7;
            this.server_name.Text = "Server Name";
            // 
            // db_name
            // 
            this.db_name.AutoSize = true;
            this.db_name.Location = new System.Drawing.Point(64, 129);
            this.db_name.Name = "db_name";
            this.db_name.Size = new System.Drawing.Size(84, 13);
            this.db_name.TabIndex = 8;
            this.db_name.Text = "Database Name";
            // 
            // u_name
            // 
            this.u_name.AutoSize = true;
            this.u_name.Location = new System.Drawing.Point(64, 181);
            this.u_name.Name = "u_name";
            this.u_name.Size = new System.Drawing.Size(60, 13);
            this.u_name.TabIndex = 9;
            this.u_name.Text = "User Name";
            // 
            // pwd
            // 
            this.pwd.AutoSize = true;
            this.pwd.Location = new System.Drawing.Point(64, 241);
            this.pwd.Name = "pwd";
            this.pwd.Size = new System.Drawing.Size(53, 13);
            this.pwd.TabIndex = 10;
            this.pwd.Text = "Password";
            // 
            // txt_server_name
            // 
            this.txt_server_name.Location = new System.Drawing.Point(207, 75);
            this.txt_server_name.Name = "txt_server_name";
            this.txt_server_name.Size = new System.Drawing.Size(164, 20);
            this.txt_server_name.TabIndex = 1;
            // 
            // txt_database_name
            // 
            this.txt_database_name.Location = new System.Drawing.Point(207, 126);
            this.txt_database_name.Name = "txt_database_name";
            this.txt_database_name.Size = new System.Drawing.Size(164, 20);
            this.txt_database_name.TabIndex = 2;
            // 
            // txt_pwd
            // 
            this.txt_pwd.Location = new System.Drawing.Point(207, 238);
            this.txt_pwd.Name = "txt_pwd";
            this.txt_pwd.PasswordChar = '*';
            this.txt_pwd.Size = new System.Drawing.Size(164, 20);
            this.txt_pwd.TabIndex = 4;
            // 
            // txt_u_nmae
            // 
            this.txt_u_nmae.Location = new System.Drawing.Point(207, 178);
            this.txt_u_nmae.Name = "txt_u_nmae";
            this.txt_u_nmae.Size = new System.Drawing.Size(164, 20);
            this.txt_u_nmae.TabIndex = 3;
            // 
            // btn_next
            // 
            this.btn_next.Location = new System.Drawing.Point(296, 302);
            this.btn_next.Name = "btn_next";
            this.btn_next.Size = new System.Drawing.Size(75, 23);
            this.btn_next.TabIndex = 5;
            this.btn_next.Text = "Next >";
            this.btn_next.UseVisualStyleBackColor = true;
            this.btn_next.Click += new System.EventHandler(this.Btn_next_Click);
            // 
            // ConString
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btn_next);
            this.Controls.Add(this.txt_u_nmae);
            this.Controls.Add(this.txt_pwd);
            this.Controls.Add(this.txt_database_name);
            this.Controls.Add(this.txt_server_name);
            this.Controls.Add(this.pwd);
            this.Controls.Add(this.u_name);
            this.Controls.Add(this.db_name);
            this.Controls.Add(this.server_name);
            this.Controls.Add(this.Title1);
            this.Name = "ConString";
            this.Text = "Connection String";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label Title1;
        private System.Windows.Forms.Label server_name;
        private System.Windows.Forms.Label db_name;
        private System.Windows.Forms.Label u_name;
        private System.Windows.Forms.Label pwd;
        private System.Windows.Forms.TextBox txt_server_name;
        private System.Windows.Forms.TextBox txt_database_name;
        private System.Windows.Forms.TextBox txt_pwd;
        private System.Windows.Forms.TextBox txt_u_nmae;
        private System.Windows.Forms.Button btn_next;
    }
}

