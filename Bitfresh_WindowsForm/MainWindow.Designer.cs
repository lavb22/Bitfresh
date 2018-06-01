namespace Bitfresh_WindowsForm
{
    partial class MainWindow
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
            this.components = new System.ComponentModel.Container();
            this.ConnectButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.StatusLabel = new System.Windows.Forms.Label();
            this.confBtn = new System.Windows.Forms.Button();
            this.OrdersView = new System.Windows.Forms.DataGridView();
            this.iDDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.eXCHANGEDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.aMMOUNTDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tYPEDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sTATUSDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cREATEDDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tIMELEFTDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.orderListBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.mainWindowBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.UsersComboBox = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.ProfManagerButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.OrdersView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.orderListBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mainWindowBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // ConnectButton
            // 
            this.ConnectButton.BackColor = System.Drawing.Color.LightBlue;
            this.ConnectButton.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.ConnectButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ConnectButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ConnectButton.Location = new System.Drawing.Point(500, 27);
            this.ConnectButton.Name = "ConnectButton";
            this.ConnectButton.Size = new System.Drawing.Size(154, 38);
            this.ConnectButton.TabIndex = 0;
            this.ConnectButton.Text = "Connect";
            this.ConnectButton.UseVisualStyleBackColor = false;
            this.ConnectButton.Click += new System.EventHandler(this.Button_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(497, 83);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(40, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Status:";
            // 
            // StatusLabel
            // 
            this.StatusLabel.AutoSize = true;
            this.StatusLabel.Location = new System.Drawing.Point(543, 83);
            this.StatusLabel.Name = "StatusLabel";
            this.StatusLabel.Size = new System.Drawing.Size(56, 13);
            this.StatusLabel.TabIndex = 5;
            this.StatusLabel.Text = "DEFAULT";
            // 
            // confBtn
            // 
            this.confBtn.BackgroundImage = global::Bitfresh_WindowsForm.Properties.Resources.gear;
            this.confBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.confBtn.Location = new System.Drawing.Point(674, 30);
            this.confBtn.Name = "confBtn";
            this.confBtn.Size = new System.Drawing.Size(34, 33);
            this.confBtn.TabIndex = 6;
            this.confBtn.UseVisualStyleBackColor = true;
            this.confBtn.Click += new System.EventHandler(this.conf_Clik);
            // 
            // OrdersView
            // 
            this.OrdersView.AutoGenerateColumns = false;
            this.OrdersView.BackgroundColor = System.Drawing.Color.White;
            this.OrdersView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.OrdersView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.iDDataGridViewTextBoxColumn,
            this.eXCHANGEDataGridViewTextBoxColumn,
            this.aMMOUNTDataGridViewTextBoxColumn,
            this.tYPEDataGridViewTextBoxColumn,
            this.sTATUSDataGridViewTextBoxColumn,
            this.cREATEDDataGridViewTextBoxColumn,
            this.tIMELEFTDataGridViewTextBoxColumn});
            this.OrdersView.DataSource = this.orderListBindingSource;
            this.OrdersView.Location = new System.Drawing.Point(35, 124);
            this.OrdersView.MultiSelect = false;
            this.OrdersView.Name = "OrdersView";
            this.OrdersView.ReadOnly = true;
            this.OrdersView.RowHeadersVisible = false;
            this.OrdersView.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.OrdersView.Size = new System.Drawing.Size(703, 258);
            this.OrdersView.TabIndex = 9;
            // 
            // iDDataGridViewTextBoxColumn
            // 
            this.iDDataGridViewTextBoxColumn.DataPropertyName = "ID";
            this.iDDataGridViewTextBoxColumn.HeaderText = "ID Number";
            this.iDDataGridViewTextBoxColumn.Name = "iDDataGridViewTextBoxColumn";
            this.iDDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // eXCHANGEDataGridViewTextBoxColumn
            // 
            this.eXCHANGEDataGridViewTextBoxColumn.DataPropertyName = "EXCHANGE";
            this.eXCHANGEDataGridViewTextBoxColumn.HeaderText = "Exchange";
            this.eXCHANGEDataGridViewTextBoxColumn.Name = "eXCHANGEDataGridViewTextBoxColumn";
            this.eXCHANGEDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // aMMOUNTDataGridViewTextBoxColumn
            // 
            this.aMMOUNTDataGridViewTextBoxColumn.DataPropertyName = "AMMOUNT";
            this.aMMOUNTDataGridViewTextBoxColumn.HeaderText = "Ammount";
            this.aMMOUNTDataGridViewTextBoxColumn.Name = "aMMOUNTDataGridViewTextBoxColumn";
            this.aMMOUNTDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // tYPEDataGridViewTextBoxColumn
            // 
            this.tYPEDataGridViewTextBoxColumn.DataPropertyName = "TYPE";
            this.tYPEDataGridViewTextBoxColumn.HeaderText = "Type";
            this.tYPEDataGridViewTextBoxColumn.Name = "tYPEDataGridViewTextBoxColumn";
            this.tYPEDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // sTATUSDataGridViewTextBoxColumn
            // 
            this.sTATUSDataGridViewTextBoxColumn.DataPropertyName = "STATUS";
            this.sTATUSDataGridViewTextBoxColumn.HeaderText = "Status";
            this.sTATUSDataGridViewTextBoxColumn.Name = "sTATUSDataGridViewTextBoxColumn";
            this.sTATUSDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // cREATEDDataGridViewTextBoxColumn
            // 
            this.cREATEDDataGridViewTextBoxColumn.DataPropertyName = "CREATED";
            this.cREATEDDataGridViewTextBoxColumn.HeaderText = "Created";
            this.cREATEDDataGridViewTextBoxColumn.Name = "cREATEDDataGridViewTextBoxColumn";
            this.cREATEDDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // tIMELEFTDataGridViewTextBoxColumn
            // 
            this.tIMELEFTDataGridViewTextBoxColumn.DataPropertyName = "TIMELEFT";
            this.tIMELEFTDataGridViewTextBoxColumn.HeaderText = "Age";
            this.tIMELEFTDataGridViewTextBoxColumn.Name = "tIMELEFTDataGridViewTextBoxColumn";
            this.tIMELEFTDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // orderListBindingSource
            // 
            this.orderListBindingSource.DataMember = "OrderList";
            this.orderListBindingSource.DataSource = this.mainWindowBindingSource;
            // 
            // mainWindowBindingSource
            // 
            this.mainWindowBindingSource.DataSource = typeof(Bitfresh_WindowsForm.MainWindow);
            // 
            // UsersComboBox
            // 
            this.UsersComboBox.FormattingEnabled = true;
            this.UsersComboBox.Location = new System.Drawing.Point(55, 44);
            this.UsersComboBox.Name = "UsersComboBox";
            this.UsersComboBox.Size = new System.Drawing.Size(361, 21);
            this.UsersComboBox.TabIndex = 10;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(52, 27);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 13);
            this.label2.TabIndex = 11;
            this.label2.Text = "Selected User:";
            // 
            // ProfManagerButton
            // 
            this.ProfManagerButton.Location = new System.Drawing.Point(298, 73);
            this.ProfManagerButton.Name = "ProfManagerButton";
            this.ProfManagerButton.Size = new System.Drawing.Size(118, 23);
            this.ProfManagerButton.TabIndex = 12;
            this.ProfManagerButton.Text = "Add/Remove Users";
            this.ProfManagerButton.UseVisualStyleBackColor = true;
            this.ProfManagerButton.Click += new System.EventHandler(this.ProfManagerButton_Click);
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.AliceBlue;
            this.ClientSize = new System.Drawing.Size(771, 425);
            this.Controls.Add(this.ProfManagerButton);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.UsersComboBox);
            this.Controls.Add(this.OrdersView);
            this.Controls.Add(this.confBtn);
            this.Controls.Add(this.StatusLabel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ConnectButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "MainWindow";
            this.Text = "Bitfresh V1.1.0";
            ((System.ComponentModel.ISupportInitialize)(this.OrdersView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.orderListBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mainWindowBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button ConnectButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label StatusLabel;
        private System.Windows.Forms.Button confBtn;
        private System.Windows.Forms.DataGridView OrdersView;
        private System.Windows.Forms.BindingSource orderListBindingSource;
        private System.Windows.Forms.BindingSource mainWindowBindingSource;
        private System.Windows.Forms.DataGridViewTextBoxColumn iDDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn eXCHANGEDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn aMMOUNTDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn tYPEDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn sTATUSDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn cREATEDDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn tIMELEFTDataGridViewTextBoxColumn;
        private System.Windows.Forms.ComboBox UsersComboBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button ProfManagerButton;
    }
}

