using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace SpacePool
{
    public sealed partial class Bullet : UserControl
    {
        // location in x
        public double LocationX { get; set; }
        public double LocationY { get; set; }

        
        
        private double speed = 5;

        public Bullet()
        {
            this.InitializeComponent();
        }
        public void Move()
        {
            //more speed
            
           // if (speed > MaxSpeed) speed = MaxSpeed;
            SetValue(Canvas.TopProperty, LocationY);


            // update location values
            LocationY -= speed * 10;

            UpdateLocation();
        }

        public void UpdateLocation()
        {
            SetValue(Canvas.LeftProperty, LocationX);
            SetValue(Canvas.TopProperty, LocationY);
        }
    }
}
