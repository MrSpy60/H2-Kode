using System.IO;
using System.Net.Http.Headers;

string _file = "beta.txt";
int _wordLength = 5;
int _wordCount = 5;

List<string> words = [];

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
}
Console.WriteLine("Loading done!");
List<string> uniqueWords = [];
//'words' contains all 5 letter words, that have no repeating letters.
int sameLetters = 0;
uniqueWords.Add(words[0]);
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
    endOfForeach:;
}
Console.WriteLine("Anograms done!");
// removed anograms (words containing the same letters)
int _countSolutions = 0;
Console.WriteLine(uniqueWords.Count);

// check for solutions

recursiveFindSolution(uniqueWords, new List<string>(), uniqueWords.Count - 1);
Console.WriteLine($"{_countSolutions} Solutions");

void recursiveFindSolution(List<string> words , List<string> usedWords, int index)
{
    if (usedWords.Count == _wordCount)
    {
        Console.WriteLine(string.Join(" ", usedWords));
        _countSolutions++;
        return;
    }
    for (int i = index; i >= 0; i--)
    {
        if ((string.Join("", usedWords )+ words[i]).Distinct().Count() == _wordLength * (usedWords.Count() + 1) )
        {
            List<string> newWords = new List<string>(usedWords);
            newWords.Add(words[i]);
            recursiveFindSolution(words, newWords, i-1);
        }
    }
}

void findSolution()
{
    int count1 = 0;
    for (int word1 = 0; word1 < uniqueWords.Count-4 ;word1++ )
    {
        count1++;
        if (count1 % 10 == 0)
        {
            Console.Write(".");
        }
        for (int word2 = word1+1; word2 < uniqueWords.Count - 3; word2++)
        {
            if (compareword(word1, word2))
            {
                for (int word3 = word2 + 1; word3 < uniqueWords.Count -2; word3++)
                {
                    if (compareword(word1, word3) && compareword(word2, word3))
                    {
                        for (int word4 = word3 + 1; word4 < uniqueWords.Count - 1; word4++)
                        {
                            if (compareword(word1, word4) && compareword(word2, word4) && compareword(word3, word4))
                            {
                                for (int word5 = word4 + 1; word5 < uniqueWords.Count; word5++)
                                {
                                    if (compareword(word1, word5) && compareword(word2, word5) && compareword(word3, word5) && compareword(word4, word5))
                                    {
                                        _countSolutions++;
                                        Console.WriteLine(uniqueWords[word1] + " " + uniqueWords[word2] + " " + uniqueWords[word3] + " " + uniqueWords[word4] + " " + uniqueWords[word5]);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
    Console.WriteLine(_countSolutions);
}
bool compareword(int word1, int word2)
{
    for (int i = 0; i < 5; i++)
    {
        for (int j = 0; j < 5; j++)
        {
            if (uniqueWords[word1][i] == uniqueWords[word2][j])
            {
                return false;
            }
        }
    }
    return true;
}