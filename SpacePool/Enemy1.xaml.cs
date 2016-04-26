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
    public sealed partial class Enemy1 : UserControl
    {
        private List<Enemy1> enemies1;
        public double LocationX { get; set; }
        public double LocationY { get; set; }

       // private Canvas MyCanvas;

        public Enemy1(/*Canvas canvas*/)
        {
            this.InitializeComponent();
            //MyCanvas = canvas;
            Width = 25;
            Height = 50;
        }
       /* public void CreateEnemies1()
        {
            enemies1 = new List<Enemy1>();
            int enemy1Count = 72;
            int cols = 24;
            int xStartPos = 55;
            int yStartPos = 150;
            int step = 5;
            int row = 0;
            int col = 0;

            for (int i = 0; i < enemy1Count; i++)
            {
                if (i % cols == 0 && i > 0)
                {
                    row++;
                    //Debug.WriteLine("COL");
                    col = 0;
                }
                else if (i > 0)
                {
                    col++;
                }
                int x = (45 + step) * col + xStartPos;
                int y = (60 + step) * row + yStartPos;
               // Debug.WriteLine(x + " " + y);

                Enemy1 enemy1 = new Enemy1
                {
                    LocationX = x,
                    LocationY = y
                };
                enemies1.Add(enemy1);
                MyCanvas.Children.Add(enemy1);
                enemy1.SetLocation();

            }
        }
        */
        public void Move()
        {
            if (enemies1.Count > 20)
            {
                if (LocationY % 0.5 == 0)
                    LocationX = LocationX + 1;
                else
                    LocationX = LocationX - 1;
            }
            else
            {
                if (LocationY % 0.5 == 0)
                    LocationX = LocationX + 3;
                else
                    LocationX = LocationX - 3;
            }
            for (int i = 0; i < 8; i++)
            {
                if (i == 8)
                {
                    LocationY = LocationY + 1.5;
                    i = 0;
                }
            }
        }


        public void SetLocation()
        {
            SetValue(Canvas.LeftProperty, LocationX);
            SetValue(Canvas.TopProperty, LocationY);
        }
    }
}
