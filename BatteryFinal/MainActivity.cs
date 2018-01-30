using Android.App;
using Android.Widget;
using Android.OS;

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

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView (Resource.Layout.Main);
            FinalCharge=FindViewById<TextView>(Resource.Id.FinalCharge);
             FinalChargeValue = FindViewById<TextView>(Resource.Id.FinalChargerValue);
            InitialValue = FindViewById<TextView>(Resource.Id.InitialCharge);
            InitialChargeValue = FindViewById<TextView>(Resource.Id.InitialChargerValue);
            PercentByMinute = FindViewById<TextView>(Resource.Id.PercentByMinute);
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
            
        }

        private void Cancel_Click(object sender, System.EventArgs e)
        {
            System.Environment.Exit(0);
        }
    }
}

