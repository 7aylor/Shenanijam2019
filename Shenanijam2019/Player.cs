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
        public BoundingBox CollisionBox { get; set; }
        public BoundingBox SpriteBox { get; set; }
        public float DrawLayer { get; set; }
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
            this.CollisionBox = new BoundingBox(new Vector2(this.Position.X + this.bXOffset, this.Position.Y - this.bYOffset) * Scale, bWidth * this.Scale, bHeight * this.Scale);
            _idleTime = 0;
            this.SpriteBox = new BoundingBox(new Vector2(this.Position.X, this.Position.Y) * Scale, bWidth * this.Scale, bHeight * this.Scale);
            _maxIdleTime = 180;
            DrawLayer = 0.1f;
        }

        public virtual void Update()
        {
            UpdateBoundingPositions();
            currAnim.Update();
        }

        public void UpdateBoundingPositions()
        {
            CollisionBox.Position = new Vector2(this.Position.X + bXOffset, this.Position.Y - CollisionBox.Height - bYOffset);
        }

        public void Draw(SpriteBatch sb, Camera camera, Texture2D pixel)
        {
            if(Game1.DEBUG_MODE)
            {
                //sb.Begin(transformMatrix: camera.TransformationMatrix);
                sb.Draw(pixel, this.CollisionBox.Bounds, Color.Red);
                //sb.End();
            }

            currAnim.Draw(sb, this.Position, this.Scale, spriteEffects, camera, DrawLayer);
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
        public Dialog Dialog { get; set; }
        public bool ShowDialogPrompt { get; set; }

        public Character(Vector2 position, int speed, float Scale, string name, int bWidth, int bHeight, int bXOffset = 0, int bYOffset = 0) : base(position, speed, Scale, name, bWidth, bHeight, bXOffset, bYOffset)
        {
            Dialog = new Dialog();
            ShowDialogPrompt = false;
        }

        public void Update(GameTime gameTime)
        {
            base.Update();

            if(Dialog.IsScrolling)
            {
                Dialog.Update(gameTime);
            }
        }
    }

    public class Player : Character
    {
        private KeyboardState _kbs;
        private KeyboardState _prevKbs;
        private int _talkDistance = 48;
        public int Wrenches { get; set; }

        public Player(Vector2 position, int speed, float Scale, string name, int bWidth, int bHeight, int bXOffset, int bYOffset) : base(position, speed, Scale, name, bWidth, bHeight, bXOffset, bYOffset)
        {
            _prevKbs = Keyboard.GetState();
            this.Wrenches = 0;
            DrawLayer = 0.1f;
        }

        public void Update(Camera camera, float dt, List<Obstacle> obstacles)
        {
            List<Character> npcs = Characters.Npcs;
            List<GameObject> gameObjects = GameObjects.Objects;

            if (_kbs != null)
            {
                _prevKbs = _kbs;
            }

            _kbs = Keyboard.GetState();
            float speedModifier = 1;

            BoundingBox b = this.CollisionBox;
            GameObject wrench = null;

            Character closestNpc = FindClosestCharacter(npcs);

            UpdateSpriteLayers();
            Debug.WriteLine("Keys pressed" + _kbs.GetPressedKeys().Length);

            //handles talking animation
            if(Math.Abs(Vector2.Distance(this.Position, closestNpc.Position)) < _talkDistance)
            {
                closestNpc.ShowDialogPrompt = true;
            }

            //controls when the player talks to an npc
            if(_kbs.IsKeyDown(Keys.E) && !_prevKbs.IsKeyDown(Keys.E) && !closestNpc.Dialog.IsScrolling)
            {
                closestNpc.Dialog.DrawDialog = true;
                closestNpc.Dialog.IsScrolling = true;
                closestNpc.Dialog.UpdateDialogLine();
                FaceTalkingNpc(closestNpc);
            }

            //handles diagonals
            if (_kbs.GetPressedKeys().Length >= 2)
            {
                speedModifier *= 0.707f;
            }

            if (!closestNpc.Dialog.DrawDialog)
            {
                spriteEffects = SpriteEffects.None;

                //up
                if (_kbs.IsKeyDown(Keys.W))
                {
                    _idleTime = 0;

                    b.Position = new Vector2(this.CollisionBox.Position.X, this.CollisionBox.Position.Y - Speed * dt * speedModifier);

                    wrench = CheckWrenchCollisions(gameObjects, b);

                    if (!CheckCollisions(npcs, b) && !CheckCollisions(gameObjects, b) &&
                        !CheckCollisions(obstacles, b) || wrench != null)
                    {
                        this.Position -= new Vector2(0, Speed * dt * speedModifier);
                        Direction = Direction.Up;
                        SetCurrentAnimation("move_back");
                    }
                }
                //down
                if (_kbs.IsKeyDown(Keys.S))
                {
                    _idleTime = 0;

                    b.Position = new Vector2(this.CollisionBox.Position.X, this.CollisionBox.Position.Y + Speed * dt * speedModifier);

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
                if (_kbs.IsKeyDown(Keys.A))
                {
                    _idleTime = 0;

                    b.Position = new Vector2(this.CollisionBox.Position.X - Speed * dt * speedModifier, this.CollisionBox.Position.Y);

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
                }
                //right
                if (_kbs.IsKeyDown(Keys.D))
                {
                    _idleTime = 0;

                    b.Position = new Vector2(this.CollisionBox.Position.X + Speed * dt * speedModifier, this.CollisionBox.Position.Y);

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
            }

            //Idle animations
            if (_kbs.GetPressedKeys().Length == 0)
            {
                SetCurrentAnimation("idle_side");
                _idleTime++;
                if (_idleTime > _maxIdleTime && currAnim.IsComplete())
                {
                    //don't go to sleep if talking
                    if(!closestNpc.Dialog.DrawDialog)
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
                    else
                    {
                        //reset idle time if talking so when done talking you don't instantly sleep
                        _idleTime = 0;
                    }
                }
            }

            if(wrench != null)
            {
                gameObjects.Remove(wrench);
                Wrenches += 10;
            }

            if (Direction == Direction.Left)
            {
                spriteEffects = SpriteEffects.FlipHorizontally;
            }

            camera.X = this.Position.X - (1240 / 6) + 16;
            camera.Y = this.Position.Y - (720 / 6);
            UpdateBoundingPositions();
            currAnim.Update();
        }

        /// <summary>
        /// Method used to determine the sprite layer of the objects and characters.
        /// Only change the layer if the y position is above or below the player's.
        /// Could improve this for efficiency.
        /// </summary>
        private void UpdateSpriteLayers()
        {
            var offset = 4;
            foreach(var gameObject in GameObjects.Objects)
            {
                if(this.Position.Y + offset < gameObject.Position.Y)
                {
                    gameObject.DrawLayer = 0.2f;
                }
                else
                {
                    gameObject.DrawLayer = 0;
                }
            }

            foreach (var character in Characters.Npcs)
            {
                if (this.Position.Y + offset < character.Position.Y)
                {
                    character.DrawLayer = 0.2f;
                }
                else
                {
                    character.DrawLayer = 0;
                }
            }
        }

        /// <summary>
        /// comapares the positions of the players that are talking together and makes them face eachother
        /// </summary>
        /// <param name="pos"></param>
        private void FaceTalkingNpc(Character c)
        {
            if (c.Position.X < this.Position.X)
            {
                this.Direction = Direction.Left;
                c.Direction = Direction.Right;
                c.spriteEffects = SpriteEffects.FlipHorizontally;
            }
            else
            {
                this.Direction = Direction.Right;
                this.spriteEffects = SpriteEffects.None;
                c.Direction = Direction.Left;
                c.spriteEffects = SpriteEffects.None;
            }
        }

        public bool CheckCollisions(List<GameObject> list, BoundingBox b)
        {
            foreach(GameObject g in list)
            {
                if(b.CollisionCheck(g.CollisionBox))
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
                if (b.CollisionCheck(c.CollisionBox))
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
                if (b.CollisionCheck(g.CollisionBox) && g.Name == "wrench")
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

            //Debug.WriteLine("Closet Character: " + closestChar.Name);

            return closestChar;
        }
    }
}