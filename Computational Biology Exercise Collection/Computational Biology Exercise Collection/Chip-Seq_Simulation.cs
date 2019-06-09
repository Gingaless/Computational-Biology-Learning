using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Computational_Biology_Exercise_Collection
{
    namespace Chip_Seq_Simulation
    {
        public struct Protein_Binding { int gen_len; int bs; int _5p_eob; int _3p_eob; }
        //bs = binding site, _5p_eob = 5' end of binding, _3p_eob = 3' end of binding

        public static class RP
        {
            public static Genome GetRndGenSeq(int len)
            {
                char[] seq = new char[len];
                Random rnd = new Random();
                for (int i = 0; i < len; i++)
                {
                    switch (rnd.Next(4))
                    {
                        case 0: seq[i] = 'A'; break;
                        case 1: seq[i] = 'C'; break;
                        case 2: seq[i] = 'G'; break;
                        case 3: seq[i] = 'T'; break;
                        default: throw new Exception("Random Exception.");
                    }
                }
                return new Genome(seq);
            }

            public static int[] PBG_Sonication(Genome gen,int n, Protein_Binding[] pb)
                //n=Sonication Occur Number
            {
                int l = gen.Length;
                Random rnd = new Random();
                return new int[1] { 1 };
            }
        }
    }
}
