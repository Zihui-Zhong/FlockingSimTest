using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace BreakingOut
{
    class Ball
    {
        Vector2 motion;
        Vector2 position;
        float ballSpeed = 4;
        Texture2D texture;
        Rectangle screenBounds;
        bool collided = false;
        public Ball(Texture2D texture, Rectangle screenBounds)
        {
            this.texture = texture;
            this.screenBounds = screenBounds;
        }
        public void Update()
        {
            collided = false;
            position += motion * ballSpeed;
            CheckWallCollision();
            ballSpeed += 0.01f;
        }
        private void CheckWallCollision()
        {
            if (position.X < 0)
            {
                position.X = 0;
                motion.X *= -1;
            }
            if (position.X + texture.Width > screenBounds.Width)
            {
                position.X = screenBounds.Width - texture.Width;
                motion.X *= -1;
            }
            if (position.Y < 0)
            {
                position.Y = 0;
                motion.Y *= -1;
            }
        }
        public void SetInStartPosition(Rectangle paddleLocation)
        {
            ballSpeed = 4;
            motion = new Vector2(1, -1);
            position.Y = paddleLocation.Y - texture.Height;
            position.X = paddleLocation.X + (paddleLocation.Width - texture.Width) / 2;
        }
        public bool OffBottom()
        {
            if (position.Y > screenBounds.Height)
                return true;
            return false;
        }
        public void PaddleCollision(Rectangle paddleLocation)
        {
            Rectangle ballLocation = new Rectangle(
            (int)position.X,
            (int)position.Y,
            texture.Width,
            texture.Height);
            if (paddleLocation.Intersects(ballLocation))
            {
                position.Y = paddleLocation.Y - texture.Height;
                motion.Y *= -1;
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, Color.White);
        }
        public Rectangle getLocation()
        {
            return new Rectangle(
            (int)position.X,
            (int)position.Y,
            texture.Width,
            texture.Height);
        }
        public void setMotion(float x, float y)
        {
            motion.X = x;
            motion.Y = y;
        }
        public void inverseX()
        {
            motion.X = -motion.X;
        }

        public void inverseY()
        {
            motion.Y = -motion.Y;
        }

        public void deflection(Brick brick)
        {
            if (!collided)
            {
                collided = true;
                motion.Y = -motion.Y;
            }
        }
    }
}
