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
        public int currFrame { get; set; }
        public int totalFrames { get; set; }
        public int spriteHeight { get; set; }
        public int spriteWidth { get; set; }
        public Texture2D spriteSheet { get; set; }

        public Animation(int totalFrames, int spriteHeight, int spriteWidth, Texture2D spriteSheet)
        {
            currFrame = 0;
            this.totalFrames = totalFrames;
            this.spriteHeight = spriteHeight;
            this.spriteWidth = spriteWidth;
        }

        public void Update()
        {
            if(currFrame < totalFrames)
            {
                currFrame++;
            }
            else
            {
                currFrame = 0;
            }
        }

        public void Draw(SpriteBatch sb)
        {
            Rectangle source = new Rectangle(currFrame * spriteWidth, spriteHeight, spriteWidth, spriteHeight);

            sb.Begin();

            sb.End();
        }
    }
}