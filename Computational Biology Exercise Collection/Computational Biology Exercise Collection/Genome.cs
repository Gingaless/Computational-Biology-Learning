using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Computational_Biology_Exercise_Collection
{
    public class Genome
    {
        private char[] seq; private int len;
        public string Sequence { get { return Useful.CharToString(seq, len); } }
        public int Length { get { return this.len; } }

        public Genome(char[] seq)
        {
            this.len = seq.Length;
            this.seq = Useful.ArrayCopy<char>(seq,seq.Length);
        }

        public Genome(string seq)
        {
            this.len = seq.Length;
            this.seq = seq.ToArray();
        }

        public static Genome ReverseStr(Genome gen)//Complementary strand
        {
            int l = gen.Length;
            char[] buf = new char[l];
            for (int i = 0; i < l; i++) { buf[i] = gen.Sequence[l - i - 1]; }
            return new Genome(buf);
        }

        public static char CompNu(char nu)//complementary nucleotide
        {
            switch (nu)
            {
                case 'A': return 'T';
                case 'G': return 'C';
                case 'C': return 'G';
                case 'T': return 'A';
                default: throw new Exception();
            }
        }

        public static Genome CompStr(Genome gen)//Complementary strand
        {
            int l = gen.Length;
            char[] buf = new char[l];
            for (int i = 0; i < l; i++) { buf[i] = CompNu(gen.Sequence[l - i - 1]); }
            return new Genome(buf);
        }
    }
}

class NotNucleotideException : Exception
{
    public NotNucleotideException():base("It has to be nucleotide.") {}
}