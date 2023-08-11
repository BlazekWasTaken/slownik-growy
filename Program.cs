using System.IO.Compression;
using System.Reflection;

string resourcePath = Assembly.GetExecutingAssembly().GetManifestResourceNames().FirstOrDefault(x => x.Contains("slowa.zip")) ?? string.Empty;
Stream? stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourcePath);
if (stream is null) return;

ZipArchive archive = new(stream);

ZipArchiveEntry? wordsArchiveEntry = archive.Entries.FirstOrDefault(x => x.FullName == "slowa.txt");
if (wordsArchiveEntry is null) return;

Stream unzippedFileStream = wordsArchiveEntry.Open();
StreamReader reader = new(unzippedFileStream);

List<string> words = reader.ReadToEnd().Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None).ToList();

Console.Clear();
while (true)
{
    Console.Write("Jakie słowo sprawdzić?: ");
    try
    {
        string word = Console.ReadLine() ?? string.Empty;
        Console.WriteLine(words.Contains(word.ToLower()) ? "Słowo jest dobre :)" : "Słowo nie jest dobre :(");
    }
    catch
    {
        Console.WriteLine("Słowo nie jest dobre :(");
    }
    Console.ReadLine();
    Console.Clear();
}