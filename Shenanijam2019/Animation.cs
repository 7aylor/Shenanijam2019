using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Shenanijam2019
{
    class Animation
    {
        public int TotalFrames { get; set; }
        public int SpriteWidth { get; set; }
        public int SpriteHeight { get; set; }
        public int speed { get; set; }
        public int LoopFrame { get; set; }
        public Texture2D spriteSheet { get; set; }

        private int _currFrame;
        private int _tickCount;

        public Animation(int totalFrames, int spriteWidth, int spriteHeight, int speed, Texture2D spriteSheet, int loopFrame = -1)
        {
            _currFrame = 0;
            this.TotalFrames = totalFrames;
            this.SpriteWidth = spriteWidth;
            this.SpriteHeight = spriteHeight;
            this.speed = speed;
            this.spriteSheet = spriteSheet;
            this.LoopFrame = loopFrame;
            _tickCount = 0;
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
                if(_currFrame < TotalFrames)
                {
                    _currFrame++;
                }
                else
                {
                    //goes back to 0 unless there is a loop frame set
                    _currFrame = (LoopFrame != -1 && _currFrame >= LoopFrame) ? LoopFrame : 0;
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

            sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise);
            sb.Draw(spriteSheet, destination, source, Color.White, 0, Vector2.Zero, spriteEffects, 1);
            sb.End();
        }
    }
}