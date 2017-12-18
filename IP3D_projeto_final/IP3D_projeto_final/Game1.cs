using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace IP3D_projeto_final
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        static short cooldownB = 5;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        GameTime time;
        KeyboardState kb;

        Camera camera;
        Terreno terreno;
        ClsTank tank, tankEnemy;
        SistemaParticulas Po;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
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
            kb = new KeyboardState();
            camera = new Camera(GraphicsDevice);
            terreno = new Terreno(GraphicsDevice, Content);
            tank = new ClsTank(GraphicsDevice, Content, new Vector3(64, 10, 64), 1);
            tankEnemy = new ClsTank(GraphicsDevice, Content, new Vector3(54, 10, 54), 2);
            Po = new SistemaParticulas(GraphicsDevice);

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
            camera.Update(GraphicsDevice, terreno, tank, tankEnemy);
            tankEnemy.Update(GraphicsDevice, Content, gameTime, terreno, tank);
            tank.Update(GraphicsDevice, Content, gameTime, terreno, tank);
            if ((tank.BoundingSphere.Contains(tankEnemy.BoundingSphere)) == ContainmentType.Intersects)
            {
                tank.positionTank = tank.tempPosition;
            }
            Po.Update(gameTime, tank);

            // TODO: Add your update logic here
            Mouse.SetPosition(Window.ClientBounds.Width / 2, Window.ClientBounds.Height / 2);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            terreno.Draw(GraphicsDevice, camera);
            tank.Draw(GraphicsDevice, camera);
            tankEnemy.Draw(GraphicsDevice, camera);
            Po.Draw(GraphicsDevice, camera);
            base.Draw(gameTime);
        }
    }
}

