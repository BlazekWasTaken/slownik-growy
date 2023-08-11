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
    Console.WriteLine("Co chcesz zrobić? [S]prawdź poprawność słowa [Z]najdź słowo z posiadanych liter");
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
            string word = String.Empty;
            try
            {
                word = Console.ReadLine() ?? string.Empty;
            }
            catch
            {
                Console.WriteLine("Słowo nie jest dobre :(");
            }
            Console.WriteLine(words.Contains(word.ToLower()) && word != "" ? "Słowo jest dobre :)" : "Słowo nie jest dobre :(");
            break;
        case "Z" or "z":
            Console.Write("Jakie masz litery?: ");
            string letters;
            try
            {
                letters = Console.ReadLine() ?? string.Empty;
                if (!letters.All(char.IsLetter)) throw new Exception();
            }
            catch
            {
                Console.WriteLine("Litery są niepoprawne :(");
                Console.Clear();
                continue;
            }

            List<char> distinctLetters = letters.ToCharArray().Distinct().ToList();
            Dictionary<char, int> letterCounts = distinctLetters.ToDictionary(letter => letter, letter => letters.Count(x => x == letter));

            List<string> possibleWords = new();
            
            foreach (string w in words)
            {
                List<char> distinctLettersInWord = w.ToCharArray().Distinct().ToList();
                Dictionary<char, int> letterCountsInWord = distinctLettersInWord.ToDictionary(letter => letter, letter => w.Count(x => x == letter));

                if (distinctLetters.Count < distinctLettersInWord.Count) continue;
                if (!new HashSet<char>(distinctLetters).IsSupersetOf(new HashSet<char>(distinctLettersInWord))) continue;
                
                possibleWords.Add(w);
            }
            
            break;
        default:
            Console.Clear();
            continue;
    }
    
    Console.ReadLine();
    Console.Clear();
}

#endregion

