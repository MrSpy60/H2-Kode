using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Diagnostics.Metrics;

namespace FiveWordsFiveLettersCL
{
    public class FiveWordsFiveLettersCL
    {
        string _file;
        int _wordLength;
        int _wordCount;
        int _alphabetLenght = 26;
        int _Skips;
        int _maxSearch;
        public int _countSolutions = 0;
        int _countWords = 0;
        public List<Solution> solutions = new();
        int[] LetterFrequencePosition = { 16, 9, 23, 25, 22, 10, 21, 5, 24, 1, 7, 12, 15, 6, 20, 3, 2, 11, 14, 19, 13, 17, 0, 8, 18, 4};
        List<string> words = [];
        public IDictionary<int, string> bitWords = new Dictionary<int, string>();
        public List<int>[] letterArray = new List<int>[26];


        public event EventHandler<int> SearchIndex;

        protected virtual void OnUpdateSearchIndex(int e)
        {
            EventHandler<int> handler = SearchIndex;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        public event EventHandler<int> SearchMaxFound;

        protected virtual void OnMaxIndexFound(int e)
        {
            EventHandler<int> handler = SearchMaxFound;
            if (handler != null)
            {
                handler(this, e);
            }
        }


        public FiveWordsFiveLettersCL(string file, int wordCount = 5, int wordLength = 5)
        {
            this._file = file;
            this._wordCount = wordCount;
            this._wordLength = wordLength;
        }

        public Task DoWork()
        {
            return Task.Run(() =>
            {
                _Skips = _alphabetLenght - (_wordCount * _wordLength);
                //'words' contains all 5 letter words, that have no repeating letters.
                try
                {
                    using (StreamReader sr = new StreamReader(_file))
                    {
                        string? line;
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
                    return;
                }

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

                foreach (List<int> i in letterArray)
                {
                    _countWords += i.Count();
                }
                _maxSearch = 0;
                for (int i = 0; i <= _Skips; i++)
                {
                    _maxSearch += letterArray[i].Count();
                }
                OnMaxIndexFound(_maxSearch);
                // check for solutions
                _countSolutions = parallelFindSolutions(letterArray);
            });
        }

        int recursiveFindSolutionBits(List<int>[] words, int usedLetters, int dictIndex, List<(int, int)> keysUsed, Solution solution, int skips = 0)
        {
            int solutionsFound = 0;
            for (int j = dictIndex; (j < 26 && skips < 2); j++)
            {
                while ((usedLetters & (1 << j)) != 0)
                {
                    j++;
                };
                for (int i = 0; i < words[j].Count(); i++)
                {
                    if ((usedLetters & words[j][i]) == 0)
                    {
                        if (keysUsed.Count + 1 == _wordCount)
                        {
                            solution.values[keysUsed.Count] = (j, i);
                            solutions.Add(new Solution(solution.values));
                            solutionsFound++;
                            continue;
                        }
                        List<(int, int)> tempKeys = new List<(int, int)>(keysUsed);
                        tempKeys.Add((i, j));
                        solution.values[keysUsed.Count] = (j, i);
                        solutionsFound += recursiveFindSolutionBits(words, (usedLetters | words[j][i]), j + 1, tempKeys, solution, skips);
                    }
                }
                skips++;
            }
            return solutionsFound;
        }

        int parallelFindSolutions(List<int>[] letterArray)
        {
            var solutionsBag = new ConcurrentBag<int>();
            var pOpt = new ParallelOptions { MaxDegreeOfParallelism = 8 };
            var counter = 0;
            for (int j = 0; j <= _Skips; j++)
            {
                Parallel.For(0, letterArray[j].Count(), pOpt, i =>
                {
                    Solution solution = new Solution([]);
                    solution.values[0] = (j, (int)i);
                    int solutionsFound = recursiveFindSolutionBits(letterArray, letterArray[j][i], j + 1, [(j, i)], solution, j);
                    solutionsBag.Add(solutionsFound);
                    Interlocked.Increment(ref counter);
                    int tmp = counter / _maxSearch * 100;
                    OnUpdateSearchIndex(((counter*100)/_maxSearch));
                });
            }
            int solutions = 0;
            foreach (int item in solutionsBag)
            {
                solutions += item;
            }
            return solutions;
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

        public string[] printSolutions()
        {
            var output = new List<string>();
            foreach (Solution solution in solutions)
            {
                List<string> str = [];
                foreach ((int, int) value in solution.values)
                {
                    str.Add(getWord(value));
                }
                output.Add(string.Join(" ", str));
            }
            return output.ToArray();
        }
    }
    public class Solution
    {
        public (int, int)[] values = new (int, int)[5];

        public Solution((int, int)[] values)
        {
            if (values.Length == 5)
                values.CopyTo(this.values, 0);
        }
    }
}


