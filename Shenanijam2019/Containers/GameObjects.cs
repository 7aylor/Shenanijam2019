using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shenanijam2019
{
    public class GameObjects
    {
        public static List<GameObject> Objects;
        public static GameObject wrench;
        public static GameObject vendingMachineBlue;
        public static GameObject lootBox;
        const int SPRITE_SCALE = 1;


        public static void Initialize()
        {
            Objects = new List<GameObject>();

            wrench = new GameObject(new Vector2(700, 700), 5, SPRITE_SCALE, "wrench", 13, 32, 10, -8);
            vendingMachineBlue = new GameObject(new Vector2(928, 192), 5, SPRITE_SCALE, "Vending Machine", 32, 32);
            lootBox = new GameObject(new Vector2(1184, 192), 5, SPRITE_SCALE, "Loot Box", 32, 32);

            Objects.Add(wrench);
            Objects.Add(vendingMachineBlue);
            Objects.Add(lootBox);
        }

        public static void Load()
        {
            wrench.AddAnimation("spin", new Animation(32, 32, 8, Textures.wrenchSpin));
            wrench.SetCurrentAnimation("spin");

            vendingMachineBlue.AddAnimation("default", new Animation(32, 64, 8, Textures.vendingMachineBlueDefault));
            vendingMachineBlue.SetCurrentAnimation("default");

            lootBox.AddAnimation("default", new Animation(32, 64, 8, Textures.lootBoxDefault));
            lootBox.SetCurrentAnimation("default");
        }

        public static void Update()
        {

            foreach (GameObject g in Objects)
            {
                g.Update();
            }
        }

        public static void Draw(SpriteBatch spriteBatch, Camera camera)
        {
            foreach (GameObject g in Objects)
            {
                g.Draw(spriteBatch, camera, Textures.pixel);
            }
        }
    }
}
