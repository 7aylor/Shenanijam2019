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
        public float X { get; set; }
        public float Y { get; set; }
        public int Speed { get; set; }
        public float Scale { get; set; }
        public string Name { get; set; }
        public Dictionary<string, Animation> Animations;
        public Animation currAnim;
        public SpriteEffects spriteEffects;
        public Direction direction { get; set; }
        public BoundingBox boundingBox { get; set; }
        internal int bXOffset;
        internal int bYOffset;
        internal int _idleTime;
        internal int _maxIdleTime;

        public GameObject(int x, int y, int speed, float scale, string name, int bWidth, int bHeight, int bXOffset = 0, int bYOffset = 0)
        {
            this.X = x;
            this.Y = y;
            this.Speed = speed;
            this.Scale = scale;
            this.Name = name;
            this.Animations = new Dictionary<string, Animation>();
            direction = Direction.Right;
            spriteEffects = SpriteEffects.None;
            this.bXOffset = (int)(bXOffset * scale);
            this.bYOffset = (int)(bYOffset * scale);
            this.boundingBox = new BoundingBox(new Vector2(this.X + this.bXOffset, this.Y - this.bYOffset) * Scale, bWidth * this.Scale, bHeight * this.Scale);
            _idleTime = 0;
            _maxIdleTime = 180;
        }

        public virtual void Update()
        {
            UpdateBoundingPositions();
            currAnim.Update();
        }

        public void UpdateBoundingPositions()
        {
            boundingBox.Position = new Vector2(this.X + bXOffset, this.Y - boundingBox.Height - bYOffset);
        }

        public void Draw(SpriteBatch sb, Camera camera, Texture2D pixel)
        {
            sb.Begin(transformMatrix: camera.TransformationMatrix);
            sb.Draw(pixel, this.boundingBox.Bounds, Color.Red);
            sb.End();

            currAnim.Draw(sb, new Vector2(this.X, this.Y), this.Scale, spriteEffects, camera);
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
        public Character(int x, int y, int speed, float Scale, string name, int bWidth, int bHeight, int bXOffset = 0, int bYOffset = 0) : base(x, y, speed, Scale, name, bWidth, bHeight, bXOffset, bYOffset)
        {
        }
    }

    public class Player : Character
    {
        internal KeyboardState _prevKbs;

        public Player(int x, int y, int speed, float Scale, string name, int bWidth, int bHeight, int bXOffset, int bYOffset) : base(x, y, speed, Scale, name, bWidth, bHeight, bXOffset, bYOffset)
        {
            _prevKbs = Keyboard.GetState();
        }

        public void Update(Camera camera, float dt, List<Character> npcs, List<GameObject> gameObjects)
        {
            KeyboardState kbs = Keyboard.GetState();
            spriteEffects = SpriteEffects.None;
            float speedModifier = 1;

            //handles diagonals
            if (kbs.GetPressedKeys().Length == 2)
            {
                speedModifier *= 0.707f;
            }

            //up
            if (kbs.IsKeyDown(Keys.W))
            {
                Y -= Speed * dt * speedModifier;
                _idleTime = 0;
                direction = Direction.Up;
                SetCurrentAnimation("move_back");
            }
            //down
            if (kbs.IsKeyDown(Keys.S))
            {
                Y += Speed * dt * speedModifier;
                _idleTime = 0;
                direction = Direction.Down;
                SetCurrentAnimation("move_forward");
            }
            //left
            if (kbs.IsKeyDown(Keys.A))
            {
                X -= Speed * dt * speedModifier;
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
                X += Speed * dt * speedModifier;
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
            camera.X = this.X - (1240 / 2) + 16;
            camera.Y = this.Y - (720 / 2);
            UpdateBoundingPositions();
            currAnim.Update();
        }
    }
}