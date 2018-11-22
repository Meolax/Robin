using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Robin.Models;

namespace Robin.Models
{
    class Executer
    {
        public List<Proces> proceses = new List<Proces>();
        public Queue<Proces> queue = new Queue<Proces>();
        int completeProceses = 0;
        int secondKvant = 0;
        int kvant {
            get
            {
                if (secondKvant != 0)
                {
                    secondKvant = 0;
                    return secondKvant + 3;
                } else
                {
                    return 3;
                }
                 
            }
        }
        public double AvgWaitTime
        {
            get
            {
                double AvgWait = 0;
                foreach (Proces proc in proceses)
                {
                    AvgWait += proc.WaitTime;
                }
                return AvgWait / proceses.Count;
            }
        }
        public double AvgExectTime
        {
            get
            {
                double AvgExect = 0;
                foreach (Proces proc in proceses)
                {
                    AvgExect += proc.Te + proc.WaitTime;
                }
                return AvgExect / proceses.Count;
            }
        }
        public double TimeExect { get; set; } = 0;
        private int CurrentTime = 0;
        enum Status
        {
            Ready,
            Executing
        }
        Status status
        {
            get
            {
                return TimeExect <= CurrentTime ? Status.Ready : Status.Executing;
            }
        }


        #region Executing
        public Executer (List<Proces> proceses)
        {
            this.proceses = proceses;
            proceses.Sort(new ProcComarer());
        }

        public void Execute ()
        {
            int addedProcesInloop = 0;
            while (completeProceses != proceses.Count)
            {
                if (addedProcesInloop < proceses.Count)
                {
                    if (CurrentTime == proceses[addedProcesInloop].Tc)
                    {
                        queue.Enqueue(proceses[addedProcesInloop]);
                        addedProcesInloop++;
                    }
                }
                
                if (status == Status.Ready && queue.Count > 0)
                {
                    ExecuteProces();
                }                
                CurrentTime++;
            }
        }

        private void ExecuteProces ()
        {
            Proces proces = queue.Dequeue();
            if (proces.LeftTime - kvant <= 0)
            {
                secondKvant = kvant - proces.LeftTime;
                Executing(proces, proces.LeftTime);
                completeProceses++;
            } else
            {
                Executing(proces, kvant);
                queue.Enqueue(proces);
            }

        }

        public void Executing (Proces proces, int time)
        {
            TimeExect = CurrentTime + time;
            if (proces.iteration == 0)
            {
                proces.TimeWaiting.Add(CurrentTime - proces.Tc);
            }
            else
            {
                proces.TimeWaiting.Add(CurrentTime - proces.ExectTime - proces.Tc);
            }
            proces.TimeExecuting.Add(time);
            proces.LeftTime -= time;
            proces.iteration++;
        }
        #endregion
    }
}