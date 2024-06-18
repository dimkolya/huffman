using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Huffman_compressor
{
    public partial class formHuffmanCompressorForm : Form
    {
        [DllImport("huffman_lib.dll", EntryPoint = "huffman_encode", CallingConvention = CallingConvention.Cdecl)]
        private static extern int huffmanEncode(string inputFilename, string outputFilename, ref ulong analysis, ref ulong encoding);
        [DllImport("huffman_lib.dll", EntryPoint = "huffman_decode", CallingConvention = CallingConvention.Cdecl)]
        private static extern int huffmanDecode(string inputFilename, string outputFilename, ref ulong decoding);

        private HashSet<Thread> threads = new HashSet<Thread>();

        public formHuffmanCompressorForm()
        {
            if (!File.Exists("huffman_lib.dll"))
            {
                MessageBox.Show(
                    "Не удается найти динамически загружаемую библиотеку \"huffman_lib.dll\".",
                    "Ошибка",
                    MessageBoxButtons.OK);
                this.Close();
            }
            InitializeComponent();
            buttonEncode.Enabled = false;
            buttonDecode.Enabled = false;
            numericUpDownCoresCount.Maximum = Environment.ProcessorCount;
            numericUpDownCoresCount.Minimum = 1;
            numericUpDownCoresCount.Value =  1;
        }

        private void buttonBrowseFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Обзор файлов";
            openFileDialog.Multiselect = true;
            if (openFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                StringBuilder stringBuilder = new StringBuilder();
                foreach (string fileName in openFileDialog.FileNames)
                {
                    stringBuilder.Append(fileName);
                    stringBuilder.Append(';');
                }
                --stringBuilder.Length;
                textBoxBrowseFile.Text = stringBuilder.ToString();
            }
        }

        private void textBoxBrowseFile_TextChanged(object sender, EventArgs e)
        {
            enableButtons();
        }

        private void enableButtons()
        {
            if (!Directory.Exists(textBoxBrowseDirectory.Text))
            {
                buttonEncode.Enabled = false;
                buttonDecode.Enabled = false;
                return;
            }
            if (textBoxBrowseFile.Text.Length == 0)
            {
                buttonEncode.Enabled = false;
                buttonDecode.Enabled = false;
                return;
            }
            string[] files = textBoxBrowseFile.Text.Split(';');
            bool decode = true;
            foreach (string file in files)
            {
                if (!File.Exists(file))
                {
                    buttonEncode.Enabled = false;
                    buttonDecode.Enabled = false;
                    return;
                }
                if (!file.EndsWith(".dmk"))
                {
                    decode = false;
                }
            }
            buttonEncode.Enabled = true;
            buttonDecode.Enabled = true;
            if (decode)
            {
                buttonDecode.Focus();
            }
            else
            {
                buttonEncode.Focus();
            }
        }

        private void panelDragAndDrop_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                labelDragAndDrop.Text = "Отпустите для вставки";
                e.Effect = DragDropEffects.Copy;
            }
        }

        private void panelDragAndDrop_DragDrop(object sender, DragEventArgs e)
        {
            labelDragAndDrop.Text = "Перетащите файлы сюда";
            StringBuilder stringBuilder = new StringBuilder();
            foreach (string file in (string[])e.Data.GetData(DataFormats.FileDrop))
            {
                if (Directory.Exists(file))
                {
                    foreach (string fileName in Directory.GetFiles(file, "*", SearchOption.AllDirectories))
                    {
                        stringBuilder.Append(fileName);
                        stringBuilder.Append(';');
                    }
                }
                else
                {
                    stringBuilder.Append(file);
                    stringBuilder.Append(';');
                }
            }
            --stringBuilder.Length;
            textBoxBrowseFile.Text= stringBuilder.ToString();
        }

        private void panelDragAndDrop_DragLeave(object sender, EventArgs e)
        {
            labelDragAndDrop.Text = "Перетащите файлы сюда";
        }

        private void buttonBrowseDirectory_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK) {
                textBoxBrowseDirectory.Text = folderBrowserDialog.SelectedPath;
            }
        }

        private void textBoxBrowseDirectory_TextChanged(object sender, EventArgs e)
        {
            enableButtons();
        }

        private void buttonEncode_Click(object sender, EventArgs e)
        {
            buttonCode_Click(true);
        }

        private void buttonDecode_Click(object sender, EventArgs e)
        {
            buttonCode_Click(false);
        }

        private void buttonCode_Click(bool encode)
        {
            int count;
            if (encode)
            {
                count = 2;
            }
            else
            {
                count = 1;
            }
            ulong[][] progresses = new ulong[count][];
            string[] files = textBoxBrowseFile.Text.Split(';');
            int coreCount = Math.Min((int)numericUpDownCoresCount.Value, files.Length);
            for (int i = 0; i < count; i++)
            {
                progresses[i] = new ulong[coreCount];
            }
            bool[] changed = new bool[coreCount];
            ulong[] curFileSizes = new ulong[coreCount];
            string[] curFiles = new string[coreCount];
            for (int i = 0; i < coreCount; ++i)
            {
                changed[i] = false;
                curFileSizes[i] = 0;
                curFiles[i] = null;
                for (int j = 0; j < count; ++j)
                {
                    progresses[j][i] = 0;
                }
            }

            Thread thread = new Thread(delegate ()
            {
                code(
                    encode,
                    coreCount,
                    textBoxBrowseDirectory.Text,
                    files,
                    ref changed,
                    ref curFileSizes,
                    ref progresses,
                    ref curFiles);
            })
            {
                IsBackground = true,
                Priority = ThreadPriority.Normal
            };
            Thread formThread = new Thread(() =>
            {
                new formHuffmanCoding(
                    coreCount,
                    ref changed,
                    ref progresses,
                    ref curFileSizes,
                    ref curFiles,
                    thread).ShowDialog();
            })
            {
                IsBackground = true,
                Priority = ThreadPriority.Normal
            };
            threads.Add(thread);
            thread.Start();
            formThread.Start();
        }

        private void code(
            bool encode,
            int coreCount,
            string dir,
            string[] files,
            ref bool[] _changed,
            ref ulong[] _curFileSizes,
            ref ulong[][] _progresses,
            ref string[] _curFiles)
        {
            bool[] changed = _changed;
            ulong[] curFileSizes = _curFileSizes;
            ulong[][] progresses = _progresses;
            string[] curFiles = _curFiles;
            ConcurrentStack<int> freeIndexes = new ConcurrentStack<int>();
            for (int i = 0; i < coreCount; ++i)
            {
                freeIndexes.Push(i);
            }
            Parallel.ForEach(
                files,
                new ParallelOptions
                {
                    MaxDegreeOfParallelism = coreCount
                },
                file =>
                {
                    string inputFileName = Path.GetFileName(file);
                    string outputFileName;
                    if (encode) outputFileName = dir + '\\' + inputFileName + ".dmk";
                    else
                    {
                        if (inputFileName.EndsWith(".dmk"))
                            outputFileName = dir + '\\' + inputFileName.Remove(inputFileName.Length - 4);
                        else
                            outputFileName = dir + "\\" + inputFileName;
                    }
                    int index;
                    if (!freeIndexes.TryPop(out index))
                    {
                        MessageBox.Show("Что-то пошло не так, кажется, у вас всё сдохло...", "Ошибка");
                        return;
                    }
                    curFileSizes[index] = (ulong)new FileInfo(file).Length;
                    curFiles[index] = inputFileName;
                    changed[index] = true;
                    int result;
                    if (encode) result = huffmanEncode(file, outputFileName, ref progresses[0][index], ref progresses[1][index]);
                    else result = huffmanDecode(file, outputFileName, ref progresses[0][index]);
                    if (result != 0)
                    {
                        try { File.Delete(outputFileName); } catch { }

                        string errorMessage;
                        if (result == -1) errorMessage = "не удается открыть/прочитать файл.";
                        else if (result == -2) errorMessage = "не удается записать/создать файл в указанной папке.";
                        else if (result == -3) errorMessage = "неверный формат входного файла.";
                        else errorMessage = "неизвестная ошибка.";

                        string type;
                        if (encode) type = "сжатия";
                        else type = "распаковки";
                        MessageBox.Show("Ошибка " + type + " файла \"" + file + "\": " + errorMessage, "Ошибка");
                    }
                    freeIndexes.Push(index);
                });
            threads.Remove(Thread.CurrentThread);
        }

        private void buttonAbout_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Приложение выполняет сжатие/разжатие файла любого формата " +
                "используя алгоритм Хаффмана. Сжатый файл имеет расширение имени \".dmk\". " +
                "Не рекомендуется использовать больше 4 логических ядер процессора, так как " +
                "скорость работы все равно будет подрезаться скоростью чтения/записи в диск.",
                "Справка");
        }

        private void formHuffmanCompressorForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (threads.Count != 0)
            {
                DialogResult dialogResult = MessageBox.Show(
                    "Выполняются операции, и они будут прерваны при закрытии формы.",
                    "Предупреждение",
                    MessageBoxButtons.YesNoCancel);
                if (dialogResult == DialogResult.Cancel)
                {
                    e.Cancel = true;
                }
                else if (dialogResult == DialogResult.Yes)
                {
                    foreach (Thread thread in threads)
                    {
                        thread.IsBackground = false;
                    }
                }
            }
            
        }
    }
}
