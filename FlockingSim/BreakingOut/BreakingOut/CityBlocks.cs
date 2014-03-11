using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BreakingOut
{
    class CityBlocks : Obstacle
    {
        Rectangle rect;
        public CityBlocks(Texture2D tex, Vector2 pos): base(tex, pos)
        {
            rect = new Rectangle((int)pos.X, (int)pos.Y, texture.Width, tex.Height);
            
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, rect, Color.White);
        }


        public override Vector2 Avoid(Bloid bloid)
        {
       /*     float a = bloid.getMotion().Y;
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

            Vector2 i = position - bloid.getLocation();
            if ((i).X * i.X + i.Y * i.Y < bloid.getAvoidRange() && (collision).X * collision.X + collision.Y * collision.Y < rayon * rayon + 2 && (x - position.X) / bloid.getMotion().X > 0 && (y - position.Y) / bloid.getMotion().Y > 0)
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
        public override Vector2 Fear(Bloid bloid)
        {






            return new Vector2 (0,0);


        }

        public override void Collide(Bloid bloid)
        {
            int x, y;
            if (bloid.getPosition().X > rect.Left)
            {
                if (bloid.getPosition().X > rect.Right)
                {
                    x = 1;
                }
                else
                {
                    x = 0;
                }

            }
            else
            {
                x = -1;
            }
            if (bloid.getPosition().Y> rect.Top)
            {
                if (bloid.getPosition().Y > rect.Bottom)
                {
                    y = 1;
                }
                else
                {
                    y = 0;
                }

            }
            else
            {
                y = -1;
            }

            switch (x)
            {
                case -1:
                    switch (y)
                    {
                        case -1:
                            break;
                        case 0:
                            if (bloid.getPosition().X + bloid.getMotion().X>rect.Left&&bloid.getMotion().X>0)
                            {
                                bloid.setMotion(new Vector2(0,bloid.getMotion().Y));
                            }
                            break;
                        case 1:
                            break;
                    }
                    break;
                case 0:
                    switch (y)
                    {
                        case -1:
                            if (bloid.getPosition().Y + bloid.getMotion().Y > rect.Top && bloid.getMotion().Y > 0)
                            {
                                bloid.setMotion(new Vector2(bloid.getMotion().X,0));
                            }
                            break;
                        case 0:
                            break;
                        case 1:
                            if (bloid.getPosition().Y + bloid.getMotion().Y < rect.Bottom && bloid.getMotion().Y <0)
                            {
                                bloid.setMotion(new Vector2(bloid.getMotion().X, 0));
                            }
                            break;
                    }
                    break;
                case 1:
                    switch (y)
                    {
                        case -1:
                            break;
                        case 0:
                            if (bloid.getPosition().X + bloid.getMotion().X < rect.Right && bloid.getMotion().X < 0)
                            {
                                bloid.setMotion(new Vector2(0, bloid.getMotion().Y));
                            }
                            break;
                        case 1:
                            break;
                    }
                    break;

            }



        }

    }

}
