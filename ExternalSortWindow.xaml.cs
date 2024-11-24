using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace SortingDemo
{
    public partial class ExternalSortWindow : Window
    {
        private string inputFilePath;
        private string outputFilePath;

        public ExternalSortWindow()
        {
            InitializeComponent();
        }

        private void SelectInputFileButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                inputFilePath = openFileDialog.FileName;
                InputFilePath.Text = inputFilePath;
            }
        }

        private void SelectOutputFileButton_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            if (saveFileDialog.ShowDialog() == true)
            {
                outputFilePath = saveFileDialog.FileName;
                OutputFilePath.Text = outputFilePath;
            }
        }

        private async void StartSortButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(KeyIndexInput.Text) || string.IsNullOrWhiteSpace(inputFilePath) || string.IsNullOrWhiteSpace(outputFilePath))
            {
                MessageBox.Show("Заполните все поля и выберите файлы!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!int.TryParse(KeyIndexInput.Text, out int keyIndex) || keyIndex < 0)
            {
                MessageBox.Show("Введите корректный индекс ключевого поля!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            int delay = (int)DelaySlider.Value;

            SortingStepsLog.Clear();
            SortingComparisonLog.Clear();

            try
            {
                string selectedMethod = (SortMethodSelector.SelectedItem as ComboBoxItem)?.Content.ToString();
                switch (selectedMethod)
                {
                    case "Естественное слияние":
                        await ExternalSort.NaturalMergeSort(inputFilePath, outputFilePath, keyIndex, delay, LogStep, LogComparison);
                        MessageBox.Show("Естественное слияние завершено!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                        break;

                    case "Прямое слияние":
                        await ExternalSort.DirectMergeSort(inputFilePath, outputFilePath, keyIndex, delay, LogStep, LogComparison);
                        MessageBox.Show("Прямое слияние завершено!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                        break;

                    case "Многопутевое слияние":
                        await ExternalSort.MultiWayMergeSort(inputFilePath, outputFilePath, keyIndex, delay, LogStep, LogComparison);
                        MessageBox.Show("Многопутевое слияние завершено!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                        break;

                    default:
                        MessageBox.Show("Выберите метод сортировки!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сортировки: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LogStep(List<string> step)
        {
            Dispatcher.Invoke(() =>
            {
                SortingStepsLog.AppendText($"Шаг: {string.Join(", ", step)}\n");
                SortingStepsLog.ScrollToEnd();
            });
        }

        private void LogComparison(string comparison)
        {
            Dispatcher.Invoke(() =>
            {
                SortingComparisonLog.AppendText($"{comparison}\n");
                SortingComparisonLog.ScrollToEnd();
            });
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
