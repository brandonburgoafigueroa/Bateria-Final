using Android.App;
using Android.Widget;
using Android.OS;
using BatteryFinal.Resources;
using System.Threading;
using System;
namespace BatteryFinal
{
    [Activity(Label = "BatteryFinal", MainLauncher = true, Icon = "@drawable/icon", Immersive =true)]
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
        Thread UI;
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
            Timer = new Clock();
            SetContentView (Resource.Layout.Main);
            BatteryManager = new Battery();
            FinalCharge=FindViewById<TextView>(Resource.Id.FinalCharge);
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
            Cancel.Click += Cancel_Click;
            Start.Click += Start_Click;
            
        }

        private void Start_Click(object sender, System.EventArgs e)
        {
            /*Battery= new Thread(BatteryManager.Start);
            Battery.Start();*/
            UI = new Thread(() => UpdateUI());
            UI.Start();
            
        }
        private void UpdateTime()
        {
            while (true)
            {
                if (Timer.Seconds.OnChange())
                {
                    RunOnUiThread(() => {

                        TimeValue.Text = Timer.GetFormatClock();
                    });
                }
            }
        }

        private void UpdateBattery()
        {
            string Now = "";
            while (true)
            {

                try
                {
                    Now = BatteryManager.FinalCharge.ToString();
                if (!FinalChargeValue.Text.Equals(Now))
                {
                    RunOnUiThread(() =>
                    {
                        InitialChargeValue.Text = BatteryManager.InitialCharge.ToString();
                        FinalChargeValue.Text = Now;
                    });
                }
                }
                catch { }
            }
        }
        private void UpdatePresision()
        {
            string Now = "";
            while (true)
            {
                try
                {
                    Now = BatteryManager.Status.ToString();
                    if (!PrecisionValue.Text.Equals(Now))
                    {
                        RunOnUiThread(() =>
                        {
                            PrecisionValue.Text = Now;
                        });
                    }
                }
                catch { }
            }
        }
        private void UpdateSpeed()
        {
            string Now = "";
            try
            {
                while (true)
                {
                    Now = BatteryManager.Speed.ToString();
                    if (!PercentByMinute.Text.Equals(Now))
                    {
                        RunOnUiThread(() =>
                        {
                            PercentByMinute.Text = Now+("% / Min");
                        });
                    }
                }
            }
            catch { }
        }
        private void UpdateUI()
        {
             TimeTh= new Thread(() => UpdateTime());
            Timer.Start();
            TimeTh.Start();
             /* ChargeNow = new Thread(() => UpdateBattery());
              ChargeNow.Start();
              Pressision=new Thread(() => UpdatePresision());
              Pressision.Start();
              Speed = new Thread(() => UpdateSpeed());
              Speed.Start();*/
        }
        private void DestroyThreads()
        {
            try
            {
                TimeTh.Suspend();
                TimeTh.Abort();
                ChargeNow.Suspend();
                ChargeNow.Abort();
                Pressision.Suspend();
                Pressision.Abort();
                Speed.Suspend();
                Speed.Abort();
                UI.Abort();
                Battery.Abort();
            }
            catch {

            }
        }
        
        private void Cancel_Click(object sender, System.EventArgs e)
        {
            DestroyThreads();
            System.Environment.Exit(0);
        }
    }
}

