using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Net.Http.Headers;

string _file = "alpha.txt";
int _wordLength = 5;
int _wordCount = 5;

List<string> words = [];
List<int> bitWord = [];

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
List<string> uniqueWords = [];
//'words' contains all 5 letter words, that have no repeating letters.
int sameLetters = 0;
uniqueWords.Add(words[0]);
bitWord.Add(wordtoInt(words[0]));
foreach (string word in words)
{
    for (int k = 0;  k < uniqueWords.Count; k++)
    {
        sameLetters = 0;
        for (int i = 0; i < _wordLength; i++)
        {
            for (int j = 0; j < _wordLength; j++)
            {
                if (word[i] == uniqueWords[k][j])
                {
                    sameLetters++;
                }
            }
        }
        if (sameLetters == _wordLength)
        {
            goto endOfForeach;
        }
    }
    uniqueWords.Add(word);
    bitWord.Add(wordtoInt(word));
    endOfForeach:;
}
Console.WriteLine("Anograms done!");
// removed anograms (words containing the same letters)

// 
int _countSolutions = 0;
Console.WriteLine(uniqueWords.Count);
Console.WriteLine(bitWord.Count);
// check for solutions

//recursiveFindSolution(uniqueWords, new List<string>(), uniqueWords.Count - 1);
recursiveFindSolutionBits(bitWord, 0, 0, bitWord.Count-1,"");
sw.Stop();
Console.WriteLine($"{_countSolutions} Solutions");
Console.WriteLine($"{sw.ElapsedMilliseconds } Milliseconds");

void recursiveFindSolutionBits(List<int> words, int usedWords, int wordscount, int index, string output)
{
    for (int i = index ; i >= 0; i--)
    {
        if ((usedWords & words[i]) == 0)
        {
            if (wordscount == _wordCount-1)
            {
                Console.WriteLine($"{output} {uniqueWords[i]}");
                _countSolutions++;
                continue;
            }
            recursiveFindSolutionBits(words, (usedWords | words[i]), wordscount + 1, i - 1, $"{output} {uniqueWords[i]}");
        }
    }
}

int bitPos(char c)
{
    return (int)(c - 'a');
}

int wordtoInt(string word)
{
    int output = 0;
    foreach (char c in word)
    {
        output |= 1 << bitPos(c);
    }
    return output;
}
