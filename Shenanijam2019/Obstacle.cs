using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Shenanijam2019
{
    public class Obstacle : iHasBoundingBox
    {
        public Vector2 Position { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }
        public string Name { get; set; }
        public BoundingBox BoundingBox { get; set; }

        public Obstacle(Vector2 position, float width, float height, string name)
        {
            this.Position = position;
            this.Width = width;
            this.Height = height;
            this.Name = name;
            this.BoundingBox = new BoundingBox(this.Position, this.Width, this.Height);
        }
    }
}
