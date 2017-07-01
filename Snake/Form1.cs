using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Snake
{
    public partial class Form1 : Form
    {
        private List<Circle> Snake = new List<Circle>();
        private Circle food = new Circle();
        public Form1()
        {
            InitializeComponent();

            //Set settings to default
            new Settings();
            //set gamespeed and strt timer
            gameTimer.Interval = 1000 / Settings.Speed;
            gameTimer.Tick += UpdateScreen;
            gameTimer.Start();
            //Start new game
            StartGame();
        }
           

        private void StartGame()
        {
            lblGameOver.Visible = false;
            //Set settings to default
            new Settings();
            //Create new snake
            Snake.Clear();
            Circle head = new Circle();
            head.X = 10;
            head.X = 5;
            Snake.Add(head);

            lblScore.Text = Settings.Score.ToString();
            GenerateFood();
        }
        //plase food game randonly
        private void GenerateFood()
        {
            int maxXpos = pbCanvas.Size.Width / Settings.Width;
            int maxYpos = pbCanvas.Size.Height / Settings.Height;
            Random rnd = new Random();
            food = new Circle();
            food.X = rnd.Next(0, maxXpos);
            food.Y = rnd.Next(0, maxYpos);
        }

        private void UpdateScreen(object sender, EventArgs e)
        {
            //Chek for GameOver
            if (Settings.GameOver == true)
            {
                //Check if Enter pressed
                if (Input.KeyPresed(Keys.Enter))
                {
                    StartGame();
                }
            }
            else
            {
                if (Input.KeyPresed(Keys.Right)  && Settings.direction != Direction.Left)
                    Settings.direction = Direction.Right;
                else if (Input.KeyPresed(Keys.Left)  && Settings.direction != Direction.Right)
                    Settings.direction = Direction.Left;
                else if (Input.KeyPresed(Keys.Up) && Settings.direction != Direction.Down)
                    Settings.direction = Direction.Up;
                else if (Input.KeyPresed(Keys.Down) && Settings.direction != Direction.Up)
                    Settings.direction = Direction.Down;
                MoveSnake();
            }
            pbCanvas.Invalidate();
        }

        private void MoveSnake()
        {
            for (int i = Snake.Count - 1; i >= 0; i--)
            {
                if (i == 0)
                {
                    switch (Settings.direction)
                    {
                        case Direction.Right:
                            Snake[i].X++;
                            break;
                        case Direction.Left:
                            Snake[i].X--;
                            break;
                        case Direction.Up:
                            Snake[i].Y--;
                            break;
                        case Direction.Down:
                            Snake[i].Y++;
                            break;
                    }

                    //get max X and Y pos
                    int maxXpos = pbCanvas.Size.Width / Settings.Width;
                    int maxYPos = pbCanvas.Size.Height / Settings.Height;
                    //collision with borders
                    if(Snake[i].X<0|| Snake[i].X >=maxXpos || Snake[i].Y<0 || Snake[i].Y>= maxYPos)
                    {
                       Die();
                    }

                    //if snake bite itself
                    for(int k =1; k<Snake.Count; k++)
                    {
                        if(Snake[i].X == Snake[k].X && Snake[i].Y == Snake[k].Y)
                        {
                            Die();
                        }
                    }

                    //Snake eat food
                    if(Snake[0].X == food.X && Snake[0].Y == food.Y)
                    {
                       Eat();
                    }
                }
                else
                {
                    Snake[i].X = Snake[i - 1].X;
                    Snake[i].Y = Snake[i - 1].Y;
                }
            }
        }

        private void Eat()
        {
            Circle food = new Circle();
            food.X = Snake[Snake.Count - 1].X;
            food.Y = Snake[Snake.Count - 1].Y;
            Snake.Add(food);

            //update score
            Settings.Score += Settings.Points;
            lblScore.Text = Settings.Score.ToString();
            GenerateFood();
        }

        private void Die()
        {
            Settings.GameOver = true;
        }

        private void pbCanvas_Paint(object sender, PaintEventArgs e)
        {
            Graphics canvas = e.Graphics;
            if (!Settings.GameOver)
            {
                //Set snake colour
                Brush snakeColour;

                //Draw snake
                for (int i = 0; i < Snake.Count; i++)
                {
                    if (i == 0)
                        snakeColour = Brushes.DarkGreen; //head
                    else
                        snakeColour = Brushes.Green; //rest of body
                    //draw snake
                    canvas.FillEllipse(snakeColour,
                        new Rectangle(Snake[i].X * Settings.Width, Snake[i].Y * Settings.Height,
                        Settings.Width, Settings.Height));
                    //draw food
                    canvas.FillEllipse(Brushes.Red,
                         new Rectangle(food.X * Settings.Width, food.Y * Settings.Height,
                        Settings.Width, Settings.Height));
                }
            }
            else
            {
                string gameOver = "Game Over \nYour score:" + Settings.Score + "\nTo start new game press Enter";
                lblGameOver.Text = gameOver;
                lblGameOver.Visible = true;
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            Input.ChangeState(e.KeyCode, true);
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            Input.ChangeState(e.KeyCode, false);
        }
    }
}
