using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace BreakingOut
{
    class Brick
    {
        Texture2D texture;
        Rectangle location;
        Color tint;
        bool alive;
        public Rectangle Location
        {
            get { return location; }
        }
        public Brick(Texture2D texture, Rectangle location, Color tint)
        {
            this.texture = texture;
            this.location = location;
            this.tint = tint;
            this.alive = true;
        }
        public void CheckCollision(Ball ball)
        {
            if (alive)
            {
                if(location.Intersects(ball.getLocation())){
                    alive = false;
                    ball.deflection(this);

                }
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            if (alive)
                spriteBatch.Draw(texture, location, tint);
        }
    }
}
