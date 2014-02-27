using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BreakingOut
{
    class ControlledCube : Obstacle
    {
        private KeyboardState keyboardState;
        public ControlledCube(Texture2D tex, Vector2 pos):base( tex,  pos)
        {

        }
        public override Vector2 Collide(Bloid bloid)
        {
            Vector2 d = bloid.getLocation() - position;
            float dis = d.Length();
            if (dis < rayon*3)
            {
                if (dis < rayon)
                {
                    bloid.kill();
                }
                else
                {
                    d.Normalize();
                    d.X *= 5;
                    d.Y *= 5;
                }
            }
            else
            {
                d = new Vector2(0, 0);
            }
            return d;
        }

        public  override void update(){

            keyboardState = Keyboard.GetState();
            if (keyboardState.IsKeyDown(Keys.Left))
                position.X--;

            if (keyboardState.IsKeyDown(Keys.Right))
                position.X ++;
            if (keyboardState.IsKeyDown(Keys.Up))
                position.Y--;
            if (keyboardState.IsKeyDown(Keys.Down))
                position.Y++;
        }

    }
}
