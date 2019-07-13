using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace Shenanijam2019
{
    class Player
    {
        public int X { get; set; }
        public int Y { get; set; }
        public string Name { get; set; }
        public Dictionary<string, Animation> Animations;

        public Player()
        {
            Animations = new Dictionary<string, Animation>();
        }

        public void Update()
        {

        }

        public void Draw(SpriteBatch sb)
        {
            sb.Begin();

            sb.End();
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
    }
}