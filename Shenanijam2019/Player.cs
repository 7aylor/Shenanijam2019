using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Shenanijam2019
{
    class Player
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Speed { get; set; }
        public float Scale { get; set; }
        public string Name { get; set; }
        public Vector2 SpriteOrigin { get; set; } //need to figure out how to incorporate this
        public Dictionary<string, Animation> Animations;
        public Animation currAnim;
        public SpriteEffects spriteEffects;

        public Player(int x, int y, int speed, float Scale, string name)
        {
            this.X = x;
            this.Y = y;
            this.Speed = speed;
            this.Scale = Scale;
            this.Name = name;
            this.Animations = new Dictionary<string, Animation>();
            spriteEffects = SpriteEffects.None;
        }

        public void Update()
        {
            KeyboardState kbs = Keyboard.GetState();
            spriteEffects = SpriteEffects.None;

            if (kbs.IsKeyDown(Keys.W))
            {
                Y -= Speed;
                SetCurrentAnimation("move_back");
            }
            if(kbs.IsKeyDown(Keys.S))
            {
                Y += Speed;
                SetCurrentAnimation("move_forward");
            }
            if(kbs.IsKeyDown(Keys.A))
            {
                X -= Speed;
                SetCurrentAnimation("move_side");
                spriteEffects = SpriteEffects.FlipHorizontally;
            }
            if (kbs.IsKeyDown(Keys.D))
            {
                X += Speed;
                SetCurrentAnimation("move_side");
            }

            currAnim.Update();
        }

        public void Draw(SpriteBatch sb)
        {
            currAnim.Draw(sb, new Vector2(this.X, this.Y), this.Scale, spriteEffects);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="animation"></param>
        public void AddAnimation(string name, Animation animation)
        {
            Animations.Add(name, animation);
        }

        /// <summary>
        /// Sets the current animations by name
        /// </summary>
        /// <param name="name"></param>
        public void SetCurrentAnimation(string name)
        {
            currAnim = Animations.FirstOrDefault(x => x.Key == name).Value;
        }
    }
}