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
        Vector2 orgin = new Vector2(23f,13f); //-- Hitta mitten av bilden för att vera var skottet ska rotera runt.
        MouseState MouseState;
        Vector2 direction;
        float speed = 6;

        //Hitbox
        Rectangle skotthitbox;

    public Skott(Texture2D texture, Vector2 position,float rot, MouseState MouseState) {
            //När konstruktion kallas vill vi "Få ut variablerna"
            this.texture = texture;
            pos = position;
            Rotation = rot;
            MouseState = MouseState;

            //Skapa hitbox
            skotthitbox = new Rectangle((int)pos.X, (int)pos.Y, 8, 8);
    }

    public Rectangle getskottHitbox() { return skotthitbox; }

    public void Update(){
        // Gör om spelarens nuvarande rotation till en riktning som sedan används för att flytta skotten.
        direction = new Vector2((float)Math.Cos(Rotation),(float)Math.Sin(Rotation));
        //Flytta skotten och en variabel för fart för att lätt kunna ändra det.
        pos -= direction * speed;
        skotthitbox.X = (int)pos.X -4;
        skotthitbox.Y = (int)pos.Y -4;

    }

    public void Draw(SpriteBatch spriteBatch) {
            //Rita ut skottet samma som overloads som spelaren
            spriteBatch.Draw(texture,pos,null,null,orgin,Rotation);
        }

    }

    class Fiende {
        Texture2D texture;
        Vector2 pos;
        float Rotation;
        Vector2 orgin = new Vector2(50,50); //-- Hitta mitten av bilden för att vera var skottet ska rotera runt.
        Vector2 direction;
        float speed = 2; 
        double x, y;
        int angle;
        Random r = new Random();

        //hitbox 
        Rectangle Fiendehitbox;

        Texture2D TestTex;
        Texture2D Blob;

        public Fiende(Texture2D texture, Texture2D Dot) {
         this.texture = texture;
         //----------------------------------------------------------------------------------------------------------
         //Fienden skapas genom att bestämma en slumpald vinkel på en imaginär cirkel runt (utanför) spelplanen   
         //För att sedan ta reda på vilken X och Y värde denna plats på cirklen har.
         //Efter det roteras figuren mot spelaren och börjar fördas med x speed mot denna positon.
         //----------------------------------------------------------------------------------------------------------

         //Bestämmer en slumpad vinkel som fiende ska skapas från spelaren.
         angle = r.Next(0,361);

         // Ta reda på vad X respektive Y måste vara för att denna vinklen ska skapas och radien på cirklen ska vara 700
         x = 590 + Math.Cos(angle)*700;
         y = 590 + Math.Sin(angle)*700;
         pos = new Vector2((float)x,(float)y); // gör om det till vector för att kunna användas

         // Ställer in rotation mot spelaren och sedan gör om det till direction som sedan används för att uppdatera postionen i Update
         Rotation = (float)Math.Atan2(pos.Y - 500 , pos.X - 500);
         direction = new Vector2((float)Math.Cos(Rotation),(float)Math.Sin(Rotation));
         
        //Skapa hitbox
        Fiendehitbox = new Rectangle((int)pos.X, (int)pos.Y, 70, 70);
        
        Blob = Dot; // för test
        }

        public Rectangle getFiendeHitbox() { return Fiendehitbox; }


        public void Update() {
            pos -= direction * speed; // Gå rakt mot spelaren (direction) som är förutbestämd i konstruktorn
            Fiendehitbox.X = (int)pos.X -35; // -35 för att få hitboxen imitten av fienden 
            Fiendehitbox.Y = (int)pos.Y -35; // -35 för att få hitboxen imitten av fienden 
        }

        public void Draw(SpriteBatch spriteBatch) {
            //Rita ut skottet samma som overloads som spelaren (för att kunna använda rotation)
            spriteBatch.Draw(texture,pos,null,null,orgin,Rotation);
            
            // Visualisera hitboxen med koden under
            //spriteBatch.Draw(Blob,new Vector2(Fiendehitbox.X - 35,Fiendehitbox.Y - 35),Color.Red);
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

        //Fiende
        Texture2D fiende;
        List<Fiende> Fiendelista = new List<Fiende>();
        double SenaseFiende = 0;
        double TidmellanFiende = 1;

        //Temp
        int träff = 0;
        Texture2D Dot;

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
            fiende = Content.Load<Texture2D>("FiendeTieFighter"); //-- Fiende
            Dot = Content.Load<Texture2D>("Dot"); // test
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


        //Uppdatera skott possition
        for (int i = 0; i < Skottlista.Count; i++) {
                 Skottlista[i].Update();
        }

        //uppdatera fiende positon
        for (int i = 0; i < Fiendelista.Count; i++) {
                 Fiendelista[i].Update();
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
        if(gameTime.TotalGameTime.Seconds - SenaseFiende > TidmellanFiende) {
          SenaseFiende = gameTime.TotalGameTime.Seconds; //-- uppdatera tiden för senaste fiende
          Fiendelista.Add(new Fiende(fiende,Dot));
        }

        //Kollar om användaren klickar om detta sker skapa skott 
        if(MouseState.LeftButton == ButtonState.Pressed) {
           Skottlista.Add(new Skott(skott,new Vector2(490,490),PlayerRotation,MouseState)); //-- !Fixa vad skottet utgår från!
        }


            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();
            //Rita ut bakgrund 
            spriteBatch.Draw(Bakgrund, new Vector2(0,0), Color.White);

            //Rita ut skott
            foreach (var item in Skottlista) {
                item.Draw(spriteBatch);
            }

            // Rita ut spelaren (med rotation)
            spriteBatch.Draw(Player,PlayerPos,null,null,orgin,PlayerRotation);

            // Rita ut text
            spriteBatch.DrawString(font, träff.ToString() , new Vector2(100, 100), Color.White);
            


            //Rita ut Fiende
            foreach (var item in Fiendelista) {
                item.Draw(spriteBatch);
            }

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }   
}
