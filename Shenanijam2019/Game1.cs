using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Graphics;
using System;
using System.Diagnostics;

namespace Shenanijam2019
{
    public class Game1 : Game
    {
        public static bool DEBUG_MODE = false;
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Camera camera;
        TiledMapRenderer mapRender;
        TiledMap map;
        const int SPRITE_SCALE = 1;

        public static Player player;
        List<Obstacle> obstacles;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;
            camera = new Camera(1280, 720);
            this.Window.Title = "Excuse me, but you can't park here...";
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            mapRender = new TiledMapRenderer(GraphicsDevice);
            camera.Zoom = new Vector2(3, 3);

            player = new Player(new Vector2(750, 750), 150, SPRITE_SCALE, "Ralphie", 16, 8, 8, 8);

            Characters.Initialize();
            GameObjects.Initialize();

            obstacles = new List<Obstacle>();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            Textures.Load(this.Content, this.GraphicsDevice);

            #region Load Map obstacles
            map = Content.Load<TiledMap>("spaceport");

            var objectLayers = map.ObjectLayers;
            foreach(var objLayer in objectLayers)
            {
                for(int i = 0; i < objLayer.Objects.Length; i++)
                {
                    var obj = objLayer.Objects[i];

                    obstacles.Add(new Obstacle(obj.Position, obj.Size.Width, obj.Size.Height, obj.Name));
                }
            }
            #endregion

            //player-robot
            player.AddAnimation("move_forward", new Animation(32, 33, 8, Textures.playerMoveForward, 4));
            player.AddAnimation("move_back", new Animation(32, 33, 8, Textures.playerMoveBack, 4));
            player.AddAnimation("move_side", new Animation(32, 32, 8, Textures.playerMoveSide, 4));
            player.AddAnimation("idle_side", new Animation(32, 33, 8, Textures.playerIdleSide));
            player.AddAnimation("idlelong_right", new Animation(31, 42, 8, Textures.playerIdleLongRight, 3));
            player.AddAnimation("idlelong_left", new Animation(31, 42, 8, Textures.playerIdleLongLeft, 3));
            player.SetCurrentAnimation("idlelong_side");

            Characters.Load();
            GameObjects.Load();
            UI.Load(this.Content);
        }

        protected override void UnloadContent() {}

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            Characters.Update(gameTime);
            GameObjects.Update();
            UI.Update();
            player.Update(camera, deltaTime, obstacles);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(new Color(20 ,16, 19));

            #region draw ground
            spriteBatch.Begin(transformMatrix: camera.TransformationMatrix, samplerState: SamplerState.PointClamp);
            spriteBatch.Draw(Textures.main, new Vector2(0, 0), color: Color.White, scale: SPRITE_SCALE * Vector2.One);
            spriteBatch.End();
            #endregion

            #region  draw map
            Matrix projection = Matrix.CreateOrthographicOffCenter(0, GraphicsDevice.Viewport.Width / SPRITE_SCALE, GraphicsDevice.Viewport.Height / SPRITE_SCALE, 0, 0, -1);

            mapRender.Draw(map, camera.TransformationMatrix, projection);
            #endregion 

            //Start the drawing spritebatch for all game objects here
            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.PointClamp,
                DepthStencilState.None, RasterizerState.CullCounterClockwise,
                transformMatrix: camera.TransformationMatrix);

            Characters.Draw(spriteBatch, camera);
            GameObjects.Draw(spriteBatch, camera);
            player.Draw(spriteBatch, camera, Textures.pixel);
            UI.Draw(spriteBatch, graphics);

            spriteBatch.End();

            #region draw dialog
            foreach (Character c in Characters.Npcs)
            {
                if(c.Dialog.DrawDialog)
                {
                    spriteBatch.Begin();
                    spriteBatch.Draw(Textures.pixel, new Rectangle(0, 500, graphics.PreferredBackBufferWidth, 300), Color.Black);
                    spriteBatch.End();

                    c.Dialog.Draw(spriteBatch, UI.uiFont, new Vector2(100, 600), Color.White);
                }
            }
            #endregion

            #region draw debug
            if(DEBUG_MODE)
            {
                spriteBatch.Begin(transformMatrix: camera.TransformationMatrix, samplerState: SamplerState.PointClamp);
                spriteBatch.Draw(Textures.pixel, new Rectangle((int)player.Position.X, (int)player.Position.Y, 2, 2), Color.Red);
                spriteBatch.DrawString(UI.debugFont, "(" + Math.Round(player.Position.X) + ", " + Math.Round(player.Position.Y) + ")", new Vector2(player.Position.X, player.Position.Y), Color.Red);
                spriteBatch.End();
            }
            #endregion

            base.Draw(gameTime);
        }
    }
}