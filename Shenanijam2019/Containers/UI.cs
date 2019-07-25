using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Shenanijam2019
{
    public class UI
    {
        public static Animation dialogPromptAnim;
        public static SpriteFont debugFont;
        public static SpriteFont uiFont;
        private static object graphics;

        public static void Load(ContentManager Content)
        {
            dialogPromptAnim = new Animation(32, 32, 5, Textures.dialogPrompt, 2, 20);
            debugFont = Content.Load<SpriteFont>("default_font");
            uiFont = Content.Load<SpriteFont>("UI_Text");
        }

        public static void Update()
        {
            dialogPromptAnim.Update();
        }

        public static void Draw(SpriteBatch spriteBatch, GraphicsDeviceManager graphics)
        {
            spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            spriteBatch.Draw(Textures.pixel, new Rectangle(0, 0, graphics.PreferredBackBufferWidth, 64), new Color(0, 0, 0, 175));
            spriteBatch.Draw(Textures.wrenchSpin, Vector2.Zero, new Rectangle(0, 0, 32, 32), Color.White, 0, Vector2.Zero, 2, SpriteEffects.None, 0);
            spriteBatch.DrawString(uiFont, Game1.player.Wrenches.ToString(), new Vector2(56, 12), Color.White);

            spriteBatch.End();
        }
    }
}
