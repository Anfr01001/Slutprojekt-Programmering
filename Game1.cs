using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Shooter_Game_slutprojekt {
    public class Game1 : Game {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        // Spelarens karaktär variabler
        Vector2 PlayerPos = new Vector2(490,490);
        float PlayerRotation = 0;
        Vector2 orgin;
        float degrees;
        Texture2D Player;


        // Mus
        MouseState MouseState;

        //Text
        SpriteFont font;

        public Game1() {
            graphics = new GraphicsDeviceManager(this);

            // Ändra fönstrets storlek
            graphics.PreferredBackBufferWidth = 1000; 
            graphics.PreferredBackBufferHeight = 1000;
            graphics.ApplyChanges();

            Content.RootDirectory = "Content";
        }

       
        protected override void Initialize() {

            base.Initialize();
            this.IsMouseVisible = true;
        }

        protected override void LoadContent() {
            spriteBatch = new SpriteBatch(GraphicsDevice);
          
            Player = Content.Load<Texture2D>("Dot"); //-- Temp för spelare
            font = Content.Load<SpriteFont>("Ubuntu32"); //-- Fonten för att skriva ut text
        }

        protected override void UnloadContent() {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime) {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            // Beräkna vilken vingel karaktären ska ha för att "Kolla" mot muspekaren
            MouseState = Mouse.GetState();
            PlayerRotation = (float)Math.Atan2(PlayerPos.Y - MouseState.Y, PlayerPos.X - MouseState.X);
            degrees = PlayerRotation * 180 / (float) Math.PI; //-- Lättare att förstå

            // Vilken punkt ska karaktären rotera runt (mitten av sig själv)
            orgin = new Vector2(Player.Width / 2f, Player.Height / 2f);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();

            // Rita ut spelaren (med rotation)
            spriteBatch.Draw(Player,PlayerPos,null,null,orgin,PlayerRotation);

            // Rita ut text
            spriteBatch.DrawString(font, ("Grader: " + degrees), new Vector2(100, 100), Color.White);
            



            spriteBatch.End();
            base.Draw(gameTime);
        }
    }   
}
