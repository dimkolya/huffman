namespace Huffman_compressor
{
    partial class formHuffmanCompressorForm
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBoxOpenFiles = new System.Windows.Forms.GroupBox();
            this.textBoxBrowseFile = new System.Windows.Forms.TextBox();
            this.buttonBrowseFile = new System.Windows.Forms.Button();
            this.panelDragAndDrop = new System.Windows.Forms.Panel();
            this.labelDragAndDrop = new System.Windows.Forms.Label();
            this.buttonEncode = new System.Windows.Forms.Button();
            this.buttonDecode = new System.Windows.Forms.Button();
            this.groupBoxChooseDirectory = new System.Windows.Forms.GroupBox();
            this.textBoxBrowseDirectory = new System.Windows.Forms.TextBox();
            this.buttonBrowseDirectory = new System.Windows.Forms.Button();
            this.numericUpDownCoresCount = new System.Windows.Forms.NumericUpDown();
            this.buttonAbout = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBoxOpenFiles.SuspendLayout();
            this.panelDragAndDrop.SuspendLayout();
            this.groupBoxChooseDirectory.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownCoresCount)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBoxOpenFiles
            // 
            this.groupBoxOpenFiles.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxOpenFiles.Controls.Add(this.textBoxBrowseFile);
            this.groupBoxOpenFiles.Controls.Add(this.buttonBrowseFile);
            this.groupBoxOpenFiles.Location = new System.Drawing.Point(12, 12);
            this.groupBoxOpenFiles.Name = "groupBoxOpenFiles";
            this.groupBoxOpenFiles.Size = new System.Drawing.Size(410, 157);
            this.groupBoxOpenFiles.TabIndex = 0;
            this.groupBoxOpenFiles.TabStop = false;
            this.groupBoxOpenFiles.Text = "Выберите файлы";
            // 
            // textBoxBrowseFile
            // 
            this.textBoxBrowseFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxBrowseFile.BackColor = System.Drawing.SystemColors.Window;
            this.textBoxBrowseFile.Location = new System.Drawing.Point(13, 19);
            this.textBoxBrowseFile.Margin = new System.Windows.Forms.Padding(10, 3, 3, 3);
            this.textBoxBrowseFile.Name = "textBoxBrowseFile";
            this.textBoxBrowseFile.Size = new System.Drawing.Size(303, 20);
            this.textBoxBrowseFile.TabIndex = 1;
            this.textBoxBrowseFile.Tag = "";
            this.textBoxBrowseFile.TextChanged += new System.EventHandler(this.textBoxBrowseFile_TextChanged);
            // 
            // buttonBrowseFile
            // 
            this.buttonBrowseFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonBrowseFile.Location = new System.Drawing.Point(322, 19);
            this.buttonBrowseFile.Margin = new System.Windows.Forms.Padding(3, 3, 10, 3);
            this.buttonBrowseFile.Name = "buttonBrowseFile";
            this.buttonBrowseFile.Size = new System.Drawing.Size(75, 20);
            this.buttonBrowseFile.TabIndex = 0;
            this.buttonBrowseFile.Text = "Обзор...";
            this.buttonBrowseFile.UseVisualStyleBackColor = true;
            this.buttonBrowseFile.Click += new System.EventHandler(this.buttonBrowseFile_Click);
            // 
            // panelDragAndDrop
            // 
            this.panelDragAndDrop.AllowDrop = true;
            this.panelDragAndDrop.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelDragAndDrop.BackColor = System.Drawing.SystemColors.ControlLight;
            this.panelDragAndDrop.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelDragAndDrop.Controls.Add(this.labelDragAndDrop);
            this.panelDragAndDrop.Location = new System.Drawing.Point(25, 63);
            this.panelDragAndDrop.Margin = new System.Windows.Forms.Padding(10, 3, 10, 3);
            this.panelDragAndDrop.Name = "panelDragAndDrop";
            this.panelDragAndDrop.Size = new System.Drawing.Size(384, 100);
            this.panelDragAndDrop.TabIndex = 1;
            this.panelDragAndDrop.DragDrop += new System.Windows.Forms.DragEventHandler(this.panelDragAndDrop_DragDrop);
            this.panelDragAndDrop.DragEnter += new System.Windows.Forms.DragEventHandler(this.panelDragAndDrop_DragEnter);
            this.panelDragAndDrop.DragLeave += new System.EventHandler(this.panelDragAndDrop_DragLeave);
            // 
            // labelDragAndDrop
            // 
            this.labelDragAndDrop.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelDragAndDrop.Location = new System.Drawing.Point(100, 30);
            this.labelDragAndDrop.Margin = new System.Windows.Forms.Padding(100, 30, 100, 30);
            this.labelDragAndDrop.Name = "labelDragAndDrop";
            this.labelDragAndDrop.Size = new System.Drawing.Size(182, 40);
            this.labelDragAndDrop.TabIndex = 0;
            this.labelDragAndDrop.Text = "Перетащите файлы сюда\r\n";
            this.labelDragAndDrop.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // buttonEncode
            // 
            this.buttonEncode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonEncode.Location = new System.Drawing.Point(12, 226);
            this.buttonEncode.Name = "buttonEncode";
            this.buttonEncode.Size = new System.Drawing.Size(75, 23);
            this.buttonEncode.TabIndex = 2;
            this.buttonEncode.Text = "Сжать";
            this.buttonEncode.UseVisualStyleBackColor = true;
            this.buttonEncode.Click += new System.EventHandler(this.buttonEncode_Click);
            // 
            // buttonDecode
            // 
            this.buttonDecode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonDecode.Location = new System.Drawing.Point(93, 226);
            this.buttonDecode.Name = "buttonDecode";
            this.buttonDecode.Size = new System.Drawing.Size(75, 23);
            this.buttonDecode.TabIndex = 3;
            this.buttonDecode.Text = "Разжать";
            this.buttonDecode.UseVisualStyleBackColor = true;
            this.buttonDecode.Click += new System.EventHandler(this.buttonDecode_Click);
            // 
            // groupBoxChooseDirectory
            // 
            this.groupBoxChooseDirectory.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxChooseDirectory.Controls.Add(this.textBoxBrowseDirectory);
            this.groupBoxChooseDirectory.Controls.Add(this.buttonBrowseDirectory);
            this.groupBoxChooseDirectory.Location = new System.Drawing.Point(12, 175);
            this.groupBoxChooseDirectory.Name = "groupBoxChooseDirectory";
            this.groupBoxChooseDirectory.Size = new System.Drawing.Size(410, 45);
            this.groupBoxChooseDirectory.TabIndex = 4;
            this.groupBoxChooseDirectory.TabStop = false;
            this.groupBoxChooseDirectory.Text = "Выберите папку";
            // 
            // textBoxBrowseDirectory
            // 
            this.textBoxBrowseDirectory.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxBrowseDirectory.Location = new System.Drawing.Point(13, 19);
            this.textBoxBrowseDirectory.Margin = new System.Windows.Forms.Padding(10, 3, 3, 3);
            this.textBoxBrowseDirectory.Name = "textBoxBrowseDirectory";
            this.textBoxBrowseDirectory.Size = new System.Drawing.Size(303, 20);
            this.textBoxBrowseDirectory.TabIndex = 1;
            this.textBoxBrowseDirectory.TextChanged += new System.EventHandler(this.textBoxBrowseDirectory_TextChanged);
            // 
            // buttonBrowseDirectory
            // 
            this.buttonBrowseDirectory.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonBrowseDirectory.Location = new System.Drawing.Point(322, 19);
            this.buttonBrowseDirectory.Margin = new System.Windows.Forms.Padding(3, 3, 10, 3);
            this.buttonBrowseDirectory.Name = "buttonBrowseDirectory";
            this.buttonBrowseDirectory.Size = new System.Drawing.Size(75, 20);
            this.buttonBrowseDirectory.TabIndex = 0;
            this.buttonBrowseDirectory.Text = "Обзор...";
            this.buttonBrowseDirectory.UseVisualStyleBackColor = true;
            this.buttonBrowseDirectory.Click += new System.EventHandler(this.buttonBrowseDirectory_Click);
            // 
            // numericUpDownCoresCount
            // 
            this.numericUpDownCoresCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.numericUpDownCoresCount.Location = new System.Drawing.Point(382, 229);
            this.numericUpDownCoresCount.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.numericUpDownCoresCount.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownCoresCount.Name = "numericUpDownCoresCount";
            this.numericUpDownCoresCount.Size = new System.Drawing.Size(40, 20);
            this.numericUpDownCoresCount.TabIndex = 5;
            this.numericUpDownCoresCount.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // buttonAbout
            // 
            this.buttonAbout.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonAbout.Location = new System.Drawing.Point(174, 226);
            this.buttonAbout.Name = "buttonAbout";
            this.buttonAbout.Size = new System.Drawing.Size(75, 23);
            this.buttonAbout.TabIndex = 6;
            this.buttonAbout.Text = "Справка";
            this.buttonAbout.UseVisualStyleBackColor = true;
            this.buttonAbout.Click += new System.EventHandler(this.buttonAbout_Click);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(269, 231);
            this.label1.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(113, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Количество потоков:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // formHuffmanCompressorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(434, 261);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonAbout);
            this.Controls.Add(this.numericUpDownCoresCount);
            this.Controls.Add(this.groupBoxChooseDirectory);
            this.Controls.Add(this.buttonDecode);
            this.Controls.Add(this.buttonEncode);
            this.Controls.Add(this.panelDragAndDrop);
            this.Controls.Add(this.groupBoxOpenFiles);
            this.MinimumSize = new System.Drawing.Size(450, 300);
            this.Name = "formHuffmanCompressorForm";
            this.Text = "Архиватор Хаффмана";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.formHuffmanCompressorForm_FormClosing);
            this.groupBoxOpenFiles.ResumeLayout(false);
            this.groupBoxOpenFiles.PerformLayout();
            this.panelDragAndDrop.ResumeLayout(false);
            this.groupBoxChooseDirectory.ResumeLayout(false);
            this.groupBoxChooseDirectory.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownCoresCount)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.GroupBox groupBoxOpenFiles;
        private System.Windows.Forms.TextBox textBoxBrowseFile;
        private System.Windows.Forms.Button buttonBrowseFile;
        private System.Windows.Forms.Panel panelDragAndDrop;
        private System.Windows.Forms.Label labelDragAndDrop;
        private System.Windows.Forms.Button buttonEncode;
        private System.Windows.Forms.Button buttonDecode;
        private System.Windows.Forms.GroupBox groupBoxChooseDirectory;
        private System.Windows.Forms.TextBox textBoxBrowseDirectory;
        private System.Windows.Forms.Button buttonBrowseDirectory;
        private System.Windows.Forms.NumericUpDown numericUpDownCoresCount;
        private System.Windows.Forms.Button buttonAbout;
        private System.Windows.Forms.Label label1;
    }
}

