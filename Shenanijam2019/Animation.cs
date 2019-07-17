using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Shenanijam2019
{
    public class Animation
    {
        public int TotalFrames { get; set; }
        public int SpriteWidth { get; set; }
        public int SpriteHeight { get; set; }
        public int speed { get; set; }
        public int LoopFrameStart { get; set; }
        public int LoopFrameEnd { get; set; }
        public bool Reverse { get; set; }
        public Texture2D spriteSheet { get; set; }
        public Vector2 SpriteOrigin { get; set; }

        private int _currFrame;
        private int _tickCount;

        public Animation(int spriteWidth, int spriteHeight, int speed, Texture2D spriteSheet, int loopFrameStart = -1, int loopFrameEnd = -1, bool reverse = false)
        {
            this.SpriteWidth = spriteWidth;
            this.SpriteHeight = spriteHeight;
            this.speed = speed;
            this.spriteSheet = spriteSheet;
            this.LoopFrameStart = loopFrameStart;
            this.LoopFrameEnd = loopFrameEnd;
            this.TotalFrames = (spriteSheet.Width / SpriteWidth) - 1;
            this.Reverse = reverse;
            _tickCount = 0;
            _currFrame = this.Reverse ? this.TotalFrames - 1 : 0;
            SpriteOrigin = new Vector2(0, SpriteHeight);
        }

        /// <summary>
        /// Update animation frames
        /// </summary>
        public void Update()
        {
            _tickCount++;

            if(_tickCount >= speed)
            {
                _tickCount = 0;

                if(Reverse)
                {
                    if (_currFrame > 0)
                    {
                        _currFrame--;
                    }
                    else
                    {
                        //goes back to 0 unless there is a loop frame set - TERNARY PROOF FOR GGG
                        _currFrame = (LoopFrameStart != -1 && _currFrame >= LoopFrameStart) ? LoopFrameStart : TotalFrames - 1;
                    }
                }
                else
                {
                    if(_currFrame < TotalFrames)
                    {
                        _currFrame++;
                    }
                    else
                    {
                        //goes back to 0 unless there is a loop frame set - TERNARY PROOF FOR GGG
                        _currFrame = (LoopFrameStart != -1 && _currFrame >= LoopFrameStart) ? LoopFrameStart : 0;
                    }
                }
            }
        }

        /// <summary>
        /// draw the sprite, considering size
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="position"></param>
        public void Draw(SpriteBatch sb, Vector2 position, float scale, SpriteEffects spriteEffects)
        {
            Rectangle source = new Rectangle(_currFrame * SpriteWidth, 0, SpriteWidth, SpriteHeight);
            Rectangle destination = new Rectangle((int)position.X, (int)position.Y, (int)(SpriteWidth * scale), (int)(SpriteHeight * scale));

            sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp,
                DepthStencilState.None, RasterizerState.CullCounterClockwise);
            sb.Draw(spriteSheet, destination, source, Color.White, 0, SpriteOrigin, spriteEffects, 1);
            sb.End();
        }

        /// <summary>
        /// draw the sprite, considering size
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="position"></param>
        public void Draw(SpriteBatch sb, Vector2 position, float scale, SpriteEffects spriteEffects, Camera camera)
        {
            Rectangle source = new Rectangle(_currFrame * SpriteWidth, 0, SpriteWidth, SpriteHeight);
            Rectangle destination = new Rectangle((int)position.X, (int)position.Y, (int)(SpriteWidth * scale), (int)(SpriteHeight * scale));

            sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, 
                DepthStencilState.None, RasterizerState.CullCounterClockwise, 
                transformMatrix: camera.TransformationMatrix);
            sb.Draw(spriteSheet, destination, source, Color.White, 0, SpriteOrigin, spriteEffects, 1);
            sb.End();
        }

        public void ResetFrames()
        {
            _currFrame = 0;
        }
        
        public bool IsComplete()
        {
            return (_currFrame == TotalFrames) ? true : false;
        }
    }
}