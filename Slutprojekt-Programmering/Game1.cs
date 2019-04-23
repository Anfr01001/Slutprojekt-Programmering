﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Shooter_Game_slutprojekt {

    public class Game1 : Game {

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        // Spelarens karaktär variabler
        Vector2 PlayerPos = new Vector2(490, 490);
        float PlayerRotation = 0;
        Vector2 orgin;
        float degrees;
        Texture2D Player;

        // Mus
        MouseState MouseState;

        //Text
        SpriteFont font;
        SpriteFont font2;

        //Bakgrund 
        Texture2D Bakgrund;

        //Skott 
        Texture2D skott;
        List<Skott> Skottlista = new List<Skott>();

        //Fiende
        Texture2D fiende;
        List<Fiende> Fiendelista = new List<Fiende>();
        double SenaseFiende = 0;
        double TidmellanFiende = 0.01;

        Texture2D Dot;

        bool GameOver = true;

        public void RestartGame() {
            SenaseFiende = 0;
            TidmellanFiende = 0.1;

            PlayerRotation = 0;

            Fiendelista.Clear();
            Skottlista.Clear();

            GameOver = false;
        }

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
            font2 = Content.Load<SpriteFont>("Font2"); //-- Ytterligare en font
            skott = Content.Load<Texture2D>("Skott"); //-- Skotten som spelaren skjuter
            Bakgrund = Content.Load<Texture2D>("SpaceBackground"); //-- Bakgrund
            fiende = Content.Load<Texture2D>("FiendeTieFighter"); //-- Fiende
            Dot = Content.Load<Texture2D>("Dot"); // test
        }

        protected override void UnloadContent() {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime) {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            MouseState = Mouse.GetState();
            if (!GameOver) {
                // Beräkna vilken vingel karaktären ska ha för att "Kolla" mot muspekaren
                PlayerRotation = (float)Math.Atan2(PlayerPos.Y - MouseState.Y, PlayerPos.X - MouseState.X);
                degrees = PlayerRotation * 180 / (float)Math.PI; //-- Lättare att förstå

                // Vilken punkt ska karaktären rotera runt (mitten av sig själv)
                orgin = new Vector2(Player.Width / 2f, Player.Height / 2f);

                //Uppdatera skott possition
                for (int i = 0; i < Skottlista.Count; i++) {
                    Skottlista[i].Update(i);
                }


                for (int i = 0; i < Fiendelista.Count; i++) {
                    //uppdatera fiende positon 
                    Fiendelista[i].Update(i);

                    // kolla om fiende har träffat spelaren.
                    if (Fiendelista[i].Träffad(i)) {
                        GameOver = true;
                    }
                }


                //Kollar om något skott har träffat någon Fiende (hitbox)
                for (int i = 0; i < Skottlista.Count; i++) {
                    for (int j = 0; j < Fiendelista.Count; j++) {

                        if (Fiendelista[j].getFiendeHitbox().Intersects(Skottlista[i].getskottHitbox())) {
                            Fiendelista.RemoveAt(j);
                            j--;
                            Skottlista.RemoveAt(i);
                            i--;
                        }
                    }
                }

                //Skapar nya fienden om x tid har gått sedan senaste.
                if (gameTime.TotalGameTime.Seconds - SenaseFiende > TidmellanFiende) {
                    SenaseFiende = gameTime.TotalGameTime.Seconds; //-- uppdatera tiden för senaste fiende
                    Fiendelista.Add(new Fiende(fiende, Dot, Fiendelista));
                }

                //Kollar om användaren klickar om detta sker skapa skott 
                if (MouseState.LeftButton == ButtonState.Pressed) {
                    Skottlista.Add(new Skott(skott, new Vector2(490, 490), PlayerRotation, Skottlista));
                }
            } else {
                if (MouseState.LeftButton == ButtonState.Pressed) {
                    RestartGame();
                }
            }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();
            //Rita ut bakgrund 
            spriteBatch.Draw(Bakgrund, new Vector2(0, 0), Color.White);

            //Rita ut skott
            foreach (var item in Skottlista) {
                item.Draw(spriteBatch);
            }

            // Rita ut spelaren (med rotation)
            spriteBatch.Draw(Player, PlayerPos, null, null, orgin, PlayerRotation);


            //Rita ut Fiende
            foreach (var item in Fiendelista) {
                item.Draw(spriteBatch);
            }

            // Rita ut text
            spriteBatch.DrawString(font, Skottlista.Count.ToString(), new Vector2(100, 100), Color.White);

            if (GameOver) {
                spriteBatch.DrawString(font2, "Game over", new Vector2(340, 300), Color.White);
                spriteBatch.DrawString(font, "Klick to restart", new Vector2(430, 400), Color.White);
            }

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
