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
        const int SPRITE_SCALE = 1;
        Camera camera;
        TiledMapRenderer mapRender;
        TiledMap map;

        SpriteFont debugFont;
        SpriteFont uiFont;

        #region Characters
        Player player;
        List<Character> npcs;
        List<GameObject> gameObjects;
        Character tsaMale;
        Character tsaFemale;
        Character greenGorblork;
        Character orangeGorblork;
        Character purpleGorblork;
        Character greenNiblix;
        Character brownNiblix;
        Character purpleNiblix;
        Character looselyRelatedRobot;
        #endregion

        #region Textures
        private Texture2D _pixel;

        //Player-Robot
        Texture2D playerMoveForward;
        Texture2D playerMoveBack;
        Texture2D playerMoveSide;
        Texture2D playerIdleSide;
        Texture2D playerIdleLongRight;
        Texture2D playerIdleLongLeft;

        //NPCs
        Texture2D tsaMaleIdle;
        Texture2D tsaFemaleIdle;
        Texture2D greenGorblorkIdle;
        Texture2D orangeGorblorkIdle;
        Texture2D purpleGorblorkIdle;
        Texture2D looselyRelatedIdle;
        Texture2D greenNiblixIdle;
        Texture2D purpleNiblixIdle;
        Texture2D brownNiblixIdle;

        //GameObjects
        Texture2D wrenchSpin;
        Texture2D vendingMachineBlueDefault;
        Texture2D lootBoxDefault;

        //Environment
        Texture2D main;

        //UI
        Texture2D dialogPrompt;
        #endregion

        #region Obstacles
        List<Obstacle> obstacles;
        #endregion

        #region GameObjects
        GameObject wrench;
        GameObject vendingMachineBlue;
        GameObject lootBox;
        #endregion

        Animation dialogPromptAnim;

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

            npcs = new List<Character>();
            gameObjects = new List<GameObject>();
            obstacles = new List<Obstacle>();

            tsaMale = new Character(new Vector2(545, 830), 5, SPRITE_SCALE, "Mike", 16, 24, 8, 0);
            tsaFemale = new Character(new Vector2(1092, 505), 5, SPRITE_SCALE, "Megan", 16, 24, 8, 0);

            greenGorblork = new Character(new Vector2(670, 720), 5, SPRITE_SCALE, "Garble", 16, 20, 6);
            orangeGorblork = new Character(new Vector2(755, 660), 5, SPRITE_SCALE, "Gurgle", 16, 20, 6);
            purpleGorblork = new Character(new Vector2(830, 720), 5, SPRITE_SCALE, "Grundle", 16, 20, 6);

            greenNiblix = new Character(new Vector2(1000, 720), 5, SPRITE_SCALE, "Garble", 16, 20, 6); ;
            brownNiblix = new Character(new Vector2(640, 565), 5, SPRITE_SCALE, "Garble", 16, 20, 6); ;
            purpleNiblix = new Character(new Vector2(800, 900), 5, SPRITE_SCALE, "Garble", 16, 20, 6); ;

            looselyRelatedRobot = new Character(new Vector2(900, 860), 5, SPRITE_SCALE, "L.R.Y.", 16, 24, 10);

            wrench = new GameObject(new Vector2(700, 700), 5, SPRITE_SCALE, "wrench", 13, 32, 10, -8);
            vendingMachineBlue = new GameObject(new Vector2(928, 192), 5, SPRITE_SCALE, "Vending Machine", 32, 64);
            lootBox = new GameObject(new Vector2(1184, 192), 5, SPRITE_SCALE, "Loot Box", 32, 64);

            npcs.Add(tsaMale);
            npcs.Add(tsaFemale);

            npcs.Add(greenGorblork);
            npcs.Add(orangeGorblork);
            npcs.Add(purpleGorblork);

            npcs.Add(greenNiblix);
            npcs.Add(brownNiblix);
            npcs.Add(purpleNiblix);

            npcs.Add(looselyRelatedRobot);

            gameObjects.Add(wrench);
            gameObjects.Add(vendingMachineBlue);
            gameObjects.Add(lootBox);

            foreach (Character c in npcs)
            {
                Debug.WriteLine(c.Name + " dialog added");
                c.Dialog.AddDialog(c.Name + " test dialog");
            }

            tsaMale.Dialog.AddDialog("This is line 2");
            tsaMale.Dialog.AddDialog("This is line 31231231231231231231231231231231231231231231231");


            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //set up debug pixel
            _pixel = new Texture2D(GraphicsDevice, 1, 1);
            _pixel.SetData<Color>(new Color[] { Color.White });

            debugFont = Content.Load<SpriteFont>("default_font");
            uiFont = Content.Load<SpriteFont>("UI_Text");

            #region Load Map
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

            #region LoadTextures

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
            #endregion

            #region add animations
            //player-robot
            player.AddAnimation("move_forward", new Animation(32, 33, 8, playerMoveForward, 4));
            player.AddAnimation("move_back", new Animation(32, 33, 8, playerMoveBack, 4));
            player.AddAnimation("move_side", new Animation(32, 32, 8, playerMoveSide, 4));
            player.AddAnimation("idle_side", new Animation(32, 33, 8, playerIdleSide));
            player.AddAnimation("idlelong_right", new Animation(31, 42, 8, playerIdleLongRight, 3));
            player.AddAnimation("idlelong_left", new Animation(31, 42, 8, playerIdleLongLeft, 3));
            player.SetCurrentAnimation("idlelong_side");

            //NPCs
            tsaMale.AddAnimation("idle_side", new Animation(32, 32, 8, tsaMaleIdle));
            tsaMale.SetCurrentAnimation("idle_side");
            tsaFemale.AddAnimation("idle_side", new Animation(32, 32, 8, tsaFemaleIdle));
            tsaFemale.SetCurrentAnimation("idle_side");

            greenGorblork.AddAnimation("idle_side", new Animation(32, 32, 8, greenGorblorkIdle));
            greenGorblork.SetCurrentAnimation("idle_side");
            orangeGorblork.AddAnimation("idle_side", new Animation(32, 32, 8, orangeGorblorkIdle));
            orangeGorblork.SetCurrentAnimation("idle_side");
            purpleGorblork.AddAnimation("idle_side", new Animation(32, 32, 8, purpleGorblorkIdle));
            purpleGorblork.SetCurrentAnimation("idle_side");

            greenNiblix.AddAnimation("idle_side", new Animation(32, 32, 8, greenNiblixIdle));
            greenNiblix.SetCurrentAnimation("idle_side");
            purpleNiblix.AddAnimation("idle_side", new Animation(32, 32, 8, purpleNiblixIdle));
            purpleNiblix.SetCurrentAnimation("idle_side");
            brownNiblix.AddAnimation("idle_side", new Animation(32, 32, 8, brownNiblixIdle));
            brownNiblix.SetCurrentAnimation("idle_side");

            looselyRelatedRobot.AddAnimation("idle_side", new Animation(32, 32, 8, looselyRelatedIdle));
            looselyRelatedRobot.SetCurrentAnimation("idle_side");

            //GameObjects
            wrench.AddAnimation("spin", new Animation(32, 32, 8, wrenchSpin));
            wrench.SetCurrentAnimation("spin");

            vendingMachineBlue.AddAnimation("default", new Animation(32, 64, 8, vendingMachineBlueDefault));
            vendingMachineBlue.SetCurrentAnimation("default");

            lootBox.AddAnimation("default", new Animation(32, 64, 8, lootBoxDefault));
            lootBox.SetCurrentAnimation("default");

            //UI
            dialogPromptAnim = new Animation(32, 32, 5, dialogPrompt, 2, 20);
            
            #endregion
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            foreach (Character c in npcs)
            {
                c.Update(gameTime);
            }

            foreach(GameObject g in gameObjects)
            {
                g.Update();
            }

            dialogPromptAnim.Update();

            player.Update(camera, deltaTime, npcs, gameObjects, obstacles);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(new Color(20 ,16, 19));

            #region draw ground
            spriteBatch.Begin(transformMatrix: camera.TransformationMatrix, samplerState: SamplerState.PointClamp);
            spriteBatch.Draw(main, new Vector2(0, 0), color: Color.White, scale: SPRITE_SCALE * Vector2.One);
            spriteBatch.End();
            #endregion

            #region  draw map
            Matrix projection = Matrix.CreateOrthographicOffCenter(0, GraphicsDevice.Viewport.Width / SPRITE_SCALE, GraphicsDevice.Viewport.Height / SPRITE_SCALE, 0, 0, -1);

            mapRender.Draw(map, camera.TransformationMatrix, projection);
            #endregion

            #region draw dialog prompt, characters
            bool hasDrawnDialogPrompt = false;

            for(int i = 0; i < npcs.Count; i++)
            {
                //draw characters
                npcs[i].Draw(spriteBatch, camera, _pixel);
                if(npcs[i].ShowDialogPrompt)
                {
                    hasDrawnDialogPrompt = true;
                    dialogPromptAnim.Draw(spriteBatch, npcs[i].Position - new Vector2(6, 26), SPRITE_SCALE, SpriteEffects.None, camera);
                    npcs[i].ShowDialogPrompt = false; //set to false so only one prompt will appear
                }
                //ensures a fade in on each change of character with a prompt
                else if(i == npcs.Count - 1 && hasDrawnDialogPrompt == false)
                {
                    dialogPromptAnim.ResetFrames();
                }
            }
            #endregion

            #region draw gameobjects
            foreach (GameObject g in gameObjects)
            {
                g.Draw(spriteBatch, camera, _pixel);
            }
            #endregion

            //draw player
            player.Draw(spriteBatch, camera, _pixel);

            #region draw debug
            if(DEBUG_MODE)
            {
                spriteBatch.Begin(transformMatrix: camera.TransformationMatrix, samplerState: SamplerState.PointClamp);
                spriteBatch.Draw(_pixel, new Rectangle((int)player.Position.X, (int)player.Position.Y, 2, 2), Color.Red);
                spriteBatch.DrawString(debugFont, "(" + Math.Round(player.Position.X) + ", " + Math.Round(player.Position.Y) + ")", new Vector2(player.Position.X, player.Position.Y), Color.Red);
                spriteBatch.End();
            }
            #endregion

            #region draw ui
            spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            spriteBatch.Draw(_pixel, new Rectangle(0, 0, graphics.PreferredBackBufferWidth, 64), new Color(0, 0, 0, 175));
            spriteBatch.Draw(wrenchSpin, Vector2.Zero, new Rectangle(0, 0, 32, 32), Color.White, 0, Vector2.Zero, 2, SpriteEffects.None, 0);
            spriteBatch.DrawString(uiFont, player.Wrenches.ToString(), new Vector2(56, 12), Color.White);

            spriteBatch.End();
            #endregion

            #region draw dialog
            foreach(Character c in npcs)
            {
                if(c.Dialog.DrawDialog)
                {
                    spriteBatch.Begin();
                    spriteBatch.Draw(_pixel, new Rectangle(0, 500, graphics.PreferredBackBufferWidth, 300), Color.Black);
                    spriteBatch.End();

                    c.Dialog.Draw(spriteBatch, uiFont, new Vector2(100, 600), Color.White);
                }
            }
            #endregion

            base.Draw(gameTime);
        }
    }
}