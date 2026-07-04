namespace WingLoader_GUI
{
    partial class WingLoader_GUI_Form : Form
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            tb_messages = new TextBox();
            tb_Message1 = new TextBox();
            tb_Message2 = new TextBox();
            tb_Firstname = new TextBox();
            tb_Surname = new TextBox();
            tb_System = new TextBox();
            tb_Callsign = new TextBox();
            tb_Year = new TextBox();
            tb_Day = new TextBox();
            tb_Hex = new TextBox();
            btn_StartProcessElevated = new Button();
            btn_ExecuteDebugger = new Button();
            btn_ExecuteDebuggerAsync = new Button();
            btn_StartDebugger = new Button();
            btn_StopDebugger = new Button();
            lbl_Message1 = new Label();
            lbl_Message2 = new Label();
            lbl_firstName = new Label();
            lbl_surname = new Label();
            lbl_callsign = new Label();
            lbl_system = new Label();
            lbl_year = new Label();
            lbl_day = new Label();
            rb_WC1 = new RadioButton();
            rb_WC2 = new RadioButton();
            rb_SO1 = new RadioButton();
            rb_SM2 = new RadioButton();
            rb_SO2 = new RadioButton();
            tb_string = new TextBox();
            tb_hexstring = new TextBox();
            tb_Address = new TextBox();
            btn_StartProcess = new Button();
            btn_testDoSomething = new Button();
            cb_WCAT = new CheckBox();
            btn_testDoSomething2 = new Button();
            cb_WCKS = new CheckBox();
            rb_SM1 = new RadioButton();
            SuspendLayout();
            // 
            // tb_messages
            // 
            tb_messages.AcceptsReturn = true;
            tb_messages.AcceptsTab = true;
            tb_messages.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tb_messages.Location = new Point(11, 220);
            tb_messages.Margin = new Padding(2);
            tb_messages.Multiline = true;
            tb_messages.Name = "tb_messages";
            tb_messages.PlaceholderText = "tb_messages";
            tb_messages.ScrollBars = ScrollBars.Vertical;
            tb_messages.Size = new Size(1081, 206);
            tb_messages.TabIndex = 0;
            // 
            // tb_Message1
            // 
            tb_Message1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            tb_Message1.Location = new Point(855, 4);
            tb_Message1.Margin = new Padding(2);
            tb_Message1.Name = "tb_Message1";
            tb_Message1.Size = new Size(237, 23);
            tb_Message1.TabIndex = 1;
            // 
            // tb_Message2
            // 
            tb_Message2.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            tb_Message2.Location = new Point(855, 31);
            tb_Message2.Margin = new Padding(2);
            tb_Message2.Name = "tb_Message2";
            tb_Message2.Size = new Size(237, 23);
            tb_Message2.TabIndex = 2;
            // 
            // tb_Firstname
            // 
            tb_Firstname.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            tb_Firstname.Location = new Point(855, 58);
            tb_Firstname.Margin = new Padding(2);
            tb_Firstname.Name = "tb_Firstname";
            tb_Firstname.Size = new Size(237, 23);
            tb_Firstname.TabIndex = 3;
            // 
            // tb_Surname
            // 
            tb_Surname.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            tb_Surname.Location = new Point(855, 85);
            tb_Surname.Margin = new Padding(2);
            tb_Surname.Name = "tb_Surname";
            tb_Surname.Size = new Size(237, 23);
            tb_Surname.TabIndex = 4;
            // 
            // tb_System
            // 
            tb_System.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            tb_System.Location = new Point(855, 139);
            tb_System.Margin = new Padding(2);
            tb_System.Name = "tb_System";
            tb_System.Size = new Size(237, 23);
            tb_System.TabIndex = 5;
            // 
            // tb_Callsign
            // 
            tb_Callsign.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            tb_Callsign.Location = new Point(855, 112);
            tb_Callsign.Margin = new Padding(2);
            tb_Callsign.Name = "tb_Callsign";
            tb_Callsign.Size = new Size(237, 23);
            tb_Callsign.TabIndex = 6;
            // 
            // tb_Year
            // 
            tb_Year.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            tb_Year.Location = new Point(855, 166);
            tb_Year.Margin = new Padding(2);
            tb_Year.Name = "tb_Year";
            tb_Year.Size = new Size(237, 23);
            tb_Year.TabIndex = 8;
            // 
            // tb_Day
            // 
            tb_Day.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            tb_Day.Location = new Point(855, 193);
            tb_Day.Margin = new Padding(2);
            tb_Day.Name = "tb_Day";
            tb_Day.Size = new Size(237, 23);
            tb_Day.TabIndex = 7;
            // 
            // tb_Hex
            // 
            tb_Hex.AcceptsReturn = true;
            tb_Hex.AcceptsTab = true;
            tb_Hex.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            tb_Hex.Location = new Point(11, 4);
            tb_Hex.Margin = new Padding(2);
            tb_Hex.Multiline = true;
            tb_Hex.Name = "tb_Hex";
            tb_Hex.PlaceholderText = "tb_Hex";
            tb_Hex.ScrollBars = ScrollBars.Vertical;
            tb_Hex.Size = new Size(766, 212);
            tb_Hex.TabIndex = 9;
            tb_Hex.Visible = false;
            // 
            // btn_StartProcessElevated
            // 
            btn_StartProcessElevated.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btn_StartProcessElevated.Location = new Point(8, 480);
            btn_StartProcessElevated.Margin = new Padding(2);
            btn_StartProcessElevated.Name = "btn_StartProcessElevated";
            btn_StartProcessElevated.Size = new Size(125, 52);
            btn_StartProcessElevated.TabIndex = 10;
            btn_StartProcessElevated.Text = "Start Game as Admin";
            btn_StartProcessElevated.UseVisualStyleBackColor = true;
            btn_StartProcessElevated.Click += btn_StartProcessElevated_Click;
            // 
            // btn_ExecuteDebugger
            // 
            btn_ExecuteDebugger.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btn_ExecuteDebugger.Location = new Point(244, 458);
            btn_ExecuteDebugger.Name = "btn_ExecuteDebugger";
            btn_ExecuteDebugger.Size = new Size(260, 35);
            btn_ExecuteDebugger.TabIndex = 11;
            btn_ExecuteDebugger.Text = "Execute Debugger Synchronous";
            btn_ExecuteDebugger.UseVisualStyleBackColor = true;
            btn_ExecuteDebugger.Visible = false;
            btn_ExecuteDebugger.Click += btn_ExecuteDebugger_Sync_Click;
            // 
            // btn_ExecuteDebuggerAsync
            // 
            btn_ExecuteDebuggerAsync.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btn_ExecuteDebuggerAsync.Location = new Point(244, 497);
            btn_ExecuteDebuggerAsync.Name = "btn_ExecuteDebuggerAsync";
            btn_ExecuteDebuggerAsync.Size = new Size(260, 35);
            btn_ExecuteDebuggerAsync.TabIndex = 12;
            btn_ExecuteDebuggerAsync.Text = "Execute Debugger Asynchronous";
            btn_ExecuteDebuggerAsync.UseVisualStyleBackColor = true;
            btn_ExecuteDebuggerAsync.Visible = false;
            btn_ExecuteDebuggerAsync.Click += btn_ExecuteDebugger_Async_Click;
            // 
            // btn_StartDebugger
            // 
            btn_StartDebugger.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btn_StartDebugger.Location = new Point(138, 458);
            btn_StartDebugger.Name = "btn_StartDebugger";
            btn_StartDebugger.Size = new Size(100, 35);
            btn_StartDebugger.TabIndex = 13;
            btn_StartDebugger.Text = "Start Debugger";
            btn_StartDebugger.UseVisualStyleBackColor = true;
            btn_StartDebugger.Click += btn_StartDebugger_Click;
            // 
            // btn_StopDebugger
            // 
            btn_StopDebugger.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btn_StopDebugger.Location = new Point(138, 498);
            btn_StopDebugger.Name = "btn_StopDebugger";
            btn_StopDebugger.Size = new Size(100, 34);
            btn_StopDebugger.TabIndex = 14;
            btn_StopDebugger.Text = "Stop Debugger";
            btn_StopDebugger.UseVisualStyleBackColor = true;
            btn_StopDebugger.Click += btn_StopDebugger_Click;
            // 
            // lbl_Message1
            // 
            lbl_Message1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lbl_Message1.AutoSize = true;
            lbl_Message1.Location = new Point(782, 4);
            lbl_Message1.Name = "lbl_Message1";
            lbl_Message1.Size = new Size(59, 15);
            lbl_Message1.TabIndex = 15;
            lbl_Message1.Text = "Message1";
            // 
            // lbl_Message2
            // 
            lbl_Message2.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lbl_Message2.AutoSize = true;
            lbl_Message2.Location = new Point(782, 31);
            lbl_Message2.Name = "lbl_Message2";
            lbl_Message2.Size = new Size(59, 15);
            lbl_Message2.TabIndex = 16;
            lbl_Message2.Text = "Message2";
            // 
            // lbl_firstName
            // 
            lbl_firstName.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lbl_firstName.AutoSize = true;
            lbl_firstName.Location = new Point(782, 58);
            lbl_firstName.Name = "lbl_firstName";
            lbl_firstName.Size = new Size(64, 15);
            lbl_firstName.TabIndex = 17;
            lbl_firstName.Text = "First Name";
            // 
            // lbl_surname
            // 
            lbl_surname.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lbl_surname.AutoSize = true;
            lbl_surname.Location = new Point(782, 85);
            lbl_surname.Name = "lbl_surname";
            lbl_surname.Size = new Size(54, 15);
            lbl_surname.TabIndex = 18;
            lbl_surname.Text = "Surname";
            // 
            // lbl_callsign
            // 
            lbl_callsign.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lbl_callsign.AutoSize = true;
            lbl_callsign.Location = new Point(782, 112);
            lbl_callsign.Name = "lbl_callsign";
            lbl_callsign.Size = new Size(50, 15);
            lbl_callsign.TabIndex = 19;
            lbl_callsign.Text = "CallSign";
            // 
            // lbl_system
            // 
            lbl_system.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lbl_system.AutoSize = true;
            lbl_system.Location = new Point(782, 139);
            lbl_system.Name = "lbl_system";
            lbl_system.Size = new Size(45, 15);
            lbl_system.TabIndex = 20;
            lbl_system.Text = "System";
            // 
            // lbl_year
            // 
            lbl_year.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lbl_year.AutoSize = true;
            lbl_year.Location = new Point(782, 166);
            lbl_year.Name = "lbl_year";
            lbl_year.Size = new Size(29, 15);
            lbl_year.TabIndex = 21;
            lbl_year.Text = "Year";
            // 
            // lbl_day
            // 
            lbl_day.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lbl_day.AutoSize = true;
            lbl_day.Location = new Point(782, 193);
            lbl_day.Name = "lbl_day";
            lbl_day.Size = new Size(27, 15);
            lbl_day.TabIndex = 22;
            lbl_day.Text = "Day";
            // 
            // rb_WC1
            // 
            rb_WC1.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            rb_WC1.AutoSize = true;
            rb_WC1.Location = new Point(985, 458);
            rb_WC1.Name = "rb_WC1";
            rb_WC1.Size = new Size(50, 19);
            rb_WC1.TabIndex = 0;
            rb_WC1.TabStop = true;
            rb_WC1.Text = "WC1\r\n";
            rb_WC1.UseVisualStyleBackColor = true;
            rb_WC1.CheckedChanged += rb_selectGameMode;
            // 
            // rb_WC2
            // 
            rb_WC2.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            rb_WC2.AutoSize = true;
            rb_WC2.Location = new Point(1045, 458);
            rb_WC2.Name = "rb_WC2";
            rb_WC2.Size = new Size(50, 19);
            rb_WC2.TabIndex = 24;
            rb_WC2.TabStop = true;
            rb_WC2.Text = "WC2";
            rb_WC2.UseVisualStyleBackColor = true;
            rb_WC2.CheckedChanged += rb_selectGameMode;
            // 
            // rb_SO1
            // 
            rb_SO1.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            rb_SO1.AutoSize = true;
            rb_SO1.Location = new Point(1045, 488);
            rb_SO1.Name = "rb_SO1";
            rb_SO1.Size = new Size(46, 19);
            rb_SO1.TabIndex = 26;
            rb_SO1.TabStop = true;
            rb_SO1.Text = "SO1";
            rb_SO1.UseVisualStyleBackColor = true;
            rb_SO1.CheckedChanged += rb_selectGameMode;
            // 
            // rb_SM2
            // 
            rb_SM2.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            rb_SM2.AutoSize = true;
            rb_SM2.Location = new Point(985, 518);
            rb_SM2.Name = "rb_SM2";
            rb_SM2.Size = new Size(48, 19);
            rb_SM2.TabIndex = 25;
            rb_SM2.TabStop = true;
            rb_SM2.Text = "SM2";
            rb_SM2.UseVisualStyleBackColor = true;
            rb_SM2.CheckedChanged += rb_selectGameMode;
            // 
            // rb_SO2
            // 
            rb_SO2.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            rb_SO2.AutoSize = true;
            rb_SO2.Location = new Point(1045, 518);
            rb_SO2.Name = "rb_SO2";
            rb_SO2.Size = new Size(46, 19);
            rb_SO2.TabIndex = 27;
            rb_SO2.TabStop = true;
            rb_SO2.Text = "SO2";
            rb_SO2.UseVisualStyleBackColor = true;
            rb_SO2.CheckedChanged += rb_selectGameMode;
            // 
            // tb_string
            // 
            tb_string.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            tb_string.Location = new Point(655, 480);
            tb_string.Name = "tb_string";
            tb_string.PlaceholderText = "tb_string - String to convert";
            tb_string.Size = new Size(324, 23);
            tb_string.TabIndex = 28;
            tb_string.Visible = false;
            // 
            // tb_hexstring
            // 
            tb_hexstring.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            tb_hexstring.Location = new Point(655, 509);
            tb_hexstring.Name = "tb_hexstring";
            tb_hexstring.PlaceholderText = "tb_hexstring - Hex of String";
            tb_hexstring.Size = new Size(324, 23);
            tb_hexstring.TabIndex = 29;
            tb_hexstring.Visible = false;
            // 
            // tb_Address
            // 
            tb_Address.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            tb_Address.Location = new Point(655, 454);
            tb_Address.Name = "tb_Address";
            tb_Address.PlaceholderText = "tb_Address - Address of String pulled in FindSomeStuff";
            tb_Address.Size = new Size(324, 23);
            tb_Address.TabIndex = 30;
            tb_Address.Visible = false;
            // 
            // btn_StartProcess
            // 
            btn_StartProcess.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btn_StartProcess.Location = new Point(8, 457);
            btn_StartProcess.Margin = new Padding(2);
            btn_StartProcess.Name = "btn_StartProcess";
            btn_StartProcess.Size = new Size(125, 20);
            btn_StartProcess.TabIndex = 31;
            btn_StartProcess.Text = "Start Game";
            btn_StartProcess.UseVisualStyleBackColor = true;
            btn_StartProcess.Click += btn_StartProcess_Click;
            // 
            // btn_testDoSomething
            // 
            btn_testDoSomething.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btn_testDoSomething.Location = new Point(510, 458);
            btn_testDoSomething.Name = "btn_testDoSomething";
            btn_testDoSomething.Size = new Size(139, 35);
            btn_testDoSomething.TabIndex = 32;
            btn_testDoSomething.Text = "Find some stuff?";
            btn_testDoSomething.UseVisualStyleBackColor = true;
            btn_testDoSomething.Visible = false;
            btn_testDoSomething.Click += btn_testDoSomething_Click;
            // 
            // cb_WCAT
            // 
            cb_WCAT.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            cb_WCAT.AutoSize = true;
            cb_WCAT.Checked = true;
            cb_WCAT.CheckState = CheckState.Checked;
            cb_WCAT.Location = new Point(1037, 431);
            cb_WCAT.Name = "cb_WCAT";
            cb_WCAT.Size = new Size(58, 19);
            cb_WCAT.TabIndex = 33;
            cb_WCAT.Text = "WCAT";
            cb_WCAT.UseVisualStyleBackColor = true;
            cb_WCAT.CheckedChanged += cb_WCAT_CheckedChanged;
            // 
            // btn_testDoSomething2
            // 
            btn_testDoSomething2.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btn_testDoSomething2.Location = new Point(510, 498);
            btn_testDoSomething2.Name = "btn_testDoSomething2";
            btn_testDoSomething2.Size = new Size(139, 34);
            btn_testDoSomething2.TabIndex = 34;
            btn_testDoSomething2.Text = "Scan da memories?";
            btn_testDoSomething2.UseVisualStyleBackColor = true;
            btn_testDoSomething2.Visible = false;
            btn_testDoSomething2.Click += btn_testDoSomething2_Click;
            // 
            // cb_WCKS
            // 
            cb_WCKS.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            cb_WCKS.AutoSize = true;
            cb_WCKS.Location = new Point(940, 431);
            cb_WCKS.Name = "cb_WCKS";
            cb_WCKS.Size = new Size(91, 19);
            cb_WCKS.TabIndex = 35;
            cb_WCKS.Text = "Kilrathi Saga";
            cb_WCKS.UseVisualStyleBackColor = true;
            cb_WCKS.CheckedChanged += cb_WCKS_CheckedChanged;
            // 
            // rb_SM1
            // 
            rb_SM1.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            rb_SM1.AutoSize = true;
            rb_SM1.Location = new Point(985, 488);
            rb_SM1.Name = "rb_SM1";
            rb_SM1.Size = new Size(48, 19);
            rb_SM1.TabIndex = 37;
            rb_SM1.TabStop = true;
            rb_SM1.Text = "SM1";
            rb_SM1.UseVisualStyleBackColor = true;
            // 
            // WingLoader_GUI_Form
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1100, 540);
            Controls.Add(rb_SM1);
            Controls.Add(cb_WCKS);
            Controls.Add(btn_testDoSomething2);
            Controls.Add(cb_WCAT);
            Controls.Add(btn_testDoSomething);
            Controls.Add(btn_StartProcess);
            Controls.Add(tb_Address);
            Controls.Add(tb_hexstring);
            Controls.Add(tb_string);
            Controls.Add(rb_SO2);
            Controls.Add(rb_SO1);
            Controls.Add(rb_SM2);
            Controls.Add(rb_WC2);
            Controls.Add(rb_WC1);
            Controls.Add(lbl_day);
            Controls.Add(lbl_year);
            Controls.Add(lbl_system);
            Controls.Add(lbl_callsign);
            Controls.Add(lbl_surname);
            Controls.Add(lbl_firstName);
            Controls.Add(lbl_Message2);
            Controls.Add(lbl_Message1);
            Controls.Add(btn_StopDebugger);
            Controls.Add(btn_StartDebugger);
            Controls.Add(btn_ExecuteDebuggerAsync);
            Controls.Add(btn_ExecuteDebugger);
            Controls.Add(btn_StartProcessElevated);
            Controls.Add(tb_Hex);
            Controls.Add(tb_Year);
            Controls.Add(tb_Day);
            Controls.Add(tb_Callsign);
            Controls.Add(tb_System);
            Controls.Add(tb_Surname);
            Controls.Add(tb_Firstname);
            Controls.Add(tb_Message2);
            Controls.Add(tb_Message1);
            Controls.Add(tb_messages);
            Margin = new Padding(2);
            Name = "WingLoader_GUI_Form";
            Text = "WingLoader.Net";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox tb_messages;
        private TextBox tb_Message1;
        private TextBox tb_Message2;
        private TextBox tb_Firstname;
        private TextBox tb_Surname;
        private TextBox tb_System;
        private TextBox tb_Callsign;
        private TextBox tb_Year;
        private TextBox tb_Day;
        private TextBox tb_Hex;
        private Button btn_StartProcessElevated;
        private Button btn_ExecuteDebugger;
        private Button btn_ExecuteDebuggerAsync;
        private Button btn_StartDebugger;
        private Button btn_StopDebugger;
        private Label lbl_Message1;
        private Label lbl_Message2;
        private Label lbl_firstName;
        private Label lbl_surname;
        private Label lbl_callsign;
        private Label lbl_system;
        private Label lbl_year;
        private Label lbl_day;
        private RadioButton rb_WC1;
        private RadioButton rb_WC2;
        private RadioButton rb_SO1;
        private RadioButton rb_SM2;
        private RadioButton rb_SO2;
        private TextBox tb_string;
        private TextBox tb_hexstring;
        private TextBox tb_Address;
        private Button btn_StartProcess;
        private Button btn_testDoSomething;
        private CheckBox cb_WCAT;
        private Button btn_testDoSomething2;
        private CheckBox cb_WCKS;
        private RadioButton rb_SM1;
    }
}
