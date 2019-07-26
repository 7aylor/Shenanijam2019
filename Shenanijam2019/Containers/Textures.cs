using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Shenanijam2019
{
    public class Textures
    {
        #region declarations
        //Debug
        public static Texture2D pixel;

        //Player-Robot
        public static Texture2D playerMoveForward;
        public static Texture2D playerMoveBack;
        public static Texture2D playerMoveSide;
        public static Texture2D playerIdleSide;
        public static Texture2D playerIdleLongRight;
        public static Texture2D playerIdleLongLeft;

        //NPCs
        public static Texture2D tsaMaleIdle;
        public static Texture2D tsaFemaleIdle;
        public static Texture2D greenGorblorkIdle;
        public static Texture2D orangeGorblorkIdle;
        public static Texture2D purpleGorblorkIdle;
        public static Texture2D looselyRelatedIdle;
        public static Texture2D greenNiblixIdle;
        public static Texture2D purpleNiblixIdle;
        public static Texture2D brownNiblixIdle;

        //GameObjects
        public static Texture2D wrenchSpin;
        public static Texture2D vendingMachineBlueDefault;
        public static Texture2D lootBoxDefault;

        //Environment
        public static Texture2D main;

        //UI
        public static Texture2D dialogPrompt;
        #endregion

        public static void Load(ContentManager Content, GraphicsDevice graphics)
        {
            pixel = new Texture2D(graphics, 1, 1);
            pixel.SetData<Color>(new Color[] { Color.White });

            //player-robot
            playerMoveForward = Content.Load<Texture2D>("player_move_forward");
            playerMoveBack = Content.Load<Texture2D>("player_move_back");
            playerMoveSide = Content.Load<Texture2D>("player_move_side");
            playerIdleSide = Content.Load<Texture2D>("player_idle_side");
            playerIdleLongRight = Content.Load<Texture2D>("player_idlelong_right");
            playerIdleLongLeft = Content.Load<Texture2D>("player_idlelong_left");

            //NPCs
            tsaMaleIdle = Content.Load<Texture2D>("tsa_male_idle");
            tsaFemaleIdle = Content.Load<Texture2D>("tsa_female_idle");
            greenGorblorkIdle = Content.Load<Texture2D>("green_gorblork_idle");
            orangeGorblorkIdle = Content.Load<Texture2D>("orange_gorblork_idle");
            purpleGorblorkIdle = Content.Load<Texture2D>("purple_gorblork_idle");
            looselyRelatedIdle = Content.Load<Texture2D>("loosely_related_idle");
            greenNiblixIdle = Content.Load<Texture2D>("green_niblix_idle");
            purpleNiblixIdle = Content.Load<Texture2D>("purple_niblix_idle");
            brownNiblixIdle = Content.Load<Texture2D>("brown_niblix_idle");

            //GameObjects
            wrenchSpin = Content.Load<Texture2D>("wrench");
            vendingMachineBlueDefault = Content.Load<Texture2D>("vending_machine");
            lootBoxDefault = Content.Load<Texture2D>("lootbox_machine");

            //Environment
            main = Content.Load<Texture2D>("main");

            //UI
            dialogPrompt = Content.Load<Texture2D>("dialog_prompt");
        }
    }
}
