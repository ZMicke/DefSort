using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public static class SortingAlgorithms
{
    // Bubble Sort
    public static async Task<List<string>> BubbleSort(int[] array, Action<int[], int, int> logStep, int delay)
    {
        var log = new List<string>();
        for (int i = 0; i < array.Length - 1; i++)
        {
            for (int j = 0; j < array.Length - i - 1; j++)
            {
                log.Add($"Сравнение: {array[j]} и {array[j + 1]}");
                logStep(array, j, j + 1);
                await Task.Delay(delay);

                if (array[j] > array[j + 1])
                {
                    log.Add($"Перестановка: {array[j]} и {array[j + 1]}");
                    (array[j], array[j + 1]) = (array[j + 1], array[j]);
                }

                logStep(array, j, j + 1);
                await Task.Delay(delay);
            }
        }

        logStep(array, -1, -1); // Финальная визуализация
        return log;
    }

    // Selection Sort
    public static async Task<List<string>> SelectSort(int[] array, Action<int[], int, int> logStep, int delay)
    {
        var log = new List<string>();
        for (int i = 0; i < array.Length - 1; i++)
        {
            int minIndex = i;
            for (int j = i + 1; j < array.Length; j++)
            {
                log.Add($"Сравнение: {array[j]} и {array[minIndex]}");
                logStep(array, j, minIndex);
                await Task.Delay(delay);

                if (array[j] < array[minIndex])
                {
                    minIndex = j;
                }
            }

            if (minIndex != i)
            {
                log.Add($"Перестановка: {array[i]} и {array[minIndex]}");
                (array[i], array[minIndex]) = (array[minIndex], array[i]);
            }

            logStep(array, i, minIndex); // Визуализация после перестановки
            await Task.Delay(delay);
        }

        logStep(array, -1, -1); // Финальная визуализация
        return log;
    }

    // Quick Sort
    public static async Task<List<string>> QuickSort(int[] array, int low, int high, Action<int[], int, int> logStep, int delay)
    {
        var log = new List<string>();
        if (low < high)
        {
            int pivotIndex = await Partition(array, low, high, log, logStep, delay);

            var leftLog = await QuickSort(array, low, pivotIndex - 1, logStep, delay);
            log.AddRange(leftLog);

            var rightLog = await QuickSort(array, pivotIndex + 1, high, logStep, delay);
            log.AddRange(rightLog);
        }

        logStep(array, -1, -1); // Финальная визуализация
        return log;
    }

    private static async Task<int> Partition(int[] array, int low, int high, List<string> log, Action<int[], int, int> logStep, int delay)
    {
        int pivot = array[high];
        int i = low - 1;

        for (int j = low; j < high; j++)
        {
            log.Add($"Сравнение: {array[j]} и {pivot}");
            if (array[j] <= pivot)
            {
                i++;
                log.Add($"Перестановка: {array[i]} и {array[j]}");
                (array[i], array[j]) = (array[j], array[i]);
            }

            logStep(array, i, j);
            await Task.Delay(delay);
        }

        log.Add($"Перестановка опорного элемента: {array[i + 1]} и {array[high]}");
        (array[i + 1], array[high]) = (array[high], array[i + 1]);
        logStep(array, i + 1, high);
        await Task.Delay(delay);

        return i + 1;
    }

    // Heap Sort
    public static async Task<List<string>> HeapSort(int[] array, Action<int[], int, int> logStep, int delay)
    {
        var log = new List<string>();
        int n = array.Length;

        for (int i = n / 2 - 1; i >= 0; i--)
        {
            await Heapify(array, n, i, log, logStep, delay);
        }

        for (int i = n - 1; i > 0; i--)
        {
            log.Add($"Перестановка корня: {array[0]} и {array[i]}");
            (array[0], array[i]) = (array[i], array[0]);

            logStep(array, 0, i);
            await Task.Delay(delay);

            await Heapify(array, i, 0, log, logStep, delay);
        }

        logStep(array, -1, -1); // Финальная визуализация
        return log;
    }

    private static async Task Heapify(int[] array, int n, int i, List<string> log, Action<int[], int, int> logStep, int delay)
    {
        int largest = i;
        int left = 2 * i + 1;
        int right = 2 * i + 2;

        if (left < n && array[left] > array[largest])
        {
            log.Add($"Сравнение: {array[left]} и {array[largest]}");
            largest = left;
        }

        if (right < n && array[right] > array[largest])
        {
            log.Add($"Сравнение: {array[right]} и {array[largest]}");
            largest = right;
        }

        if (largest != i)
        {
            log.Add($"Перестановка: {array[i]} и {array[largest]}");
            (array[i], array[largest]) = (array[largest], array[i]);

            logStep(array, i, largest);
            await Task.Delay(delay);

            await Heapify(array, n, largest, log, logStep, delay);
        }
    }
}
