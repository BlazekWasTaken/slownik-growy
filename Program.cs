using System.Reflection;

string tempPath = Path.GetTempPath();
string zipPath = Path.Combine(tempPath, "slowa.zip");
string filePath = Path.Combine(tempPath, "slowa.txt");

string resourcePath = Assembly.GetExecutingAssembly().GetManifestResourceNames().FirstOrDefault(x => x.Contains("slowa.zip")) ?? string.Empty;
var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourcePath);
var fileStream = new FileStream(zipPath, FileMode.Create);

stream.CopyTo(fileStream);
stream.Close();
fileStream.Close();

System.IO.Compression.ZipFile.ExtractToDirectory(zipPath,tempPath);
File.Delete(zipPath);

List<string> words = File.ReadAllLines(filePath).ToList();
File.Delete(filePath);

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