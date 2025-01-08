using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Net.Http.Headers;

string _file = "alpha.txt";
int _wordLength = 4;
int _wordCount = 6;

List<string> words = [];
IDictionary<int, string> bitWords = new Dictionary<int, string>();

Stopwatch sw = new Stopwatch();
sw.Start();
try
{
    using (StreamReader sr = new StreamReader(_file))
    {
        string line;
        while ((line = sr.ReadLine()) != null)
        {   
            //Console.WriteLine(line);
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
//'words' contains all 5 letter words, that have no repeating letters.
foreach (string word in words)
{
    int bits = wordToInt(word);
    if (!bitWords.ContainsKey(bits))
    {
        bitWords.Add(bits, word);
    }
}
Console.WriteLine("Anograms done!");
// removed anograms (words containing the same letters)

int _countSolutions = 0;
Console.WriteLine(bitWords.Count);
// check for solutions

//recursiveFindSolution(uniqueWords, new List<string>(), uniqueWords.Count - 1);
recursiveFindSolutionBits(bitWords.Keys.ToArray(), 0, 0, bitWords.Count-1, []);
sw.Stop();
Console.WriteLine($"{_countSolutions} Solutions");
Console.WriteLine($"{sw.ElapsedMilliseconds } Milliseconds");

void recursiveFindSolutionBits(int[] words, int usedWords, int wordscount, int index, List<int> keysUsed)
{
    for (int i = index ; i >= 0; i--)
    {
        if ((usedWords & words[i]) == 0)
        {
            if (wordscount == _wordCount-1)
            {
                //foreach (int key in keysUsed)
                //{
                //    Console.Write($"{bitWords[key]} ");
                //}
                //Console.WriteLine(bitWords[words[i]]);
                _countSolutions++;
                continue;
            }
            List<int> temp = new List<int>(keysUsed);
            temp.Add(words[i]);
            recursiveFindSolutionBits(words, (usedWords | words[i]), wordscount + 1, i - 1, temp);
        }
    }
}

int wordToInt(string word)
{
    int output = 0;
    foreach (char c in word)
    {
        output |= 1 << (int)(c - 'a');
    }
    return output;
}
