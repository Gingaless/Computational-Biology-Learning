using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Computational_Biology_Exercise_Collection
{
    public static class Useful
    {
        static public void Print2DArray<T>(T[,] arr, int l1, int l2)
        {
            for (int i = 0; i < l1; i++) { for (int j = 0; j < l2; j++) Console.Write(arr[i, j].ToString() + " "); Console.WriteLine(); }
        }

        static public string ArrayToString<T>(T[] arr, int l)
        {
            StringBuilder sb = new StringBuilder("");
            for (int i = 0; i < l; i++) sb.Append(arr[i].ToString() + ", ");
            sb.Remove(l - 2, 2);
            return sb.ToString();

        }

        static public String[] Char2DArrToStringArr(char[,] arr, int l1, int l2)
        {
            StringBuilder sb;
            String[] s = new String[l1];
            for (int i = 0; i < l1; i++) { sb = new StringBuilder();  for (int j = 0; j < l2; j++) { sb.Append(arr[i,j]); } s[i] = sb.ToString(); }
            return s;
        }

        static public char[,] StringTo2DArr(string[] s)
        {
            int l1 = s.Length;
            int ini1 = s[0].Length;
            int ini2 = ini1;
            int ini3 = ini1;
            for (int i = 0; i < l1; i++) if (ini2 < s[i].Length) ini2 = s[i].Length;
            for (int i = 0; i < l1; i++) if (ini3 > s[i].Length) ini3 = s[i].Length;
            if (ini2 != ini3) throw (new IsJaggedArrException());
            int l2 = ini1;
            char[,] r = new char[l1,l2];
            for (int i = 0; i < l1; i++) for (int j = 0; j < l2; j++) r[i, j] = s[i][j];
            return r;
        }

        static public string CharToString(char[] s, int l)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < l; i++) sb.Append(s[i]);
            return sb.ToString();
        }

        static public string StringReverse(string s)
        {
            char[] arr = s.ToArray();
            Array.Reverse(arr);
            return new string(arr);
        }

        static public T[] ArrayCopy<T>(T[] ori, int l)
        {
            T[] arr = new T[l];
            for (int i = 0; i < l; i++) arr[i] = ori[i];
            return arr;
        }

    }

    class IsJaggedArrException : Exception
    {
        public IsJaggedArrException() : base("The process cannot be performed since a array is jagged.") { }
    }
}

