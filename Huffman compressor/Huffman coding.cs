using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlTypes;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;

namespace Huffman_compressor
{
    public partial class formHuffmanCoding : Form
    {
        private const int PROGRESS_BAR_HEIGHT = 20;
        private const int PROGRESS_BAR_MAX_VALUE = 100;

        private int coreCount;
        private bool[] changed;
        private ulong[] curFileSizes;
        private ulong[][] progresses;
        private string[] curFiles;

        private GroupBox[] groupBoxes;
        private Label[] fileNameLabels;
        private Label[][] percentageLabels;
        private ProgressBar[][] progressBars;

        Thread thread;

        public formHuffmanCoding(
            int coreCount,
            ref bool[] changed,
            ref ulong[][] progresses,
            ref ulong[] curFileSizes,
            ref string[] curFiles,
            Thread thread)
        {
            this.coreCount = coreCount;
            this.changed = changed;
            this.progresses = progresses;
            this.curFileSizes = curFileSizes;
            this.curFiles = curFiles;
            this.thread = thread;

            InitializeComponent();

            fileNameLabels = new Label[coreCount];

            int count = progresses.Length;
            groupBoxes = new GroupBox[count];
            percentageLabels = new Label[count][];
            progressBars = new ProgressBar[count][];
            for (int i = 0; i < count; ++i)
            {
                percentageLabels[i] = new Label[coreCount];
                progressBars[i] = new ProgressBar[coreCount];
            }

            int groupBoxesWidth = (Width - 48 - 6 * (groupBoxes.Length - 1)) / groupBoxes.Length;
            int currentX = 12;

            for (int i = 0; i < groupBoxes.Length; ++i)
            {
                groupBoxes[i] = new GroupBox();
                groupBoxes[i].Location = new Point(currentX, 12);
                groupBoxes[i].Width = groupBoxesWidth;
                Controls.Add(groupBoxes[i]);
                currentX += 6 + groupBoxesWidth;
            }

            if (count == 1)
            {
                Text = "Распаковка";
                groupBoxes[0].Text = "Распаковка";
            }
            else if (count == 2)
            {
                Text = "Сжатие";
                groupBoxes[0].Text = "Обработка данных";
                groupBoxes[1].Text = "Сжатие";
            }

            fillGroupBoxes();
            this.Height = 60 + groupBoxes[0].Height;

            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            timer1.Interval = 250;
            timer1.Start();
        }

        private void fillGroupBoxes()
        {
            for (int i = 0; i < groupBoxes.Length; ++i)
            {
                int currentLocationY = 12;
                GroupBox groupBox = groupBoxes[i];
                for (int j = 0; j < coreCount; j++)
                {
                    if (i == 0)
                    {
                        fileNameLabels[j] = new Label();
                        fileNameLabels[j].AutoSize = false;
                        fileNameLabels[j].Text = "н/д";
                        fileNameLabels[j].Location = new Point(
                            fileNameLabels[j].Margin.Left
                            + groupBox.Padding.Left,
                            currentLocationY);
                        groupBox.Controls.Add(fileNameLabels[j]);
                    }

                    Label[] percentageLabel = percentageLabels[i];
                    percentageLabel[j] = new Label();
                    percentageLabel[j].AutoSize = false;
                    percentageLabel[j].Text = "н/д";
                    percentageLabel[j].TextAlign = System.Drawing.ContentAlignment.MiddleRight;
                    percentageLabel[j].Location = new Point(
                        groupBox.Width
                        - groupBox.Padding.Right
                        - percentageLabel[j].Width
                        - percentageLabel[j].Margin.Right,
                        currentLocationY);
                    groupBox.Controls.Add(percentageLabel[j]);

                    currentLocationY += percentageLabel[j].Height + percentageLabel[j].Margin.Bottom;

                    ProgressBar[] progressBar = progressBars[i];
                    progressBar[j] = new ProgressBar();
                    progressBar[j].Location = new Point(
                        progressBar[j].Margin.Left + groupBox.Padding.Left,
                        currentLocationY);
                    progressBar[j].Size = new Size(
                        groupBox.Width
                        - groupBox.Padding.Horizontal
                        - progressBar[j].Margin.Horizontal,
                        PROGRESS_BAR_HEIGHT);
                    progressBar[j].Maximum = PROGRESS_BAR_MAX_VALUE;
                    groupBox.Controls.Add(progressBar[j]);

                    currentLocationY += progressBar[j].Height + progressBar[j].Margin.Bottom;
                }
                groupBox.Height = currentLocationY + groupBox.Padding.Bottom;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (thread.ThreadState == ThreadState.Stopped)
            {
                Close();
                return;
            }
            for (int i = 0; i < coreCount; ++i)
            {
                if (curFileSizes == null) continue;
                if (changed[i])
                {
                    changed[i] = false;
                    fileNameLabels[i].Text = curFiles[i];
                }
                ulong curFileSize = curFileSizes[i];
                if (curFileSize == 0)
                {
                    for (int j = 0; j < groupBoxes.Length; ++j)
                    {
                        percentageLabels[j][i].Text = "100 %";
                        progressBars[j][i].Value = PROGRESS_BAR_MAX_VALUE;
                    }
                }
                else
                {
                    for (int j = 0; j < groupBoxes.Length; ++j)
                    {
                        int currentValue = (int)(PROGRESS_BAR_MAX_VALUE * progresses[j][i] / curFileSize);
                        progressBars[j][i].Value = currentValue;
                        percentageLabels[j][i].Text = (currentValue / (PROGRESS_BAR_MAX_VALUE / 100)).ToString() + " %";
                    }
                }
            }
        }

        private void formHuffmanEncoding_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (thread.ThreadState == ThreadState.Stopped) return;
            if (MessageBox.Show(
                "Операция еще не закончена и будет продолжать выполнение в фоновом режиме.",
                "Предупреждение",
                MessageBoxButtons.OKCancel) == DialogResult.Cancel)
            {
                e.Cancel = true;
            }
        }
    }
}
