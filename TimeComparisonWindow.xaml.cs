using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace SortingDemo
{
    public partial class TimeComparisonWindow : Window
    {
        public TimeComparisonWindow()
        {
            InitializeComponent();
            UpdateLog();
        }

        // Метод для добавления результата сортировки
        public void AddSortingResult(SortingResult result)
        {
            ComparisonData.Results.Add(result);
            UpdateLog();
        }

        // Метод для обновления логов
        private void UpdateLog()
        {
            ProcessingTimeLog.Clear();
            if (ComparisonData.Results.Any())
            {
                foreach (var result in ComparisonData.Results)
                {
                    ProcessingTimeLog.AppendText(result.ToString() + "\n");
                }
            }
            else
            {
                ProcessingTimeLog.AppendText("Результаты пока отсутствуют.\n");
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }

    // Статический класс для хранения данных о сравнениях
    public static class ComparisonData
    {
        public static List<SortingResult> Results { get; } = new List<SortingResult>();
    }

    // Класс для хранения данных о результатах сортировки
    public class SortingResult
    {
        public string Method { get; set; } // Название алгоритма
        public int WordCount { get; set; }    // Количество слов в файле
        public long Time { get; set; }        // Время выполнения (в миллисекундах)

        public override string ToString()
        {
            return $"Алгоритм: {Method}, Количество слов: {WordCount}, Время: {Time} мс";
        }
    }
}
