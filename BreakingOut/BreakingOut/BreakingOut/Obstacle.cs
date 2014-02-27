using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BreakingOut
{
    class Obstacle
    {
        protected Vector2 position;
        protected float rayon;
        protected Texture2D texture;
        protected float scale;
        public Obstacle(Texture2D tex, Vector2 pos)
        {
            position = pos;
            texture = tex;
            rayon = texture.Height/2;
            scale = 1;
        }
        public Obstacle(Texture2D tex, Vector2 pos,float s)
        {
            position = pos;
            texture = tex;
            rayon = (texture.Height / 2 )* scale;
            scale = s;
        }
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, new Vector2(position.X - scale*(texture.Width / 2), position.Y - scale*(texture.Height / 2)), null,Color.White,0,Vector2.Zero,scale,SpriteEffects.None,0f);

        }
        public Vector2 getPos()
        {
            return position;
        }
        public float getRayon()
        {
            return rayon;

        }
        public Vector2 Avoid(Bloid bloid){
            float a = bloid.getMotion().Y;
            float b = -bloid.getMotion().X;
            float c = a * bloid.getLocation().X + b * bloid.getLocation().Y;
            float d = -b;
            float e = a;
            float f = d * position.X + e * position.Y;
            float x = (f * b - c * e) / (d * b - a * e);
            float y = (c - a * x) / b;
            Vector2 collision = new Vector2(x, y);
            collision -= position;

            Vector2 z = new Vector2(0, 0);
            if ((position - position).Length() < bloid.getAvoidRange() && (collision).Length() < rayon + 2 && (x - position.X) / bloid.getMotion().X > 0 && (y - position.Y) / bloid.getMotion().Y > 0)
            {

                if ((collision.Y - position.Y) / (-d) > 0 && (collision.X - position.X) / (e) > 0)
                {
                    z = new Vector2(e, d);
                }
                else
                {
                    z = new Vector2(e, -d);
                }

                z.Normalize();

            }
            return z;
        }
        public virtual Vector2 Collide(Bloid bloid)
        {
            Vector2 d = bloid.getLocation() - position;
            float dis = d.Length();
            if (dis < rayon + bloid.getCollisionRange())
            {


                d.X += d.X / dis;
                d.Y += d.Y / dis;

            }
            else
            {
                d = new Vector2(0, 0);
            }
            return d;
        }
        public virtual void update()
        {
        }
    }
}
