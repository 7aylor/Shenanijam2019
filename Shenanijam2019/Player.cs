using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Shenanijam2019
{
    public enum Direction { Up, Down, Left, Right }

    public abstract class Entity
    {
        //Look at Cookie's code https://pastebin.com/QcNEU9DM
    }

    public class GameObject
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Speed { get; set; }
        public float Scale { get; set; }
        public string Name { get; set; }
        public Dictionary<string, Animation> Animations;
        public Animation currAnim;
        public SpriteEffects spriteEffects;
        public Direction direction { get; set; }
        internal int _idleTime;
        internal int _maxIdleTime;

        public GameObject(int x, int y, int speed, float Scale, string name)
        {
            this.X = x;
            this.Y = y;
            this.Speed = speed;
            this.Scale = Scale;
            this.Name = name;
            this.Animations = new Dictionary<string, Animation>();
            direction = Direction.Right;
            spriteEffects = SpriteEffects.None;
            _idleTime = 0;
            _maxIdleTime = 180;
        }

        public virtual void Update()
        {
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

    public class Character : GameObject
    {



        public Character(int x, int y, int speed, float Scale, string name) : base(x, y, speed, Scale, name)
        {
        }
    }

    public class Player : Character
    {
        internal KeyboardState _prevKbs;

        public Player(int x, int y, int speed, float Scale, string name) : base(x, y, speed, Scale, name)
        {
            _prevKbs = Keyboard.GetState();
        }

        public override void Update()
        {
            KeyboardState kbs = Keyboard.GetState();
            spriteEffects = SpriteEffects.None;

            //up
            if (kbs.IsKeyDown(Keys.W))
            {
                Y -= Speed;
                _idleTime = 0;
                direction = Direction.Up;
                SetCurrentAnimation("move_back");
            }
            //down
            if (kbs.IsKeyDown(Keys.S))
            {
                Y += Speed;
                _idleTime = 0;
                direction = Direction.Down;
                SetCurrentAnimation("move_forward");
            }
            //left
            if (kbs.IsKeyDown(Keys.A))
            {
                X -= Speed;
                _idleTime = 0;
                SetCurrentAnimation("move_side");
                spriteEffects = SpriteEffects.FlipHorizontally;
                direction = Direction.Left;
                if (!_prevKbs.IsKeyDown(Keys.A))
                {
                    currAnim.ResetFrames();
                }
            }
            //right
            if (kbs.IsKeyDown(Keys.D))
            {
                X += Speed;
                _idleTime = 0;
                SetCurrentAnimation("move_side");
                direction = Direction.Right;
                if (!_prevKbs.IsKeyDown(Keys.D))
                {
                    currAnim.ResetFrames();
                }
            }
            if (kbs.GetPressedKeys().Length == 0)
            {
                Debug.WriteLine("No keys pressed");

                SetCurrentAnimation("idle_side");
                if(direction == Direction.Left)
                {
                    spriteEffects = SpriteEffects.FlipHorizontally;
                }
                _idleTime++;
                if (_idleTime > _maxIdleTime && currAnim.IsComplete())
                {
                    spriteEffects = SpriteEffects.None;
                    if (direction == Direction.Left)
                    {
                        SetCurrentAnimation("idlelong_left");
                    }
                    else
                    {
                        SetCurrentAnimation("idlelong_right");
                    }
                }
            }

            _prevKbs = kbs;
            currAnim.Update();
        }
    }
}