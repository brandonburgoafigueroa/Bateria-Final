using Android.App;
using Android.Widget;
using Android.OS;
using BatteryFinal.Resources;
using System.Threading;
using System;
namespace BatteryFinal
{
    [Activity(Label = "Velocimetro de bateria", MainLauncher = true, Icon = "@drawable/icon", Immersive = true)]
    public class MainActivity : Activity
    {
        TextView FinalCharge;
        TextView FinalChargeValue;
        TextView InitialValue;
        TextView InitialChargeValue;
        TextView PercentByMinute;
        TextView PrecisionValue;
        TextView PresissionText;
        TextView TimeValue;
        TextView Time;
        Button Cancel;
        Button Start;
        Battery BatteryManager;
        Thread Battery;
        Thread TimeTh;
        Thread ChargeNow;
        Thread Pressision;
        Thread Speed;
        Clock Timer;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            
            SetContentView(Resource.Layout.Main);
            LoadElements();
            LoadEvents();
            LoadThreads();
        }
        private void LoadEvents()
        {
            Cancel.Click += Cancel_Click;
            Start.Click += Start_Click;
        }
        private void LoadElements()
        {
            Timer = new Clock();
            BatteryManager = new Battery();
            FinalCharge = FindViewById<TextView>(Resource.Id.FinalCharge);
            FinalChargeValue = FindViewById<TextView>(Resource.Id.FinalChargerValue);
            InitialValue = FindViewById<TextView>(Resource.Id.InitialCharge);
            InitialChargeValue = FindViewById<TextView>(Resource.Id.InitialChargerValue);
            PercentByMinute = FindViewById<TextView>(Resource.Id.PercentByMinuteValue);
            PrecisionValue = FindViewById<TextView>(Resource.Id.PrecisionValue);
            PresissionText = FindViewById<TextView>(Resource.Id.PresissionText);
            TimeValue = FindViewById<TextView>(Resource.Id.TimeValue);
            Time = FindViewById<TextView>(Resource.Id.Time);
            Cancel = FindViewById<Button>(Resource.Id.Cancel);
            Start = FindViewById<Button>(Resource.Id.Start);
        }
        private void LoadTexts()
        {
            RunOnUiThread(() =>
            {
                TimeValue.Text = Timer.GetFormatClock();
                InitialChargeValue.Text = BatteryManager.InitialCharge.Value.ToString();
                FinalChargeValue.Text = BatteryManager.ActualCharge.Value.ToString();
                PrecisionValue.Text = BatteryManager.Status.Value.ToString();
                PercentByMinute.Text = BatteryManager.Speed.Value.ToString() + ("% / Min");

            });
        }
        private void Start_Click(object sender, System.EventArgs e)
        {    
            Battery.Start();
            UpdateUI();
            Timer.Start();
        }
        private void UpdateTime()
        {
            while (true)
            {
                if (Timer.Seconds.OnChange())
                {
                    RunOnUiThread(() =>
                    {
                        TimeValue.Text = Timer.GetFormatClock();
                    });
                }
            }
        }

        private void UpdateBattery()
        {
            while (true)
                {
                if (BatteryManager.ActualCharge.OnChange()) { 
                    RunOnUiThread(() =>
                    {
                        InitialChargeValue.Text = BatteryManager.InitialCharge.Value.ToString();
                        FinalChargeValue.Text = BatteryManager.ActualCharge.Value.ToString();
                    });
                }
            }
            
        }
        private void UpdatePresision()
        {
            
            while (true)
            {
                if (BatteryManager.ActualCharge.OnChange())
                {
                    BatteryManager.UpdateStatus();
                    RunOnUiThread(() =>
                    {
                        PrecisionValue.Text = BatteryManager.Status.Value.ToString();
                    });
                }
            }
        }
        private void UpdateSpeed()
        {
            while (true)
            {
                if (BatteryManager.ActualCharge.OnChange())
                {
                    BatteryManager.Calculate(Timer.OnlySeconds());
                    RunOnUiThread(() =>
                    {
                        PercentByMinute.Text = BatteryManager.Speed.Value.ToString() + ("% / Min");
                    });
                }
            }
        }
     private void LoadThreads()
        {
            TimeTh = new Thread(() => UpdateTime());
            ChargeNow = new Thread(() => UpdateBattery());
            Pressision = new Thread(() => UpdatePresision());
            Speed = new Thread(() => UpdateSpeed());
            Battery = new Thread(BatteryManager.Start);
        }
        private void StartThreads()
        {
            TimeTh.Start();
            TimeTh.Priority = System.Threading.ThreadPriority.Highest;
            ChargeNow.Start();
            Pressision.Start();
            Speed.Start();
        }
        private void UpdateUI()
        {
            LoadTexts();
            StartThreads();
        }
        private void DestroyThreads()
        {
            if (TimeTh.IsAlive)
            { TimeTh.Abort(); }

            if (ChargeNow.IsAlive)
            { ChargeNow.Abort(); }

            if (Pressision.IsAlive)
            { Pressision.Abort(); }

            if (Speed.IsAlive)
            { Speed.Abort(); }

            if (Battery.IsAlive)
            { Battery.Abort(); }
        }
        
        private void Cancel_Click(object sender, System.EventArgs e)
        {
            DestroyThreads();
            System.Environment.Exit(0);
        }
    }
}

