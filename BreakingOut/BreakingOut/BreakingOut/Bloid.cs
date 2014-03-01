using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BreakingOut
{
    class Bloid
    {
        Vector2 position;
        Vector2 motion;

        Vector2 acceleration;
        double cohesionRange;
        double alignementRange;
        double separationRange;
        double avoidRange;
        double fearRange;
        float maxSpeed;
        Texture2D texture;
        float alignementWeight;
        float separationWeight;
        float cohesionWeight;
        float avoidWeight;
        float fearWeight;
        Rectangle screenRegion;

        bool alive;

        public Bloid(float x, float y, Texture2D t, Rectangle c)
        {

            acceleration = new Vector2(0, 0);
            texture = t;
            screenRegion = c;
            position = new Vector2(c.Width / 2 - 25, c.Height / 2 - 25);
            motion = new Vector2(x, y);
            alignementRange = 100*100;
            separationRange = 10*10;
            cohesionRange = 100*100;
            avoidRange = 100*100;
            fearRange = 5;
            maxSpeed = 0.5f*0.5f;
            alignementWeight = 0.01f * maxSpeed;
            avoidWeight = 0.06f * maxSpeed;
            separationWeight = 0.08f * maxSpeed;
            cohesionWeight = 0.0001f * maxSpeed;
            fearWeight = 0.1f * maxSpeed;
            alive = true;

        }

        public void updateAcceleration(Bloid[] neighbours, Obstacle[] o)
        {

            acceleration.X = 0;
            acceleration.Y = 0;
            Vector2 c = Cohesion(neighbours);
            Vector2 a = Alignement(neighbours);
            Vector2 s = Separation(neighbours);
            Vector2 v = Avoid(o);
            Vector2 l = Fear(o);
            c.X *= cohesionWeight;
            c.Y *= cohesionWeight;
            a.X *= alignementWeight;
            a.Y *= alignementWeight;
            s.X *= separationWeight;
            s.Y *= separationWeight;
            v.X *= avoidWeight;
            v.Y *= avoidWeight;
            l.X *= fearWeight;
            l.Y *= fearWeight;
            acceleration = new Vector2(0, 0);
            acceleration += c;
            acceleration += s;
            acceleration += a;
            acceleration += v;
            acceleration += l;



        }
        public void updateSpeed(Obstacle[] o)
        {
            motion += acceleration;
            foreach (Obstacle obs in o)
            {
                obs.Collide(this);
            }
            if (motion.X*motion.X+motion.Y*motion.Y > maxSpeed)
            {
                motion.Normalize();
                motion.X *= maxSpeed;
                motion.Y *= maxSpeed;
            }
 
        }
        public void updatePosition()
        {
            position += motion;
        }
        public void warpIfNecessairy()
        {
            position.X = position.X % screenRegion.Width;
            position.Y = position.Y % screenRegion.Height;
            if (position.X < 0)
                position.X += screenRegion.Width;
            if (position.Y < 0)
                position.Y += screenRegion.Height;

        }
        public void Draw(SpriteBatch spriteBatch)
        {
            if (alive)
            {
                spriteBatch.Draw(texture, new Vector2(position.X - texture.Width / 2, position.Y - texture.Height / 2), Color.White);
            }
        }

        public Vector2 Avoid(Obstacle[] o)
        {
            int count = 0;
            Vector2 sum = new Vector2(0, 0);

            foreach (Obstacle obs in o)
            {
                Vector2 z=obs.Avoid(this);

                if(z.X!=0&&z.Y!=0){
                    sum += z;
                    count++;
                }
            }


            if (count > 0)
            {
                sum.X /= count;
                sum.Y /= count;
            }

            return sum;

        }
        public Vector2 Fear(Obstacle[] o)
        {
            int count = 0;
            Vector2 sum = new Vector2(0, 0);
            foreach (Obstacle obs in o)
            {
                Vector2 d = obs.Fear(this);

                if(d.X!=0&&d.Y!=0){
                    sum += d;
                    count++;
                }
                
            }
            if (count > 0)
            {
                sum.X /= count;
                sum.Y /= count;
            }

            return sum;
        }
        public Vector2 Cohesion(Bloid[] neighbours)
        {
            Vector2 sum = new Vector2(0, 0);
            int count = 0;
            foreach (Bloid bloid in neighbours)
            {
                if (bloid.isAlive())
                {
                    Vector2 d = bloid.getPosition() - position;
                    if ((d).X*d.X+d.Y*d.Y < cohesionRange)
                    {
                        sum += d;
                        count++;
                    }
                }
            }
            if (count > 0)
            {
                sum.X = sum.X / count;
                sum.Y = sum.Y / count;

            }
            else
            {
                sum = new Vector2(0, 0);
            }
            return sum;
        }
        public Vector2 Alignement(Bloid[] neighbours)
        {
            Vector2 sum = new Vector2(0, 0);
            int count = 0;
            foreach (Bloid bloid in neighbours)
            {
                if (bloid.isAlive())
                {
                    Vector2 d=(bloid.getPosition() - position);
                    if ((d).X*d.X+d.Y*d.Y< alignementRange)
                    {
                        sum += bloid.getMotion();
                        count++;
                    }
                }
            }
            if (count > 0)
            {
                sum.X = sum.X / count;
                sum.Y = sum.Y / count;
  
            }
            else
            {
                sum = new Vector2(0, 0);
            }
            return sum;
        }
        public Vector2 Separation(Bloid[] neighbours)
        {
            Vector2 sum = new Vector2(0, 0);
            int count = 0;
            foreach (Bloid bloid in neighbours)
            {
                if (bloid.isAlive())
                {
                    Vector2 d = new Vector2(position.X - bloid.getPosition().X, position.Y - bloid.getPosition().Y);
                    float dis = (d).X * d.X + d.Y * d.Y;
                    if (dis > 0 && dis < separationRange)
                    {
                        dis = (float) Math.Sqrt(dis);
                        sum.X += d.X / dis;
                        sum.Y += d.Y / dis;
                        count++;
                    }
                }
            }
            if (count > 0)
            {
                sum.X = sum.X / count;
                sum.Y = sum.Y / count;

            }
            else
            {
                sum = new Vector2(0, 0);
            }
            return sum;
        }


        public Vector2 getPosition()
        {
            return position;
        }

        public Vector2 getMotion()
        {
            return motion;
        }
        public double getAvoidRange()
        {
            return avoidRange;
        }
        public double getCollisionRange()
        {
            return fearRange;
        }
        public void kill()
        {
            alive = false;
        }
        public bool isAlive()
        {
            return alive;
        }
        public void setMotion(Vector2 m)
        {
            motion = m;
        }
    }


}
