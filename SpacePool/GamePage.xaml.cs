﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.System;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace SpacePool
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class GamePage : Page
    {
        private Player player;
        private Enemy1 enemy1;
        private DispatcherTimer timer;
        private DispatcherTimer etimer;
        

        // enemies
        private List<Enemy1> enemies1;
        private List<Enemy2> enemies2;
        SpacePool.Enemy1 viholline = null;
        SpacePool.Bullet ammus = null;

        // audio
        private MediaElement mediaElement;
        private MediaElement shootElement;

        // location
        public double LocationX { get; set; }
        public double LocationY { get; set; }
        

        // canvas width and height (used to randomize a new flower)
        private double CanvasWidth;
        private double CanvasHeight;

        //keys
        private bool SpacePressed;
        private bool LeftPressed;
        private bool RightPressed;

        int score = 0;

        List<Bullet> bullets = new List<Bullet>();
        public GamePage()
        {
            this.InitializeComponent();


            // change the default startup mode
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;
            // specify the size
            ApplicationView.PreferredLaunchViewSize = new Size(1280, 720);


            // key listeners
            Window.Current.CoreWindow.KeyDown += CoreWindow_KeyDown;
            Window.Current.CoreWindow.KeyUp += CoreWindow_KeyUp;

            // get canvas width and height
            CanvasWidth = MyCanvas.Width;
            CanvasHeight = MyCanvas.Height;

            // add player
            player = new Player
            {
                LocationX = CanvasWidth/2,
                LocationY = 600
            
            };
            // tähän vielä children tavalla pelaajan lisäys peliin
            MyCanvas.Children.Add(player);

            // load audio elements into game
            LoadAudio();
            mediaElement.Play();

            CreateEnemies1();
            CreateEnemies2();
            

            player.UpdateLocation();

            // game loop
            timer = new DispatcherTimer();
            timer.Tick += Timer_Tick;
            timer.Interval = new TimeSpan(0, 0, 0, 0, 1000 / 60);
            timer.Start();

            etimer = new DispatcherTimer();
            etimer.Tick += Timer_Tick1;
            etimer.Interval = new TimeSpan(0, 0, 0,0,1000/10);
            etimer.Start();


        }
        
        

        // audio elements
        public async void LoadAudio()
        {
            mediaElement = new MediaElement();
            mediaElement.AutoPlay = true;
            mediaElement.IsLooping = true;
            StorageFolder folder1 = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync("Assets");
            StorageFile file1 = await folder1.GetFileAsync("spaceambience.wav");
            var stream1 = await file1.OpenAsync(FileAccessMode.Read);
            mediaElement.SetSource(stream1, file1.ContentType);
            // shooting sound
            shootElement = new MediaElement();
            shootElement.AutoPlay = false;
            shootElement.IsHoldingEnabled = true;
            StorageFolder folder2 = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync("Assets");
            StorageFile file2 = await folder2.GetFileAsync("shoot.wav");
            var stream2 = await file2.OpenAsync(FileAccessMode.Read);
            shootElement.SetSource(stream2, file2.ContentType);

        }

        // creating first kind enemies as a list
        public void CreateEnemies1()
        {
            enemies1 = new List<Enemy1>();
            int enemy1Count = 60;
            int cols = 20;
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
                    Debug.WriteLine("COL");
                    col = 0;
                }
                else if (i > 0)
                {
                    col++;
                }
                int x = (45 + step) * col + xStartPos;
                int y = (60 + step) * row + yStartPos;
               Debug.WriteLine(x + " " + y);

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
        
        public void CreateEnemies2()
        {
            enemies2 = new List<Enemy2>();
            int enemy2Count = 10;
            int cols = 10;
            int xStartPos = 55;
            int yStartPos = 50;
            int step = 5;
            int row = 0;
            int col = 0;

            for (int i = 0; i < enemy2Count; i++)
            {
                if (i % cols == 0 && i > 0)
                {
                    row++;
                    Debug.WriteLine("COL");
                    col = 0;
                }
                else if (i > 0)
                {
                    col++;
                }
                int x = (95 + step) * col + xStartPos;
                int y = (120 + step) * row + yStartPos;
                Debug.WriteLine(x + " " + y);

                Enemy2 enemy2 = new Enemy2
                {
                    LocationX = x,
                    LocationY = y
                };
                enemies2.Add(enemy2);
                MyCanvas.Children.Add(enemy2);
                enemy2.SetLocation();

            }
        }
        double iii = 0;
        int dir = 1;
        private void Timer_Tick1(object sender, object e)
        {
            foreach (Enemy1 enemies in enemies1)
            {
                enemies.LocationX = enemies.LocationX + 3*dir;
                

                if (iii > 59)
                {
                    enemies.LocationY = enemies.LocationY + 40;

                }

                
                enemies.SetLocation();
            }
            Debug.WriteLine(iii);
            iii++;
            if (iii > 60)
            {
                dir = dir * -1;
                iii = 0;
            }

            /*foreach (Enemy1 enemies in enemies1)
            {
                viholline = enemies;
                if (viholline.LocationY < 500)
                    viholline.LocationY = viholline.LocationY + 1.5;
                else if (viholline.LocationY >= 500)
                    viholline.LocationY = 55;
                viholline.SetLocation();
            }*/

            foreach (Enemy2 enemies in enemies2)
            {
                enemies.LocationX = enemies.LocationX + 3 * dir;
                
                if (iii > 59)
                {
                    enemies.LocationY = enemies.LocationY + 40;

                }
                enemies.SetLocation();
            }
        }

        
        private void CoreWindow_KeyUp(Windows.UI.Core.CoreWindow sender, Windows.UI.Core.KeyEventArgs args)
        {
            switch (args.VirtualKey)
            {
                case VirtualKey.Left:
                    LeftPressed = false;
                    Debug.WriteLine("Left Pressed");
                    break;
                case VirtualKey.Right:
                    RightPressed = false;
                    break;
                case VirtualKey.Space:
                    SpacePressed = false;
                    break;
                default:
                    break;
            }
        }

        private void CoreWindow_KeyDown(Windows.UI.Core.CoreWindow sender, Windows.UI.Core.KeyEventArgs args)
        {
            switch (args.VirtualKey)
            {
                case VirtualKey.Left:
                    LeftPressed = true;
                    break;
                case VirtualKey.Right:
                    RightPressed = true;
                    break;
                case VirtualKey.Space:
                    SpacePressed = true;
                    Debug.WriteLine("Testi");
                    break;
                case VirtualKey.Up:
                    SpacePressed = false;
                    break;
                default:
                    break;
            }
        }
        
        private void Timer_Tick(object sender, object e)
        {
            scoreBlock.Text = Convert.ToString(score);
            if (SpacePressed)
            {
                Bullet bullet = new Bullet
                {
                    LocationX = player.LocationX,
                    LocationY = player.LocationY
                };
                MyCanvas.Children.Add(bullet);
                if (bullets.Count == 0)
                    bullets.Add(bullet);
                // shoot sound when shooting
                shootElement.Stop();
                shootElement.Play();

                SpacePressed = false;
                
            }
            foreach (Bullet bullet in bullets)
            {
                bullet.Move();
                bullet.UpdateLocation();

                if (bullet.LocationY < -5)
                {
                    bullets.Remove(bullet);
                    break;
                }

            }


           // enemy1.SetLocation();

            if (LeftPressed) player.Move(1);
            if (RightPressed) player.Move(-1);

            if (player.LocationX < -1) player.LocationX = 1270;
            if (player.LocationX > 1279) player.LocationX = 1;

            CheckCollision();


        }

        private void CheckCollision()
        {
            double bx = 0;
            double by = 0;
            double baw = 0;
            double bah = 0;

            double e1x = 0;
            double e1y = 0;
            double e1aw = 0;
            double e1ah = 0;

            double e2x = 0;
            double e2y = 0;
            double e2aw = 0;
            double e2ah = 0;

            

            foreach (Bullet bullet in bullets)
            {
                bx = bullet.LocationX;
                by = bullet.LocationY;
                baw = bullet.ActualWidth;
                bah = bullet.ActualHeight;
                ammus = bullet;


                foreach (Enemy1 enemies in enemies1)
                {
                    e1x = enemies.LocationX;
                    e1y = enemies.LocationY;
                    e1aw = enemies.ActualWidth;
                    e1ah = enemies.ActualHeight;
                    viholline = enemies;


                    Rect r1 = new Rect(bx, by, baw, bah);
                    Rect r2 = new Rect(e1x, e1y, e1aw, e1ah);

                    r1.Intersect(r2);

                    if (!r1.IsEmpty)
                    {
                        //Debug.WriteLine("nononon");
                        MyCanvas.Children.Remove(ammus);
                        MyCanvas.Children.Remove(viholline);

                        bullets.Remove(ammus);
                        enemies1.Remove(viholline);
                        score = score + 100;
                        return;
                    }
                }

                foreach (Enemy2 enemies in enemies2)
                {
                    e2x = enemies.LocationX;
                    e2y = enemies.LocationY;
                    e2aw = enemies.ActualWidth;
                    e2ah = enemies.ActualHeight;


                    Rect r1 = new Rect(bx, by, baw, bah);
                    Rect r2 = new Rect(e2x, e2y, e2aw, e2ah);

                    r1.Intersect(r2);

                    if (!r1.IsEmpty)
                    {
                        //Debug.WriteLine("nononon");
                        MyCanvas.Children.Remove(ammus);
                        MyCanvas.Children.Remove(enemies);

                        bullets.Remove(ammus);
                        enemies2.Remove(enemies);
                        return;
                    }
                }
            }
        }
        
    }
}
