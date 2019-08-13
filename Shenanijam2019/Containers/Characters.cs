using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Shenanijam2019
{
    class Characters
    {
        #region declarations
        public static List<Character> Npcs;
        public static Character tsaMale;
        public static Character tsaFemale;
        public static Character greenGorblork;
        public static Character orangeGorblork;
        public static Character purpleGorblork;
        public static Character greenNiblix;
        public static Character brownNiblix;
        public static Character purpleNiblix;
        public static Character looselyRelatedRobot;
        public static Character burgerBot;
        const int SPRITE_SCALE = 1;
        #endregion

        public static void Initialize()
        {
            Npcs = new List<Character>();
            tsaMale = new Character(new Vector2(545, 830), 5, SPRITE_SCALE, "Mike", 16, 24, 8, 0);
            tsaFemale = new Character(new Vector2(1092, 505), 5, SPRITE_SCALE, "Megan", 16, 24, 8, 0);
            greenGorblork = new Character(new Vector2(670, 720), 5, SPRITE_SCALE, "Garble", 16, 20, 6);
            orangeGorblork = new Character(new Vector2(755, 660), 5, SPRITE_SCALE, "Gurgle", 16, 20, 6);
            purpleGorblork = new Character(new Vector2(830, 720), 5, SPRITE_SCALE, "Grundle", 16, 20, 6);
            greenNiblix = new Character(new Vector2(1000, 720), 5, SPRITE_SCALE, "Garble", 16, 20, 6); ;
            brownNiblix = new Character(new Vector2(640, 565), 5, SPRITE_SCALE, "Garble", 16, 20, 6); ;
            purpleNiblix = new Character(new Vector2(800, 900), 5, SPRITE_SCALE, "Garble", 16, 20, 6); ;
            looselyRelatedRobot = new Character(new Vector2(900, 860), 5, SPRITE_SCALE, "L.R.Y.", 16, 24, 10);
            burgerBot = new Character(new Vector2(996, 220), 5, SPRITE_SCALE, "BRGRBOI", 16, 20, 4, 0);
            Npcs.Add(tsaMale);
            Npcs.Add(tsaFemale);
            Npcs.Add(greenGorblork);
            Npcs.Add(orangeGorblork);
            Npcs.Add(purpleGorblork);
            Npcs.Add(greenNiblix);
            Npcs.Add(brownNiblix);
            Npcs.Add(purpleNiblix);
            Npcs.Add(looselyRelatedRobot);
            Npcs.Add(burgerBot);

            InitDialog();
        }

        public static void InitDialog()
        {
            foreach (Character c in Npcs)
            {
                Debug.WriteLine(c.Name + " dialog added");
                c.Dialog.AddDialog(c.Name + " test dialog");
            }

            tsaMale.Dialog.AddDialog("This is line 2");
            tsaMale.Dialog.AddDialog("This is line 31231231231231231231231231231231231231231231231");
        }

        public static void Load()
        {
            tsaMale.AddAnimation("idle_side", new Animation(32, 32, 8, Textures.tsaMaleIdle));
            tsaMale.SetCurrentAnimation("idle_side");
            tsaFemale.AddAnimation("idle_side", new Animation(32, 32, 8, Textures.tsaFemaleIdle));
            tsaFemale.SetCurrentAnimation("idle_side");
            greenGorblork.AddAnimation("idle_side", new Animation(32, 32, 8, Textures.greenGorblorkIdle));
            greenGorblork.SetCurrentAnimation("idle_side");
            orangeGorblork.AddAnimation("idle_side", new Animation(32, 32, 8, Textures.orangeGorblorkIdle));
            orangeGorblork.SetCurrentAnimation("idle_side");
            purpleGorblork.AddAnimation("idle_side", new Animation(32, 32, 8, Textures.purpleGorblorkIdle));
            purpleGorblork.SetCurrentAnimation("idle_side");
            greenNiblix.AddAnimation("idle_side", new Animation(32, 32, 8, Textures.greenNiblixIdle));
            greenNiblix.SetCurrentAnimation("idle_side");
            purpleNiblix.AddAnimation("idle_side", new Animation(32, 32, 8, Textures.purpleNiblixIdle));
            purpleNiblix.SetCurrentAnimation("idle_side");
            brownNiblix.AddAnimation("idle_side", new Animation(32, 32, 8, Textures.brownNiblixIdle));
            brownNiblix.SetCurrentAnimation("idle_side");
            looselyRelatedRobot.AddAnimation("idle_side", new Animation(32, 32, 8, Textures.looselyRelatedIdle));
            looselyRelatedRobot.SetCurrentAnimation("idle_side");
            burgerBot.AddAnimation("idle_side", new Animation(26, 33, 8, Textures.burgerBot));
            burgerBot.SetCurrentAnimation("idle_side");

        }

        public static void Update(GameTime gameTime)
        {
            foreach (Character c in Npcs)
            {
                c.Update(gameTime);
            }
        }

        public static void Draw(SpriteBatch spriteBatch, Camera camera)
        {
            bool hasDrawnDialogPrompt = false;

            for (int i = 0; i < Npcs.Count; i++)
            {
                //draw characters
                Npcs[i].Draw(spriteBatch, camera, Textures.pixel);
                if (Npcs[i].ShowDialogPrompt)
                {
                    hasDrawnDialogPrompt = true;
                    UI.dialogPromptAnim.Draw(spriteBatch, Npcs[i].Position - new Vector2(6, 26), SPRITE_SCALE, SpriteEffects.None, camera, 1);
                    Npcs[i].ShowDialogPrompt = false; //set to false so only one prompt will appear
                }
                //ensures a fade in on each change of character with a prompt
                else if (i == Npcs.Count - 1 && hasDrawnDialogPrompt == false)
                {
                    UI.dialogPromptAnim.ResetFrames();
                }
            }
        }
    }
}
