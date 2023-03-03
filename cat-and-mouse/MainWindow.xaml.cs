using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace cat_and_mouse
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        readonly DispatcherTimer GameTimer = new DispatcherTimer();

        bool goLeftC, goRightC, goUpC, goDownC;
        bool noLeftC, noRightC, noUpC, noDownC;

        bool goLeftM, goRightM, goUpM, goDownM;
        bool noLeftM, noRightM, noUpM, noDownM;
        
        readonly int speedC = 10;
        readonly int speedM = 6;

        Rect mouseHitBox;
        Rect catHitBox;

        int scoreL = 0;
        int scoreR = 0;
        int roundCount = 0;
        int iHidden = 0;

        Key[] leftKeys = { Key.Up, Key.Down, Key.Right, Key.Left};
        Key[] rightKeys = { Key.W, Key.S, Key.D, Key.A};
        Key[] temp;

        public MainWindow()
        {
            InitializeComponent();
            
            GameSetUp();

        }

        private void CanvasKeyDown(object sender, KeyEventArgs e)
        {
            // Checking if the key is down for a cat
            if (e.Key == leftKeys[3] && noLeftM == false)
            {
                noRightM = noUpM = noDownM = false;
                goRightM = goUpM = goDownM = false;

                goLeftM = true;

                mouse.RenderTransform = new RotateTransform(-180, mouse.Width / 2, mouse.Height / 2);

            }
            if (e.Key == leftKeys[2] && noRightM == false) 
            {
                noLeftM = noUpM= noDownM = false;
                goLeftM = goUpM = goDownM = false;
                
                goRightM = true;

                mouse.RenderTransform = new RotateTransform(0, mouse.Width / 2, mouse.Height / 2);

            }
            if (e.Key == leftKeys[0] && noUpM == false)
            { 
                noLeftM = noRightM = noDownM = false;
                goLeftM = goRightM = goDownM = false;

                goUpM = true;

                mouse.RenderTransform = new RotateTransform(-90, mouse.Width / 2, mouse.Height / 2);

            }
            if (e.Key == leftKeys[1] && noDownM == false)
            { 
                noLeftM = noRightM = noUpM = false;
                goLeftM = goRightM = goUpM = false;

                goDownM = true;

                mouse.RenderTransform = new RotateTransform(90, mouse.Width / 2, mouse.Height / 2);

            }

           // Checking if key is down for a mouse
            if (Keyboard.IsKeyDown(rightKeys[3]) && noLeftC == false)
            {
                noRightC = noUpC = noDownC = false;
                goRightC = goUpC = goDownC = false;

                goLeftC = true;

                cat.RenderTransform = new RotateTransform(-180, cat.Width / 2, cat.Height / 2);

            }
            if (Keyboard.IsKeyDown(rightKeys[2]) && noRightC == false)
            {
                noLeftC = noUpC = noDownC = false;
                goLeftC = goUpC = goDownC = false;

                goRightC = true;

                cat.RenderTransform = new RotateTransform(0, cat.Width / 2, cat.Height / 2);

            }
            if (Keyboard.IsKeyDown(rightKeys[0]) && noUpC == false)
            {
                noLeftC = noRightC = noDownC = false;
                goLeftC = goRightC = goDownC = false;

                goUpC = true;

                cat.RenderTransform = new RotateTransform(-90, cat.Width / 2, cat.Height / 2);

            }
            if (Keyboard.IsKeyDown(rightKeys[1]) && noDownC == false)
            {
                noLeftC = noRightC = noUpC = false;
                goLeftC = goRightC = goUpC = false;

                goDownC = true;

                cat.RenderTransform = new RotateTransform(90, cat.Width / 2, cat.Height / 2);

            }



        }

        private void GameLoop(object sender, EventArgs e)
        {
            leftScore.Content = "Score of the left user: " + scoreL;
            rightScore.Content = "Score of the right user: " + scoreR;

            if (goLeftC)
            {
                Canvas.SetLeft(cat, Canvas.GetLeft(cat) - speedC);
            }
            if (goRightC)
            {
                Canvas.SetLeft(cat, Canvas.GetLeft(cat) + speedC);
            }
            if (goUpC)
            {
                Canvas.SetTop(cat, Canvas.GetTop(cat) - speedC);
            }
            if (goDownC)
            {
                Canvas.SetTop(cat, Canvas.GetTop(cat) + speedC);
            }

            if (goDownC && Canvas.GetTop(cat) + 80 > Application.Current.MainWindow.Height)
            {
                noDownC = true;
                goDownC = false;
            }
            if (goUpC && Canvas.GetTop(cat) < 1)
            {
                noUpC = true;
                goUpC = false;
            }
            if (goLeftC && Canvas.GetLeft(cat) - 10 < 1)
            {
                noLeftC = true;
                goLeftC = false;
            }
            if (goRightC && Canvas.GetLeft(cat) + 70 > Application.Current.MainWindow.Width)
            {
                noRightC = true;
                goRightC = false;
            }



            if (goLeftM)
            {
                Canvas.SetLeft(mouse, Canvas.GetLeft(mouse) - speedM);
            }
            if (goRightM)
            {
                Canvas.SetLeft(mouse, Canvas.GetLeft(mouse) + speedM);
            }
            if (goUpM)
            {
                Canvas.SetTop(mouse, Canvas.GetTop(mouse) - speedM);
            }
            if (goDownM)
            {
                Canvas.SetTop(mouse, Canvas.GetTop(mouse) + speedM);
            }

            if (goDownM && Canvas.GetTop(mouse) + 80 > Application.Current.MainWindow.Height)
            {
                noDownM = true;
                goDownM = false;
            }
            if (goUpM && Canvas.GetTop(mouse) - 20 < 1)
            {
                noUpM = true;
                goUpM = false;
            }
            if (goLeftM && Canvas.GetLeft(mouse) - 20 < 1)
            {
                noLeftM = true;
                goLeftM = false;
            }
            if (goRightM && Canvas.GetLeft(mouse) + 40 > Application.Current.MainWindow.Width)
            {
                noRightM = true;
                goRightM = false;
            }

            mouseHitBox = new Rect(Canvas.GetLeft(mouse), Canvas.GetTop(mouse), mouse.Width, mouse.Height);
            catHitBox = new Rect(Canvas.GetLeft(cat), Canvas.GetTop(cat), cat.Width, cat.Height);

            foreach (var x in MyCanvas.Children.OfType<Rectangle>())
            {
                Rect hitBox = new Rect(Canvas.GetLeft(x), Canvas.GetTop(x), x.Width, x.Height);

                if ((string)x.Tag == "wall")
                {
                    // mouse
                    if (goLeftM == true && mouseHitBox.IntersectsWith(hitBox))
                    {
                        Canvas.SetLeft(mouse, Canvas.GetLeft(mouse) + 10);
                        noLeftM = true;
                        goLeftM = false;
                    }
                    if (goRightM == true && mouseHitBox.IntersectsWith(hitBox))
                    {
                        Canvas.SetLeft(mouse, Canvas.GetLeft(mouse) - 10);
                        noRightM = true;
                        goRightM = false;
                    }
                    if (goDownM == true && mouseHitBox.IntersectsWith(hitBox))
                    {
                        Canvas.SetTop(mouse, Canvas.GetTop(mouse) - 10);
                        noDownM = true;
                        goDownM = false;
                    }
                    if (goUpM == true && mouseHitBox.IntersectsWith(hitBox))
                    {
                        Canvas.SetTop(mouse, Canvas.GetTop(mouse) + 10);
                        noUpM = true;
                        goUpM = false;
                    }

                    // cat  
                    if (goLeftC == true && catHitBox.IntersectsWith(hitBox))
                    {
                        Canvas.SetLeft(cat, Canvas.GetLeft(cat) + 10);
                        noLeftC = true;
                        goLeftC = false;
                    }
                    if (goRightC == true && catHitBox.IntersectsWith(hitBox))
                    {
                        Canvas.SetLeft(cat, Canvas.GetLeft(cat) - 10);
                        noRightC = true;
                        goRightC = false;
                    }
                    if (goDownC == true && catHitBox.IntersectsWith(hitBox))
                    {
                        Canvas.SetTop(cat, Canvas.GetTop(cat) - 10);
                        noDownC = true;
                        goDownC = false;
                    }
                    if (goUpC == true && catHitBox.IntersectsWith(hitBox))
                    {
                        Canvas.SetTop(cat, Canvas.GetTop(cat) + 10);
                        noUpC = true;
                        goUpC = false;
                    }
                }

                if ((string)x.Tag == "cheese")
                {
                    if (mouseHitBox.IntersectsWith(hitBox) && x.Visibility == Visibility.Visible)
                    {
                        x.Visibility = Visibility.Hidden;
                        iHidden++;

                        if (roundCount % 2 == 0)
                            scoreR++;
                        else
                            scoreL++;

                        if (iHidden == 93)
                            NewRound();

                    }
                }
                if ((string)x.Name == "cat")
                {
                    if (mouseHitBox.IntersectsWith(hitBox))
                        NewRound();

                }
            }
        }

        private void NewRound()
        {
            temp = leftKeys;
            leftKeys = rightKeys;
            rightKeys = temp;

            if (roundCount % 2 == 0)
            {
                Canvas.SetLeft(mouse, 91);
                Canvas.SetTop(mouse, 540);
                Canvas.SetLeft(cat, 752);
                Canvas.SetTop(cat, 724);
                
            }
            else 
            {
                Canvas.SetLeft(mouse, 752);
                Canvas.SetTop(mouse, 724);
                Canvas.SetLeft(cat, 91);
                Canvas.SetTop(cat, 540);
            }

            foreach (var x in MyCanvas.Children.OfType<Rectangle>())
                if ((string)x.Tag == "cheese" && x.Visibility == Visibility.Hidden)
                    x.Visibility = Visibility.Visible;
            iHidden = 0;

            roundCount++;

            if (roundCount < 4)
                round.Content = "ROUND" + (roundCount + 1);
            else if (scoreL > scoreR)
                GameOver("Left is win!");
            else if (scoreR > scoreL)
                GameOver("Right is win!");
            else
                GameOver("Draw!");
        }

        private void GameSetUp()
        { 
            MyCanvas.Focus();

            GameTimer.Tick += GameLoop;
            GameTimer.Interval = TimeSpan.FromMilliseconds(20);
            GameTimer.Start();


            ImageBrush catImage = new ImageBrush
            {
                ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/cat.png"))
            };
            cat.Fill = catImage;

            ImageBrush mouseImage = new ImageBrush
            {
                ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/mouse.png"))
            };
            mouse.Fill = mouseImage;

        }

        private void GameOver(string message)
        { 
            GameTimer.Stop();
            MessageBox.Show(message);

            //roundCount--;
            //if(roundСount != 0)
            System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
            Application.Current.Shutdown();
        }
    }
}
