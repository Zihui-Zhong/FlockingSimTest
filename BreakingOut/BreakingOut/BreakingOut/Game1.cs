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
        Obstacle[] obs;
        int alive;
        SpriteFont font;
        List<Bloid>[,] bloids;
        int rows, columbs;
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = 1500;
            graphics.PreferredBackBufferHeight = 1000;
            screenRectangle = new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
            rows = graphics.PreferredBackBufferHeight / 50;
            columbs = graphics.PreferredBackBufferWidth / 50;
            bloids = new List<Bloid>[columbs, rows];
            for (int i = 0; i < columbs; i++)
                for (int j = 0; j < rows; j++)
                    bloids[i, j] = new List<Bloid>();
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
            int count = 0;
            int a = 1400;
            int rangee = 5;
            int colones = 7;


            alive = a;
            List<Bloid> temp = new List<Bloid>();
            Random random = new Random();
            ;
            for (int j = 1; j < 6; j++)
            {
                for (int i = 0; i < a/5; i++)
                {
                    temp.Add(new Bloid(screenRectangle.Width / rangee * j-25,3*i+100, (float)random.NextDouble() * 2 - 1, (float)random.NextDouble() * 2 - 1, tempTexture, screenRectangle));
                    count++;
                }
            }
            Sort(temp);
            font = Content.Load<SpriteFont>("myFont");

            obs = new Obstacle[rangee * colones + 1];
            count = 0;
            tempTexture = Content.Load<Texture2D>("Disaster");
            obs[count] = new ControlledCube(tempTexture, new Vector2(0, 0));
            tempTexture = Content.Load<Texture2D>("CityBlock");
            count++;
            for (int i = 0; i < rangee; i++)
            {
                for (int j = 0; j < colones; j++)
                {


                    obs[count] = new CityBlocks(tempTexture, new Vector2((screenRectangle.Width - tempTexture.Width) - i * (screenRectangle.Width - tempTexture.Width) / (rangee - 1), (screenRectangle.Height - tempTexture.Height) - j * (screenRectangle.Height - tempTexture.Height) / (colones- 1)));

                    count++;
                }
            }

            StartGame();
        }
        private void Sort(List<Bloid> l)
        {
            foreach (Bloid bloid in l)
            {
                bloids[(int)bloid.getPosition().X / 50, (int)bloid.getPosition().Y / 50].Add(bloid);
            }
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




        private List<Bloid>[] Neighbours(int x, int y)
        {

            int count = 0;
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    if (x + i >= 0 && y + j >= 0 && x + i < columbs && y + j < rows)
                    {
                        count++;
                    }
                }
            }
            List <Bloid>[] bloid = new List<Bloid>[count];
            count = 0;
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    if (x + i >= 0 && y + j >= 0 && x + i < columbs && y + j < rows)
                    {

                        bloid[count] = bloids[i+x,j+y];
                        count++;
                        

                    }
                }
            }
            return bloid;

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
            List<Bloid> temp = new List<Bloid>();
            List<Bloid>[] tempT;
            alive = 0;
            for (int i = 0; i < columbs; i++)
                for (int j = 0; j < rows; j++)
                {
                    tempT = Neighbours(i, j);
                    foreach (Bloid bloid in bloids[i, j])
                        if(bloid.isAlive())
                        {
                            bloid.updateAcceleration(tempT, obs);
                            alive++;
                        }
                }

            for (int i = 0; i < columbs; i++)
                for (int j = 0; j < rows; j++)
                    foreach (Bloid bloid in bloids[i, j])
                        bloid.updateMotion(obs);
            List<Bloid> temp2 = new List<Bloid>();
            for (int i = 0; i < columbs; i++)
                for (int j = 0; j < rows; j++)
                {
                    temp = new List<Bloid>();
                    foreach (Bloid bloid in bloids[i, j])
                    {
                        if (bloid.updatePosition(i * 100, j * 100))
                        {
                            temp.Add(bloid);
                        }
                    }
                    foreach (Bloid b in temp)
                    {
                        bloids[i, j].Remove(b);
                        temp2.Add(b);
                    }

                }
            Sort(temp2);

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
            for (int i = 0; i < columbs; i++)
                for (int j = 0; j < rows; j++)
                    foreach (Bloid bloid in bloids[i, j])
                        bloid.Draw(spriteBatch);



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