using System;
using System.Collections.Generic;
using System.Diagnostics;

class Student
{
    public string Name;
    public string Group;
    public int Score;

    public Student(string name, string group, int score)
    {
        Name = name;
        Group = group;
        Score = score;
    }
}

class Program
{
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        int N = 4;
        int K = 17;

        Task1_LetterFrequencies(K);
        Task2_GroupStudents();
        Task3_RemoveDuplicates(N, K);
        Task4_SetUnion(N, K);
        Task5_MeasurePerformance();
        RunEdgeCases();
    }

    public static void Task1_LetterFrequencies(int K)
    {
        string text = "Алгоритм описує послідовність дій. Алгоритм можна записати словами, а можна записати кодом. Кодом алгоритм стає тоді, коли його виконує машина, а не людина. Одна задача — багато алгоритмів.";
        int threshold = (K % 3) + 2;

        Dictionary<char, int> freqs = GetLetterFrequencies(text);

        Console.WriteLine($"Усього різних літер: {freqs.Count}");
        Console.WriteLine($"Літери з частотою >= {threshold}:");

        foreach (var pair in freqs)
        {
            if (pair.Value >= threshold)
            {
                Console.WriteLine($"'{pair.Key}': {pair.Value}");
            }
        }
    }

    public static Dictionary<char, int> GetLetterFrequencies(string text)
    {
        Dictionary<char, int> result = new Dictionary<char, int>();
        if (string.IsNullOrEmpty(text)) return result;

        string lowerText = text.ToLower();
        for (int i = 0; i < lowerText.Length; i++)
        {
            char c = lowerText[i];
            if (char.IsLetter(c))
            {
                if (result.TryGetValue(c, out int count))
                {
                    result[c] = count + 1;
                }
                else
                {
                    result[c] = 1;
                }
            }
        }
        return result;
    }

    public static void Task2_GroupStudents()
    {
        List<Student> students = new List<Student>
        {
            new Student("Коваленко", "КН-3/1", 88),
            new Student("Іщенко", "КН-3/1", 93),
            new Student("Савченко", "КН-3/2", 55),
            new Student("Кравець", "КН-3/1", 76),
            new Student("Мороз", "КН-3/2", 82),
            new Student("Бойко", "КН-3/2", 95),
            new Student("Мельник", "КН-3/2", 64),
            new Student("Бондар", "КН-3/1", 71),
            new Student("Ткачук", "КН-3/2", 90),
            new Student("Іванів", "КН-3/1", 48),
            new Student("Шевчук", "КН-3/1", 67),
            new Student("Кушнір", "КН-3/2", 73)
        };

        Dictionary<char, List<Student>> grouped = GroupByFirstLetter(students);

        foreach (var pair in grouped)
        {
            double sum = 0;
            for (int i = 0; i < pair.Value.Count; i++) sum += pair.Value[i].Score;
            double avg = sum / pair.Value.Count;

            Console.WriteLine($"Ключ '{pair.Key}': Студентів = {pair.Value.Count}, Середній бал = {avg:F2}");
        }
    }

    public static Dictionary<char, List<Student>> GroupByFirstLetter(List<Student> students)
    {
        Dictionary<char, List<Student>> result = new Dictionary<char, List<Student>>();
        if (students == null) return result;

        for (int i = 0; i < students.Count; i++)
        {
            Student st = students[i];
            if (string.IsNullOrEmpty(st.Name)) continue;

            char firstLetter = char.ToUpper(st.Name[0]);

            if (!result.TryGetValue(firstLetter, out List<Student> list))
            {
                list = new List<Student>();
                result[firstLetter] = list;
            }
            list.Add(st);
        }
        return result;
    }

    public static void Task3_RemoveDuplicates(int nVal, int kVal)
    {
        int range = (kVal % 10) + 6;
        List<int> source = new List<int>();
        Random rnd = new Random(nVal);

        for (int i = 0; i < 25; i++)
        {
            source.Add(rnd.Next(1, range + 1));
        }

        Console.WriteLine("Початковий список: " + string.Join(", ", source));

        HashSet<int> setUnordered = new HashSet<int>(source);
        Console.WriteLine("1. Без збереження порядку (HashSet): " + string.Join(", ", setUnordered));

        List<int> listOrdered = new List<int>();
        HashSet<int> seen = new HashSet<int>();
        for (int i = 0; i < source.Count; i++)
        {
            if (seen.Add(source[i]))
            {
                listOrdered.Add(source[i]);
            }
        }
        Console.WriteLine("2. Зі збереженням порядку: " + string.Join(", ", listOrdered));
    }

    public static void Task4_SetUnion(int nVal, int kVal)
    {
        int range = 20 + kVal;
        List<int> list1 = new List<int>();
        List<int> list2 = new List<int>();

        Random rnd1 = new Random(nVal);
        Random rnd2 = new Random(nVal + 100);

        for (int i = 0; i < 30; i++)
        {
            list1.Add(rnd1.Next(1, range + 1));
            list2.Add(rnd2.Next(1, range + 1));
        }

        HashSet<int> unionSet = new HashSet<int>(list1);
        unionSet.UnionWith(list2);

        List<int> unionList = new List<int>();
        for (int i = 0; i < list1.Count; i++)
        {
            if (!ContainsManual(unionList, list1[i])) unionList.Add(list1[i]);
        }
        for (int i = 0; i < list2.Count; i++)
        {
            if (!ContainsManual(unionList, list2[i])) unionList.Add(list2[i]);
        }

        Console.WriteLine($"Розмір об'єднання (HashSet): {unionSet.Count}");
        Console.WriteLine($"Розмір об'єднання (Manual):  {unionList.Count}");
        Console.WriteLine($"Сума розмірів початкових списків: {list1.Count + list2.Count}");
        Console.WriteLine($"Результати двох способів збігаються: {unionSet.Count == unionList.Count}");
    }

    static bool ContainsManual(List<int> list, int value)
    {
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i] == value) return true;
        }
        return false;
    }

    public static void Task5_MeasurePerformance()
    {
        Stopwatch sw = new Stopwatch();

        List<int> bigList = new List<int>();
        HashSet<int> bigSet = new HashSet<int>();
        for (int i = 0; i < 100000; i++) { bigList.Add(i); bigSet.Add(i); }

        sw.Restart();
        for (int i = 0; i < 10000; i++) ContainsManual(bigList, -1);
        sw.Stop();
        long t1_list = sw.ElapsedMilliseconds;

        sw.Restart();
        for (int i = 0; i < 10000; i++) bigSet.Contains(-1);
        sw.Stop();
        long t1_set = sw.ElapsedMilliseconds;

        List<string> words = new List<string>();
        for (int i = 0; i < 50000; i++) words.Add("word" + (i % 500));

        sw.Restart();
        List<string> keysList = new List<string>();
        List<int> valsList = new List<int>();
        for (int i = 0; i < words.Count; i++)
        {
            string w = words[i];
            int idx = -1;
            for (int j = 0; j < keysList.Count; j++)
            {
                if (keysList[j] == w) { idx = j; break; }
            }
            if (idx != -1) valsList[idx]++;
            else { keysList.Add(w); valsList.Add(1); }
        }
        sw.Stop();
        long t2_list = sw.ElapsedMilliseconds;

        sw.Restart();
        Dictionary<string, int> dict = new Dictionary<string, int>();
        for (int i = 0; i < words.Count; i++)
        {
            string w = words[i];
            if (dict.TryGetValue(w, out int c)) dict[w] = c + 1;
            else dict[w] = 1;
        }
        sw.Stop();
        long t2_dict = sw.ElapsedMilliseconds;

        List<int> dupNumbers = new List<int>();
        Random rnd = new Random(42);
        for (int i = 0; i < 20000; i++) dupNumbers.Add(rnd.Next(1, 1000));

        sw.Restart();
        List<int> noDupsList = new List<int>();
        for (int i = 0; i < dupNumbers.Count; i++)
        {
            if (!ContainsManual(noDupsList, dupNumbers[i])) noDupsList.Add(dupNumbers[i]);
        }
        sw.Stop();
        long t3_list = sw.ElapsedMilliseconds;

        sw.Restart();
        HashSet<int> noDupsSet = new HashSet<int>(dupNumbers);
        sw.Stop();
        long t3_set = sw.ElapsedMilliseconds;

        List<int> setA = new List<int>();
        List<int> setB = new List<int>();
        for (int i = 0; i < 5000; i++) { setA.Add(i); setB.Add(i + 2500); }

        sw.Restart();
        List<int> commonList = new List<int>();
        for (int i = 0; i < setA.Count; i++)
        {
            if (ContainsManual(setB, setA[i]) && !ContainsManual(commonList, setA[i]))
            {
                commonList.Add(setA[i]);
            }
        }
        sw.Stop();
        long t4_list = sw.ElapsedMilliseconds;

        sw.Restart();
        HashSet<int> hashA = new HashSet<int>(setA);
        HashSet<int> hashB = new HashSet<int>(setB);
        hashA.IntersectWith(hashB);
        sw.Stop();
        long t4_set = sw.ElapsedMilliseconds;

        Console.WriteLine($"1. Перевірка 10,000 в 100,000:   Список = {t1_list} мс | HashSet = {t1_set} мс");
        Console.WriteLine($"2. Частоти 50,000 слів:          Списки = {t2_list} мс | Словник = {t2_dict} мс");
        Console.WriteLine($"3. Дублікати з 20,000 чисел:    Список = {t3_list} мс | HashSet = {t3_set} мс");
        Console.WriteLine($"4. Спільні у двох по 5,000:      Список = {t4_list} мс | HashSet = {t4_set} мс");
    }

    static void RunEdgeCases()
    {
        GetLetterFrequencies("");
        GetLetterFrequencies("abcde");
        GroupByFirstLetter(new List<Student> { new Student("Кравець", "КН-3/1", 100) });
        new HashSet<int>(new List<int> { 1, 2, 3, 4, 5 });
    }
}