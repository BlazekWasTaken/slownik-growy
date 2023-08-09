List<string> words = new();

bool isFilePathCorrect = true;
while (isFilePathCorrect)
{
    Console.WriteLine("Gdzie jest plik ze słowami?");
    try
    {
        string path = Console.ReadLine() ?? string.Empty;
        words = File.ReadAllLines(path).ToList();
        isFilePathCorrect = false;
    }
    catch (Exception e)
    {
        isFilePathCorrect = true;
    }
    Console.Clear();
}

bool isRunning = true;
while (isRunning)
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