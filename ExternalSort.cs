namespace SortingDemo
{
    public static class ExternalSort
    {
        public static void Sort(
            Table table,
            string filterColumn,
            string filterValue,
            string sortKey,
            string sortingMethod,
            Action<string> log,
            Action<string> stepLog,
            int delay)
        {
            log("Фильтрация данных...");
            var filteredRows = table.Rows.Where(row =>
            {
                var value = row[filterColumn];

                // Если фильтрация по числу
                if (int.TryParse(value, out var numericValue) && int.TryParse(filterValue, out var numericFilter))
                {
                    return numericValue == numericFilter; // Равно числу
                }

                // Если фильтрация по строке
                return value.Equals(filterValue, StringComparison.OrdinalIgnoreCase);
            }).ToList();
            if (filterColumn == "Площадь" && filterValue.Contains("-"))
            {
                var range = filterValue.Split('-').Select(int.Parse).ToArray();
                var min = range[0];
                var max = range[1];

                filteredRows = table.Rows.Where(row =>
                {
                    var value = int.Parse(row["Площадь"]);
                    return value >= min && value <= max;
                }).ToList();
            }


            if (!filteredRows.Any())
            {
                log($"Нет данных, соответствующих условию фильтрации: '{filterValue}'");
                return;
            }

            log($"Начало сортировки методом '{sortingMethod}' по ключу '{sortKey}'...");

            List<Dictionary<string, string>> sortedRows = sortingMethod switch
            {
                "Прямое слияние" => DirectMergeSort(filteredRows, sortKey, log, stepLog, delay),
                "Естественное слияние" => NaturalMergeSort(filteredRows, sortKey, log, stepLog, delay),
                "Многопутевое слияние" => MultiwayMergeSort(filteredRows, sortKey, log, stepLog, delay),
                _ => throw new ArgumentException($"Неизвестный метод сортировки: {sortingMethod}")
            };

            table.Rows = sortedRows;
            log("Сортировка завершена.");
        }

        private static List<Dictionary<string, string>> DirectMergeSort(
            List<Dictionary<string, string>> rows,
            string sortKey,
            Action<string> log,
            Action<string> stepLog,
            int delay)
        {
            if (rows.Count <= 1)
                return rows;

            int mid = rows.Count / 2;
            var left = DirectMergeSort(rows.Take(mid).ToList(), sortKey, log, stepLog, delay);
            var right = DirectMergeSort(rows.Skip(mid).ToList(), sortKey, log, stepLog, delay);
            stepLog($"Разделение на левую часть: {string.Join(", ", left.Select(r => r[sortKey]))}");
            stepLog($"Разделение на правую часть: {string.Join(", ", right.Select(r => r[sortKey]))}");

            return Merge(left, right, sortKey, log, stepLog, delay);
        }

        private static List<Dictionary<string, string>> NaturalMergeSort(
    List<Dictionary<string, string>> rows,
    string sortKey,
    Action<string> log,
    Action<string> stepLog,
    int delay)
        {
            if (rows.Count <= 1)
                return rows;

            log("Поиск естественных последовательностей...");
            // Разделяем массив на естественные последовательности
            var runs = FindNaturalRuns(rows, sortKey, log);

            log($"Найдено {runs.Count} естественных последовательностей.");
            int step = 1;

            // Постепенно сливаем последовательности
            while (runs.Count > 1)
            {
                log($"Шаг {step}: слияние последовательностей...");
                var mergedRuns = new List<List<Dictionary<string, string>>>();

                for (int i = 0; i < runs.Count; i += 2)
                {
                    if (i + 1 < runs.Count)
                    {
                        // Слияние двух последовательностей
                        var merged = Merge(runs[i], runs[i + 1], sortKey, log, stepLog, delay);
                        mergedRuns.Add(merged);
                        log($"Слияние последовательностей {i} и {i + 1} завершено.");
                    }
                    else
                    {
                        // Последовательность остается без изменений
                        mergedRuns.Add(runs[i]);
                        log($"Последовательность {i} осталась без изменений.");
                    }
                }

                runs = mergedRuns;
                step++;
            }

            log("Естественная сортировка завершена.");
            return runs[0];
        }

        private static List<List<Dictionary<string, string>>> FindNaturalRuns(
            List<Dictionary<string, string>> rows,
            string sortKey,
            Action<string> log)
        {
            var runs = new List<List<Dictionary<string, string>>>();
            var currentRun = new List<Dictionary<string, string>> { rows[0] };

            for (int i = 1; i < rows.Count; i++)
            {
                if (string.Compare(rows[i - 1][sortKey], rows[i][sortKey], StringComparison.OrdinalIgnoreCase) <= 0)
                {
                    // Добавляем элемент в текущую последовательность
                    currentRun.Add(rows[i]);
                }
                else
                {
                    // Закрываем текущую последовательность и начинаем новую
                    runs.Add(currentRun);
                    currentRun = new List<Dictionary<string, string>> { rows[i] };
                }
            }

            if (currentRun.Count > 0)
            {
                runs.Add(currentRun);
            }

            log($"Сформированы {runs.Count} естественных последовательностей.");
            return runs;
        }


        private static List<Dictionary<string, string>> MultiwayMergeSort(
    List<Dictionary<string, string>> rows,
    string sortKey,
    Action<string> log,
    Action<string> stepLog,
    int delay)
        {
            if (rows.Count <= 1)
                return rows;

            log("Разделение данных на чанки для многопутевого слияния...");

            // Разбиваем на чанки для многопутевого слияния
            int chunkSize = Math.Max(rows.Count / 4, 1); // Например, делим на 4 части
            var chunks = SplitIntoChunks(rows, chunkSize, sortKey, log);

            log($"Разделение завершено. Количество чанков: {chunks.Count}.");

            log("Начало многопутевого слияния...");
            // Выполняем многопутевое слияние
            var sortedRows = KWayMerge(chunks, sortKey, log, delay);

            log("Многопутевое слияние завершено.");
            return sortedRows;
        }

        private static List<List<Dictionary<string, string>>> SplitIntoChunks(
            List<Dictionary<string, string>> rows,
            int chunkSize,
            string sortKey,
            Action<string> log)
        {
            var chunks = new List<List<Dictionary<string, string>>>();

            for (int i = 0; i < rows.Count; i += chunkSize)
            {
                var chunk = rows.Skip(i).Take(chunkSize).OrderBy(row => row[sortKey], StringComparer.OrdinalIgnoreCase).ToList();
                chunks.Add(chunk);
                log($"Создан чанк: {string.Join(", ", chunk.Select(row => row[sortKey]))}");
            }

            return chunks;
        }

        private static List<Dictionary<string, string>> KWayMerge(
            List<List<Dictionary<string, string>>> chunks,
            string sortKey,
            Action<string> log,
            int delay)
        {
            var result = new List<Dictionary<string, string>>();

            // Используем приоритетную очередь для хранения текущих элементов каждого чанка
            var comparer = Comparer<(Dictionary<string, string> row, int chunkIndex)>.Create((a, b) =>
                string.Compare(a.row[sortKey], b.row[sortKey], StringComparison.OrdinalIgnoreCase));

            var priorityQueue = new SortedSet<(Dictionary<string, string> row, int chunkIndex)>(comparer);
            var indices = new int[chunks.Count];

            // Инициализация очереди
            for (int i = 0; i < chunks.Count; i++)
            {
                if (chunks[i].Count > 0)
                {
                    priorityQueue.Add((chunks[i][0], i));
                    indices[i] = 1;
                }
            }

            while (priorityQueue.Count > 0)
            {
                // Извлекаем минимальный элемент
                var smallest = priorityQueue.Min;
                priorityQueue.Remove(smallest);

                result.Add(smallest.row);
                log($"Добавлено: {smallest.row[sortKey]} из чанка {smallest.chunkIndex + 1}");
                System.Threading.Thread.Sleep(delay);

                int chunkIndex = smallest.chunkIndex;
                if (indices[chunkIndex] < chunks[chunkIndex].Count)
                {
                    priorityQueue.Add((chunks[chunkIndex][indices[chunkIndex]++], chunkIndex));
                }
            }

            return result;
        }


        private static List<Dictionary<string, string>> Merge(
    List<Dictionary<string, string>> left,
    List<Dictionary<string, string>> right,
    string sortKey,
    Action<string> log,
    Action<string> stepLog,
    int delay)
        {
            var result = new List<Dictionary<string, string>>();
            int i = 0, j = 0;

            while (i < left.Count && j < right.Count)
            {
                var leftValue = left[i][sortKey];
                var rightValue = right[j][sortKey];

                log($"Сравнение: {leftValue} с {rightValue}");
                stepLog($"Сравнение: {left[i][sortKey]} и {right[j][sortKey]}");

                if (string.Compare(leftValue, rightValue, StringComparison.OrdinalIgnoreCase) <= 0)
                {
                    result.Add(left[i]);
                    log($"Добавлено из левой части: {leftValue}");
                    stepLog($"Добавлено из левой части: {left[i][sortKey]}");
                    i++;
                }
                else
                {
                    result.Add(right[j]);
                    log($"Добавлено из правой части: {rightValue}");
                    stepLog($"Добавлено из правой части: {right[j][sortKey]}");
                    j++;
                }

                System.Threading.Thread.Sleep(delay);
            }

            while (i < left.Count)
            {
                result.Add(left[i]);
                log($"Добавлено из оставшейся левой части: {left[i][sortKey]}");
                stepLog($"Добавлено из оставшейся левой части: {left[i][sortKey]}");
                i++;
            }

            while (j < right.Count)
            {
                result.Add(right[j]);
                log($"Добавлено из оставшейся правой части: {right[j][sortKey]}");
                stepLog($"Добавлено из оставшейся правой части: {right[j][sortKey]}");
                j++;
            }

            return result;
        }

    }
}

