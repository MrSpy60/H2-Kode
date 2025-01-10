using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Net.Http.Headers;
using System.Reflection;

string _file = "alpha.txt";
int _wordLength = 5;
int _wordCount = 5;
int _alphabetLenght = 26;
int _countSolutions = 0;
int _countWords = 0;
List<Solution> solutions = new();
int[] LetterFrequencePosition = { 16, 9, 23, 25, 22, 10, 21, 5, 24, 1, 7, 12, 15, 6, 20, 3, 2, 11, 14, 19, 13, 17, 0, 8, 18, 4 };

List<string> words = [];
IDictionary<int, string> bitWords = new Dictionary<int, string>();
List<int>[] letterArray = new List<int>[26];
Stopwatch sw = new Stopwatch();
sw.Start();
//'words' contains all 5 letter words, that have no repeating letters.
try
{
    using (StreamReader sr = new StreamReader(_file))
    {
        string line;
        while ((line = sr.ReadLine()) != null)
        {   
            if (line.Length == _wordLength && line.Distinct().Count() == _wordLength)
            {
                    words.Add(line);
            }
        }
    }
}
catch (Exception e)
{
    Console.WriteLine(e.Message);
    Environment.Exit(1);
}
Console.WriteLine("Loading done!");
// make dictionaries in dictionaries
for (int i = 0; i < _alphabetLenght; i++)
{
    letterArray[i] = new List<int>();
}
// remove anograms (words containing the same letters)
foreach (string word in words)
{
    var leastbit = 0;
    int bits = wordToInt(word, ref leastbit);
    if (!bitWords.ContainsKey(bits))
    {
        bitWords.Add(bits, word);
        letterArray[leastbit].Add(bits);
    }
}
Console.WriteLine("Anograms done!");
foreach (List<int> i in letterArray)
{
    _countWords += i.Count();
}

Console.WriteLine($"{_countWords} unique words");

// check for solutions
recursiveFindSolutionBits(letterArray, 0, 0, [], new Solution([]));
Console.WriteLine($"{_countSolutions} Solutions");
foreach (Solution solution in solutions)
{
    List<string> output = [];
    foreach ((int, int) value in solution.values)
    {
        output.Add(getWord(value));
    }
    Console.WriteLine(string.Join(" ", output));
}
sw.Stop();
Console.WriteLine($"{sw.ElapsedMilliseconds } Milliseconds");

void recursiveFindSolutionBits(List<int>[] words, int usedLetters, int dictIndex, List<(int, int)> keysUsed, Solution solution, int skips = 0)
{
    for (int j = dictIndex; (j < 26 && skips < 2); j++)
    {
        if ((usedLetters & (1 << j)) != 0) continue;
        for (int i = 0; i < words[j].Count(); i++)
        {
            if ((usedLetters & words[j][i]) == 0)
            {                
                if (keysUsed.Count + 1 == _wordCount)
                {
                    solution.values[keysUsed.Count] = (j, i);
                    solutions.Add(new Solution(solution.values));
                    _countSolutions++;
                    continue;
                }
                List<(int, int)> tempKeys = new List<(int, int)>(keysUsed);
                tempKeys.Add((i,j));
                solution.values[keysUsed.Count] = (j, i);
                recursiveFindSolutionBits(words, (usedLetters | words[j][i]), j + 1, tempKeys, solution, skips);
            }
        }
        skips++;
    }
}


int wordToInt(string word, ref int leastbit)
{
    leastbit = 30;
    int output = 0;
    foreach (char c in word)
    {
        var position = LetterFrequencePosition[c - 'a'];
        output |= 1 << position;
        if (position < leastbit) leastbit = position;
    }
    return output;
}

string getWord((int, int) index)
{
    return bitWords[letterArray[index.Item1][index.Item2]];
}

class Solution
{
    public (int, int)[] values = new (int, int)[5];

    public Solution((int, int)[] values)
    {
        if (values.Length == 5)
        values.CopyTo(this.values,0);
    }
}
