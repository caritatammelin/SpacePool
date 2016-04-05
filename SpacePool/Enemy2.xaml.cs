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
    public sealed partial class Enemy2 : UserControl
    {
        public double LocationX { get; set; }
        public double LocationY { get; set; }

        public Enemy2()
        {
            this.InitializeComponent();
            Width = 70;
            Height = 100;

        }

        // rect for enemy2
        public Rect GetRect()
        {
            return new Rect(LocationX, LocationY, Height, Width);
        }

        // method to set the enemy on canvas
        public void SetLocation()
        {
            SetValue(Canvas.LeftProperty, LocationX);
            SetValue(Canvas.TopProperty, LocationY);
        }
    }
}
