using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSS = Computational_Biology_Exercise_Collection.Chip_Seq_Simulation;

namespace Computational_Biology_Exercise_Collection
{
    public class Test
    {

        static public void GibbsTest()
        {
            int N = 4;
            string[] seqs = new string[4] {"qwertshungkeyuiop","asdfgshungkesdfgh","ghjklshungkezxcvb", "xvcmklfshungkemny" };
            int L = 17;
            int W = 7;
            GibbsMotifData motifdata;
            motifdata = Gibbs_Sampling.ForStringSeq(seqs, L, W);
            Console.WriteLine(motifdata.Motif);
            Console.WriteLine("\nWeightMatrix : \n");
            for (int i = 0; i<W; i++)
            {
                Console.Write("{0} : ",i);
                foreach (var key in motifdata.WeightMatrix[i].Keys)
                {
                    Console.Write("({0},{1}), ",key,motifdata.WeightMatrix[i][key]);
                }
                Console.WriteLine();
                //니미럴 제대로 된 결과 안나오는거 무엇; tolerance를 높이면 비교적 슝케가 자주 나오긴 하는데;
                //converge를 잘 하도록 짜는 법을 생각해봐야게꾼.
            }
        }
        static public void AllRotaitonsTest()
        {
            string s = "abcd$";
            Useful.Print2DArray(BWT.AllRotations(s), s.Length, s.Length);
        }

        static public void ViterbiTest()
        {
            HMM.CpGIslandExampleForViterbiAlgorithm();
        }

        static public void BWTTest()
        {
            string s = "attgccatgaaatggcgcgctttttttt$";
            string q = "tg";
            Console.WriteLine(s);
            Console.WriteLine(q);
            BWT bwt = new BWT(s);
            FM_Matches fm = bwt.Match(q);
            Console.WriteLine("{0}, {1}", fm.top, fm.bot);
            for (int i=0; i<fm.num; i++)
            {
                Console.WriteLine("{0}", fm.qpos[i]);
                for (int j = 0; j < q.Length; j++)
                    Console.Write("{0}", s[fm.qpos[i]+j]);
                Console.WriteLine();
            }
                
            //s 안의 q의 갯수만큼 top과 bot의 차이가 생긴다.
        }

        static public void gentest()
        {
            Console.WriteLine(CSS.RP.GetRndGenSeq(40).Sequence);
        }
    }
}
