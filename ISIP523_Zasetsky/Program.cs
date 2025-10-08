using System;
using System.Collections.Generic;

class Program
{
    class Characteristic
    {
        public string text;
        public int wordsCount = 0;
        public string shortestWord = "";
        public int sentencesCount = 0;
        public int consonant = 0;
        public int vowel = 0;
        public string longestWord = "";
        Dictionary<char, int> azbyka = new Dictionary<char, int>();


        static List<Characteristic> textHistory = new List<Characteristic>();

        public void TextAdd()
        {
            Console.WriteLine("Введите текст (не менее 100 символов)");
            Console.WriteLine("Вводите текст построчно. Для завершения ввода введите пустую строку:");

            string input = "";
            string line;
            int totalLength = 0;


            while (!string.IsNullOrWhiteSpace(line = Console.ReadLine()))
            {
                input += line + "\n";
                totalLength += line.Length;



            }

            text = input.Trim();

            if (text.Length < 100)
            {
                Console.WriteLine("Вы ввели некорректный текст (менее 100 символов)");
                text = null;
            }
            else
            {
                Console.WriteLine($"Текст успешно принят! Длина: {text.Length} символов");
            }
        }



        void WordCount()
        {
            char[] separators = { ' ', ',', '.', '!', '?', ';', ':', '\t', '\n', '\r', '(', ')', '[', ']', '{', '}', '"', '\'' };
            string[] words = text.Split(separators, StringSplitOptions.RemoveEmptyEntries);
            wordsCount = words.Length;
        }

        void ShortestWordSearch()
        {
            char[] separators = { ' ', ',', '.', '!', '?', ';', ':', '\t', '\n', '\r', '(', ')', '[', ']', '{', '}', '"', '\'' };
            string[] words = text.Split(separators, StringSplitOptions.RemoveEmptyEntries);

            if (words.Length == 0)
            {
                shortestWord = "";
                return;
            }

            shortestWord = words[0];
            for (int i = 1; i < words.Length; i++)
            {
                if (words[i].Length < shortestWord.Length)
                {
                    shortestWord = words[i];
                }
            }
        }
        void LongestWordSearch()
        {
            char[] separators = { ' ', ',', '.', '!', '?', ';', ':', '\t', '\n', '\r', '(', ')', '[', ']', '{', '}', '"', '\'' };
            string[] words = text.Split(separators, StringSplitOptions.RemoveEmptyEntries);

            if (words.Length == 0)
            {
                longestWord = "";
                return;
            }

            longestWord = words[0];
            for (int i = 1; i < words.Length; i++)
            {
                if (words[i].Length > longestWord.Length)
                {
                    longestWord = words[i];
                }
            }
        }

        void SentencesCount()
        {
            sentencesCount = 0;
            foreach (char ch in text)
            {
                if (ch == '.' || ch == '!' || ch == '?')
                {
                    sentencesCount++;
                }
            }
        }

        void LettersQuantity()
        {
            consonant = 0;
            vowel = 0;

            string allConsonant = "цкнгшщзхъфвпрлджчсмтьбйЦКНГШЩЗХЪФВПРЛДЖЧСМТЬБЙ";
            string allVowel = "уеыаоэяиюУЕЫАОЭЯИЮ";

            foreach (char ch in text)
            {
                if (char.IsLetter(ch))
                {
                    bool isConsonant = false;
                    for (int i = 0; i < allConsonant.Length; i++)
                    {
                        if (allConsonant[i] == ch)
                        {
                            isConsonant = true;
                            break;
                        }
                    }


                    if (isConsonant)
                    {
                        consonant++;
                    }
                    else
                    {
                        bool isVowel = false;
                        for (int i = 0; i < allVowel.Length; i++)
                        {
                            if (allVowel[i] == ch)
                            {
                                isVowel = true;
                                break;
                            }
                        }

                        if (isVowel)
                        {
                            vowel++;
                        }
                    }
                }
            }
        }

        void StatsLetters()
        {
            azbyka.Clear();
            foreach (char ch in text)
            {
                if (!char.IsLetter(ch))
                    continue;

                char lowerChar = char.ToLower(ch);

                if (azbyka.ContainsKey(lowerChar))
                    azbyka[lowerChar]++;
                else
                    azbyka[lowerChar] = 1;
            }
        }
        void SaveToHistory()
        {

            Characteristic historyCopy = new Characteristic();
            historyCopy.text = this.text.Length > 30 ? this.text.Substring(0, 30) + "..." : this.text;
            historyCopy.wordsCount = this.wordsCount;
            historyCopy.shortestWord = this.shortestWord;
            historyCopy.sentencesCount = this.sentencesCount;
            historyCopy.consonant = this.consonant;
            historyCopy.vowel = this.vowel;
            historyCopy.longestWord = this.longestWord;
            historyCopy.azbyka = new Dictionary<char, int>(this.azbyka);

            textHistory.Add(historyCopy);
        }

        public void CalculateAllStats()
        {
            if (string.IsNullOrEmpty(text))
                return;

            WordCount();
            ShortestWordSearch();
            LongestWordSearch();
            SentencesCount();
            LettersQuantity();
            StatsLetters();
            SaveToHistory();
        }

        public void StatsOutput()
        {
            Console.WriteLine("\n=== СТАТИСТИКА ТЕКСТА ===");
            Console.WriteLine($"Текст: {(text.Length > 50 ? text.Substring(0, 50) + "..." : text)}");
            Console.WriteLine($"Общая длина текста: {text.Length} символов");
            Console.WriteLine($"Количество слов: {wordsCount}");
            Console.WriteLine($"Самое короткое слово: '{shortestWord}' (длина: {shortestWord.Length})");
            Console.WriteLine($"Самое длинное слово: '{longestWord}' (длина: {longestWord.Length})");
            Console.WriteLine($"Количество предложений: {sentencesCount}");
            Console.WriteLine($"Количество согласных букв: {consonant}");
            Console.WriteLine($"Количество гласных букв: {vowel}");
            Console.WriteLine($"Всего букв: {consonant + vowel}");


            Console.WriteLine("\n--- Статистика букв ---");
            if (azbyka.Count > 0)
            {

                List<KeyValuePair<char, int>> sortedList = new List<KeyValuePair<char, int>>();


                foreach (KeyValuePair<char, int> pair in azbyka)
                {
                    sortedList.Add(pair);
                }


                for (int i = 0; i < sortedList.Count - 1; i++)
                {
                    for (int j = 0; j < sortedList.Count - i - 1; j++)
                    {
                        if (sortedList[j].Value < sortedList[j + 1].Value)
                        {

                            KeyValuePair<char, int> temp = sortedList[j];
                            sortedList[j] = sortedList[j + 1];
                            sortedList[j + 1] = temp;
                        }
                    }
                }



                foreach (KeyValuePair<char, int> pair in sortedList)
                {
                    Console.WriteLine($"  Буква '{pair.Key}': {pair.Value} раз");
                }
            }
            else
            {
                Console.WriteLine("  Нет данных о буквах");
            }
            Console.WriteLine("=======================\n");
        }


        public static void ShowHistory()
        {
            if (textHistory.Count == 0)
            {
                Console.WriteLine("\nИстория пуста. Сначала проанализируйте тексты.");
                return;
            }

            Console.WriteLine($"\n=== ИСТОРИЯ АНАЛИЗА ({textHistory.Count} текстов) ===");

            for (int i = 0; i < textHistory.Count; i++)
            {
                Console.WriteLine($"\n--- Текст #{i + 1} ---");
                Console.WriteLine($"Предпросмотр: {textHistory[i].text}");
                Console.WriteLine($"Слов: {textHistory[i].wordsCount}");
                Console.WriteLine($"Предложений: {textHistory[i].sentencesCount}");
                Console.WriteLine($"Самое короткое слово: '{textHistory[i].shortestWord}'");
                Console.WriteLine($"Самое длинное слово: '{textHistory[i].longestWord}'");
                Console.WriteLine($"Букв: {textHistory[i].vowel + textHistory[i].consonant} (гл: {textHistory[i].vowel}, согл: {textHistory[i].consonant})");
            }
            Console.WriteLine("===================================\n");
        }


        public void ClearAllStats()
        {
            text = null;
            wordsCount = 0;
            shortestWord = "";
            sentencesCount = 0;
            consonant = 0;
            vowel = 0;
            longestWord = "";
            azbyka.Clear();
        }
    }
    static void Main(string[] args)
    {
        Characteristic stats = new Characteristic();
        bool continueWorking = true;

        Console.WriteLine("Программа для анализа текста");

        while (continueWorking)
        {

            stats.TextAdd();
            if (stats.text == null)
            {
                Console.WriteLine("Не удалось получить текст. Попробуйте снова.");
                continue;
            }


            stats.CalculateAllStats();

            stats.StatsOutput();


            Console.WriteLine("Выберите действие:");
            Console.WriteLine("1 - Проанализировать другой текст");
            Console.WriteLine("2 - Показать историю текстов");
            Console.WriteLine("3 - Выйти из программы");
            Console.Write("Ваш выбор: ");

            string answer = Console.ReadLine();

            if (answer == "3")
            {
                continueWorking = false;
            }
            else if (answer == "2")
            {
                Characteristic.ShowHistory();
                Console.Write("Нажмите любую клавишу для продолжения...");
                Console.ReadKey();
                stats.ClearAllStats();
                Console.Clear();
            }
            else
            {
                stats.ClearAllStats();
                Console.Clear();
            }
        }

        Console.WriteLine("Программа завершена. Спасибо за использование!");

    }
}