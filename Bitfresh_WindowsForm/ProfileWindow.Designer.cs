namespace Bitfresh_WindowsForm
{
    partial class ProfileWindow
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
            this.UsersListView = new System.Windows.Forms.ListBox();
            this.label6 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.UserStatusLabel = new System.Windows.Forms.Label();
            this.AddButton = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.PassBox2 = new System.Windows.Forms.TextBox();
            this.PassBox1 = new System.Windows.Forms.TextBox();
            this.SecretBox = new System.Windows.Forms.TextBox();
            this.ApiBox = new System.Windows.Forms.TextBox();
            this.UserBox = new System.Windows.Forms.TextBox();
            this.Deletebtn = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // UsersListView
            // 
            this.UsersListView.FormattingEnabled = true;
            this.UsersListView.Location = new System.Drawing.Point(377, 47);
            this.UsersListView.Name = "UsersListView";
            this.UsersListView.Size = new System.Drawing.Size(214, 251);
            this.UsersListView.TabIndex = 9;
            this.UsersListView.SelectedIndexChanged += new System.EventHandler(this.UsersListView_SelectedIndexChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(374, 31);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(83, 13);
            this.label6.TabIndex = 10;
            this.label6.Text = "Loaded Profiles:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.UserStatusLabel);
            this.groupBox1.Controls.Add(this.AddButton);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.PassBox2);
            this.groupBox1.Controls.Add(this.PassBox1);
            this.groupBox1.Controls.Add(this.SecretBox);
            this.groupBox1.Controls.Add(this.ApiBox);
            this.groupBox1.Controls.Add(this.UserBox);
            this.groupBox1.Location = new System.Drawing.Point(28, 31);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(323, 313);
            this.groupBox1.TabIndex = 12;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Add User";
            // 
            // UserStatusLabel
            // 
            this.UserStatusLabel.AutoSize = true;
            this.UserStatusLabel.Location = new System.Drawing.Point(6, 280);
            this.UserStatusLabel.Name = "UserStatusLabel";
            this.UserStatusLabel.Size = new System.Drawing.Size(56, 13);
            this.UserStatusLabel.TabIndex = 20;
            this.UserStatusLabel.Text = "DEFAULT";
            this.UserStatusLabel.Visible = false;
            // 
            // AddButton
            // 
            this.AddButton.Location = new System.Drawing.Point(231, 275);
            this.AddButton.Name = "AddButton";
            this.AddButton.Size = new System.Drawing.Size(75, 23);
            this.AddButton.TabIndex = 19;
            this.AddButton.Text = "Add";
            this.AddButton.UseVisualStyleBackColor = true;
            this.AddButton.Click += new System.EventHandler(this.AddButton_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(2, 234);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(94, 13);
            this.label5.TabIndex = 18;
            this.label5.Text = "Repeat Password:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(40, 201);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(56, 13);
            this.label4.TabIndex = 17;
            this.label4.Text = "Password:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(53, 92);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(43, 13);
            this.label3.TabIndex = 15;
            this.label3.Text = "KeyApi:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(37, 128);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 13);
            this.label2.TabIndex = 16;
            this.label2.Text = "KeySecret:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(49, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 13);
            this.label1.TabIndex = 14;
            this.label1.Text = "Username:";
            // 
            // PassBox2
            // 
            this.PassBox2.Location = new System.Drawing.Point(102, 231);
            this.PassBox2.Name = "PassBox2";
            this.PassBox2.PasswordChar = '•';
            this.PassBox2.Size = new System.Drawing.Size(193, 20);
            this.PassBox2.TabIndex = 13;
            this.PassBox2.TextChanged += new System.EventHandler(this.PassBox2_TextChanged);
            // 
            // PassBox1
            // 
            this.PassBox1.Location = new System.Drawing.Point(102, 198);
            this.PassBox1.Name = "PassBox1";
            this.PassBox1.PasswordChar = '•';
            this.PassBox1.Size = new System.Drawing.Size(193, 20);
            this.PassBox1.TabIndex = 12;
            this.PassBox1.TextChanged += new System.EventHandler(this.PassBox1_TextChanged);
            // 
            // SecretBox
            // 
            this.SecretBox.Location = new System.Drawing.Point(102, 125);
            this.SecretBox.Name = "SecretBox";
            this.SecretBox.PasswordChar = '•';
            this.SecretBox.Size = new System.Drawing.Size(193, 20);
            this.SecretBox.TabIndex = 11;
            this.SecretBox.TextChanged += new System.EventHandler(this.SecretBox_TextChanged);
            // 
            // ApiBox
            // 
            this.ApiBox.Location = new System.Drawing.Point(102, 89);
            this.ApiBox.Name = "ApiBox";
            this.ApiBox.Size = new System.Drawing.Size(193, 20);
            this.ApiBox.TabIndex = 10;
            this.ApiBox.TextChanged += new System.EventHandler(this.ApiBox_TextChanged);
            // 
            // UserBox
            // 
            this.UserBox.Location = new System.Drawing.Point(113, 19);
            this.UserBox.Name = "UserBox";
            this.UserBox.Size = new System.Drawing.Size(193, 20);
            this.UserBox.TabIndex = 9;
            this.UserBox.TextChanged += new System.EventHandler(this.UserBox_TextChanged);
            // 
            // Deletebtn
            // 
            this.Deletebtn.Location = new System.Drawing.Point(516, 301);
            this.Deletebtn.Name = "Deletebtn";
            this.Deletebtn.Size = new System.Drawing.Size(75, 23);
            this.Deletebtn.TabIndex = 13;
            this.Deletebtn.Text = "Delete";
            this.Deletebtn.UseVisualStyleBackColor = true;
            this.Deletebtn.Click += new System.EventHandler(this.Deletebtn_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(6, 173);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(127, 13);
            this.label7.TabIndex = 21;
            this.label7.Text = "Password Protection:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(2, 64);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(114, 13);
            this.label8.TabIndex = 22;
            this.label8.Text = "Bittrex Information:";
            // 
            // ProfileWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(626, 376);
            this.Controls.Add(this.Deletebtn);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.UsersListView);
            this.Name = "ProfileWindow";
            this.ShowIcon = false;
            this.Text = "Profile Manager";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ProfileWindow_FormClosing);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ListBox UsersListView;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button AddButton;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox PassBox2;
        private System.Windows.Forms.TextBox PassBox1;
        private System.Windows.Forms.TextBox SecretBox;
        private System.Windows.Forms.TextBox ApiBox;
        private System.Windows.Forms.TextBox UserBox;
        private System.Windows.Forms.Button Deletebtn;
        private System.Windows.Forms.Label UserStatusLabel;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
    }
}