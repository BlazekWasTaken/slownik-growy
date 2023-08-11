using System.Reflection;

string resourcePath = Assembly.GetExecutingAssembly().GetManifestResourceNames().FirstOrDefault(x => x.Contains("slowa.zip")) ?? string.Empty;
var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourcePath);

List<string> words = new();

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