using System.IO.Compression;
using System.Reflection;

#region File decompression and list creation

string resourcePath = Assembly.GetExecutingAssembly().GetManifestResourceNames().FirstOrDefault(x => x.Contains("slowa.zip")) ?? string.Empty;
Stream? stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourcePath);
if (stream is null) return;

ZipArchive archive = new(stream);

ZipArchiveEntry? wordsArchiveEntry = archive.Entries.FirstOrDefault(x => x.FullName == "slowa.txt");
if (wordsArchiveEntry is null) return;

Stream unzippedFileStream = wordsArchiveEntry.Open();
StreamReader reader = new(unzippedFileStream);

List<string> words = reader.ReadToEnd().Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None).ToList();

#endregion

#region Game loop

Console.Clear();
while (true)
{
    Console.WriteLine("Co chcesz zrobić? [S]prawdź poprawność słowa [Z]najdź słowo z posiadanych liter [W]yjdź");
    string choice = String.Empty;
    try
    {
        choice = Console.ReadLine() ?? string.Empty;
    }
    catch
    {
        Console.Clear();
        continue;
    }

    switch (choice)
    {
        case "S" or "s":
            Console.Write("Jakie słowo sprawdzić?: ");
            string word = string.Empty;
            try
            {
                word = Console.ReadLine() ?? string.Empty;
                word = word.Trim().ToLower();
            }
            catch
            {
                Console.WriteLine("Słowo nie jest dobre :(");
            }
            Console.WriteLine(words.Contains(word) && word != "" ? "Słowo jest dobre :)" : "Słowo nie jest dobre :(");
            break;
        case "Z" or "z":
            Console.Write("Jakie masz litery?: ");
            string letters;
            try
            {
                letters = Console.ReadLine() ?? string.Empty;
                letters = letters.Trim().ToLower();
                if (!letters.All(char.IsLetter)) throw new Exception();
                if (letters.Contains('q') || letters.Contains('x') || letters.Contains('v')) throw new Exception();
            }
            catch
            {
                Console.WriteLine("Litery są niepoprawne :(");
                break;
            }

            Dictionary<char, int> pointsForLetters = new()
            {
                { 'a', 1 }, { 'e', 1 }, { 'i', 1 }, { 'n', 1 }, { 'o', 1 }, { 'r', 1 }, { 's', 1 }, { 'w', 1 }, { 'z', 1 },
                { 'c', 2 }, { 'd', 2 }, { 'k', 2 }, { 'l', 2 }, { 'm', 2 }, { 'p', 2 }, { 't', 2 }, { 'y', 2 },
                { 'b', 3 }, { 'g', 3 }, { 'h', 3 }, { 'j', 3 }, { 'ł', 3 }, { 'u', 3 },
                { 'ą', 5 }, { 'ę', 5 }, { 'f', 5 }, { 'ó', 5 }, { 'ś', 5 }, { 'ż', 5 },
                { 'ć', 6 },
                { 'ń', 7 },
                { 'ź', 9 }
            };

            List<char> distinctLetters = letters.ToCharArray().Distinct().ToList();
            Dictionary<char, int> letterCounts = distinctLetters.ToDictionary(letter => letter, letter => letters.Count(x => x == letter));

            List<string> possibleWords = new();

            foreach (string w in words)
            {
                List<char> distinctLettersInWord = w.ToCharArray().Distinct().ToList();
                Dictionary<char, int> letterCountsInWord = distinctLettersInWord.ToDictionary(letter => letter, letter => w.Count(x => x == letter));

                if (distinctLetters.Count < distinctLettersInWord.Count) continue;
                if (!new HashSet<char>(distinctLetters).IsSupersetOf(new HashSet<char>(distinctLettersInWord))) continue;

                if (!distinctLettersInWord.All(x => letterCountsInWord[x] <= letterCounts[x])) continue;
                
                possibleWords.Add(w);
            }
 
            Dictionary<string, int> wordsWithPoints = possibleWords.ToDictionary(x => x, x =>
            {
                int sum = 0;
                x.ToList().ForEach(y => sum += pointsForLetters[y]);
                return sum;
            });

            int maxPoints = wordsWithPoints.Max(x => x.Value);

            if (maxPoints == 0)
            {
                Console.WriteLine("Nie da się stworzyć słowa z tych liter :(");
                break;
            }
            
            List<KeyValuePair<string, int>> maxPointsWords = wordsWithPoints.Where(x => x.Value == maxPoints).ToList();

            foreach (KeyValuePair<string, int> maxPointsWord in maxPointsWords)
            {
                Console.Write($"{maxPointsWord.Key} ");
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.Write($"{maxPointsWord.Value} ");
                Console.ResetColor();
            }
            Console.WriteLine();
            
            break;
        case "W" or "w":
            Console.Clear();
            return;
        default:
            Console.Clear();
            continue;
    }
    
    Console.ReadLine();
    Console.Clear();
}

#endregion