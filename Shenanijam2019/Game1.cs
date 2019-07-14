using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Graphics;

namespace Shenanijam2019
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        const int SPRITE_SCALE = 1;
        Camera camera;
        TiledMapRenderer mapRender;
        TiledMap map;

        #region Characters
        Player player;
        List<Character> npcs;
        List<GameObject> gameObjects;
        Character tsaMale;
        Character tsaFemale;
        Character greenGorblork;
        Character orangeGorblork;
        Character purpleGorblork;
        Character looselyRelatedRobot;
        GameObject wrench;
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

        //GameObjects
        Texture2D wrenchSpin;

        //Environment
        Texture2D main;
        #endregion

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;
            camera = new Camera(1280, 720);
            this.Window.Title = "Excuse me, but you can't park here...";
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            mapRender = new TiledMapRenderer(GraphicsDevice);

            player = new Player(900, 900, 150, SPRITE_SCALE, "Ralphie", 18, 20, 6, 4);
            camera.Zoom = new Vector2(3, 3);

            npcs = new List<Character>();
            gameObjects = new List<GameObject>();

            tsaMale = new Character(750, 1050, 5, SPRITE_SCALE, "Mike", 16, 24, 8, 0);
            tsaFemale = new Character(880, 560, 5, SPRITE_SCALE, "Megan", 16, 24, 8, 0);
            greenGorblork = new Character(480, 650, 5, SPRITE_SCALE, "Garble", 16, 20, 6);
            orangeGorblork = new Character(965, 650, 5, SPRITE_SCALE, "Gurgle", 16, 20, 6);
            purpleGorblork = new Character(730, 440, 5, SPRITE_SCALE, "Grundle", 16, 20, 6);
            looselyRelatedRobot = new Character(1000, 1000, 5, SPRITE_SCALE, "L.R.Y.", 16, 24, 10);
            wrench = new GameObject(575, 575, 5, SPRITE_SCALE, "wrench", 13, 24, 10, 4);

            npcs.Add(tsaMale);
            npcs.Add(tsaFemale);
            npcs.Add(greenGorblork);
            npcs.Add(orangeGorblork);
            npcs.Add(purpleGorblork);
            npcs.Add(looselyRelatedRobot);

            gameObjects.Add(wrench);

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //set up debug pixel
            _pixel = new Texture2D(GraphicsDevice, 1, 1);
            _pixel.SetData<Color>(new Color[] { Color.White });

            map = Content.Load<TiledMap>("spaceport");

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
            #endregion

            //GameObjects
            wrenchSpin = Content.Load<Texture2D>("wrench");

            //Environment
            main = Content.Load<Texture2D>("main");

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

            looselyRelatedRobot.AddAnimation("idle_side", new Animation(32, 32, 8, looselyRelatedIdle));
            looselyRelatedRobot.SetCurrentAnimation("idle_side");

            //GameObjects
            wrench.AddAnimation("spin", new Animation(32, 32, 8, wrenchSpin));
            wrench.SetCurrentAnimation("spin");
            
            #endregion
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            foreach (Character c in npcs)
            {
                c.Update();
            }

            foreach(GameObject g in gameObjects)
            {
                g.Update();
            }

            player.Update(camera, deltaTime, npcs, gameObjects);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(new Color(20 ,16, 19));

            // TODO: Add your drawing code here
            //spriteBatch.Begin(transformMatrix: camera.TransformationMatrix, samplerState: SamplerState.PointClamp);
            //spriteBatch.Draw(main, new Vector2(512, 512), color: Color.White, scale: SPRITE_SCALE * Vector2.One);
            //spriteBatch.End();

            Matrix projection = Matrix.CreateOrthographicOffCenter(0, GraphicsDevice.Viewport.Width / SPRITE_SCALE, GraphicsDevice.Viewport.Height / SPRITE_SCALE, 0, 0, -1);

            mapRender.Draw(map, camera.TransformationMatrix, projection);

            
            foreach(Character c in npcs)
            {
                c.Draw(spriteBatch, camera, _pixel);
            }

            foreach (GameObject g in gameObjects)
            {
                g.Draw(spriteBatch, camera, _pixel);
            }

            player.Draw(spriteBatch, camera, _pixel);

            spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            spriteBatch.Draw(wrenchSpin, Vector2.Zero, new Rectangle(0,0,32,32), Color.White, 0, Vector2.Zero, SPRITE_SCALE, SpriteEffects.None, 0);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}