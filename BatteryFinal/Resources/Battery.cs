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
        public int _Minutes { set; get; }
        public int _Seconds { set; get; }
        public int Minutes {
            set {
                _Minutes = value;
                
            }
            get { return _Minutes; }
        }
        public int Seconds {
            set {
                _Seconds = value;
                _IsChange = true;
            }
            get {
                return _Seconds;
            }
        }
        public bool _IsChange { set; get; }
        public bool IsChange {
            set { _IsChange = value; }
            get {
                if (_IsChange)
                {
                    _IsChange = false;
                    return true;
                }
                return _IsChange; }
        }
        private Thread Thread;
        public Clock() {
            Thread = new Thread(() => Counter());
            Minutes = 0;
            Seconds = 0;
            IsChange = false;
        }
        public string GetFormatClock()
        {
            string min, sec;
            if (Minutes < 10)
            {
                min = "0" + Convert.ToString(Minutes);
            }
            else { min = Convert.ToString(Minutes); }

            if (Seconds < 10)
            {
                sec = "0" + Convert.ToString(Seconds);
            }
            else { sec = Convert.ToString(Seconds); }
            return min + ":" + sec;
        }
        public void Counter()
        {
            bool enabled = true;
            while (enabled)
            {
                Thread.Sleep(1000);
                Seconds++;
                if (Seconds > 59)
                {
                    Seconds = 0;
                    Minutes++;
                }
            }
        }

        public void Start()
        {
            Thread.Start();
        }
        public void Stop()
        {
            Thread.Abort();
        }
        public int OnlySeconds()
        {
            if (Seconds==0 && Minutes==0)
            {
                return 1;
            }
            return (Minutes * 60) + Seconds;
        }
    };
    public enum Level
    {
        High, Medium, Low
    }
    class Battery
    {
        public double InitialCharge { set; get; }
        public double FinalCharge { set; get; }
        public double Speed { set; get; }
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