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
    //Prototype d'un simulation de type flocking
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
            this.IsMouseVisible = true;


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
            int rangee = 7;
            int colones = 6;
            int border = 50;

            alive = a;
            List<Bloid> temp = new List<Bloid>();
            Random random = new Random();
            
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
            
            int r = (screenRectangle.Width - border/2 - tempTexture.Width);
            int c = (screenRectangle.Height - border/2 - tempTexture.Height);
            int ecartWidth = tempTexture.Width + (screenRectangle.Width - tempTexture.Width * colones - border)/(colones-1);
            int ecartHeight = tempTexture.Height + (screenRectangle.Height - tempTexture.Height * rangee - border) / (rangee - 1);
            for (int i = 0; i < colones; i++)
            {
                for (int j = 0; j < rangee; j++)
                {
                    obs[count] = new CityBlocks(tempTexture, new Vector2(r-ecartWidth*i, c-ecartHeight*j));

                    count++;
                }
            }

            StartGame();
        }
        private void Sort(List<Bloid> l)
        {
            foreach (Bloid bloid in l)
            {
                bloid.warpIfNecessairy();
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