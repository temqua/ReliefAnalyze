﻿namespace ReliefAnalyze
{
    partial class MainForm
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
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.MainMenu = new System.Windows.Forms.MenuStrip();
            this.файлToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openFileMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.дополнительноToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.histogramMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1 = new System.Windows.Forms.Panel();
            this.mainPictureBox = new System.Windows.Forms.PictureBox();
            this.fragmentAnalyzeButton = new System.Windows.Forms.Button();
            this.detectColors = new System.Windows.Forms.Button();
            this.scaleBox = new System.Windows.Forms.ComboBox();
            this.materialLabel1 = new MaterialSkin.Controls.MaterialLabel();
            this.materialLabel2 = new MaterialSkin.Controls.MaterialLabel();
            this.detectSizeRadio = new MaterialSkin.Controls.MaterialRadioButton();
            this.notDetectSizeRadio = new MaterialSkin.Controls.MaterialRadioButton();
            this.AnalyzePlaceButton = new MaterialSkin.Controls.MaterialRaisedButton();
            this.materialFlatButton1 = new MaterialSkin.Controls.MaterialFlatButton();
            this.chooseMainColorsFlatButton = new MaterialSkin.Controls.MaterialFlatButton();
            this.criteriaButton = new MaterialSkin.Controls.MaterialFlatButton();
            this.MainMenu.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mainPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // MainMenu
            // 
            this.MainMenu.Dock = System.Windows.Forms.DockStyle.None;
            this.MainMenu.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.MainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.файлToolStripMenuItem,
            this.дополнительноToolStripMenuItem});
            this.MainMenu.Location = new System.Drawing.Point(0, 0);
            this.MainMenu.Name = "MainMenu";
            this.MainMenu.Padding = new System.Windows.Forms.Padding(5, 2, 0, 2);
            this.MainMenu.Size = new System.Drawing.Size(200, 28);
            this.MainMenu.TabIndex = 1;
            this.MainMenu.Text = "menuStrip1";
            // 
            // файлToolStripMenuItem
            // 
            this.файлToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openFileMenuItem,
            this.exitMenuItem});
            this.файлToolStripMenuItem.Name = "файлToolStripMenuItem";
            this.файлToolStripMenuItem.Size = new System.Drawing.Size(59, 26);
            this.файлToolStripMenuItem.Text = "Файл";
            // 
            // openFileMenuItem
            // 
            this.openFileMenuItem.Name = "openFileMenuItem";
            this.openFileMenuItem.Size = new System.Drawing.Size(150, 26);
            this.openFileMenuItem.Text = "Открыть";
            this.openFileMenuItem.Click += new System.EventHandler(this.OpenFileMenuItem_Click);
            // 
            // exitMenuItem
            // 
            this.exitMenuItem.Name = "exitMenuItem";
            this.exitMenuItem.Size = new System.Drawing.Size(150, 26);
            this.exitMenuItem.Text = "Выход";
            this.exitMenuItem.Click += new System.EventHandler(this.ExitMenuItem_Click);
            // 
            // дополнительноToolStripMenuItem
            // 
            this.дополнительноToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.histogramMenuItem});
            this.дополнительноToolStripMenuItem.Name = "дополнительноToolStripMenuItem";
            this.дополнительноToolStripMenuItem.Size = new System.Drawing.Size(134, 26);
            this.дополнительноToolStripMenuItem.Text = "Дополнительно";
            // 
            // histogramMenuItem
            // 
            this.histogramMenuItem.Name = "histogramMenuItem";
            this.histogramMenuItem.Size = new System.Drawing.Size(183, 26);
            this.histogramMenuItem.Text = "Гистограмма";
            this.histogramMenuItem.Click += new System.EventHandler(this.HistogramMenuItem_Click);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.mainPictureBox);
            this.panel1.Controls.Add(this.fragmentAnalyzeButton);
            this.panel1.Location = new System.Drawing.Point(117, 233);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1488, 642);
            this.panel1.TabIndex = 5;
            // 
            // mainPictureBox
            // 
            this.mainPictureBox.Cursor = System.Windows.Forms.Cursors.Cross;
            this.mainPictureBox.Location = new System.Drawing.Point(0, 0);
            this.mainPictureBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.mainPictureBox.Name = "mainPictureBox";
            this.mainPictureBox.Size = new System.Drawing.Size(800, 400);
            this.mainPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.mainPictureBox.TabIndex = 1;
            this.mainPictureBox.TabStop = false;
            this.mainPictureBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MainPictureBox_MouseDown);
            // 
            // fragmentAnalyzeButton
            // 
            this.fragmentAnalyzeButton.Location = new System.Drawing.Point(-101, 39);
            this.fragmentAnalyzeButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.fragmentAnalyzeButton.Name = "fragmentAnalyzeButton";
            this.fragmentAnalyzeButton.Size = new System.Drawing.Size(264, 28);
            this.fragmentAnalyzeButton.TabIndex = 17;
            this.fragmentAnalyzeButton.Text = "Анализ фрагмента";
            this.fragmentAnalyzeButton.UseVisualStyleBackColor = true;
            this.fragmentAnalyzeButton.Click += new System.EventHandler(this.FragmentAnalyzeButton_Click);
            // 
            // detectColors
            // 
            this.detectColors.Location = new System.Drawing.Point(1464, 98);
            this.detectColors.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.detectColors.Name = "detectColors";
            this.detectColors.Size = new System.Drawing.Size(175, 28);
            this.detectColors.TabIndex = 9;
            this.detectColors.Text = "определить цвета";
            this.detectColors.UseVisualStyleBackColor = true;
            this.detectColors.Visible = false;
            this.detectColors.Click += new System.EventHandler(this.DetectColors_Click);
            // 
            // scaleBox
            // 
            this.scaleBox.AllowDrop = true;
            this.scaleBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.scaleBox.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.scaleBox.FormattingEnabled = true;
            this.scaleBox.Items.AddRange(new object[] {
            "5",
            "10",
            "20",
            "50",
            "100"});
            this.scaleBox.Location = new System.Drawing.Point(1305, 153);
            this.scaleBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.scaleBox.Name = "scaleBox";
            this.scaleBox.Size = new System.Drawing.Size(101, 31);
            this.scaleBox.TabIndex = 15;
            // 
            // materialLabel1
            // 
            this.materialLabel1.AutoSize = true;
            this.materialLabel1.Depth = 0;
            this.materialLabel1.Font = new System.Drawing.Font("Roboto", 11F);
            this.materialLabel1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.materialLabel1.Location = new System.Drawing.Point(1300, 101);
            this.materialLabel1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.materialLabel1.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialLabel1.Name = "materialLabel1";
            this.materialLabel1.Size = new System.Drawing.Size(92, 24);
            this.materialLabel1.TabIndex = 18;
            this.materialLabel1.Text = "Масштаб";
            // 
            // materialLabel2
            // 
            this.materialLabel2.AutoSize = true;
            this.materialLabel2.Depth = 0;
            this.materialLabel2.Font = new System.Drawing.Font("Roboto", 11F);
            this.materialLabel2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.materialLabel2.Location = new System.Drawing.Point(1429, 156);
            this.materialLabel2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.materialLabel2.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialLabel2.Name = "materialLabel2";
            this.materialLabel2.Size = new System.Drawing.Size(132, 24);
            this.materialLabel2.TabIndex = 19;
            this.materialLabel2.Text = "км / 100 пикс";
            // 
            // detectSizeRadio
            // 
            this.detectSizeRadio.AutoSize = true;
            this.detectSizeRadio.Depth = 0;
            this.detectSizeRadio.Font = new System.Drawing.Font("Roboto", 10F);
            this.detectSizeRadio.Location = new System.Drawing.Point(556, 101);
            this.detectSizeRadio.Margin = new System.Windows.Forms.Padding(0);
            this.detectSizeRadio.MouseLocation = new System.Drawing.Point(-1, -1);
            this.detectSizeRadio.MouseState = MaterialSkin.MouseState.HOVER;
            this.detectSizeRadio.Name = "detectSizeRadio";
            this.detectSizeRadio.Ripple = true;
            this.detectSizeRadio.Size = new System.Drawing.Size(233, 30);
            this.detectSizeRadio.TabIndex = 21;
            this.detectSizeRadio.TabStop = true;
            this.detectSizeRadio.Text = "С выявлением размеров";
            this.detectSizeRadio.UseVisualStyleBackColor = true;
            // 
            // notDetectSizeRadio
            // 
            this.notDetectSizeRadio.AutoSize = true;
            this.notDetectSizeRadio.Depth = 0;
            this.notDetectSizeRadio.Font = new System.Drawing.Font("Roboto", 10F);
            this.notDetectSizeRadio.Location = new System.Drawing.Point(556, 154);
            this.notDetectSizeRadio.Margin = new System.Windows.Forms.Padding(0);
            this.notDetectSizeRadio.MouseLocation = new System.Drawing.Point(-1, -1);
            this.notDetectSizeRadio.MouseState = MaterialSkin.MouseState.HOVER;
            this.notDetectSizeRadio.Name = "notDetectSizeRadio";
            this.notDetectSizeRadio.Ripple = true;
            this.notDetectSizeRadio.Size = new System.Drawing.Size(238, 30);
            this.notDetectSizeRadio.TabIndex = 22;
            this.notDetectSizeRadio.TabStop = true;
            this.notDetectSizeRadio.Text = "Без выявления размеров";
            this.notDetectSizeRadio.UseVisualStyleBackColor = true;
            // 
            // AnalyzePlaceButton
            // 
            this.AnalyzePlaceButton.Depth = 0;
            this.AnalyzePlaceButton.Location = new System.Drawing.Point(117, 101);
            this.AnalyzePlaceButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.AnalyzePlaceButton.MouseState = MaterialSkin.MouseState.HOVER;
            this.AnalyzePlaceButton.Name = "AnalyzePlaceButton";
            this.AnalyzePlaceButton.Primary = true;
            this.AnalyzePlaceButton.Size = new System.Drawing.Size(388, 90);
            this.AnalyzePlaceButton.TabIndex = 23;
            this.AnalyzePlaceButton.Text = "Анализ выбранного места";
            this.AnalyzePlaceButton.UseVisualStyleBackColor = true;
            this.AnalyzePlaceButton.Click += new System.EventHandler(this.AnalyzePlaceButton_Click);
            // 
            // materialFlatButton1
            // 
            this.materialFlatButton1.AutoSize = true;
            this.materialFlatButton1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.materialFlatButton1.Depth = 0;
            this.materialFlatButton1.Location = new System.Drawing.Point(1350, 58);
            this.materialFlatButton1.Margin = new System.Windows.Forms.Padding(5, 7, 5, 7);
            this.materialFlatButton1.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialFlatButton1.Name = "materialFlatButton1";
            this.materialFlatButton1.Primary = false;
            this.materialFlatButton1.Size = new System.Drawing.Size(211, 36);
            this.materialFlatButton1.TabIndex = 24;
            this.materialFlatButton1.Text = "Выявление контуров";
            this.materialFlatButton1.UseVisualStyleBackColor = true;
            this.materialFlatButton1.Visible = false;
            // 
            // chooseMainColorsFlatButton
            // 
            this.chooseMainColorsFlatButton.AutoSize = true;
            this.chooseMainColorsFlatButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.chooseMainColorsFlatButton.Depth = 0;
            this.chooseMainColorsFlatButton.Location = new System.Drawing.Point(900, 98);
            this.chooseMainColorsFlatButton.Margin = new System.Windows.Forms.Padding(5, 7, 5, 7);
            this.chooseMainColorsFlatButton.MouseState = MaterialSkin.MouseState.HOVER;
            this.chooseMainColorsFlatButton.Name = "chooseMainColorsFlatButton";
            this.chooseMainColorsFlatButton.Primary = false;
            this.chooseMainColorsFlatButton.Size = new System.Drawing.Size(238, 36);
            this.chooseMainColorsFlatButton.TabIndex = 25;
            this.chooseMainColorsFlatButton.Text = "Выбрать главные цвета";
            this.chooseMainColorsFlatButton.UseVisualStyleBackColor = true;
            this.chooseMainColorsFlatButton.Click += new System.EventHandler(this.ChooseMainColorsButton_Click);
            // 
            // criteriaButton
            // 
            this.criteriaButton.AutoSize = true;
            this.criteriaButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.criteriaButton.Depth = 0;
            this.criteriaButton.Location = new System.Drawing.Point(900, 148);
            this.criteriaButton.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.criteriaButton.MouseState = MaterialSkin.MouseState.HOVER;
            this.criteriaButton.Name = "criteriaButton";
            this.criteriaButton.Primary = false;
            this.criteriaButton.Size = new System.Drawing.Size(103, 36);
            this.criteriaButton.TabIndex = 28;
            this.criteriaButton.Text = "Критерии";
            this.criteriaButton.UseVisualStyleBackColor = true;
            this.criteriaButton.Click += new System.EventHandler(this.CriteriaButton_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1715, 949);
            this.Controls.Add(this.criteriaButton);
            this.Controls.Add(this.chooseMainColorsFlatButton);
            this.Controls.Add(this.materialFlatButton1);
            this.Controls.Add(this.AnalyzePlaceButton);
            this.Controls.Add(this.notDetectSizeRadio);
            this.Controls.Add(this.detectSizeRadio);
            this.Controls.Add(this.materialLabel2);
            this.Controls.Add(this.materialLabel1);
            this.Controls.Add(this.scaleBox);
            this.Controls.Add(this.detectColors);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.MainMenu);
            this.MainMenuStrip = this.MainMenu;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "MainForm";
            this.Text = "Анализ рельефа";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.MainMenu.ResumeLayout(false);
            this.MainMenu.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mainPictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.MenuStrip MainMenu;
        private System.Windows.Forms.ToolStripMenuItem файлToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openFileMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitMenuItem;
        private System.Windows.Forms.ToolStripMenuItem дополнительноToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem histogramMenuItem;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox mainPictureBox;
        private System.Windows.Forms.Button detectColors;
        private System.Windows.Forms.ComboBox scaleBox;
        private System.Windows.Forms.Button fragmentAnalyzeButton;
        private MaterialSkin.Controls.MaterialLabel materialLabel1;
        private MaterialSkin.Controls.MaterialLabel materialLabel2;
        private MaterialSkin.Controls.MaterialRadioButton detectSizeRadio;
        private MaterialSkin.Controls.MaterialRadioButton notDetectSizeRadio;
        private MaterialSkin.Controls.MaterialRaisedButton AnalyzePlaceButton;
        private MaterialSkin.Controls.MaterialFlatButton materialFlatButton1;
        private MaterialSkin.Controls.MaterialFlatButton chooseMainColorsFlatButton;
        private MaterialSkin.Controls.MaterialFlatButton criteriaButton;
    }
}

