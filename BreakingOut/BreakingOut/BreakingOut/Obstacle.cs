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
        public virtual Vector2 Avoid(Bloid bloid){
        /*    float a = bloid.getMotion().Y;
            float b = -bloid.getMotion().X;
            float c = a * bloid.getPosition().X + b * bloid.getPosition().Y;
            float d = -b;
            float e = a;
            float f = d * position.X + e * position.Y;
            float x = (f * b - c * e) / (d * b - a * e);
            float y = (c - a * x) / b;
            Vector2 collision = new Vector2(x, y);
            collision -= position;

            Vector2 z = new Vector2(0, 0);

            Vector2 i = position - bloid.getPosition();
            if ((i).X * i.X + i.Y * i.Y < bloid.getAvoidRange() && (collision).X*collision.X+collision.Y*collision.Y< rayon*rayon + 2 && (x - position.X) / bloid.getMotion().X > 0 && (y - position.Y) / bloid.getMotion().Y > 0)
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
            return z;*/
            return new Vector2(0, 0);
        }
        public virtual Vector2 Fear(Bloid bloid)
        {
            return new Vector2(0, 0);
        }
        public virtual void update()
        {
        }

        public virtual void Collide(Bloid bloid)
        {
            Vector2 d = position-(bloid.getPosition() + bloid.getMotion());
            double dis = d.X * d.X + d.Y * d.Y;
            if (dis < rayon * rayon + 10)
            {

                double j = (d.Y * bloid.getMotion().Y + d.X * bloid.getMotion().X) / (d.X * d.X + d.Y * d.Y);
                if(j>0)
                    bloid.setMotion(bloid.getMotion()-(new Vector2((float)j*d.X,(float)j*d.Y)));

            }

        }
    }
}
