using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;

namespace SortingDemo
{
    public partial class ExternalSortWindow : Window
    {
        public ExternalSortWindow()
        {
            InitializeComponent();
        }

        private void SelectInputFileButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "CSV Files (*.csv)|*.csv|Text Files (*.txt)|*.txt|All Files (*.*)|*.*"
            };
            if (openFileDialog.ShowDialog() == true)
            {
                InputFilePath.Text = openFileDialog.FileName;
            }
        }

        private void SelectOutputFileButton_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "CSV Files (*.csv)|*.csv|Text Files (*.txt)|*.txt",
                DefaultExt = ".csv"
            };
            if (saveFileDialog.ShowDialog() == true)
            {
                OutputFilePath.Text = saveFileDialog.FileName;
            }
        }

        private async void StartSortButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string inputFilePath = InputFilePath.Text;
                string outputFilePath = OutputFilePath.Text;
                string filterColumn = KeyAttributeInput.Text;
                string filterValue = FilterValueInput.Text;
                string sortingMethod = ((ComboBoxItem)SortMethodSelector.SelectedItem).Content.ToString();
                int delay = (int)DelaySlider.Value;

                if (!File.Exists(inputFilePath))
                    throw new Exception("Файл не найден.");

                var table = Table.LoadFromFile(inputFilePath);

                SortingLog.Clear();
                StepLog.Clear();

                Action<string> sortingLog = msg =>
                {
                    Dispatcher.Invoke(() =>
                    {
                        SortingLog.AppendText(msg + Environment.NewLine);
                        SortingLog.ScrollToEnd();
                    });
                };

                Action<string> stepLog = msg =>
                {
                    Dispatcher.Invoke(() =>
                    {
                        StepLog.AppendText(msg + Environment.NewLine);
                        StepLog.ScrollToEnd();
                    });
                };

                await Task.Run(() =>
                {
                    ExternalSort.Sort(table, filterColumn, filterValue, filterColumn, sortingMethod, sortingLog, stepLog, delay);
                    table.SaveToFile(outputFilePath);
                });

                MessageBox.Show("Сортировка завершена!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void BackToMainButton_Click(object sender, RoutedEventArgs e)
        {
            if (Owner != null)
            {
                Owner.Show();
            }
            this.Close();
        }
    }
}
