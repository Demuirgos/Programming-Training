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



List<Expression> ParseExpression(string s) {
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

    Expression ParseSubExpression(string s, ref int i) {
        if(ParseString(s, out string resString, ref i)) {
            return new Simple(resString);
        } else if(ParseInt(s, out int? resInt, ref i)) {
            int rep = resInt.Value;
            List<Expression> body = [];
            i++;
            while(i < s.Length && s[i] != ']') {
                body.Add(ParseSubExpression(s, ref i));
            }
            i++;
            return new Complex(rep, body);
        } 
        Console.WriteLine(s[i]);

        throw new UnreachableException();
    } 
    int index = 0;
    List<Expression> result = [];
    while(index < s.Length && s[index] != ']') {
        result.Add(ParseSubExpression(s, ref index));
    }
    return result;
}

string HandleExpressions(List<Expression> exprs) {
    string HandleExpression(Expression expr) {

        switch (expr)
        {
            case Simple simpleNode:
                return simpleNode.Body;
            case Complex complexNode:
                int reps = complexNode.Repetition;
                StringBuilder sb = new();
                foreach (var subExpr in complexNode.Body)
                {
                    sb.Append(HandleExpression(subExpr));
                }
                return string.Join(string.Empty, Enumerable.Repeat(sb.ToString(), reps));
                
            default:
                throw new UnreachableException();
        }
    }

    return string.Join(string.Empty, exprs.Select(expr => HandleExpression(expr)));
}

Console.WriteLine("abcabccdcdcdef" == HandleExpressions(ParseExpression("2[abc]3[cd]ef")));


public record Expression;
public record Complex(int Repetition, List<Expression> Body) : Expression {
    public override string ToString() {
        StringBuilder sb = new();
        sb.Append(Repetition);
        sb.Append("[");
        foreach (var item in Body)
        {
            sb.Append(item.ToString());
        }
        sb.Append("]");
        return sb.ToString();
    }
}
public record Simple(string Body) : Expression {
    public override string ToString() {
        return Body;
    }
}
