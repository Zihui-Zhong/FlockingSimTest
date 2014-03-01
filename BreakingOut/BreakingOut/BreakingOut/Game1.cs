using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace BreakingOut
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Rectangle screenRectangle;
        Bloid[] bloids;
        Obstacle[] obs;
        int alive;
        SpriteFont font;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = 1700;
            graphics.PreferredBackBufferHeight = 1000;
            screenRectangle = new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
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
            Texture2D tempTexture = Content.Load<Texture2D>("ball");
            int a = 400;
            alive = a;
            bloids = new Bloid[a];
            Random random = new Random();
            for (int i = 0; i < a; i++)
            {
                bloids[i] = new Bloid((float)random.NextDouble() * 2 - 1, (float)random.NextDouble() * 2 - 1, tempTexture, screenRectangle);
            }
            int rangee = 6;
            int colones = 8;

            font = Content.Load<SpriteFont>("myFont");

            obs = new Obstacle[rangee * colones + 1];
            int count = 0;
            tempTexture = Content.Load<Texture2D>("Disaster");
            obs[count] = new ControlledCube(tempTexture, new Vector2(screenRectangle.Width / rangee , screenRectangle.Height / colones ));
            tempTexture = Content.Load<Texture2D>("CityBlock");
            count++;
            for (int i = 0; i < rangee; i++)
            {
                for (int j = 0; j < colones; j++)
                {


                    obs[count] = new CityBlocks(tempTexture, new Vector2(screenRectangle.Width / rangee * i, screenRectangle.Height / colones * j));

                    count++;
                }
            }

            StartGame();
        }
        private void StartGame()
        {

        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
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
            alive = 0;
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            foreach (Bloid bloid in bloids)
            {
                if (bloid.isAlive())
                {
                    bloid.updateAcceleration(bloids, obs);
                    alive++;
                }

            }
            foreach (Bloid bloid in bloids)
            {
                if (bloid.isAlive())
                    bloid.updateSpeed(obs);
            }
            foreach (Bloid bloid in bloids)
            {
                if (bloid.isAlive())
                    bloid.updatePosition();
            }
            foreach (Bloid bloid in bloids)
            {
                bloid.warpIfNecessairy();
            }
            foreach (Obstacle o in obs)
            {
                o.update();
            }

        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {

            GraphicsDevice.Clear(Color.White);

            spriteBatch.Begin();
            foreach (Obstacle o in obs)
            {
                o.Draw(spriteBatch);
            }
            foreach (Bloid bloid in bloids)
            {
                bloid.Draw(spriteBatch);
            }

            spriteBatch.DrawString(font, gameTime.ElapsedGameTime + "", new Vector2(10, 10), Color.Red);
            spriteBatch.DrawString(font, alive + "", new Vector2(10, 30), Color.Red);


            obs[0].Draw(spriteBatch);




            spriteBatch.End();






            base.Draw(gameTime);
        }
    }
}
/*
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace BreakingOut
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Paddle paddle;
        Rectangle screenRectangle;
        Ball ball;
        int bricksWide = 10;
        int bricksHigh = 5;
        Texture2D brickImage;
        Brick[,] bricks;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = 750;
            graphics.PreferredBackBufferHeight = 600;
            screenRectangle = new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
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
            Texture2D tempTexture = Content.Load<Texture2D>("paddle");
            paddle = new Paddle(tempTexture, screenRectangle);
            tempTexture = Content.Load<Texture2D>("ball");
            ball = new Ball(tempTexture, screenRectangle);

            brickImage = Content.Load<Texture2D>("brick");
            StartGame();
        }
        private void StartGame()
        {
            paddle.SetInStartPosition();
            ball.SetInStartPosition(paddle.GetBounds());
            bricks = new Brick[bricksWide, bricksHigh];

            for (int y = 0; y < bricksHigh; y++)
            {
                Color tint = Color.White;
                switch (y)
                {
                    case 0:
                        tint = Color.Blue;
                        break;
                    case 1:
                        tint = Color.Red;
                        break;
                    case 2:
                        tint = Color.Green;
                        break;
                    case 3:
                        tint = Color.Yellow;
                        break;
                    case 4:
                        tint = Color.Purple;
                        break;
                }
                for (int x = 0; x < bricksWide; x++)
                {
                    bricks[x, y] = new Brick(
                    brickImage,
                    new Rectangle(
                    x * brickImage.Width,
                    y * brickImage.Height,
                    brickImage.Width,
                    brickImage.Height),
                    tint);
                }
            }
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
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
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            paddle.Update();
            ball.Update();
            ball.PaddleCollision(paddle.GetBounds());

            foreach (Brick brick in bricks)
                brick.CheckCollision(ball);
            if (ball.OffBottom())
                StartGame();

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            paddle.Draw(spriteBatch);
            ball.Draw(spriteBatch);

            foreach (Brick brick in bricks)
                brick.Draw(spriteBatch);
                
            
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
*/