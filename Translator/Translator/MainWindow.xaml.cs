using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;

namespace Translator
{

    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        List<TextBox> OriginalText = new List<TextBox>();
        List<TextBox> EnglishText = new List<TextBox>();
        List<Button> CopyButton = new List<Button>();
        List<TextBox> TranslatedText = new List<TextBox>();
        List<TextBox> TextAvaiLength = new List<TextBox>();

        string[] readText;
        string[] readTranslatedText;
        bool translation_active;
        Char delimiter = '|';
        string filename;
        string fwname;
        bool ddmm_patch_check;
        string datefn = "";

        /*
                byte[][] fix_bytes = new byte[][] {
                    new byte[] { 0x10, 0x0, 0x33, 0x21 }, new byte[] { 0x10, 0x0, 0x0, 0x21 },
                    new byte[] { 0x10, 0x0, 0x34, 0x21 }, new byte[] { 0x10, 0x0, 0x0, 0x21 },
                    new byte[] { 0x10, 0x0, 0x38, 0x21 }, new byte[] { 0x10, 0x0, 0x0, 0x21 },
                    new byte[] { 0x10, 0x0, 0x39, 0x21 }, new byte[] { 0x10, 0x0, 0x0, 0x21 },
                    new byte[] { 0x10, 0x0, 0x40, 0x21 }, new byte[] { 0x10, 0x0, 0x0, 0x21 },
                    new byte[] { 0x10, 0x0, 0x41, 0x21 }, new byte[] { 0x10, 0x0, 0x0, 0x21 },
                    new byte[] { 0x10, 0x0, 0x42, 0x21 }, new byte[] { 0x10, 0x0, 0x0, 0x21 },
                    new byte[] { 0x10, 0x0, 0x43, 0x21 }, new byte[] { 0x10, 0x0, 0x0, 0x21 },
                    new byte[] { 0x6A, 0x22, 0x39, 0x21 }, new byte[] { 0x6A, 0x22, 0x0, 0x21 },
                    new byte[] { 0x6A, 0x22, 0x42, 0x21 }, new byte[] { 0x6A, 0x22, 0x0, 0x21 },
                     };
        */
        byte[][] fix_bytes = new byte[][] {
            new byte[] { 0x03, 0xD3, 0x20, 0x78 }, new byte[] {  0x02, 0xE0, 0x20, 0x78 }, // eng forcer
            new byte[] { 0x03, 0xD1, 0x20, 0x78 }, new byte[] {  0x02, 0xE0, 0x20, 0x78 }, // eng forcer v0.1.0.80
            new byte[] { 0x03, 0xD1, 0x28, 0x78 }, new byte[] {  0x02, 0xE0, 0x28, 0x78 }, // eng forcer v0.1.0.87

            new byte[] { 0x6A, 0x22, 0x0F, 0x21 }, new byte[] {  0x6A, 0x22, 0x0, 0x21 },
            new byte[] { 0x6A, 0x22, 0x13, 0x21 }, new byte[] {  0x6A, 0x22, 0x0, 0x21 },
            new byte[] { 0x6A, 0x22, 0x2A, 0x21 }, new byte[] {  0x6A, 0x22, 0x0, 0x21 },
            new byte[] { 0x6A, 0x22, 0x34, 0x21 }, new byte[] {  0x6A, 0x22, 0x0, 0x21 },
            new byte[] { 0x6A, 0x22, 0x37, 0x21 }, new byte[] {  0x6A, 0x22, 0x0, 0x21 },
            new byte[] { 0x6A, 0x22, 0x3B, 0x21 }, new byte[] {  0x6A, 0x22, 0x0, 0x21 },
            new byte[] { 0x6E, 0x22, 0x29, 0x21 }, new byte[] {  0x6E, 0x22, 0x0, 0x21 },

            new byte[] { 0x7A, 0x22, 0x28, 0x21 }, new byte[] {  0x7A, 0x22, 0x0, 0x21 },
            new byte[] { 0x7A, 0x22, 0x2A, 0x21 }, new byte[] {  0x7A, 0x22, 0x0, 0x21 },
            new byte[] { 0x7A, 0x22, 0x2B, 0x21 }, new byte[] {  0x7A, 0x22, 0x0, 0x21 },
            new byte[] { 0x7A, 0x22, 0x2C, 0x21 }, new byte[] {  0x7A, 0x22, 0x0, 0x21 },
            new byte[] { 0x7A, 0x22, 0x35, 0x21 }, new byte[] {  0x7A, 0x22, 0x0, 0x21 },
            new byte[] { 0x7A, 0x22, 0x38, 0x21 }, new byte[] {  0x7A, 0x22, 0x0, 0x21 },
            new byte[] { 0x7A, 0x22, 0x39, 0x21 }, new byte[] {  0x7A, 0x22, 0x0, 0x21 },
            //new byte[] { 0x78, 0x21, 0xF2, 0xE7, 0x7A}, new byte[] {0x72, 0x21, 0xF2, 0xE7, 0x7A},
             };

        byte[][] fix_date = new byte[][] {
            new byte[] {0xA8, 0x78, 0x0A, 0x21, 0x6A}, new byte[] {0xE8, 0x78, 0x0A, 0x21, 0x6A},
            new byte[] {0xA8, 0x78, 0xB0, 0xFB, 0xF1}, new byte[] {0xE8, 0x78, 0xB0, 0xFB, 0xF1},
            new byte[] {0xEB, 0x78, 0x40, 0x1C, 0x0A}, new byte[] {0xAB, 0x78, 0x40, 0x1C, 0x0A},
            new byte[] {0xEB, 0x78, 0x40, 0x1C, 0xB3}, new byte[] {0xAB, 0x78, 0x40, 0x1C, 0xB3},
            new byte[] {0xA8, 0x7D, 0x0A, 0x21, 0xB0}, new byte[] {0xE8, 0x7D, 0x0A, 0x21, 0xB0},
            new byte[] {0xEB, 0x7D, 0x40, 0x1C, 0x0A}, new byte[] {0xAB, 0x7D, 0x40, 0x1C, 0x0A},
            new byte[] {0xEB, 0x7D, 0x40, 0x1C, 0xB3}, new byte[] {0xAB, 0x7D, 0x40, 0x1C, 0xB3},
            new byte[] {0xE3, 0x78, 0xA2, 0x78, 0x19}, new byte[] {0xE2, 0x78, 0xA2, 0x78, 0x19},
            new byte[] {0xA2, 0x78, 0x19, 0xA1, 0x68}, new byte[] {0xA3, 0x78, 0x19, 0xA1, 0x68},
            new byte[] {0xE3, 0x7D, 0xA2, 0x7D, 0x2F}, new byte[] {0xE2, 0x7D, 0xA2, 0x7D, 0x2F},
            new byte[] {0xA2, 0x7D, 0x2F, 0xA1, 0x05}, new byte[] {0xA3, 0x7D, 0x2F, 0xA1, 0x05},
            new byte[] {0xE3, 0x78, 0xA2, 0x78, 0x44}, new byte[] {0xA3, 0x78, 0xA2, 0x78, 0x44},
            new byte[] {0xA2, 0x78, 0x44, 0xA1, 0x03}, new byte[] {0xE2, 0x78, 0x44, 0xA1, 0x03},
            new byte[] {0xE3, 0x78, 0xA2, 0x78, 0x33}, new byte[] {0xA3, 0x78, 0xA2, 0x78, 0x33},
            new byte[] {0xA2, 0x78, 0x33, 0xA1, 0x03}, new byte[] {0xE2, 0x78, 0x33, 0xA1, 0x03},
             };


        #region BitTools

        readonly int[] Empty = new int[0];
        private System.IO.StreamWriter logfile;

        public byte[] ToByteArray(String HexString)
        {
            string Test = HexString.Replace(" ", "");
            int NumberChars = Test.Length;
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
            {
                bytes[i / 2] = Convert.ToByte(Test.Substring(i, 2), 16);
            }
            return bytes;
        }

        private int[] Locate(byte[] self, byte[] candidate)
        {
            if (IsEmptyLocate(self, candidate))
                return Empty;

            var list = new List<int>();

            for (int i = 0; i < self.Length; i++)
            {
                if (!IsMatch(self, i, candidate))
                    continue;

                list.Add(i);
            }

            return list.Count == 0 ? Empty : list.ToArray();
        }

        private bool IsMatch(byte[] array, int position, byte[] candidate)
        {
            if (candidate.Length > (array.Length - position))
                return false;

            for (int i = 0; i < candidate.Length; i++)
                if (array[position + i] != candidate[i])
                    return false;

            return true;
        }

        private bool IsEmptyLocate(byte[] array, byte[] candidate)
        {
            return array == null
                || candidate == null
                || array.Length == 0
                || candidate.Length == 0
                || candidate.Length > array.Length;
        }
        #endregion

        public MainWindow()
        {
            InitializeComponent();

            if (File.Exists("log.txt"))
                File.Delete("log.txt");
            logfile = new System.IO.StreamWriter("log.txt");
        }

        private void CopyButton_Clicked(object sender, RoutedEventArgs e)
        {
            int x = CopyButton.IndexOf((Button)sender);
            TranslatedText[x].Text = EnglishText[x].Text;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            for (int x = 0; x < TranslatedText.Count; x++)
            {
                String[] substrings = readText[x + 1].Split(delimiter);
                while ((((substrings[1].Length + 1) / 3) % 4) != 3)
                    substrings[1] = substrings[1] + " 00";

                readText[x + 1] = substrings[0];
                for (int y = 1; y < substrings.Length - 1; y++)
                {
                    readText[x + 1] += "|" + substrings[y];
                }
                readText[x + 1] += "|" + TranslatedText[x].Text;
            }
            File.WriteAllLines(Path.GetDirectoryName(filename) + @"\en2" + Language.Text + ".txt", readText);
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            int x = TranslatedText.IndexOf((TextBox)sender);
            String[] substrings = readText[x + 1].Split(delimiter);
            int y = (((substrings[1].Length + 1) / 3) % 4);
            y = 3 - y;
            y += (substrings[1].Length + 1) / 3;

            byte[] utf8 = System.Text.Encoding.UTF8.GetBytes(((TextBox)sender).Text);
            int length = utf8.Length - y;
            TextAvaiLength[x].Text = (-length).ToString();

            if (((TextBox)sender).Text.Length > 0 && TranslatedText[x].Background != Brushes.White)
                TranslatedText[x].Background = Brushes.White;
            if (((TextBox)sender).Text.Length == 0)
                TranslatedText[x].Background = Brushes.Red;

            if (length > 0)
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    TranslatedText[x].Text = TranslatedText[x].Text.Remove(TranslatedText[x].Text.Length - 1);
                    TranslatedText[x].SelectionStart = TranslatedText[x].Text.Length; // add some logic if length is 0
                    TranslatedText[x].SelectionLength = 0;
                }));
            }
        }

        private void OpenButton_Click(object sender, RoutedEventArgs e)
        {
            // Configure open file dialog box
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.FileName = "Document"; // Default file name
            dlg.DefaultExt = ".txt"; // Default file extension
            dlg.Filter = "Text documents (.txt)|*.txt"; // Filter files by extension

            // Show open file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process open file dialog box results
            if (result == true)
            {
                // Open document
                filename = dlg.FileName;


                readText = File.ReadAllLines(filename);
                int y;
                int z = 0;
                translation_active = File.Exists(Path.GetDirectoryName(filename) + @"\en2" + Language.Text + ".txt");
                if (translation_active)
                {
                    readTranslatedText = File.ReadAllLines(Path.GetDirectoryName(filename) + @"\en2" + Language.Text + ".txt");
                }
                else
                    readTranslatedText = readText;


                for (int x = 1; x < readText.Length; x++)
                {
                    String[] substrings1 = readText[x].Split(delimiter);
                    String[] substrings2;
                    if (readTranslatedText.Length > x - z)
                        substrings2 = readTranslatedText[x - z].Split(delimiter);
                    else
                        substrings2 = new string[4];

                    bool translation_active2 = substrings1[1] == substrings2[1];
                    if (!translation_active2) z++;

                    // Toleranz korrigieren
                    y = (((substrings1[1].Length + 1) / 3) % 4);
                    y = 3 - y;
                    y += (substrings1[1].Length + 1) / 3;

                    OriginalText.Add(new TextBox() { Text = substrings1[2], Height = 24, IsReadOnly = true, FontSize = 16 });
                    EnglishText.Add(new TextBox() { Text = substrings1[3], Height = 24, IsReadOnly = true, FontSize = 16 });
                    CopyButton.Add(new Button() { Height = 24, Content = "->", FontSize = 16 });
                    CopyButton[x - 1].Click += CopyButton_Clicked;

                    if (translation_active && translation_active2)
                    {
                        if (substrings2[3].Length > 0)
                            TranslatedText.Add(new TextBox() { Text = substrings2[3], Height = 24, MaxLength = y, FontSize = 16 });
                        else
                            TranslatedText.Add(new TextBox() { Text = substrings2[3], Height = 24, MaxLength = y, FontSize = 16, Background = Brushes.Red });
                        TextAvaiLength.Add(new TextBox() { Text = (y - System.Text.Encoding.UTF8.GetBytes(substrings2[3]).Length).ToString(), Height = 24, IsReadOnly = true, FontSize = 16 });
                    }
                    else
                    {
                        if (translation_active)
                            TranslatedText.Add(new TextBox() { Height = 24, MaxLength = y, FontSize = 16, Background = Brushes.Red });
                        else
                            TranslatedText.Add(new TextBox() { Height = 24, MaxLength = y, FontSize = 16 });
                        TextAvaiLength.Add(new TextBox() { Text = y.ToString(), Height = 24, IsReadOnly = true, FontSize = 16 });
                    }
                    TranslatedText[x - 1].TextChanged += TextBox_TextChanged;

                    Original.Children.Add(OriginalText[x - 1]);
                    English.Children.Add(EnglishText[x - 1]);
                    CopyButtons.Children.Add(CopyButton[x - 1]);
                    Translated.Children.Add(TranslatedText[x - 1]);
                    LengthTextR.Children.Add(TextAvaiLength[x - 1]);

                    OpenBtn.IsEnabled = false;
                    SaveBtn.IsEnabled = true;
                    PatchFirmwareBtn.IsEnabled = true;
                }
            }
        }

        private void Language_TextChanged(object sender, TextChangedEventArgs e)
        {
            OpenBtn.IsEnabled = Language.Text.Length == 2;
        }

        public void DoWork()
        {
            byte[] data = File.ReadAllBytes(fwname);
            for (int x = 0; x < TranslatedText.Count; x++)
            {
                string curtranslation = "";
                String[] substrings = readText[x + 1].Split(delimiter);

                byte[] pattern = new byte[((substrings[1].Length + 1) / 3) + 2];
                ToByteArray(substrings[1]).CopyTo(pattern, 1);
                byte[] translated = new byte[pattern.Length];
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    curtranslation = TranslatedText[x].Text;
                    Encoding.UTF8.GetBytes(curtranslation).CopyTo(translated, 1);
                }));

                foreach (var position in Locate(data, pattern))
                {
                    if ((position + 1) % 4 == 0)
                    {
                        Log("Patching from " + substrings[2] + " to " + curtranslation + " at position " + (position + 1).ToString("X4"));
                        translated.CopyTo(data, position);
                    }
                }
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    Progress.Value++;
                }));

            }

            for (int x = 0; x < TranslatedText.Count; x++)
            {
                string curtranslation = "";
                String[] substrings = readText[x + 1].Split(delimiter);
                if (substrings[2].Length > 2)
                {
                    byte[] pattern = new byte[((substrings[1].Length + 1) / 3) + 1];
                    ToByteArray(substrings[1]).CopyTo(pattern, 0);
                    byte[] translated = new byte[pattern.Length];
                    Dispatcher.BeginInvoke(new Action(() =>
                    {
                        curtranslation = TranslatedText[x].Text;
                        Encoding.UTF8.GetBytes(curtranslation).CopyTo(translated, 0);
                    }));

                    foreach (var position in Locate(data, pattern))
                    {
                        if (position % 4 == 0)
                        {
                            Log("Patching from " + substrings[2] + " to " + curtranslation + " at position " + position.ToString("X4"));
                            translated.CopyTo(data, position);
                        }
                    }
                }
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    Progress.Value++;
                }));
            }

            for (int x = 0; x < fix_bytes.Length; x = x + 2)
            {

                foreach (var position in Locate(data, fix_bytes[x]))
                {
                    fix_bytes[x + 1].CopyTo(data, position);
                }
            }

            if (ddmm_patch_check == true)
            {
                for (int x = 0; x < fix_date.Length; x = x + 2)
                {

                    foreach (var position in Locate(data, fix_date[x]))
                    {
                        fix_date[x + 1].CopyTo(data, position);
                    }
                }
            }
            Dispatcher.BeginInvoke(new Action(() =>
            {
                File.WriteAllBytes(Path.GetDirectoryName(fwname) + @"\" + Path.GetFileNameWithoutExtension(fwname) + "_" + Language.Text + datefn + ".fw", data);
                SaveBtn.IsEnabled = true;
                PatchFirmwareBtn.IsEnabled = true;
                ddmm_patch.IsEnabled = true;
            }));
        }

        private void PatchFirmwareBtn_Click(object sender, RoutedEventArgs e)
        {
            SaveBtn.IsEnabled = false;
            PatchFirmwareBtn.IsEnabled = false;
            ddmm_patch.IsEnabled = false;

            ddmm_patch_check = ddmm_patch.IsChecked == true;

            if (ddmm_patch_check) datefn = "_dmpatch"; else datefn = "";

            // Configure open file dialog box
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.FileName = "Firmware"; // Default file name
            dlg.DefaultExt = ".fw"; // Default file extension
            dlg.Filter = "Firmware files (.fw)|*.fw"; // Filter files by extension

            // Show open file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process open file dialog box results
            if (result == true)
            {
                Progress.Maximum = TranslatedText.Count * 2;
                Progress.Value = 0;

                // Open document
                fwname = dlg.FileName;

                Thread workerThread = new Thread(DoWork);
                workerThread.Start();
            }
            else
            {
                SaveBtn.IsEnabled = true;
                PatchFirmwareBtn.IsEnabled = true;
                ddmm_patch.IsEnabled = true;
            }
        }

        public void Log(string message)
        {
            logfile.WriteLine("[" + DateTime.Now + "] " + message);
        }
    }
}
