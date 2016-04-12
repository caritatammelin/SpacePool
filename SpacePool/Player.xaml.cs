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
    public sealed partial class Player : UserControl
    {
        
        

        // location in x
        public double LocationX { get; set; }
        public double LocationY { get; set; }

        //speed
        private readonly double MaxSpeed = 50.0;
        private readonly double Accelerate = 1.0;
        private double speed;


        public Player()
        {
            this.InitializeComponent();

            
        }

        public void Move( int direction)
        {
            //more speed
            speed += Accelerate;
            if (speed > MaxSpeed) speed = MaxSpeed;
            SetValue(Canvas.TopProperty, LocationY);
            

            // update location values
            LocationX -= 10 * direction;

            UpdateLocation();

            
        }
        public void UpdateLocation()
        {
            SetValue(Canvas.LeftProperty, LocationX);
            SetValue(Canvas.TopProperty, LocationY);
        }
    }
}
