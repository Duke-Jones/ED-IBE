using System;
using System.Collections.Generic;

namespace IBE.Enums_and_Utility_Classes

{
    public class TestSqlLikeFunction
    {
        static void Main(string[] args)
        {
            TestSqlLikePattern(true, "%", "");
            TestSqlLikePattern(true, "%", " ");
            TestSqlLikePattern(true, "%", "asdfa asdf asdf");
            TestSqlLikePattern(true, "%", "%");
            TestSqlLikePattern(false, "_", "");
            TestSqlLikePattern(true, "_", " ");
            TestSqlLikePattern(true, "_", "4");
            TestSqlLikePattern(true, "_", "C");
            TestSqlLikePattern(false, "_", "CX");
            TestSqlLikePattern(false, "[ABCD]", "");
            TestSqlLikePattern(true, "[ABCD]", "A");
            TestSqlLikePattern(true, "[ABCD]", "b");
            TestSqlLikePattern(false, "[ABCD]", "X");
            TestSqlLikePattern(false, "[ABCD]", "AB");
            TestSqlLikePattern(true, "[B-D]", "C");
            TestSqlLikePattern(true, "[B-D]", "D");
            TestSqlLikePattern(false, "[B-D]", "A");
            TestSqlLikePattern(false, "[^B-D]", "C");
            TestSqlLikePattern(false, "[^B-D]", "D");
            TestSqlLikePattern(true, "[^B-D]", "A");
            TestSqlLikePattern(true, "%TEST[ABCD]XXX", "lolTESTBXXX");
            TestSqlLikePattern(false, "%TEST[ABCD]XXX", "lolTESTZXXX");
            TestSqlLikePattern(false, "%TEST[^ABCD]XXX", "lolTESTBXXX");
            TestSqlLikePattern(true, "%TEST[^ABCD]XXX", "lolTESTZXXX");
            TestSqlLikePattern(true, "%TEST[B-D]XXX", "lolTESTBXXX");
            TestSqlLikePattern(true, "%TEST[^B-D]XXX", "lolTESTZXXX");
            TestSqlLikePattern(true, "%Stuff.txt", "Stuff.txt");
            TestSqlLikePattern(true, "%Stuff.txt", "MagicStuff.txt");
            TestSqlLikePattern(false, "%Stuff.txt", "MagicStuff.txt.img");
            TestSqlLikePattern(false, "%Stuff.txt", "Stuff.txt.img");
            TestSqlLikePattern(false, "%Stuff.txt", "MagicStuff001.txt.img");
            TestSqlLikePattern(true, "Stuff.txt%", "Stuff.txt");
            TestSqlLikePattern(false, "Stuff.txt%", "MagicStuff.txt");
            TestSqlLikePattern(false, "Stuff.txt%", "MagicStuff.txt.img");
            TestSqlLikePattern(true, "Stuff.txt%", "Stuff.txt.img");
            TestSqlLikePattern(false, "Stuff.txt%", "MagicStuff001.txt.img");
            TestSqlLikePattern(true, "%Stuff.txt%", "Stuff.txt");
            TestSqlLikePattern(true, "%Stuff.txt%", "MagicStuff.txt");
            TestSqlLikePattern(true, "%Stuff.txt%", "MagicStuff.txt.img");
            TestSqlLikePattern(true, "%Stuff.txt%", "Stuff.txt.img");
            TestSqlLikePattern(false, "%Stuff.txt%", "MagicStuff001.txt.img");
            TestSqlLikePattern(true, "%Stuff%.txt", "Stuff.txt");
            TestSqlLikePattern(true, "%Stuff%.txt", "MagicStuff.txt");
            TestSqlLikePattern(false, "%Stuff%.txt", "MagicStuff.txt.img");
            TestSqlLikePattern(false, "%Stuff%.txt", "Stuff.txt.img");
            TestSqlLikePattern(false, "%Stuff%.txt", "MagicStuff001.txt.img");
            TestSqlLikePattern(true, "%Stuff%.txt", "MagicStuff001.txt");
            TestSqlLikePattern(true, "Stuff%.txt%", "Stuff.txt");
            TestSqlLikePattern(false, "Stuff%.txt%", "MagicStuff.txt");
            TestSqlLikePattern(false, "Stuff%.txt%", "MagicStuff.txt.img");
            TestSqlLikePattern(true, "Stuff%.txt%", "Stuff.txt.img");
            TestSqlLikePattern(false, "Stuff%.txt%", "MagicStuff001.txt.img");
            TestSqlLikePattern(false, "Stuff%.txt%", "MagicStuff001.txt");
            TestSqlLikePattern(true, "%Stuff%.txt%", "Stuff.txt");
            TestSqlLikePattern(true, "%Stuff%.txt%", "MagicStuff.txt");
            TestSqlLikePattern(true, "%Stuff%.txt%", "MagicStuff.txt.img");
            TestSqlLikePattern(true, "%Stuff%.txt%", "Stuff.txt.img");
            TestSqlLikePattern(true, "%Stuff%.txt%", "MagicStuff001.txt.img");
            TestSqlLikePattern(true, "%Stuff%.txt%", "MagicStuff001.txt");
            TestSqlLikePattern(true, "_Stuff_.txt_", "1Stuff3.txt4");
            TestSqlLikePattern(false, "_Stuff_.txt_", "1Stuff.txt4");
            TestSqlLikePattern(false, "_Stuff_.txt_", "1Stuff3.txt");
            TestSqlLikePattern(false, "_Stuff_.txt_", "Stuff3.txt4");

            Console.ReadKey();
        }

        public static void TestSqlLikePattern(bool expectedResult, string pattern, string testString)
        {
            bool result = testString.SqlLike(pattern);
            if (expectedResult != result)
            {
                Console.ForegroundColor = ConsoleColor.Red; System.Console.Out.Write("[SqlLike] FAIL");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Green; Console.Write("[SqlLike] PASS");
            }
            Console.ForegroundColor = ConsoleColor.White; Console.WriteLine(": \"" + testString + "\" LIKE \"" + pattern + "\" == " + expectedResult);
        }
    }

    public static class SqlLikeStringExtensions
    {
        public static bool SqlLike(this string s, string pattern)
        {
            return SqlLikeStringUtilities.SqlLike(pattern, s);
        }
    }

    public static class SqlLikeStringUtilities
    {
        public static bool SqlLike(string pattern, string str)
        {
            bool isMatch = true,
                isWildCardOn = false,
                isCharWildCardOn = false,
                isCharSetOn = false,
                isNotCharSetOn = false,
                endOfPattern = false;
            int lastWildCard = -1;
            int patternIndex = 0;
            List<char> set = new List<char>();
            char p = '\0';

            for (int i = 0; i < str.Length; i++)
            {
                char c = str[i];
                endOfPattern = (patternIndex >= pattern.Length);
                if (!endOfPattern)
                {
                    p = pattern[patternIndex];

                    if (!isWildCardOn && p == '%')
                    {
                        lastWildCard = patternIndex;
                        isWildCardOn = true;
                        while (patternIndex < pattern.Length &&
                            pattern[patternIndex] == '%')
                        {
                            patternIndex++;
                        }
                        if (patternIndex >= pattern.Length) p = '\0';
                        else p = pattern[patternIndex];
                    }
                    else if (p == '_')
                    {
                        isCharWildCardOn = true;
                        patternIndex++;
                    }
                    else if (p == '[')
                    {
                        if (pattern[++patternIndex] == '^')
                        {
                            isNotCharSetOn = true;
                            patternIndex++;
                        }
                        else isCharSetOn = true;

                        set.Clear();
                        if (pattern[patternIndex + 1] == '-' && pattern[patternIndex + 3] == ']')
                        {
                            char start = char.ToUpper(pattern[patternIndex]);
                            patternIndex += 2;
                            char end = char.ToUpper(pattern[patternIndex]);
                            if (start <= end)
                            {
                                for (char ci = start; ci <= end; ci++)
                                {
                                    set.Add(ci);
                                }
                            }
                            patternIndex++;
                        }

                        while (patternIndex < pattern.Length &&
                            pattern[patternIndex] != ']')
                        {
                            set.Add(pattern[patternIndex]);
                            patternIndex++;
                        }
                        patternIndex++;
                    }
                }

                if (isWildCardOn)
                {
                    if (char.ToUpper(c) == char.ToUpper(p))
                    {
                        isWildCardOn = false;
                        patternIndex++;
                    }
                }
                else if (isCharWildCardOn)
                {
                    isCharWildCardOn = false;
                }
                else if (isCharSetOn || isNotCharSetOn)
                {
                    bool charMatch = (set.Contains(char.ToUpper(c)));
                    if ((isNotCharSetOn && charMatch) || (isCharSetOn && !charMatch))
                    {
                        if (lastWildCard >= 0) patternIndex = lastWildCard;
                        else
                        {
                            isMatch = false;
                            break;
                        }
                    }
                    isNotCharSetOn = isCharSetOn = false;
                }
                else
                {
                    if (char.ToUpper(c) == char.ToUpper(p))
                    {
                        patternIndex++;
                    }
                    else
                    {
                        if (lastWildCard >= 0) patternIndex = lastWildCard;
                        else
                        {
                            isMatch = false;
                            break;
                        }
                    }
                }
            }
            endOfPattern = (patternIndex >= pattern.Length);

            if (isMatch && !endOfPattern)
            {
                bool isOnlyWildCards = true;
                for (int i = patternIndex; i < pattern.Length; i++)
                {
                    if (pattern[i] != '%')
                    {
                        isOnlyWildCards = false;
                        break;
                    }
                }
                if (isOnlyWildCards) endOfPattern = true;
            }
            return isMatch && endOfPattern;
        }
    }
}
    