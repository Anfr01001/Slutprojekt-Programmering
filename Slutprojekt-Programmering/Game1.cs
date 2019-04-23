using Microsoft.Xna.Framework;
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
        MouseState OldMouseState;

        //Text
        SpriteFont font;
        SpriteFont font2;
        SpriteFont font2Liten;

        //Bakgrund 
        Texture2D Bakgrund;

        //Skott 
        Texture2D skott;
        List<Skott> Skottlista = new List<Skott>();
        List<Skott> SkottTasBort = new List<Skott>();
        int Skottkvar = 5;
        double senasteSkottet = 0;
        int specialattacker = 3;

        //Fiende
        Texture2D fiende;
        List<Fiende> Fiendelista = new List<Fiende>();
        double SenaseFiende = 0;
        double TidmellanFiende = 1;

        Texture2D Dot;

        bool GameOver = false;

        int poäng = 0;

        public void RestartGame() {
            SenaseFiende = 0;
            TidmellanFiende = 1;

            PlayerRotation = 0;

            Fiendelista.Clear();
            Skottlista.Clear();

            poäng = 0;

            Skottkvar = 5;
            senasteSkottet = 0;

            specialattacker = 3;

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
            font2Liten = Content.Load<SpriteFont>("font2Liten"); //-- Mindre variant av fonten ovan
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


                //Skapar nya fienden om x tid har gått sedan senaste.
                if (gameTime.TotalGameTime.TotalSeconds - SenaseFiende > TidmellanFiende) {
                    SenaseFiende = gameTime.TotalGameTime.TotalSeconds; //-- uppdatera tiden för senaste fiende
                    Fiendelista.Add(new Fiende(fiende, Dot, Fiendelista));
                }

                // Ge spelaren nya skott efter bestämd tid
                if (gameTime.TotalGameTime.TotalSeconds - senasteSkottet > 1.5) {
                    senasteSkottet = gameTime.TotalGameTime.TotalSeconds;
                    if (Skottkvar < 5) {
                        Skottkvar++;
                    }
                }

                //Kollar om användaren klickar och knappen var inte nedtryckt förra gången (så att man inte kan hålla in knappen skjuter för snabbt) kolla också om spelaren har skott.
                if (MouseState.LeftButton == ButtonState.Pressed && !(OldMouseState.LeftButton == ButtonState.Pressed) && Skottkvar > 0) {
                    Skottlista.Add(new Skott(skott, new Vector2(490, 490), PlayerRotation, Skottlista));
                    Skottkvar--;
                }


                // Specialattack (skjuter skott åt alla håll)
                if (MouseState.RightButton == ButtonState.Pressed && !(OldMouseState.RightButton == ButtonState.Pressed) && specialattacker > 0) {
                    specialattacker--;

                    // Eftersom skotten utgår från spelarens rotation så ändrar jag den genom att göra om grader till radianer och sedan skapa ett skott.
                    for (int i = 0; i < 360; i += 10) {
                        PlayerRotation = (i * (float)Math.PI) / 180;
                        Skottlista.Add(new Skott(skott, new Vector2(490, 490), PlayerRotation, Skottlista));
                    }
                }

                OldMouseState = Mouse.GetState();

                //Kollar om något skott har träffat någon Fiende (hitbox)
                for (int i = 0; i < Skottlista.Count; i++) {

                    for (int j = 0; j < Fiendelista.Count; j++) {

                        if (Fiendelista[j].getFiendeHitbox().Intersects(Skottlista[i].getskottHitbox())) {
                            poäng++;


                            Fiendelista.RemoveAt(j);
                            j--;

                            if (i > 0) {
                                Skottlista.RemoveAt(i);
                                i--;
                            }

                        }
                    }
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

            // Rita ut text  font2Liten
            spriteBatch.DrawString(font2Liten, "Score: " + poäng, new Vector2(70, 70), Color.White);
            spriteBatch.DrawString(font2Liten, "Skott: " + Skottkvar, new Vector2(70, 90), Color.White);
            spriteBatch.DrawString(font2Liten, "Specialattacker: " + specialattacker, new Vector2(70, 110), Color.White);


            // Om spelaren har förlorat rita ut följande
            if (GameOver) {
                spriteBatch.DrawString(font2, "Game over", new Vector2(340, 300), Color.White);
                spriteBatch.DrawString(font, "Klick to restart", new Vector2(430, 400), Color.White);
            }

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
