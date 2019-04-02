using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Shooter_Game_slutprojekt {

     /// Denna klassen sköter allt gällande skott.
    class Skott {
        
        Texture2D texture;
        Vector2 pos;
        float Rotation;
        Vector2 orgin = new Vector2(13.5f,4f); //-- Hitta mitten av bilden för att vera var skottet ska rotera runt.
        MouseState MouseState;
        Vector2 direction;
        float speed = 5;

    public Skott(Texture2D texture, Vector2 position,float rot, MouseState MouseState) {
            //När konstruktion kallas vill vi "Få ut variablerna"
            this.texture = texture;
            pos = position;
            Rotation = rot;
            MouseState = MouseState;
    }

    public void Update(){
        // Gör om spelarens nuvarande rotation till en riktning som sedan används för att flytta skotten.
        direction = new Vector2((float)Math.Cos(Rotation),(float)Math.Sin(Rotation));
        //Flytta skotten och en variabel för fart för att lätt kunna ändra det.
        pos -= direction * speed;
    }

    public void Draw(SpriteBatch spriteBatch) {
            //Rita ut skottet samma som overloads som spelaren
            spriteBatch.Draw(texture,pos,null,null,orgin,Rotation);
        }

    }

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

        //Bakgrund 
        Texture2D Bakgrund;
        
        //Skott 
        Texture2D skott;
        List<Skott> Skottlista = new List<Skott>();

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
          
            Player = Content.Load<Texture2D>("SpelareXwing"); //-- Temp för spelare
            font = Content.Load<SpriteFont>("Ubuntu32"); //-- Fonten för att skriva ut text
            skott = Content.Load<Texture2D>("Skott"); //-- Skotten som spelaren skjuter
            Bakgrund = Content.Load<Texture2D>("SpaceBackground"); //-- Bakgrund
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
           
        //Kollar om användaren klickar om detta sker skapa skott 
        if(MouseState.LeftButton == ButtonState.Pressed) {
            Skottlista.Add(new Skott(skott,PlayerPos,PlayerRotation,MouseState)); //-- !Fixa vad skottet utgår från!
        }

        //Uppdatera skott possition
        for (int j = 0; j < Skottlista.Count; j++) {
                 Skottlista[j].Update();
        }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();
            //Rita ut bakgrund 
            spriteBatch.Draw(Bakgrund, new Vector2(0,0), Color.White);

            // Rita ut spelaren (med rotation)
            spriteBatch.Draw(Player,PlayerPos,null,null,orgin,PlayerRotation);

            // Rita ut text
            spriteBatch.DrawString(font, ("Grader: " + degrees), new Vector2(100, 100), Color.White);
            
            //Rita ut skott
            foreach (var item in Skottlista) {
                item.Draw(spriteBatch);
            }

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }   
}
