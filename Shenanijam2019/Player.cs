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
        public Vector2 Position { get; set; }
        public int Speed { get; set; }
        public float Scale { get; set; }
        public string Name { get; set; }
        public Dictionary<string, Animation> Animations;
        public Animation currAnim;
        public SpriteEffects spriteEffects;
        public Direction Direction { get; set; }
        public BoundingBox BoundingBox { get; set; }
        internal int bXOffset;
        internal int bYOffset;
        internal int _idleTime;
        internal int _maxIdleTime;

        public GameObject(Vector2 position, int speed, float scale, string name, int bWidth, int bHeight, int bXOffset = 0, int bYOffset = 0)
        {
            this.Position = position;
            this.Speed = speed;
            this.Scale = scale;
            this.Name = name;
            this.Animations = new Dictionary<string, Animation>();
            Direction = Direction.Right;
            spriteEffects = SpriteEffects.None;
            this.bXOffset = (int)(bXOffset * scale);
            this.bYOffset = (int)(bYOffset * scale);
            this.BoundingBox = new BoundingBox(new Vector2(this.Position.X + this.bXOffset, this.Position.Y - this.bYOffset) * Scale, bWidth * this.Scale, bHeight * this.Scale);
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
            BoundingBox.Position = new Vector2(this.Position.X + bXOffset, this.Position.Y - BoundingBox.Height - bYOffset);
        }

        public void Draw(SpriteBatch sb, Camera camera, Texture2D pixel)
        {
            if(Game1.DEBUG_MODE)
            {
                sb.Begin(transformMatrix: camera.TransformationMatrix);
                sb.Draw(pixel, this.BoundingBox.Bounds, Color.Red);
                sb.End();
            }

            currAnim.Draw(sb, this.Position, this.Scale, spriteEffects, camera);
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
        public List<string> Dialog { get; set; }
        private int _currDialogLine;
        public bool showTalking { get; set; }

        public Character(Vector2 position, int speed, float Scale, string name, int bWidth, int bHeight, int bXOffset = 0, int bYOffset = 0) : base(position, speed, Scale, name, bWidth, bHeight, bXOffset, bYOffset)
        {
            showTalking = false;
            _currDialogLine = 0;
        }

        public void AddDialog(string message)
        {
            this.Dialog.Add(message);
        }

        public string GetCurrentLine()
        {
            string line = Dialog[_currDialogLine];
            _currDialogLine++;
            if(_currDialogLine == Dialog.Count) { _currDialogLine = 0; }
            return line;
        }
    }

    public class Player : Character
    {
        private KeyboardState _prevKbs;
        public int Wrenches { get; set; }


        public Player(Vector2 position, int speed, float Scale, string name, int bWidth, int bHeight, int bXOffset, int bYOffset) : base(position, speed, Scale, name, bWidth, bHeight, bXOffset, bYOffset)
        {
            _prevKbs = Keyboard.GetState();
            this.Wrenches = 0;
        }

        public void Update(Camera camera, float dt, List<Character> npcs, List<GameObject> gameObjects, List<Obstacle> obstacles)
        {
            KeyboardState kbs = Keyboard.GetState();
            spriteEffects = SpriteEffects.None;
            float speedModifier = 1;

            BoundingBox b = this.BoundingBox;
            GameObject wrench = null;

            Character closest = FindClosestCharacter(npcs);

            //handles talking animation
            if(Math.Abs(Vector2.Distance(this.Position, closest.Position)) < 64)
            {
                closest.showTalking = true;
            }

            //handles diagonals
            if (kbs.GetPressedKeys().Length == 2)
            {
                speedModifier *= 0.707f;
            }

            //up
            if (kbs.IsKeyDown(Keys.W))
            {
                _idleTime = 0;

                b.Position = new Vector2(this.BoundingBox.Position.X, this.BoundingBox.Position.Y - Speed * dt * speedModifier);

                wrench = CheckWrenchCollisions(gameObjects, b);

                if (!CheckCollisions(npcs, b) && !CheckCollisions(gameObjects, b) &&
                    !CheckCollisions(obstacles, b) || wrench != null)
                {
                    this.Position -= new Vector2(0 , Speed * dt * speedModifier);
                    Direction = Direction.Up;
                    SetCurrentAnimation("move_back");
                }
            }
            //down
            if (kbs.IsKeyDown(Keys.S))
            {
                _idleTime = 0;

                b.Position = new Vector2(this.BoundingBox.Position.X, this.BoundingBox.Position.Y + Speed * dt * speedModifier);

                wrench = CheckWrenchCollisions(gameObjects, b);

                if (!CheckCollisions(npcs, b) && !CheckCollisions(gameObjects, b) &&
                    !CheckCollisions(obstacles, b) || wrench != null)
                {
                    this.Position += new Vector2(0, Speed * dt * speedModifier);
                    Direction = Direction.Down;
                    SetCurrentAnimation("move_forward");
                }
            }
            //left
            if (kbs.IsKeyDown(Keys.A))
            {
                _idleTime = 0;

                b.Position = new Vector2(this.BoundingBox.Position.X - Speed * dt * speedModifier, this.BoundingBox.Position.Y);

                wrench = CheckWrenchCollisions(gameObjects, b);

                if (!CheckCollisions(npcs, b) && !CheckCollisions(gameObjects, b) &&
                    !CheckCollisions(obstacles, b) || wrench != null)
                {
                    this.Position -= new Vector2(Speed * dt * speedModifier, 0);
                    SetCurrentAnimation("move_side");
                    Direction = Direction.Left;
                    if (!_prevKbs.IsKeyDown(Keys.A))
                    {
                        currAnim.ResetFrames();
                    }
                }
                spriteEffects = SpriteEffects.FlipHorizontally;
            }
            //right
            if (kbs.IsKeyDown(Keys.D))
            {
                _idleTime = 0;

                b.Position = new Vector2(this.BoundingBox.Position.X + Speed * dt * speedModifier, this.BoundingBox.Position.Y);

                wrench = CheckWrenchCollisions(gameObjects, b);

                if (!CheckCollisions(npcs, b) && !CheckCollisions(gameObjects, b) &&
                    !CheckCollisions(obstacles, b) || wrench != null)
                {
                    this.Position += new Vector2(Speed * dt * speedModifier, 0);
                    SetCurrentAnimation("move_side");
                    Direction = Direction.Right;
                    if (!_prevKbs.IsKeyDown(Keys.D))
                    {
                        currAnim.ResetFrames();
                    }
                }
            }

            if (kbs.GetPressedKeys().Length == 0)
            {
                SetCurrentAnimation("idle_side");
                if(Direction == Direction.Left)
                {
                    spriteEffects = SpriteEffects.FlipHorizontally;
                }
                _idleTime++;
                if (_idleTime > _maxIdleTime && currAnim.IsComplete())
                {
                    spriteEffects = SpriteEffects.None;
                    if (Direction == Direction.Left)
                    {
                        SetCurrentAnimation("idlelong_left");
                    }
                    else
                    {
                        SetCurrentAnimation("idlelong_right");
                    }
                }
            }

            if(wrench != null)
            {
                gameObjects.Remove(wrench);
                Wrenches += 10;
            }

            _prevKbs = kbs;
            camera.X = this.Position.X - (1240 / 6) + 16;
            camera.Y = this.Position.Y - (720 / 6);
            UpdateBoundingPositions();
            currAnim.Update();
        }

        public bool CheckCollisions(List<GameObject> list, BoundingBox b)
        {
            foreach(GameObject g in list)
            {
                if(b.CollisionCheck(g.BoundingBox))
                {
                    return true;
                }
            }
            return false;
        }

        public bool CheckCollisions(List<Character> list, BoundingBox b)
        {
            foreach (Character c in list)
            {
                if (b.CollisionCheck(c.BoundingBox))
                {
                    return true;
                }
            }
            return false;
        }

        public bool CheckCollisions(List<Obstacle> list, BoundingBox b)
        {
            foreach (Obstacle o in list)
            {
                if (b.CollisionCheck(o.BoundingBox))
                {
                    return true;
                }
            }
            return false;
        }

        public GameObject CheckWrenchCollisions(List<GameObject> list, BoundingBox b)
        {
            foreach (GameObject g in list)
            {
                if (b.CollisionCheck(g.BoundingBox) && g.Name == "wrench")
                {
                    return g;
                }
            }
            return null;
        }

        public Character FindClosestCharacter(List<Character> characters)
        {
            float closest = float.MaxValue;
            Character closestChar = null;

            foreach(Character c in characters)
            {
                float distance = Math.Abs(Vector2.Distance(this.Position, c.Position));
                if (distance < closest)
                {
                    closest = distance;
                    closestChar = c;
                }
            }

            Debug.WriteLine("Closet Character: " + closestChar.Name);

            return closestChar;
        }
    }
}