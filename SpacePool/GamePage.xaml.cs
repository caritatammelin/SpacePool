using System;
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
        private Random rnd = new Random();
        private Player player;
        private Enemy1 enemy1;
        private DispatcherTimer timer;
        private DispatcherTimer etimer;
        private TextBlock GameOver = new TextBlock();


        // enemies
        private List<Enemy1> enemies1;
        private List<Enemy2> enemies2;
        SpacePool.Enemy1 viholline = null;
        SpacePool.Bullet ammus = null;

        // audio
        private MediaElement mediaElement;
        private MediaElement destroyElement;
        private MediaElement gameoverElement;
        private MediaElement enshootElement;
        private MediaElement clickElement;

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

        public int score = 0;

        List<Bullet> bullets = new List<Bullet>();
        List<EnemyBullet> enemyBullets = new List<EnemyBullet>();
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

            Enemy1 enemy = enemies1[8];
            EnemyBullet ebullet = new EnemyBullet
            {
                LocationX = enemy.LocationX,
                LocationY = enemy.LocationY
            };

            MyCanvas.Children.Add(ebullet);
            enemyBullets.Add(ebullet);
            score = score + 1;

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
            enshootElement = new MediaElement();
            enshootElement.AutoPlay = false;
            StorageFolder folder2 = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync("Assets");
            StorageFile file2 = await folder2.GetFileAsync("shoot.wav");
            var stream2 = await file2.OpenAsync(FileAccessMode.Read);
            enshootElement.SetSource(stream2, file2.ContentType);
            // sound of enemies being destroyed
            destroyElement = new MediaElement();
            destroyElement.AutoPlay = false;
            StorageFolder folder3 = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync("Assets");
            StorageFile file3 = await folder3.GetFileAsync("explosion.mp3");
            var stream3 = await file3.OpenAsync(FileAccessMode.Read);
            destroyElement.SetSource(stream3, file3.ContentType);
            // game over sound
            gameoverElement = new MediaElement();
            gameoverElement.AutoPlay = false;
            StorageFolder folder4 = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync("Assets");
            StorageFile file4 = await folder4.GetFileAsync("gameover.mp3");
            var stream4 = await file4.OpenAsync(FileAccessMode.Read);
            gameoverElement.SetSource(stream4, file4.ContentType);
            // click sound
            clickElement = new MediaElement();
            clickElement.AutoPlay = false;
            StorageFolder folder5 = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync("Assets");
            StorageFile file5 = await folder5.GetFileAsync("click.wav");
            var stream5 = await file5.OpenAsync(FileAccessMode.Read);
            clickElement.SetSource(stream5, file5.ContentType);

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
               //Debug.WriteLine(x + " " + y);

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
                //Debug.WriteLine(x + " " + y);

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
                if (enemies1.Count < 1)
                    break;
                enemies.LocationX = enemies.LocationX + 3*dir;
                if (enemies1.Count < 20)
                {
                    enemies.LocationX = enemies.LocationX + 3 * dir;
                }

                if (iii > 59)
                {
                    enemies.LocationY = enemies.LocationY + 40;
                }

                if (enemies1.Count < 20)
                {
                    if (iii > 29)
                    {
                        enemies.LocationY = enemies.LocationY + 40;
                    }
                }
                enemies.SetLocation();
            }
            Debug.WriteLine(iii);
            iii++;
            if (enemies1.Count < 20)
            {
                if (iii > 30)
                {
                    dir = dir * -1;
                    iii = 0;
                }
            }
            else if (enemies1.Count > 20)
            {
                if (iii > 60)
                {
                    dir = dir * -1;
                    iii = 0;
                }
            }

            foreach (Enemy2 enemies in enemies2)
            {
                if (enemies2.Count < 1)
                    break;
                enemies.LocationX = enemies.LocationX + 3 * dir;
                if (enemies1.Count < 20)
                {
                    enemies.LocationX = enemies.LocationX + 3 * dir;
                }

                if (iii > 59)
                {
                    enemies.LocationY = enemies.LocationY + 40;

                }
                if (enemies1.Count < 20)
                {
                    if (iii > 29)
                    {
                        enemies.LocationY = enemies.LocationY + 40;
                    }
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
        private void ScoreButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(ScorePage), score);
            clickElement.Play();
        }
        int end = 0;
        private void Timer_Tick(object sender, object e)
        {
            if (end == 1)
            {
                gameoverElement.Play();
                Popup1.IsOpen = true;
                timer.Stop();
                etimer.Stop();
                High.IsOpen = true;
                Again.IsOpen = true;

            }


            scoreBlock.Text = Convert.ToString(score);

            if (enemies1.Count > 20)
            {
                if (enemies1.Count < 1)
                    return;
                if (enemyBullets.Count < 4)
                {
                    int r = rnd.Next(enemies1.Count);
                    Enemy1 enemy = enemies1[r];
                    EnemyBullet ebullet = new EnemyBullet
                    {
                        LocationX = enemy.LocationX,
                        LocationY = enemy.LocationY
                    };

                    MyCanvas.Children.Add(ebullet);
                    enemyBullets.Add(ebullet);
                    score = score + 1;
                    enshootElement.Stop();
                    enshootElement.Play();
                }
            }
            else
            {
                if (enemyBullets.Count < 2)
                {
                    if (enemies1.Count < 1)
                        return;
                    int r = rnd.Next(enemies1.Count);
                    Enemy1 enemy = enemies1[r];
                    EnemyBullet ebullet = new EnemyBullet
                    {
                        LocationX = enemy.LocationX,
                        LocationY = enemy.LocationY
                    };
                    MyCanvas.Children.Add(ebullet);
                    enemyBullets.Add(ebullet);
                    score = score + 1;
                    enshootElement.Stop();
                    enshootElement.Play();
                }
            }
            foreach (EnemyBullet ebullet in enemyBullets)
            {
                ebullet.UpdateLocation();
                ebullet.Move();
                // enemy shoot sound
                
                if (ebullet.LocationY > 750)
                {
                    enemyBullets.Remove(ebullet);
                    break;
                }
            }
            
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

            if (enemies1.Count == 0 && enemies2.Count == 0)
                end = 1;

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

            double ebx = 0;
            double eby = 0;
            double ebaw = 0;
            double ebah = 0;
            Debug.WriteLine(player.LocationX);
            Debug.WriteLine(player.LocationY);
            foreach (EnemyBullet ebullet in enemyBullets)
            {
                ebx = ebullet.LocationX;
                eby = ebullet.LocationY;
                ebaw = ebullet.ActualWidth;
                ebah = ebullet.ActualHeight;

                Rect r1 = new Rect(ebx, eby, ebaw, ebah);
                Rect r2 = new Rect(player.LocationX, player.LocationY, player.ActualWidth, player.ActualHeight);

                r1.Intersect(r2);

                if (!r1.IsEmpty)
                {
                    Debug.WriteLine("noooooo");
                    // game over sound
                    
                    MyCanvas.Children.Remove(player);
                    end = 1;
                    return;
                }
            }

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
                        // sound of destruction
                        destroyElement.Stop();
                        destroyElement.Play();
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
                        // sound of destruction
                        destroyElement.Stop();
                        destroyElement.Play();
                        MyCanvas.Children.Remove(ammus);
                        MyCanvas.Children.Remove(enemies);

                        bullets.Remove(ammus);
                        enemies2.Remove(enemies);
                        score = score + 200;
                        return;
                    }
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(GamePage), score);
            clickElement.Play();
        }
    }
}
