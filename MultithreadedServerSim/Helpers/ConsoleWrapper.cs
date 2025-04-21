internal class ConsoleWrapper
{
    private static List<string> _printedLines = [""];

    private static void Write(string stringToWrite, int index, bool addNewLine = false)
    {
        _printedLines[index] = string.Join("", _printedLines[index], stringToWrite);
        if (addNewLine) _printedLines.Add("");
    }

    public static void Write(string stringToWrite)
    {
        Write(stringToWrite, GetTop());
        Console.Write(stringToWrite);
    }

    public static string ReadLine()
    {
        var readLine = Console.ReadLine()?.Trim();
        if (readLine == null)
            return "";
        Write(readLine, GetTop(), addNewLine: true);
        return readLine;
    }

    public static void WriteLine(string stringToWrite)
    {
        Write(stringToWrite, GetTop(), addNewLine: true);
        Console.WriteLine(stringToWrite);
    }

    public static void WriteAtHeight(string stringToWrite, int height)
    {
        Write(stringToWrite, height);
        Console.SetCursorPosition(0, 0);
        Console.Clear();
        Console.Write(string.Join("\n", _printedLines));
    }

    public static int GetTop()
    {
        return _printedLines.Count - 1;
    }
}