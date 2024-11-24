using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace SortingDemo
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void StartButton_Click(object sender, RoutedEventArgs e)
        {
            if (AlgorithmSelector.SelectedItem == null || string.IsNullOrWhiteSpace(InputArray.Text))
            {
                MessageBox.Show("Выберите алгоритм и введите массив чисел!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string selectedAlgorithm = (AlgorithmSelector.SelectedItem as ComboBoxItem).Content.ToString();
            int[] array;

            try
            {
                array = InputArray.Text.Split(' ').Select(int.Parse).ToArray();
            }
            catch (FormatException)
            {
                MessageBox.Show("Некорректный ввод массива! Введите числа через пробел.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            int delay = (int)DelaySlider.Value;

            // Очистка вывода
            SortingCanvas.Children.Clear();
            SortingLog.Clear();

            // Построение визуальных элементов массива
            DrawArray(array);

            try
            {
                if (selectedAlgorithm == "SelectSort")
                {
                    await SortingAlgorithms.SelectSort(array, VisualizeStep, delay);
                }
                else if (selectedAlgorithm == "BubbleSort")
                {
                    await SortingAlgorithms.BubbleSort(array, VisualizeStep, delay);
                }
                else if (selectedAlgorithm == "QuickSort")
                {
                    await SortingAlgorithms.QuickSort(array, 0, array.Length - 1, VisualizeStep, delay);
                }
                else if (selectedAlgorithm == "HeapSort")
                {
                    await SortingAlgorithms.HeapSort(array, VisualizeStep, delay);
                }

                MessageBox.Show("Сортировка завершена!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сортировки: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DrawArray(int[] array)
        {
            SortingCanvas.Children.Clear(); // Очищаем Canvas перед рисованием
            double barWidth = SortingCanvas.ActualWidth / array.Length; // Ширина одного столбца

            for (int i = 0; i < array.Length; i++)
            {
                // Создаём столбец
                var bar = new Rectangle
                {
                    Width = barWidth - 2, // Небольшой отступ между столбцами
                    Height = array[i] * 5, // Высота столбца пропорциональна значению
                    Fill = System.Windows.Media.Brushes.Blue // Цвет столбца
                };

                // Устанавливаем позицию столбца
                Canvas.SetLeft(bar, i * barWidth);
                Canvas.SetBottom(bar, 20); // Оставляем место для подписи
                SortingCanvas.Children.Add(bar);

                // Создаём подпись
                var text = new TextBlock
                {
                    Text = array[i].ToString(), // Значение элемента массива
                    FontSize = 14,
                    TextAlignment = TextAlignment.Center
                };

                // Центрируем подпись относительно столбца
                Canvas.SetLeft(text, i * barWidth + (barWidth - text.ActualWidth) / 2);
                Canvas.SetBottom(text, 0); // Позиция текста (внизу Canvas)
                SortingCanvas.Children.Add(text);
            }
        }


        private void VisualizeStep(int[] array, int index1, int index2)
        {
            SortingCanvas.Children.Clear(); // Очистка Canvas перед обновлением

            double barWidth = SortingCanvas.ActualWidth / array.Length;

            for (int i = 0; i < array.Length; i++)
            {
                // Столбец
                var bar = new Rectangle
                {
                    Width = barWidth - 2,
                    Height = array[i] * 5,
                    Fill = i == index1 || i == index2
                        ? System.Windows.Media.Brushes.Red // Подсветка сравниваемых столбцов
                        : System.Windows.Media.Brushes.Blue // Обычный цвет
                };

                Canvas.SetLeft(bar, i * barWidth);
                Canvas.SetBottom(bar, 20); // Оставляем место для подписи
                SortingCanvas.Children.Add(bar);

                // Подпись
                var text = new TextBlock
                {
                    Text = array[i].ToString(),
                    FontSize = 14,
                    TextAlignment = TextAlignment.Center
                };

                // Центрируем подпись
                Canvas.SetLeft(text, i * barWidth + (barWidth - text.ActualWidth) / 2);
                Canvas.SetBottom(text, 0); // Текст внизу
                SortingCanvas.Children.Add(text);
            }

            // Логирование текущего шага
            SortingLog.AppendText($"[{string.Join(", ", array)}] (Сравнение: {index1}, {index2})\n");
            SortingLog.ScrollToEnd();
        }
        private void OpenExternalSortWindowButton_Click(object sender, RoutedEventArgs e)
        {
            var externalSortWindow = new ExternalSortWindow
            {
                Owner = this // Устанавливаем текущее окно как владельца нового
            };
            externalSortWindow.Show();
            this.Hide(); // Скрываем текущее окно
        }
        private void OpenTextSortingWindowButton_Click(object sender, RoutedEventArgs e)
        {
            var textSortingWindow = new TextSortingWindow
            {
                Owner = this
            };
            this.Hide(); // Скрываем текущее окно
            textSortingWindow.Show(); // Открываем новое окно
        }

    }
}
