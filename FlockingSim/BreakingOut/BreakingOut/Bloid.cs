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

        public Bloid(float x, float y,float dx, float dy, Texture2D t, Rectangle c)
        {

            acceleration = new Vector2(0, 0);
            texture = t;
            screenRegion = c;
            position = new Vector2(x,y);
            motion = new Vector2(dx, dy);
            alignementRange = 50*50;
            separationRange = 13*13;
            cohesionRange = 50*50;
            avoidRange = 50*50;
            fearRange = 5;
            maxSpeed = 0.5f*0.5f;
            alignementWeight = 0.01f * maxSpeed;
            avoidWeight = 0.06f * maxSpeed;
            separationWeight = 0.1f * maxSpeed;
            cohesionWeight = 0.0001f * maxSpeed;
            fearWeight = 0.1f * maxSpeed;
            alive = true;

        }


        public void updateAcceleration(List<Bloid>[] neighbours, Obstacle[] o)
        {


            acceleration.X = 0;
            acceleration.Y = 0;
            Vector2 f = Flock(neighbours);
            Vector2 i = Interract(o);
            Vector2 a = AttactMouse();

            acceleration += i;
            acceleration += f;
            acceleration += a;


        }
        
        public void updateMotion(Obstacle[] o)
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
        public bool updatePosition(float x, float y){
            position += motion;
            warpIfNecessairy();
            if (position.X >= x && position.X <= x+100&& position.Y >= y&& position.Y <= y+100)
            {
                return false;
            }
            return true;
        }
        public void warpIfNecessairy()
        {

            if (position.X < 5)
                position.X = 5;
            if (position.Y < 5)
                position.Y =5;
            if (position.X > screenRegion.Width-5)
                position.X = screenRegion.Width-5;
            if (position.Y > screenRegion.Height-5)
                position.Y = screenRegion.Height-5;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (alive)
            {
                spriteBatch.Draw(texture, new Vector2(position.X - texture.Width / 2, position.Y - texture.Height / 2), Color.White);
            }
        }

        public Vector2 AttactMouse()
        {
            Vector2 a= new Vector2(0,0);
            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                a = new Vector2(Mouse.GetState().X - position.X, Mouse.GetState().Y - position.Y);
                a.Normalize();
                a *= (float)0.002;
            }
            return a;

        }
        public Vector2 Interract(Obstacle[] o)
        {
            int count = 0;
            Vector2 sum = new Vector2(0, 0);

            foreach (Obstacle obs in o)
            {
                Vector2 z = obs.Avoid(this);

                if (z.X != 0 && z.Y != 0)
                {
                    sum += z*avoidWeight;
                    count++;
                }
                Vector2 d = obs.Fear(this);

                if (d.X != 0 && d.Y != 0)
                {
                    sum += d*fearWeight;
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

        public Vector2 Flock(List<Bloid>[] neighbours)
        {
            Vector2 c = new Vector2(0, 0);
            Vector2 a = new Vector2(0, 0);
            Vector2 s = new Vector2(0, 0);

            Vector2 sum = new Vector2(0, 0);
            int cCount = 0;
            int aCount = 0;
            int sCount = 0;

            foreach (List<Bloid> b in neighbours)
            {
                foreach (Bloid bloid in b)
                {
                    if (bloid.isAlive())
                    {
                        Vector2 d = bloid.getPosition() - position;
                        float dis=(d).X * d.X + d.Y * d.Y;
                        if (dis < cohesionRange)
                        {
                            c += d;
                            cCount++;
                        }
                        if (dis < alignementRange)
                        {
                            a += bloid.getMotion();
                            aCount++;
                        }
                        if (dis > 0 && dis < separationRange)
                        {

                            dis = (float)Math.Sqrt(dis);
                            s.X -= d.X / dis;
                            s.Y -= d.Y / dis;
                            sCount++;
                        }
                    }
                }
            }
            if (cCount > 0)
            {
                c /= cCount;
            }
            if (aCount > 0)
            {
                a /= aCount;
            }
            if (sCount > 0)
                s /= sCount;
            {
            }
            c.X *= cohesionWeight;
            c.Y *= cohesionWeight;
            a.X *= alignementWeight;
            a.Y *= alignementWeight;
            s.X *= separationWeight;
            s.Y *= separationWeight;
            sum += c += a += s;
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
