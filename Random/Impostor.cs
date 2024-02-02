/*Given an encoded string, return its decoded string.

The encoding rule is: k[encoded_string], where the encoded_string inside the square brackets is being repeated exactly k times. Note that k is guaranteed to be a positive integer.

You may assume that the input string is always valid; there are no extra white spaces, square brackets are well-formed, etc. Furthermore, you may assume that the original data does not contain any digits and that digits are only for those repeat numbers, k. For example, there will not be input like 3a or 2[4].

The test cases are generated so that the length of the output will never exceed 105.

 

Example 1:

Input: s = "3[a]2[bc]"
Output: "aaabcbc"
Example 2:

Input: s = "3[a2[c]]"
Output: "accaccacc"
Example 3:

Input: s = "2[abc]3[cd]ef"
Output: "abcabccdcdcdef"
 

Constraints:

1 <= s.length <= 30
s consists of lowercase English letters, digits, and square brackets '[]'.
s is guaranteed to be a valid input.
All the integers in s are in the range [1, 300]. */

using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Text;


string HandleExpression(string s) {
    bool ParseInt(string s, [NotNullWhen(true)] out int? result, ref int i)
    {
        int n = 0;
        if(!char.IsDigit(s[i])) {
            result = null; 
            return false;
        }
        while (i < s.Length && char.IsDigit(s[i]))
        {
            n = n * 10 + (s[i] - '0');
            i++;
        }
        result = n;
        return true;
    }

    bool ParseString(string s, out string result, ref int i)
    {
        var sb = new StringBuilder();
        if(!char.IsLetter(s[i])) {
            result = string.Empty; 
            return false;
        }

        while (i < s.Length && char.IsLetter(s[i]))
        {
            sb.Append(s[i]);
            i++;
        }
        result = sb.ToString();
        return true;
    }

    string ParseSubExpression(string s, ref int i) {
        if(ParseString(s, out string resString, ref i)) {
            return resString;
        } else if(ParseInt(s, out int? resInt, ref i)) {
            int rep = resInt.Value;
            StringBuilder sb = new();
            i++;
            while(i < s.Length && s[i] != ']') {
                sb.Append(ParseSubExpression(s, ref i));
            }
            i++;
            return string.Join(string.Empty, Enumerable.Repeat(sb.ToString(), rep));
        } 
        throw new UnreachableException();
    } 
    int index = 0;
    StringBuilder result = new();
    while(index < s.Length && s[index] != ']') {
        result.Append(ParseSubExpression(s, ref index));
    }
    return result.ToString();
}


Console.WriteLine("zzzyypqjkjkefjkjkefjkjkefjkjkefyypqjkjkefjkjkefjkjkefjkjkefef" == HandleExpression("3[z]2[2[y]pq4[2[jk]e1[f]]]ef"));
