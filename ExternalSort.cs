using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

public static class ExternalSort
{
    public static async Task NaturalMergeSort(
        string inputFile,
        string outputFile,
        int keyIndex,
        int delay,
        Action<List<string>> logStep,
        Action<string> logComparison)
    {
        var data = ReadFile(inputFile);
        ValidateData(data, keyIndex);

        while (true)
        {
            var series = SplitIntoSeries(data, keyIndex);

            if (series.Count == 1)
            {
                WriteFile(outputFile, series[0]);
                break;
            }

            data = await MergeSeries(series, keyIndex, delay, logStep, logComparison);
        }
    }

    public static async Task DirectMergeSort(
        string inputFile,
        string outputFile,
        int keyIndex,
        int delay,
        Action<List<string>> logStep,
        Action<string> logComparison)
    {
        var data = ReadFile(inputFile);
        ValidateData(data, keyIndex);

        var tempFiles = CreateTemporaryFiles(data, keyIndex);

        while (tempFiles.Count > 1)
        {
            var mergedFiles = new List<string>();

            for (int i = 0; i < tempFiles.Count; i += 2)
            {
                if (i + 1 < tempFiles.Count)
                {
                    var mergedFile = MergeTwoFiles(tempFiles[i], tempFiles[i + 1], keyIndex, delay, logStep, logComparison);
                    mergedFiles.Add(mergedFile);
                }
                else
                {
                    mergedFiles.Add(tempFiles[i]);
                }
            }

            tempFiles = mergedFiles;
        }

        File.Copy(tempFiles[0], outputFile, true);
    }

    public static async Task MultiWayMergeSort(
        string inputFile,
        string outputFile,
        int keyIndex,
        int delay,
        Action<List<string>> logStep,
        Action<string> logComparison)
    {
        var data = ReadFile(inputFile);
        ValidateData(data, keyIndex);

        var tempFiles = CreateTemporaryFiles(data, keyIndex);

        var finalResult = MultiWayMerge(tempFiles, keyIndex, delay, logStep, logComparison);
        WriteFile(outputFile, finalResult);
    }

    private static List<List<string>> SplitIntoSeries(List<string> data, int keyIndex)
    {
        var series = new List<List<string>>();
        var currentSeries = new List<string> { data[0] };

        for (int i = 1; i < data.Count; i++)
        {
            if (IsKeyLess(data[i], data[i - 1], keyIndex))
            {
                series.Add(currentSeries);
                currentSeries = new List<string>();
            }

            currentSeries.Add(data[i]);
        }

        if (currentSeries.Count > 0)
            series.Add(currentSeries);

        return series;
    }

    private static async Task<List<string>> MergeSeries(
        List<List<string>> series,
        int keyIndex,
        int delay,
        Action<List<string>> logStep,
        Action<string> logComparison)
    {
        var result = new List<string>();
        var enumerators = series.Select(s => s.GetEnumerator()).ToList();
        var currentElements = enumerators.Select(e => e.MoveNext() ? e.Current : null).ToList();

        while (currentElements.Any(e => e != null))
        {
            int minIndex = FindMinIndex(currentElements, keyIndex);

            if (minIndex != -1)
            {
                result.Add(currentElements[minIndex]);
                logComparison?.Invoke($"Добавлено: {currentElements[minIndex]}");

                if (!enumerators[minIndex].MoveNext())
                    currentElements[minIndex] = null;
                else
                    currentElements[minIndex] = enumerators[minIndex].Current;

                logStep?.Invoke(new List<string>(result));
                await Task.Delay(delay);
            }
        }

        return result;
    }

    private static string MergeTwoFiles(
        string file1,
        string file2,
        int keyIndex,
        int delay,
        Action<List<string>> logStep,
        Action<string> logComparison)
    {
        var lines1 = File.ReadLines(file1).GetEnumerator();
        var lines2 = File.ReadLines(file2).GetEnumerator();

        var merged = new List<string>();
        var fileName = Path.GetTempFileName();

        bool hasMore1 = lines1.MoveNext();
        bool hasMore2 = lines2.MoveNext();

        while (hasMore1 || hasMore2)
        {
            if (!hasMore1)
            {
                merged.Add(lines2.Current);
                logStep?.Invoke(new List<string>(merged));
                hasMore2 = lines2.MoveNext();
            }
            else if (!hasMore2)
            {
                merged.Add(lines1.Current);
                logStep?.Invoke(new List<string>(merged));
                hasMore1 = lines1.MoveNext();
            }
            else if (IsKeyLess(lines1.Current, lines2.Current, keyIndex))
            {
                merged.Add(lines1.Current);
                logComparison?.Invoke($"Сравнение: {lines1.Current} < {lines2.Current}");
                hasMore1 = lines1.MoveNext();
            }
            else
            {
                merged.Add(lines2.Current);
                logComparison?.Invoke($"Сравнение: {lines1.Current} >= {lines2.Current}");
                hasMore2 = lines2.MoveNext();
            }
        }

        File.WriteAllLines(fileName, merged);
        return fileName;
    }

    private static List<string> MultiWayMerge(
        List<string> tempFiles,
        int keyIndex,
        int delay,
        Action<List<string>> logStep,
        Action<string> logComparison)
    {
        var enumerators = tempFiles.Select(file => File.ReadLines(file).GetEnumerator()).ToList();
        var currentElements = enumerators.Select(e => e.MoveNext() ? e.Current : null).ToList();
        var result = new List<string>();

        while (currentElements.Any(e => e != null))
        {
            int minIndex = FindMinIndex(currentElements, keyIndex);

            if (minIndex != -1)
            {
                result.Add(currentElements[minIndex]);
                logComparison?.Invoke($"Добавлено: {currentElements[minIndex]}");

                if (!enumerators[minIndex].MoveNext())
                    currentElements[minIndex] = null;
                else
                    currentElements[minIndex] = enumerators[minIndex].Current;

                logStep?.Invoke(new List<string>(result));
                Task.Delay(delay).Wait();
            }
        }

        return result;
    }

    private static int FindMinIndex(List<string> currentElements, int keyIndex)
    {
        string minValue = null;
        int minIndex = -1;

        for (int i = 0; i < currentElements.Count; i++)
        {
            if (currentElements[i] == null) continue;

            var currentKey = currentElements[i].Split(',')[keyIndex];

            if (minValue == null || IsKeyLess(currentElements[i], minValue, keyIndex))
            {
                minValue = currentKey;
                minIndex = i;
            }
        }

        return minIndex;
    }

    private static bool IsKeyLess(string current, string previous, int keyIndex)
    {
        var currentKey = current.Split(',')[keyIndex];
        var previousKey = previous.Split(',')[keyIndex];

        if (double.TryParse(currentKey, out var currentNum) && double.TryParse(previousKey, out var previousNum))
        {
            return currentNum < previousNum;
        }

        return string.Compare(currentKey, previousKey, StringComparison.Ordinal) < 0;
    }

    private static List<string> ReadFile(string filePath)
    {
        if (!File.Exists(filePath))
            throw new FileNotFoundException($"Файл {filePath} не найден.");

        return File.ReadAllLines(filePath).Where(line => !string.IsNullOrWhiteSpace(line)).ToList();
    }

    private static void ValidateData(List<string> data, int keyIndex)
    {
        if (data.Count == 0)
            throw new Exception("Файл пуст. Нечего сортировать.");

        var columnCount = data[0].Split(',').Length;
        if (keyIndex < 0 || keyIndex >= columnCount)
            throw new Exception("Индекс ключевого поля выходит за границы таблицы.");
    }

    private static void WriteFile(string filePath, List<string> data)
    {
        File.WriteAllLines(filePath, data);
    }

    private static List<string> CreateTemporaryFiles(List<string> data, int keyIndex)
    {
        var tempFiles = new List<string>();
        var chunkSize = data.Count / 2;

        for (int i = 0; i < data.Count; i += chunkSize)
        {
            var chunk = data.Skip(i).Take(chunkSize).ToList();
            var tempFile = Path.GetTempFileName();
            WriteFile(tempFile, chunk);
            tempFiles.Add(tempFile);
        }

        return tempFiles;
    }
}
