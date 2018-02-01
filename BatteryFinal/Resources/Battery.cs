using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Plugin.Battery;

namespace BatteryFinal.Resources
{
    public class Clock
    {
        public Generic<int> Minutes;
        public Generic<int> Seconds;
        private Thread Thread;
        public Clock() {
            Thread = new Thread(() => Counter());
            Minutes = new Generic<int>();
            Seconds = new Generic<int>();
            Minutes.Value = 0;
            Seconds.Value = 0;
       
        }
        public string GetFormatClock()
        {
            string min, sec;
            if (Minutes.Value < 10)
            {
                min = "0" + Convert.ToString(Minutes.Value);
            }
            else { min = Convert.ToString(Minutes.Value); }

            if (Seconds.Value < 10)
            {
                sec = "0" + Convert.ToString(Seconds.Value);
            }
            else { sec = Convert.ToString(Seconds.Value); }
            return min + ":" + sec;
        }
        public void Counter()
        {
            bool enabled = true;
            while (enabled)
            {
                Thread.Sleep(1000);
                Seconds.Value++;
                if (Seconds.Value > 59)
                {
                    Seconds.Value = 0;
                    Minutes.Value++;
                }
            }
        }
        public bool OnChange()
        {
            return Minutes.OnChange()||Seconds.OnChange();
        }
        public void Start()
        {
            if (!Thread.IsAlive)
            {
                Thread.Start();
            }
            
        }
        public void Stop()
        {
            if (Thread.IsAlive)
            {
                Thread.Abort();
            }
            
        }
        public int OnlySeconds()
        {
            if (Seconds.Value==0 && Minutes.Value==0)
            {
                return 1;
            }
            return (Minutes.Value * 60) + Seconds.Value;
        }
    };
    public enum Level
    {
        High, Medium, Low
    }
    class Battery
    {
        public Generic<double> InitialCharge;
        public Generic<double> ActualCharge;
        public Generic<double> Speed;
        public Generic<Level> Status;
        private Thread UpdateLevelActual;
        public Battery()
        {
            InitialCharge = new Generic<double>();
            ActualCharge = new Generic<double>();
            Speed = new Generic<double>();
            Status = new Generic<Level>();
            Speed.Value = 0;
            InitialCharge.Value = 0;
            ActualCharge.Value = 0;
        }
        public void UpdateStatus()
        {
            while (true)
            {
                if (InitialCharge.Value == ActualCharge.Value + 2)
                {
                    Status.Value = Level.High;

                }
                if (InitialCharge.Value == ActualCharge.Value + 1)
                {
                    Status.Value = Level.Medium;
                }
                if (InitialCharge.Value == ActualCharge.Value)
                {
                    Status.Value = Level.Low;
                }
            }
        }
       public void UpdateLevel()
        {
            while (true)
            {
                ActualCharge.Value = CrossBattery.Current.RemainingChargePercent;
            }
        }
        public void Start()
        {
            InitialCharge.Value = CrossBattery.Current.RemainingChargePercent;
            UpdateLevelActual = new Thread(() => UpdateLevel());
            UpdateLevelActual.Start();    

        }
        public void Stop()
        {
            UpdateLevelActual.Abort();

        }
        public void Calculate(double Seconds)
        {
            Speed.Value = ((ActualCharge.Value - InitialCharge.Value) / Seconds) * 60;
        }
    }
}