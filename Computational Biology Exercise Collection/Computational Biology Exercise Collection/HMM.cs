using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Computational_Biology_Exercise_Collection
{
    public class HMM
    {
        enum State {G, I }
        enum NuPair {AT, CG }
        public static void CpGIslandExampleForViterbiAlgorithm()
        {
            //Set values required.
            int l_pnt = 100;
            int [] O = new int[l_pnt];//observed polynucleotide, O.
            O[0] = (int)NuPair.AT;
            O[1] = (int)NuPair.AT;
            for (int i = 2; i < l_pnt; i++)
                O[i] = 1;
            double[,] b = { { 0.3, 0.2 }, { 0.2, 0.3 } };//emission probabilities b_j(k), j:state and k:nucleotide.
            double[] pi = { 0.99,0.01 };//initiation probabilities pi_j.
            double[,] a = { {0.99999,0.00001 }, {0.001,0.999 } };//transition probabilities a_ij, i:previous state and j:next state.
            double[] delta = new double[2];//probability of optimal parse
            int[] psi = new int[2];//the state
            double max;
            int argmax;
            double p = 0;
            int q = 0;

            //Initialization
            Console.Write("t=0 : O_0 = {0}, ", O[0]);
            for (int i = 0; i < 2; i++)
            {
                delta[i] = pi[i] * b[i, O[0]]; psi[i] = 0;
                q = (delta[0] > delta[1]) ? 0 : 1;
                Console.Write("delta_0({0}) = {1}, psi_0({0}) = {2}, q_0* = {3}.", i,delta[i], psi[i],q);

            }
            Console.WriteLine();

            //Recursion
            for (int t = 1; t<l_pnt; t++)
            {
                Console.Write("t={0}: O_t = {1}, ", t, O[t]);
                double[] buf = new double[2];
                for (int j = 0; j<2; j++)
                {
                    max = 0; argmax = 0; buf[j] = 0;
                    for (int i = 0; i<2; i++)
                    {
                        buf[j] = delta[i] * a[i, j];
                        if (buf[j] > max)
                        {
                            max = buf[j];
                            argmax = i;
                        }
                    }
                    psi[j] = argmax;
                    buf[j] = max * b[j, O[t]];
                    q = (delta[0] > delta[1]) ? 0 : 1;
                    Console.Write("delta_{0}({1}) = {2}, psi_{0}({1}) = {3}, q_{0}* = {4}.",t,j, buf[j],psi[j], q);
                }
                delta = buf;
                Console.WriteLine();
            }
            
            //Termination.
            for (int i=0; i<2; i++)
            {
                if (delta[i] > p)
                {
                    p = delta[i];
                    q = i;
                }
            }
            Console.WriteLine("p* = {0}, q* = {1}.", p, q);
        }
    }
}
