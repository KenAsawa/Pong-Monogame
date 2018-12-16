using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;

namespace Pong
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Song song1;
        Song song2;
        Song song3;

        private Texture2D paddle;
        private Texture2D ball;
        private Texture2D background;
        private Texture2D centerLine;
        private Vector2 ballPosition;
        private Vector2 paddle1Position;
        private Vector2 paddle2Position;
        private float ballSpeedX;
        private float ballSpeedY;
        private bool ballRight = false;
        private float paddle1Speed;
        private float paddle2Speed;
        private int p1score = 0;
        private int p2score = 0;
        private SpriteFont font;
        //
        //Change this to false to allow w-player using the Up and Down arrow keys.
        private bool aiOpponent = true;
        //
        //
        private bool gameFinished = false;
        Random random = new Random();

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 405;  // set this value to the desired width of your window
            graphics.PreferredBackBufferHeight = 270;   // set this value to the desired height of your window
            graphics.ApplyChanges();
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            ballPosition = new Vector2(graphics.PreferredBackBufferWidth / 2, graphics.PreferredBackBufferHeight / 2);
            paddle1Position = new Vector2(10, graphics.PreferredBackBufferHeight / 2);
            paddle2Position = new Vector2(graphics.PreferredBackBufferWidth - 10, graphics.PreferredBackBufferHeight / 2);
            ballSpeedX = 70f;
            ballSpeedY = 70f;
            paddle1Speed = 120f;
            paddle2Speed = 120f;

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            ball = Content.Load<Texture2D>("ball");
            background = Content.Load<Texture2D>("bg");
            centerLine = Content.Load<Texture2D>("centerLine");
            paddle = Content.Load<Texture2D>("paddle");
            font = Content.Load<SpriteFont>("Score");
            this.song1 = Content.Load<Song>("Bing_02");
            this.song2 = Content.Load<Song>("Bong_02");
            this.song3 = Content.Load<Song>("WinTheme");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            var kstate = Keyboard.GetState();
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if((p1score < 5) && (p2score < 5)) {
                //Player 1 movement
                if (kstate.IsKeyDown(Keys.W))
                {
                    paddle1Position.Y -= paddle1Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                }
                if (kstate.IsKeyDown(Keys.S))
                {
                    paddle1Position.Y += paddle1Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                }

                //Player 2 movement
                if(aiOpponent)
                {
                    if (paddle2Position.Y > ballPosition.Y )
                    {
                        paddle2Position.Y -= paddle2Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    }
                    if (paddle2Position.Y < ballPosition.Y)
                    {
                        paddle2Position.Y += paddle2Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    }
                }
                else{
                    if (kstate.IsKeyDown(Keys.Up))
                    {
                        paddle2Position.Y -= paddle2Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    }
                    if (kstate.IsKeyDown(Keys.Down))
                    {
                        paddle2Position.Y += paddle2Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    }
                }
                

                //Keeps the paddles within bounds.
                paddle1Position.Y = System.Math.Min(System.Math.Max(paddle.Height / 2, paddle1Position.Y), graphics.PreferredBackBufferHeight - paddle.Height / 2);
                paddle2Position.Y = System.Math.Min(System.Math.Max(paddle.Height / 2, paddle2Position.Y), graphics.PreferredBackBufferHeight - paddle.Height / 2);

                //Moves the ball
                ballPosition.Y += ballSpeedY * (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (ballRight)
                {
                    ballPosition.X += ballSpeedX * (float)gameTime.ElapsedGameTime.TotalSeconds;
                }
                else
                {
                    ballPosition.X -= ballSpeedX * (float)gameTime.ElapsedGameTime.TotalSeconds;
                }

                //Bounces off of top and bottom of screen.
                if (ballPosition.Y > (graphics.PreferredBackBufferHeight - (ball.Width / 2)) || ballPosition.Y < (ball.Width / 2))
                {
                    ballSpeedY *= -1;
                }

                //Scoring
                //P1 Scores
                if (ballPosition.X > (graphics.PreferredBackBufferWidth - (ball.Width / 2)))
                {
                    p1score++;
                    ballPosition.X = graphics.PreferredBackBufferWidth / 2;
                    ballPosition.Y = graphics.PreferredBackBufferHeight / 2;
                    ballSpeedX = random.Next(50)+50;
                    ballSpeedY = random.Next(100)-50;
                    ballRight = true;
                    paddle1Speed = 120f;
                    paddle2Speed = 120f;
                }
                //P2 Scores
                if (ballPosition.X < (ball.Width / 2))
                {
                    p2score++;
                    ballPosition.X = graphics.PreferredBackBufferWidth / 2;
                    ballPosition.Y = graphics.PreferredBackBufferHeight / 2;
                    ballSpeedX = random.Next(50) + 50;
                    ballSpeedY = random.Next(100) - 50;
                    ballRight = false;
                    paddle1Speed = 120f;
                    paddle2Speed = 120f;
                }

                //Bounces off of Paddle 1.
                if (((ballPosition.X - (ball.Width / 2)) < (paddle1Position.X + (paddle.Width / 2))) && (ballPosition.X - (ball.Width / 2)) > (paddle1Position.X - (paddle.Width / 2)))
                {
                    if (((ballPosition.Y + (ball.Height / 2)) > (paddle1Position.Y - (paddle.Height / 2))) && ((ballPosition.Y - (ball.Height / 2)) < (paddle1Position.Y + (paddle.Height / 2))))
                    {
                        //ballSpeedX *= -1;
                        ballRight = true;
                        MediaPlayer.Play(song1);
                        ballSpeedX += random.Next(10) + 20;
                        if (kstate.IsKeyDown(Keys.W))
                        {
                            ballSpeedY -= random.Next(20)+40;
                        }
                        if (kstate.IsKeyDown(Keys.S))
                        {
                            ballSpeedY += random.Next(20)+40;
                        }
                        paddle1Speed += 10;
                        paddle2Speed += 10;
                    }
                }
                //Bounces off of Paddle 2.
                if (((ballPosition.X + (ball.Width / 2)) > (paddle2Position.X - (paddle.Width / 2))) && ((ballPosition.X + (ball.Width / 2)) < (paddle2Position.X + (paddle.Width / 2))))
                {
                    if (((ballPosition.Y + (ball.Height / 2)) > (paddle2Position.Y - (paddle.Height / 2))) && ((ballPosition.Y - (ball.Height / 2)) < (paddle2Position.Y + (paddle.Height / 2))))
                    {
                        //ballSpeedX *= -1;
                        ballRight = false;
                        MediaPlayer.Play(song2);
                        ballSpeedX += random.Next(10) + 20;
                        if (aiOpponent)
                        {
                            //Easy //None
                            //Medium paddle2Speed += 5;
                            //Hard paddle2Speed += 10;
                        }
                        else
                        {
                            if (kstate.IsKeyDown(Keys.Up))
                            {
                                ballSpeedY -= random.Next(20) + 40;
                            }
                            if (kstate.IsKeyDown(Keys.Down))
                            {
                                ballSpeedY += random.Next(20) + 40;
                            }
                            paddle2Speed += 10;
                        }
                    }
                }
            }
            else
            {
                if(!gameFinished)
                {
                    MediaPlayer.Play(song3);
                    gameFinished = true;
                }
            }
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(background, new Rectangle(0, 0, 405, 270), Color.White);
            spriteBatch.Draw(centerLine, new Vector2((graphics.PreferredBackBufferWidth -centerLine.Width) / 2, 0), Color.White);
            spriteBatch.DrawString(font, "" + p1score, new Vector2(180, 10), Color.White);
            spriteBatch.DrawString(font, "" + p2score, new Vector2(214, 10), Color.White);
            spriteBatch.Draw(paddle, paddle1Position, null, Color.White, 0f, new Vector2(paddle.Width / 2, paddle.Height / 2),Vector2.One,SpriteEffects.None,0f);
            spriteBatch.Draw(paddle, paddle2Position, null, Color.White, 0f, new Vector2(paddle.Width / 2, paddle.Height / 2), Vector2.One, SpriteEffects.None, 0f);
            spriteBatch.Draw(ball, ballPosition, null, Color.White, 0f, new Vector2(ball.Width / 2, ball.Height / 2), Vector2.One, SpriteEffects.None, 0f);
            if(p1score>=5)
            {
                spriteBatch.DrawString(font, "Player 1 Wins!", new Vector2(50, 127), Color.White);
            }
            if (p2score>=5)
            {
                spriteBatch.DrawString(font, "Player 2 Wins!", new Vector2(250, 127), Color.White);
                            }
            spriteBatch.End();
           
            base.Draw(gameTime);
        }
    }
}
