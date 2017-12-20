using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GroupPolicyLab5
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private String _text = "";
        private Dictionary<char, int> _dictionary = new Dictionary<char, int>();
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void buttonFile_Click(object sender, RoutedEventArgs e)
        {
            var fileName = textBoxFile.Text.Trim();
            if (string.IsNullOrEmpty(fileName))
                return;

            await ReadFileAsync();

            CalcNumEntry();

            // Save to new file
            await WriteFileAsync(fileName);
        }

        private async Task ReadFileAsync()
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "Text files (*.txt)|*.txt";
                if (openFileDialog.ShowDialog() == true)
                {
                    using (StreamReader sr = new StreamReader(openFileDialog.FileName))
                    {
                        _text = await sr.ReadToEndAsync();
                    }
                }  
            }
            catch (Exception exc)
            {
                //ResultBlock.Text = "Could not read the file";
            }
        }

        private async Task WriteFileAsync(string fileName)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Text files (*.txt)|*.txt";
            saveFileDialog.DefaultExt = "txt";
            saveFileDialog.AddExtension = true;
            saveFileDialog.FileName = fileName;
            if (saveFileDialog.ShowDialog() == true)
            {
                using (StreamWriter outputFile = new StreamWriter(saveFileDialog.FileName))
                {
                    int number = 0;
                    foreach (var pair in _dictionary)
                    {
                        await outputFile.WriteLineAsync(pair.Key + " - " + pair.Value);
                    }
                    number = _dictionary.Sum(r => r.Value);
                    await outputFile.WriteLineAsync("Total number - " + number);
                }
            }
        }

        private void CalcNumEntry()
        {
            // Read file and calculate entries
            _dictionary.Clear();
            foreach (var s in _text)
            {
                if (!char.IsSeparator(s) && !char.IsWhiteSpace(s) && !char.IsPunctuation(s))
                {
                    var n = _text.Where(r => r == s).Count();
                    if (!_dictionary.Keys.Contains(s))
                    {
                        _dictionary.Add(s, n);
                    }
                }
            }
        }
    }
}
