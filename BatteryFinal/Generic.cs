using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace BatteryFinal
{
    public class Generic<T>
    {
        private T _Value { set; get; }
        public
            T Value { set {
                _Value = value;
                IsChange = true;
            }
            get {
                return _Value;
            }
        }
        private bool IsChange { set; get; } = false;
        public bool OnChange()
        {
            if (IsChange)
            {
                IsChange = false;
                return true;
            }
            return false;
        }
        public Generic()
        {
             
        }
            
            
    }
    
}