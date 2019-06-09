using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Computational_Biology_Exercise_Collection
{

    public struct FM_Matches { public int num;  public int top; public int bot; public int[] qpos;
        /*
        public FM_Matches(int num, int top, int bot, int[] qpos)
        {
            this.num = num; this.top = top; this.bot = bot; if (top < bot) this.qpos = Useful.ArrayCopy(qpos, num); else this.qpos = null;
        }
        */
    }

    public class BWT
    {
        internal struct BWTransformation { internal string L; internal int inputidx; internal string F; internal int[] rps; }
        internal struct SortBuffer { internal string s; internal int rp;//rp = reference position.
            internal SortBuffer(string s, int rp) { this.s = s; this.rp = rp; } } 
        
        private string inputstr;
        private BWTransformation bwt;
        private int length;

        public string L { get { return bwt.L; } }
        public string OriginalString { get { return inputstr; } }
        public int IndexForReverse { get { return bwt.inputidx; } }
        public int StringLength { get { return length; } }
        public int[] ReferencePositions { get { return Useful.ArrayCopy<int>(bwt.rps,length); } }

        public BWT(string s)
        {
            this.inputstr = s;
            this.length = s.Length;
            this.bwt = BWTransform(s);
        }

        static public char[,] AllRotations(string s)
        {
            int l = s.Length;
            char[,] r = new char[l, l];
            for (int i = 0; i < l; i++) for (int j = 0; j < l; j++) { r[i, j] = s[(j + i) % l]; }
            return r;
        }

        static public char[,] AllRotations(char[] s)
        {
            return AllRotations(s.ToString());
        }

        /*
         * string s의 모든 로테이션을 구하고, 그것들로 matrix를 구성한다음,
         * 그 matrxix의 row들을 lexically하게 정렬한 후, 각 row들의 마지막 문자들을 취한 것이 BWT이다.
         * 
          */
        static private BWTransformation BWTransform(string s)
        {
            int l = s.Length;
            char[,] ar = AllRotations(s);
            List<SortBuffer> sl = new List<SortBuffer>();
            string[] sa = new string[l];
            sa = Useful.Char2DArrToStringArr(ar,l,l);
            for (int i = 0; i < l; i++) { sl.Add(new SortBuffer(sa[i],i)); }
            char[] r = new char[l];
            int[] rps = new int[l];
            //List<string> sl = sa.ToList<string>();
            StringBuilder f = new StringBuilder();
            IEnumerable<SortBuffer> query = sl.OrderBy(sb=>sb.s);
            sl = new List<SortBuffer>();
            foreach (SortBuffer e in query ) sl.Add(e);
            //Making BWM is done.
            for (int i = 0; i < l; i++) { r[i] = sl[i].s[l - 1]; f.Append(sl[i].s[0]); rps[i] = sl[i].rp; }
            BWTransformation tr = new BWTransformation();
            tr.L = Useful.CharToString(r,l);
            tr.F = f.ToString();
            tr.inputidx = sl[0].rp;
            tr.rps = Useful.ArrayCopy<int>(rps,l);
            return tr;
        }

        static public string ReverseBWT(string s, int index)
        {
            int l = s.Length;
            List<String> sl = new List<String>();
            for (int i = 0; i < l; i++) { sl.Add(""); }
            //Console.WriteLine(sl.Count);
            //Console.WriteLine(Useful.StringReverse("ab"));
            for (int i = 0; i < l; i++)
            {
                for (int j = 0; j < l; j++)
                {
                    sl[j] = s[j] + sl[j];
                }
                sl.Sort();
            }
            return sl[index];
        }

        int Occ(char c)
        {
            return this.bwt.F.IndexOf(c);
        }

        int Count(int idx, char c)
        {
            int r = 0;
            for (int i = 0; i < idx; i++) if (this.bwt.L[i] == c) r++;
            return r;
        }

        int LF(int idx, char c)
        {
            return Occ(c) + Count(idx,c);
        }

        public FM_Matches Match(string q)
        {
            FM_Matches m = new FM_Matches();
            m.top = 0;
            m.bot = length;
            string rq = Useful.StringReverse(q);
            foreach (char qc in rq) { m.top = LF(m.top, qc); m.bot = LF(m.bot, qc); }
            if (m.top < m.bot) { m.num = m.bot - m.top; m.qpos = new int[m.num];
                for (int i = 0; i < m.num; i++) m.qpos[i] = bwt.rps[m.top + i]; }
            else { m.num = 0; m.qpos = null; };
            return m;
        }

    }
}
