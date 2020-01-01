using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ConsoleApp3
{
    class MainForm : Form
    {
        private Label label2;
        private Label thongTinSVLabelName;
        private DateTimePicker ngSinhDTPicker;
        private TextBox lopTextBox;
        private TextBox mssvTextBox;
        private TextBox hoTenTextBox;
        private Label ngSinhLabel;
        private Label lopLabel;
        private Label mssvLabel;
        private Label hoTenLabel;
        private Label emailLabel;
        private Label dienThoaiLabel;
        private Label gioiTinhLabel;
        private ComboBox maVungCB;
        private TextBox emailTextBox;
        private TextBox sdtTextBox;
        private Button btnThem;
        private DataGridView dataGridView1;
        private Button btnXoa;
        private Button btnSua;
        private Panel thongTinSVPanel;

        private RadioButton namRB;
        private RadioButton khacRB;
        private RadioButton nuRB;
        string gioiTinh;
        string connectionString;
        DataTable data;
        SqlConnection sqlConnection;
        SqlDataAdapter adapter;
        private DataGridViewTextBoxColumn colHoTen;
        private DataGridViewTextBoxColumn colMSSV;
        private DataGridViewTextBoxColumn colLop;
        private DataGridViewTextBoxColumn colNgaySinh;
        private DataGridViewTextBoxColumn colGioiTinh;
        private DataGridViewTextBoxColumn colSDT;
        private DataGridViewTextBoxColumn colMaVung;
        private DataGridViewTextBoxColumn colEmail;
        SqlCommand command;

        public MainForm()
        {
            InitializeComponent();
            string[] lineOfContents = File.ReadAllLines("Zip.txt");
            foreach (var line in lineOfContents)
            {
                string[] tokens = line.Split('\n');
                maVungCB.Items.Add(tokens[0]);
            }
            DisableInput();
            connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\drago\source\repos\ConsoleApp3\ConsoleApp3\Database1.mdf;Integrated Security=True";
            sqlConnection = new SqlConnection(connectionString);
            sqlConnection.Open();
            string query = "SELECT* FROM QLSV";
            data = new DataTable();
            adapter = new SqlDataAdapter(query, sqlConnection);
            adapter.Fill(data);
            dataGridView1.DataSource = data;
            dataGridView1.RefreshEdit();
            sqlConnection.Close();
        }

        private void InitializeComponent()
        {
            this.label2 = new System.Windows.Forms.Label();
            this.thongTinSVPanel = new System.Windows.Forms.Panel();
            this.khacRB = new System.Windows.Forms.RadioButton();
            this.nuRB = new System.Windows.Forms.RadioButton();
            this.namRB = new System.Windows.Forms.RadioButton();
            this.btnXoa = new System.Windows.Forms.Button();
            this.btnSua = new System.Windows.Forms.Button();
            this.btnThem = new System.Windows.Forms.Button();
            this.emailTextBox = new System.Windows.Forms.TextBox();
            this.sdtTextBox = new System.Windows.Forms.TextBox();
            this.maVungCB = new System.Windows.Forms.ComboBox();
            this.emailLabel = new System.Windows.Forms.Label();
            this.dienThoaiLabel = new System.Windows.Forms.Label();
            this.gioiTinhLabel = new System.Windows.Forms.Label();
            this.ngSinhDTPicker = new System.Windows.Forms.DateTimePicker();
            this.lopTextBox = new System.Windows.Forms.TextBox();
            this.mssvTextBox = new System.Windows.Forms.TextBox();
            this.hoTenTextBox = new System.Windows.Forms.TextBox();
            this.ngSinhLabel = new System.Windows.Forms.Label();
            this.lopLabel = new System.Windows.Forms.Label();
            this.mssvLabel = new System.Windows.Forms.Label();
            this.hoTenLabel = new System.Windows.Forms.Label();
            this.thongTinSVLabelName = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.colHoTen = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMSSV = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLop = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colNgaySinh = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colGioiTinh = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSDT = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMaVung = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEmail = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.thongTinSVPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(227, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(274, 33);
            this.label2.TabIndex = 1;
            this.label2.Text = "Quản Lý Sinh Viên";
            // 
            // thongTinSVPanel
            // 
            this.thongTinSVPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.thongTinSVPanel.Controls.Add(this.khacRB);
            this.thongTinSVPanel.Controls.Add(this.nuRB);
            this.thongTinSVPanel.Controls.Add(this.namRB);
            this.thongTinSVPanel.Controls.Add(this.btnXoa);
            this.thongTinSVPanel.Controls.Add(this.btnSua);
            this.thongTinSVPanel.Controls.Add(this.btnThem);
            this.thongTinSVPanel.Controls.Add(this.emailTextBox);
            this.thongTinSVPanel.Controls.Add(this.sdtTextBox);
            this.thongTinSVPanel.Controls.Add(this.maVungCB);
            this.thongTinSVPanel.Controls.Add(this.emailLabel);
            this.thongTinSVPanel.Controls.Add(this.dienThoaiLabel);
            this.thongTinSVPanel.Controls.Add(this.gioiTinhLabel);
            this.thongTinSVPanel.Controls.Add(this.ngSinhDTPicker);
            this.thongTinSVPanel.Controls.Add(this.lopTextBox);
            this.thongTinSVPanel.Controls.Add(this.mssvTextBox);
            this.thongTinSVPanel.Controls.Add(this.hoTenTextBox);
            this.thongTinSVPanel.Controls.Add(this.ngSinhLabel);
            this.thongTinSVPanel.Controls.Add(this.lopLabel);
            this.thongTinSVPanel.Controls.Add(this.mssvLabel);
            this.thongTinSVPanel.Controls.Add(this.hoTenLabel);
            this.thongTinSVPanel.Location = new System.Drawing.Point(12, 63);
            this.thongTinSVPanel.Name = "thongTinSVPanel";
            this.thongTinSVPanel.Size = new System.Drawing.Size(699, 181);
            this.thongTinSVPanel.TabIndex = 2;
            // 
            // khacRB
            // 
            this.khacRB.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.khacRB.Location = new System.Drawing.Point(583, 17);
            this.khacRB.Name = "khacRB";
            this.khacRB.Size = new System.Drawing.Size(111, 20);
            this.khacRB.TabIndex = 22;
            this.khacRB.TabStop = true;
            this.khacRB.Text = "Không Xác Định";
            this.khacRB.UseVisualStyleBackColor = true;
            this.khacRB.Click += new System.EventHandler(this.khacRB_Click);
            // 
            // nuRB
            // 
            this.nuRB.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.nuRB.Location = new System.Drawing.Point(532, 17);
            this.nuRB.Name = "nuRB";
            this.nuRB.Size = new System.Drawing.Size(56, 20);
            this.nuRB.TabIndex = 21;
            this.nuRB.TabStop = true;
            this.nuRB.Text = "Nữ";
            this.nuRB.UseVisualStyleBackColor = true;
            this.nuRB.Click += new System.EventHandler(this.nuRB_Click);
            // 
            // namRB
            // 
            this.namRB.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.namRB.Location = new System.Drawing.Point(473, 17);
            this.namRB.Name = "namRB";
            this.namRB.Size = new System.Drawing.Size(64, 20);
            this.namRB.TabIndex = 20;
            this.namRB.TabStop = true;
            this.namRB.Text = "Nam";
            this.namRB.UseVisualStyleBackColor = true;
            this.namRB.Click += new System.EventHandler(this.namRB_Click);
            // 
            // btnXoa
            // 
            this.btnXoa.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnXoa.Location = new System.Drawing.Point(583, 143);
            this.btnXoa.Name = "btnXoa";
            this.btnXoa.Size = new System.Drawing.Size(75, 23);
            this.btnXoa.TabIndex = 19;
            this.btnXoa.Text = "Xóa";
            this.btnXoa.UseVisualStyleBackColor = true;
            this.btnXoa.Click += new System.EventHandler(this.btnXoa_Click);
            // 
            // btnSua
            // 
            this.btnSua.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnSua.Location = new System.Drawing.Point(485, 143);
            this.btnSua.Name = "btnSua";
            this.btnSua.Size = new System.Drawing.Size(75, 23);
            this.btnSua.TabIndex = 18;
            this.btnSua.Text = "Sửa";
            this.btnSua.UseVisualStyleBackColor = true;
            this.btnSua.Click += new System.EventHandler(this.btnSua_Click);
            // 
            // btnThem
            // 
            this.btnThem.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnThem.Location = new System.Drawing.Point(391, 143);
            this.btnThem.Name = "btnThem";
            this.btnThem.Size = new System.Drawing.Size(75, 23);
            this.btnThem.TabIndex = 17;
            this.btnThem.Text = "Thêm";
            this.btnThem.UseVisualStyleBackColor = true;
            this.btnThem.Click += new System.EventHandler(this.btnThem_Click);
            // 
            // emailTextBox
            // 
            this.emailTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.emailTextBox.Location = new System.Drawing.Point(473, 101);
            this.emailTextBox.Name = "emailTextBox";
            this.emailTextBox.Size = new System.Drawing.Size(203, 20);
            this.emailTextBox.TabIndex = 16;
            // 
            // sdtTextBox
            // 
            this.sdtTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.sdtTextBox.Location = new System.Drawing.Point(532, 61);
            this.sdtTextBox.Name = "sdtTextBox";
            this.sdtTextBox.Size = new System.Drawing.Size(144, 20);
            this.sdtTextBox.TabIndex = 15;
            // 
            // maVungCB
            // 
            this.maVungCB.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.maVungCB.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.maVungCB.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.maVungCB.FormattingEnabled = true;
            this.maVungCB.Location = new System.Drawing.Point(473, 61);
            this.maVungCB.Name = "maVungCB";
            this.maVungCB.Size = new System.Drawing.Size(53, 21);
            this.maVungCB.TabIndex = 14;
            // 
            // emailLabel
            // 
            this.emailLabel.AutoSize = true;
            this.emailLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.emailLabel.Location = new System.Drawing.Point(388, 105);
            this.emailLabel.Name = "emailLabel";
            this.emailLabel.Size = new System.Drawing.Size(45, 16);
            this.emailLabel.TabIndex = 10;
            this.emailLabel.Text = "Email:";
            // 
            // dienThoaiLabel
            // 
            this.dienThoaiLabel.AutoSize = true;
            this.dienThoaiLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dienThoaiLabel.Location = new System.Drawing.Point(388, 61);
            this.dienThoaiLabel.Name = "dienThoaiLabel";
            this.dienThoaiLabel.Size = new System.Drawing.Size(76, 16);
            this.dienThoaiLabel.TabIndex = 9;
            this.dienThoaiLabel.Text = "Điện Thoại:";
            // 
            // gioiTinhLabel
            // 
            this.gioiTinhLabel.AutoSize = true;
            this.gioiTinhLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gioiTinhLabel.Location = new System.Drawing.Point(388, 18);
            this.gioiTinhLabel.Name = "gioiTinhLabel";
            this.gioiTinhLabel.Size = new System.Drawing.Size(64, 16);
            this.gioiTinhLabel.TabIndex = 8;
            this.gioiTinhLabel.Text = "Giới Tính:";
            // 
            // ngSinhDTPicker
            // 
            this.ngSinhDTPicker.Location = new System.Drawing.Point(99, 146);
            this.ngSinhDTPicker.Name = "ngSinhDTPicker";
            this.ngSinhDTPicker.Size = new System.Drawing.Size(192, 20);
            this.ngSinhDTPicker.TabIndex = 7;
            // 
            // lopTextBox
            // 
            this.lopTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lopTextBox.Location = new System.Drawing.Point(99, 105);
            this.lopTextBox.Name = "lopTextBox";
            this.lopTextBox.Size = new System.Drawing.Size(192, 20);
            this.lopTextBox.TabIndex = 6;
            // 
            // mssvTextBox
            // 
            this.mssvTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.mssvTextBox.Location = new System.Drawing.Point(99, 61);
            this.mssvTextBox.Name = "mssvTextBox";
            this.mssvTextBox.Size = new System.Drawing.Size(192, 20);
            this.mssvTextBox.TabIndex = 5;
            // 
            // hoTenTextBox
            // 
            this.hoTenTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.hoTenTextBox.Location = new System.Drawing.Point(99, 17);
            this.hoTenTextBox.Name = "hoTenTextBox";
            this.hoTenTextBox.Size = new System.Drawing.Size(192, 20);
            this.hoTenTextBox.TabIndex = 4;
            this.hoTenTextBox.Leave += new System.EventHandler(this.hoTenTextBox_Leave);
            // 
            // ngSinhLabel
            // 
            this.ngSinhLabel.AutoSize = true;
            this.ngSinhLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ngSinhLabel.Location = new System.Drawing.Point(7, 150);
            this.ngSinhLabel.Name = "ngSinhLabel";
            this.ngSinhLabel.Size = new System.Drawing.Size(73, 16);
            this.ngSinhLabel.TabIndex = 3;
            this.ngSinhLabel.Text = "Ngày Sinh:";
            // 
            // lopLabel
            // 
            this.lopLabel.AutoSize = true;
            this.lopLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lopLabel.Location = new System.Drawing.Point(7, 106);
            this.lopLabel.Name = "lopLabel";
            this.lopLabel.Size = new System.Drawing.Size(34, 16);
            this.lopLabel.TabIndex = 2;
            this.lopLabel.Text = "Lớp:";
            // 
            // mssvLabel
            // 
            this.mssvLabel.AutoSize = true;
            this.mssvLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mssvLabel.Location = new System.Drawing.Point(7, 62);
            this.mssvLabel.Name = "mssvLabel";
            this.mssvLabel.Size = new System.Drawing.Size(49, 16);
            this.mssvLabel.TabIndex = 1;
            this.mssvLabel.Text = "MSSV:";
            // 
            // hoTenLabel
            // 
            this.hoTenLabel.AutoSize = true;
            this.hoTenLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.hoTenLabel.Location = new System.Drawing.Point(7, 18);
            this.hoTenLabel.Name = "hoTenLabel";
            this.hoTenLabel.Size = new System.Drawing.Size(56, 16);
            this.hoTenLabel.TabIndex = 0;
            this.hoTenLabel.Text = "Họ Tên:";
            // 
            // thongTinSVLabelName
            // 
            this.thongTinSVLabelName.AutoSize = true;
            this.thongTinSVLabelName.Location = new System.Drawing.Point(20, 56);
            this.thongTinSVLabelName.Name = "thongTinSVLabelName";
            this.thongTinSVLabelName.Size = new System.Drawing.Size(104, 13);
            this.thongTinSVLabelName.TabIndex = 3;
            this.thongTinSVLabelName.Text = "Thông Tin Sinh Viên";
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colHoTen,
            this.colMSSV,
            this.colLop,
            this.colNgaySinh,
            this.colGioiTinh,
            this.colSDT,
            this.colMaVung,
            this.colEmail});
            this.dataGridView1.Location = new System.Drawing.Point(12, 263);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(699, 183);
            this.dataGridView1.TabIndex = 4;
            this.dataGridView1.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            // 
            // colHoTen
            // 
            this.colHoTen.DataPropertyName = "HOTEN";
            this.colHoTen.HeaderText = "Họ Tên";
            this.colHoTen.Name = "colHoTen";
            // 
            // colMSSV
            // 
            this.colMSSV.DataPropertyName = "MSSV";
            this.colMSSV.HeaderText = "MSSV";
            this.colMSSV.Name = "colMSSV";
            this.colMSSV.Width = 50;
            // 
            // colLop
            // 
            this.colLop.DataPropertyName = "LOP";
            this.colLop.HeaderText = "Lớp";
            this.colLop.Name = "colLop";
            this.colLop.Width = 80;
            // 
            // colNgaySinh
            // 
            this.colNgaySinh.DataPropertyName = "NGSINH";
            this.colNgaySinh.HeaderText = "Ngày Sinh";
            this.colNgaySinh.Name = "colNgaySinh";
            this.colNgaySinh.Width = 70;
            // 
            // colGioiTinh
            // 
            this.colGioiTinh.DataPropertyName = "GIOITINH";
            this.colGioiTinh.HeaderText = "Giới Tính";
            this.colGioiTinh.Name = "colGioiTinh";
            this.colGioiTinh.Width = 70;
            // 
            // colSDT
            // 
            this.colSDT.DataPropertyName = "DIENTHOAI";
            this.colSDT.HeaderText = "Điện Thoại";
            this.colSDT.Name = "colSDT";
            // 
            // colMaVung
            // 
            this.colMaVung.DataPropertyName = "MAVUNG";
            this.colMaVung.HeaderText = "Mã Vùng";
            this.colMaVung.Name = "colMaVung";
            // 
            // colEmail
            // 
            this.colEmail.DataPropertyName = "EMAIL";
            this.colEmail.HeaderText = "Email";
            this.colEmail.Name = "colEmail";
            // 
            // MainForm
            // 
            this.BackColor = System.Drawing.Color.Gainsboro;
            this.ClientSize = new System.Drawing.Size(723, 458);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.thongTinSVLabelName);
            this.Controls.Add(this.thongTinSVPanel);
            this.Controls.Add(this.label2);
            this.Name = "MainForm";
            this.Text = "MainForm";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.thongTinSVPanel.ResumeLayout(false);
            this.thongTinSVPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        void DisableInput()
        {
            hoTenTextBox.Enabled = false;
            mssvTextBox.Enabled = false;
            lopTextBox.Enabled = false;
            ngSinhDTPicker.Enabled = false;
            maVungCB.Enabled = false;
            sdtTextBox.Enabled = false;
            emailTextBox.Enabled = false;
        }

        void EnableInput()
        {
            hoTenTextBox.Enabled = true;
            mssvTextBox.Enabled = true;
            lopTextBox.Enabled = true;
            ngSinhDTPicker.Enabled = true;
            maVungCB.Enabled = true;
            sdtTextBox.Enabled = true;
            emailTextBox.Enabled = true;
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            sqlConnection.Open();
            EnableInput();
            Button btn = (Button)sender;
            if (btn.Text == "Thêm")
            {
                hoTenTextBox.Text = "";
                mssvTextBox.Text = "";
                lopTextBox.Text = "";
                maVungCB.Text = "";
                sdtTextBox.Text = "";
                emailTextBox.Text = "";
                hoTenLabel.Focus();
                btn.Text = "Lưu";
                btnSua.Enabled = false;
                btnXoa.Enabled = false;
                sqlConnection.Close();
                return;
            }
            hoTenTextBox.Text = hoTenTextBox.Text.Trim();
            mssvTextBox.Text = mssvTextBox.Text.Trim();
            lopTextBox.Text = lopTextBox.Text.Trim();
            sdtTextBox.Text = sdtTextBox.Text.Trim();
            emailTextBox.Text = emailTextBox.Text.Trim();
            if (CheckAll() == false)
            {
                MessageBox.Show("Thông tin chưa được sữa chữa vì không hợp lí");

                #region Data validation handling
                DataValidationHandle();
                #endregion
                sqlConnection.Close();
                return;
            }
            string query = "SET DATEFORMAT DMY;";
            command = new SqlCommand(query, sqlConnection);
            command.ExecuteNonQuery();
            query = "INSERT INTO QLSV (HOTEN, MSSV, LOP, NGSINH, DIENTHOAI, MAVUNG, GIOITINH, EMAIL) VALUES (" + "'" + hoTenTextBox.Text + "'" + ","
                                                                                                                + "'" + mssvTextBox.Text + "'" + ","
                                                                                                                + "'" + lopTextBox.Text + "'" + ","
                                                                                                                + "'" + ngSinhDTPicker.Value.Date.ToString("dd/MM/yyyy") + "'" + ","
                                                                                                                + "'" + sdtTextBox.Text + "'" + ","
                                                                                                                + "'" + maVungCB.Text + "'" + ","
                                                                                                                + "'" + gioiTinh + "'" + ","
                                                                                                                + "'" + emailTextBox.Text + "'" + ")";

            command = new SqlCommand(query, sqlConnection);
            command.ExecuteNonQuery();

            query = "SELECT* FROM QLSV";
            data = new DataTable();
            adapter = new SqlDataAdapter(query, sqlConnection);
            adapter.Fill(data);
            dataGridView1.DataSource = data;
            dataGridView1.RefreshEdit();
            btnSua.Enabled = true;
            btnXoa.Enabled = true;
            btn.Text = "Thêm";
            sqlConnection.Close();
            DisableInput();
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            sqlConnection.Open();
            Button btn = (Button)sender;
            if (btn.Text == "Sửa")
            {
                hoTenLabel.Focus();
                btn.Text = "Lưu";
                btnThem.Enabled = false;
                btnXoa.Enabled = false;
                sqlConnection.Close();
                EnableInput();
                mssvTextBox.Enabled = false;
                return;
            }
            hoTenTextBox.Text = hoTenTextBox.Text.Trim();
            mssvTextBox.Text = mssvTextBox.Text.Trim();
            lopTextBox.Text = lopTextBox.Text.Trim();
            sdtTextBox.Text = sdtTextBox.Text.Trim();
            emailTextBox.Text = emailTextBox.Text.Trim();
            if (CheckAllExceptKey() == false)
            {
                MessageBox.Show("Thông tin chưa được sữa chữa vì không hợp lí");

                #region Data validation handling
                DataValidationHandle();
                #endregion
                sqlConnection.Close();
                return;
            }
            string query = "SET DATEFORMAT DMY;";
            command = new SqlCommand(query, sqlConnection);
            command.ExecuteNonQuery();
            query = "UPDATE QLSV SET HOTEN = '" + hoTenTextBox.Text + "', MSSV = '" + mssvTextBox.Text + "', LOP = '" + lopTextBox.Text + "', NGSINH = '" + ngSinhDTPicker.Value.Date.ToString("dd/MM/yyyy") + "', GIOITINH = '" + gioiTinh + "', DIENTHOAI = " + sdtTextBox.Text + ", MAVUNG = '" + maVungCB.Text + "', EMAIL = '" + emailTextBox.Text + "' WHERE MSSV = '" + mssvTextBox.Text + "'";
            command = new SqlCommand(query, sqlConnection);
            command.ExecuteNonQuery();

            query = "SELECT* FROM QLSV";
            data = new DataTable();
            adapter = new SqlDataAdapter(query, sqlConnection);
            adapter.Fill(data);
            dataGridView1.DataSource = data;
            dataGridView1.RefreshEdit();
            btnSua.Enabled = true;
            btnXoa.Enabled = true;
            btn.Text = "Sửa";
            sqlConnection.Close();
            DisableInput();
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            DialogResult res = MessageBox.Show("Bạn có muốn xóa không?", "Xác nhận chắc chắn thông tin", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (res == DialogResult.Yes)
            {
                sqlConnection.Open();
                string query = "DELETE QLSV WHERE MSSV = '" + mssvTextBox.Text + "'";
                command = new SqlCommand(query, sqlConnection);
                command.ExecuteNonQuery();
                sqlConnection.Close();

                query = "SELECT* FROM QLSV";
                data = new DataTable();
                adapter = new SqlDataAdapter(query, sqlConnection);
                adapter.Fill(data);
                dataGridView1.DataSource = data;
                dataGridView1.RefreshEdit();
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            
        }

        private void namRB_Click(object sender, EventArgs e)
        {
            gioiTinh = "Nam";
        }

        private void nuRB_Click(object sender, EventArgs e)
        {
            gioiTinh = "Nu";
        }

        private void khacRB_Click(object sender, EventArgs e)
        {
            gioiTinh = "khac";
        }

        private bool CheckAll()
        {
            if (hoTenTextBox.Text == "" || mssvTextBox.Text == "" || lopTextBox.Text == "" || ngSinhDTPicker.Value.Date == DateTime.Now.Date
                    || (namRB.Checked == false && nuRB.Checked == false && khacRB.Checked == false)
                    || maVungCB.Text == "" || sdtTextBox.Text == "" || emailTextBox.Text == "")
                return false;
            if (CheckPhoneNum() == false)
                return false;
            if (CheckEmail() == false)
                return false;
            if (CheckIdExist() == false)
                return false;
            if (CheckAge() == false)
                return false;
            if (CheckZip() == false)
                return false;
            return true;
        }
        private bool CheckAllExceptKey()
        {
            if (hoTenTextBox.Text == "" || mssvTextBox.Text == "" || lopTextBox.Text == "" || ngSinhDTPicker.Value.Date == DateTime.Now.Date
                    || (namRB.Checked == false && nuRB.Checked == false && khacRB.Checked == false)
                    || maVungCB.Text == "" || sdtTextBox.Text == "" || emailTextBox.Text == "")
                return false;
            if (CheckPhoneNum() == false)
                return false;
            if (CheckEmail() == false)
                return false;
            if (CheckAge() == false)
                return false;
            if (CheckZip() == false)
                return false;
            return true;
        }

        private bool CheckPhoneNum()
        {
            sdtTextBox.Text = sdtTextBox.Text.TrimStart('0');
            bool isNum = int.TryParse(sdtTextBox.Text, out int n);
            bool isLengthRight = (sdtTextBox.Text.Length == 9) || (sdtTextBox.Text.Length == 10);
            return isNum && isLengthRight;
        }

        private bool CheckEmail()
        {
            Regex rformat = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,})+)$");
            return rformat.IsMatch(emailTextBox.Text);
        }

        private bool CheckAge()
        {
            DateTime today = DateTime.Today;
            int age = today.Year - ngSinhDTPicker.Value.Year;
            if (ngSinhDTPicker.Value > today.AddYears(-age))
                age--;
            return age >= 17 && age <= 35;
        }

        private bool CheckZip()
        {
            if (maVungCB.SelectedIndex > -1)
                return true;
            return false;
        }

        private bool CheckIdExist() // the ID stands as primary key
        {
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Cells[0].Value == null)
                {
                    return true;
                }
                string id = row.Cells[0].Value.ToString().Trim();
                if (id != null && id == mssvTextBox.Text)
                {
                    return false;
                }
            }
            return true;
        }

        private void DataValidationHandle()
        {
            if (hoTenTextBox.Text == "")
            {
                hoTenTextBox.Focus();
                MessageBox.Show("Họ tên chưa được nhập");
                return;
            }

            if (mssvTextBox.Text == "")
            {
                MessageBox.Show("MSSV chưa được nhập");
                return;
            }
            else if (CheckIdExist() == false)
            {
                mssvTextBox.Focus();
                MessageBox.Show("MSSV đã bị trùng với người đã có");
                return;
            }

            if (lopTextBox.Text == "")
            {
                lopTextBox.Focus();
                MessageBox.Show("Lớp chưa được nhập");
                return;
            }

            if (ngSinhDTPicker.Value.Date == DateTime.Now.Date)
            {
                ngSinhDTPicker.Focus();
                MessageBox.Show("Ngày sinh không thể là ngày này");
                return;
            }
            else if (CheckAge() == false)
            {
                ngSinhDTPicker.Focus();
                MessageBox.Show("Chỉ nhập được ngày sinh trên 17 và dưới 35 tuổi");
                return;
            }

            if (namRB.Checked == false && nuRB.Checked == false && khacRB.Checked == false)
            {
                MessageBox.Show("Giới tính chưa chọn");
                return;
            }

            if (maVungCB.Text == "" || sdtTextBox.Text == "")
            {
                if (maVungCB.Text == "")
                {
                    maVungCB.Focus();
                }
                if (sdtTextBox.Text == "")
                {
                    sdtTextBox.Focus();
                }
                MessageBox.Show("Mã vùng hoặc SĐT chưa được nhập");
                return;
            }
            else if (CheckZip() == false)
            {
                maVungCB.Focus();
                MessageBox.Show("SĐT hoặc mã vùng không hợp lệ");
                return;
            }
            else if (CheckPhoneNum() == false)
            {
                sdtTextBox.Focus();
                MessageBox.Show("SĐT hoặc mã vùng không hợp lệ");
                return;
            }

            if (emailTextBox.Text == "")
            {
                emailTextBox.Focus();
                MessageBox.Show("Email chưa được nhập");
                return;
            }
            else if (CheckEmail() == false)
            {
                emailTextBox.Focus();
                MessageBox.Show("Email không hợp lệ");
                return;
            }
        }

        private void hoTenTextBox_Leave(object sender, EventArgs e)
        {
            string str = hoTenTextBox.Text;
            CultureInfo cultureInfo = Thread.CurrentThread.CurrentCulture;
            TextInfo textInfo = cultureInfo.TextInfo;

            str = textInfo.ToTitleCase(str);
            hoTenTextBox.Text = str;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.CurrentRow.Index != -1)
            {
                hoTenTextBox.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString().Trim();
                mssvTextBox.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString().Trim();
                lopTextBox.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString().Trim();
                DateTime dob = DateTime.Parse(dataGridView1.CurrentRow.Cells[3].Value.ToString());
                ngSinhDTPicker.Value = dob;
                if (dataGridView1.CurrentRow.Cells[4].Value.ToString().Trim() == "Nam")
                    namRB.Checked = true;
                else if (dataGridView1.CurrentRow.Cells[4].Value.ToString().Trim() == "Nu")
                    nuRB.Checked = true;
                else
                    khacRB.Checked = true;

                maVungCB.Text = dataGridView1.CurrentRow.Cells[5].Value.ToString().Trim();
                sdtTextBox.Text = dataGridView1.CurrentRow.Cells[6].Value.ToString().Trim();
                emailTextBox.Text = dataGridView1.CurrentRow.Cells[7].Value.ToString().Trim();
            }
        }

        
    }
}
