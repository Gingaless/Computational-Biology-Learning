using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Computational_Biology_Exercise_Collection;
using COW = Computational_Biology_Exercise_Collection.ColumnOfWeightMatrix<char>;

namespace Computational_Biology_Exercise_Collection
{
    public static class Gibbs_Sampling
    {
        public const int unit_iteration = 100000;
        public delegate Boolean EscapeCondition(string[] Seqs, int L, int W, WeightMatrixForGibbs<char> wmat, int[] setA, int seq1idx, int pa);
        public static GibbsMotifData ForStringSeq(string[] Seqs, int L, int W)//W is motif length and L is the length of one sequence.
        {
            int N = Seqs.Length;
            int[] setA = new int[N];//set of starting indices
            int pa;//previous a.
            int seq1idx;
            string seq1;
            WeightMatrixForGibbs<char> wmat;
            Random rnd = new Random();
            for (int i = 0; i < N; i++)
                setA[i] = rnd.Next(L-W+1);
            do
            {
                seq1idx = rnd.Next(N);
                seq1 = Seqs[seq1idx];
                wmat = MakeWeightMatrixForStringSeq(Seqs, N, W, seq1idx, setA);
                pa = setA[seq1idx];
                setA[seq1idx] = wmat.ArgMaxScore(seq1.ToArray<char>(), N, L);
                Console.WriteLine(Useful.CharToString(wmat.MotifMax(), W));
                Console.WriteLine(CutString(seq1, setA[seq1idx], setA[seq1idx] + W-1));
                Console.ReadLine();
            } while ((setA[seq1idx] != pa)||(Useful.CharToString(wmat.MotifMax(),W) != CutString(seq1, setA[seq1idx], setA[seq1idx]+W-1)));
            //(Useful.CharToString(wmat.MotifMax(),W) != CutString(seq1, setA[seq1idx], setA[seq1idx]+W-1))
            //위 조건은 좀 위험한게, shungke처럼 정확한 motif가 있으면 상관없는데, weak motif일경우 조건을 절대로 만족할 수 없게 될지도...
            StringBuilder motif = new StringBuilder();
            for (int i = 0; i < W; i++)
            {
                motif.Append(seq1[pa + i]);
            }
            return new GibbsMotifData(motif.ToString(), wmat, Seqs, N, L, W);
        }

        public static string CutString(string seq, int first, int last)
        {
            StringBuilder motif = new StringBuilder();
            for (int i = first; i <= last; i++)
            {
                motif.Append(seq[i]);
            }
            return motif.ToString();
        }

        public static Boolean ConvergenceTestBySetA(int[] p, int[] r, int N)//p = previous, r = recent.
        {
            for (int i=0;i<N;i++)
            {
                if (p[i] != r[i]) return false;
            }
            return true;
        }

        public static WeightMatrixForGibbs<char> MakeWeightMatrixForStringSeq(string[] Seqs, int N, int W, int seq1idx, int[] setA)
        {
            WeightMatrixForGibbs<char> wmat = new WeightMatrixForGibbs<char>(W);
            for (int i=0; i<N; i++)
            {
                if (i == seq1idx) continue;
                for (int j = 0; j < W; j++)
                    wmat[j].AddEntry(Seqs[i][setA[i] + j]);
            }
            return wmat;
        }
    }

    public class GibbsMotifData
    {
        private int _N; private int _L; private int _W;
        private string _motif;
        public string[] Seqs;
        public int N { get { return _N; } }
        public int L { get { return _L; } }
        public int W { get { return _W; } }
        public string Motif { get { return _motif; } }
        public WeightMatrixForGibbs<char> WeightMatrix;
        
        public GibbsMotifData(string motif, WeightMatrixForGibbs<char> weightMatrix, string[] Seqs,int N, int L, int W)
        {
            _N = N;
            _W = W;
            _L = L;
            _motif = motif;
            WeightMatrix = weightMatrix;
            this.Seqs = Seqs;
        } 
    }

    public class WeightMatrixForGibbs<T>
    {
        private ColumnOfWeightMatrix<T>[] columns;
        private int W;//width;

        public ColumnOfWeightMatrix<T> this[int idx]
        { get { if (idx >= columns.Length) throw new IndexOutOfRangeException(); else return columns[idx]; } }

        public int MotifWidth { get { return W; } }

        public WeightMatrixForGibbs(int motif_width)
        {
            W = motif_width; columns = new ColumnOfWeightMatrix<T>[W];
            for (int i = 0; i < W; i++)
                columns[i] = new ColumnOfWeightMatrix<T>();
        }

        public double[] ScoreSeq(T[] seq1, int N, int L)
        {
            double[] scores = new double[L - W + 1];
            int range = L - W + 1;
            for (int i=0; i<L-W + 1; i++)
            {
                scores[i] = 1;
                for (int j = 0; j < W; j++)
                    scores[i] *= columns[j][seq1[i + j]];
            }
            return scores;
        }

        public int ArgMaxScore(T[] seq1, int N, int L)
        {
            double[] scores = ScoreSeq(seq1,N,L);

            int argMax = Enumerable.Range(0, scores.Length).Aggregate((idxOfmax, i)=>scores[i] > scores[idxOfmax] ? i : idxOfmax);
            return argMax;
        }

        public T[] MotifMax()
        {
            T[] motif = new T[W];
            for (int i = 0; i < W; i++) motif[i] = this[i].ArgMax();
            return motif;
        }
    }

    public class ColumnOfWeightMatrix<T>
    {
        private Dictionary<T, int> entries;
        private int nF = 0; //normalizing factor.

        public ColumnOfWeightMatrix() { entries = new Dictionary<T, int>(); }

        public double this[T idx]
        {
            get {
                if (nF == 0) return 0;
                if (entries.ContainsKey(idx)) return (double)entries[idx]/(double)nF;
                else
                    return 0;
            }
        }

        public Dictionary<T, int>.KeyCollection Keys { get { return entries.Keys; } }

        public void AddEntry(T idx)
        {
            if (entries.ContainsKey(idx)) entries[idx]++;
            else entries.Add(idx, 1);
            nF++;
        }

        public T ArgMax()
        {
            return Keys.Aggregate((keymax, key) => this[key] > this[keymax] ? key : keymax);
        }


    }
}
