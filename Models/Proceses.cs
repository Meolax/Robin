using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Robin.Models
{
    public class Proces
    {
        private readonly static string ArgumentError = "Argument error!";
        public int iteration { get; set; } = 0;
        public int ID { get; }
        public int Tc { get; }
        public int kvant = 3;
        public int Te { get; }
        public int LeftTime{get; set;}
        public static List<int> TcList = new List<int>();

        public List<int> TimeWaiting = new List<int>();
        public List<int> TimeExecuting = new List<int>();

        public int WaitTime {
            get
            {
                int sum = 0;
                foreach (int value in TimeWaiting)
                {
                    sum += value;
                }
                return sum;
            }
        }

        public int ExectTime  {
            get
            {
                int sum = 0;
                foreach (int value in TimeExecuting)
                {
                    sum += value;
                }
                sum += WaitTime;
                return sum;
            }
            
        }

        public Proces (int ID, int Tc, int Te)
        {
            if (Tc < 0 || Te <= 0 || TcList.Contains(Tc))
            {
                throw new Exception($"public Proces (int {ID}, double {Tc}, double {Te}) -> "+ArgumentError);
            }
            this.ID = ID;
            this.Tc = Tc+1;
            TcList.Add(Tc);
            this.Te = Te;
            LeftTime = Te;
        }        
    }

    public class ProcComarer: IComparer<Proces>
    {
        public int Compare (Proces x, Proces y)
        {
            return x.Tc > y.Tc ? 1 : x.Tc < y.Tc ? -1 : 0;
        }
    }
}
