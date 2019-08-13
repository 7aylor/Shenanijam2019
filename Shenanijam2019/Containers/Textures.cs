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
        public static Texture2D burgerBot;

        //GameObjects
        public static Texture2D wrenchSpin;
        public static Texture2D vendingMachineBlueDefault;
        public static Texture2D lootBoxDefault;
        public static Texture2D plantBushBeige;
        public static Texture2D plantBushGreen;
        public static Texture2D plantBushYellow;
        public static Texture2D plantDeadGreen;
        public static Texture2D plantDeadRed;
        public static Texture2D plantDeadYellow;
        public static Texture2D plantFernBlue;
        public static Texture2D plantFernDarkGreen;
        public static Texture2D plantFernGreen;
        public static Texture2D box;

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
            playerMoveForward = Content.Load<Texture2D>(@"Player\player_move_forward");
            playerMoveBack = Content.Load<Texture2D>(@"Player\player_move_back");
            playerMoveSide = Content.Load<Texture2D>(@"Player\player_move_side");
            playerIdleSide = Content.Load<Texture2D>(@"Player\player_idle_side");
            playerIdleLongRight = Content.Load<Texture2D>(@"Player\player_idlelong_right");
            playerIdleLongLeft = Content.Load<Texture2D>(@"Player\player_idlelong_left");

            //NPCs
            tsaMaleIdle = Content.Load<Texture2D>(@"NPC\tsa_male_idle");
            tsaFemaleIdle = Content.Load<Texture2D>(@"NPC\tsa_female_idle");
            greenGorblorkIdle = Content.Load<Texture2D>(@"NPC\green_gorblork_idle");
            orangeGorblorkIdle = Content.Load<Texture2D>(@"NPC\orange_gorblork_idle");
            purpleGorblorkIdle = Content.Load<Texture2D>(@"NPC\purple_gorblork_idle");
            looselyRelatedIdle = Content.Load<Texture2D>(@"NPC\loosely_related_idle");
            greenNiblixIdle = Content.Load<Texture2D>(@"NPC\green_niblix_idle");
            purpleNiblixIdle = Content.Load<Texture2D>(@"NPC\purple_niblix_idle");
            brownNiblixIdle = Content.Load<Texture2D>(@"NPC\brown_niblix_idle");
            burgerBot = Content.Load<Texture2D>(@"NPC\burger_bot");

            //GameObjects
            wrenchSpin = Content.Load<Texture2D>(@"Environment\General\wrench");
            vendingMachineBlueDefault = Content.Load<Texture2D>(@"Environment\General\vending_machine");
            lootBoxDefault = Content.Load<Texture2D>(@"Environment\General\lootbox_machine");
            plantBushBeige = Content.Load<Texture2D>(@"Environment\General\plant_bush_beige");
            plantBushGreen = Content.Load<Texture2D>(@"Environment\General\plant_bush_green");
            plantBushYellow = Content.Load<Texture2D>(@"Environment\General\plant_bush_yellow");
            plantDeadGreen = Content.Load<Texture2D>(@"Environment\General\plant_dead_green");
            plantDeadRed = Content.Load<Texture2D>(@"Environment\General\plant_dead_red");
            plantDeadYellow = Content.Load<Texture2D>(@"Environment\General\plant_dead_yellow");
            plantFernBlue = Content.Load<Texture2D>(@"Environment\General\plant_fern_blue");
            plantFernDarkGreen = Content.Load<Texture2D>(@"Environment\General\plant_fern_darkgreen");
            plantFernGreen = Content.Load<Texture2D>(@"Environment\General\plant_fern_green");
            box = Content.Load<Texture2D>(@"Environment\Outdoor\box");

            //Environment
            main = Content.Load<Texture2D>(@"Environment\Background\main");

            //UI
            dialogPrompt = Content.Load<Texture2D>(@"UI\dialog_prompt");
        }
    }
}
