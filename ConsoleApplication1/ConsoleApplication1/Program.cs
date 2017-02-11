using System;
// you can also use other imports, for example:
using System.Collections.Generic;
using System.Linq;



class Solution
{
    public int solution(int A, int B)
    {
        //I suspect this would be best solved with some implementation of Dijkstra or A*
        //Unfortunately I don't know how to implement those off the top of my head, so here we go 
        int posX = 0;
        int posY = 0;
        int moveNum = 0;
        do
        {
            if (moveNum > 1000000000)
            {
                return -2;
            }
            int xSign = 1;
            int ySign = 1;
            if (A - posX < 0)
            {
                xSign = -1;
            }
            if (B - posY < 0)
            {
                ySign = -1;
            }
            if (Math.Abs(A - posX) > B - Math.Abs(posY))
            {
                posX += 2 * xSign;
                posY += 1 * ySign;
                Console.WriteLine(posX);
                Console.WriteLine(posY);
            }
            else
            {
                posX += 1 * xSign;
                posY += 2 * ySign;
                Console.WriteLine(posX);
                Console.WriteLine(posY);
            }

            moveNum++;
            if (posX == A && posY == B)
            {
                return moveNum;
            }
        } while (true);
        return moveNum;
    }
}


class Solution2
{
    public int solution(string S)
    {
        int n = S.Length;
        //if string has less than two members, it cannot be divided
        if (n < 2)
        {
            return 0;
        }
        int[] openingIndex = new int[S.Length];
        int[] closingIndex = new int[S.Length];
        if (S[0] == '(')
        {
            openingIndex[0] = 1;
        }
        for (int i = 1; i < n; i++)
        {
            if (S[i] == '(')
            {
                openingIndex[i] = openingIndex[i - 1] + 1;
            }
            else
            {
                openingIndex[i] = openingIndex[i - 1];
            }
            if (S[n - 1 - i] == ')')
            {
                closingIndex[n - 1 - i] = closingIndex[n - i] + 1;
            }
            else
            {
                closingIndex[n - 1 - i] = closingIndex[n - i];
            }
        }
        for (int i = 0; i < n; i++)
        {
            if (openingIndex[i] == closingIndex[i + 1])
            {
                return i + 1;
            }
        }

        return 0;
        // write your code in C# 6.0 with .NET 4.5 (Mono)
    }
}
