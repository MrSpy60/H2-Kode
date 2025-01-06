using System.IO;
using System.Reflection.Metadata.Ecma335;
Console.WriteLine("Hello, World!");

List<string> words = [];

try
{
    using (StreamReader sr = new StreamReader("C:\\Users\\HFGF\\source\\repos\\H2-Kode\\FiveWordsFiveLetters\\words_alpha.txt"))
    {
        string line;
        while ((line = sr.ReadLine()) != null)
        {   
            //Console.WriteLine(line);
            if (line.Length == 5)
            {
                for (int i = 0; i < 4; i++)
                {
                    for(int j = i+1; j < 5; j++)
                    {
                        if (line[i] == line[j])
                        {
                            goto EndOfWhile;
                        }
                    }
                }
                words.Add(line);
            }
        EndOfWhile:;
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
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                if (word[i] == uniqueWords[k][j])
                {
                    sameLetters++;
                }
            }
        }
        if (sameLetters == 5)
        {
            goto endOfForeach; 
        }
    }
    uniqueWords.Add(word);
    endOfForeach:;
}
Console.WriteLine("Anograms done!");
// removed anograms (words containing the same letters)
int count1 = 0;
int count2 = 0;
int count3 = 0;
int count4 = 0;
int count5 = 0;
Console.WriteLine(uniqueWords.Count);
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
                                    count5++;
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
Console.WriteLine(count5);
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