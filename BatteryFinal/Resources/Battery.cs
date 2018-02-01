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
        public double InitialCharge { set; get; }
        public double _FinalCharge { set; get; }
        public bool ActualChargeOnChange { set; get; }
        public double FinalCharge { set { } get { return _FinalCharge; } }
        public double _Speed { set; get; }
        public double Speed { set; get; }
        public Level _Status { set; get; }
        public Level Status { set; get; }
        private Thread UpdateLevelActual;
        public Battery()
        {
            Speed = 0;
            InitialCharge = 0;
            FinalCharge = 0;
        }
        public void UpdateLevel()
        {
            while(true)
            { 
            FinalCharge = CrossBattery.Current.RemainingChargePercent;
            Thread.Sleep(1000);
            }
        }
        public void Start()
        {
            InitialCharge = CrossBattery.Current.RemainingChargePercent;
            bool active = true;
            UpdateLevelActual = new Thread(() => UpdateLevel());
            UpdateLevelActual.Start();    
            while(active)
            {
                if (InitialCharge == FinalCharge + 2)
                {
                    Status = Level.High;
                    
                }
                if (InitialCharge == FinalCharge + 1)
                {
                    Status = Level.Medium;
                } 
                if (InitialCharge == FinalCharge)
                {
                    Status = Level.Low;
                }
               
            }
        }
        public void Stop()
        {
            UpdateLevelActual.Abort();

        }
        public void Calculate(double Seconds)
        {
            Speed = ((FinalCharge - InitialCharge) / Seconds) * 60;
        }
    }
}