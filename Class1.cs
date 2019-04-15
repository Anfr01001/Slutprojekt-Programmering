using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

class Fiende {
    Texture2D texture;
    Vector2 pos;
    float Rotation;
    Vector2 orgin = new Vector2(50, 50); //-- Hitta mitten av bilden för att vera var skottet ska rotera runt.
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
        angle = r.Next(0, 361);

        // Ta reda på vad X respektive Y måste vara för att denna vinklen ska skapas och radien på cirklen ska vara 700
        x = 590 + Math.Cos(angle) * 700;
        y = 590 + Math.Sin(angle) * 700;
        pos = new Vector2((float)x, (float)y); // gör om det till vector för att kunna användas

        // Ställer in rotation mot spelaren och sedan gör om det till direction som sedan används för att uppdatera postionen i Update
        Rotation = (float)Math.Atan2(pos.Y - 500, pos.X - 500);
        direction = new Vector2((float)Math.Cos(Rotation), (float)Math.Sin(Rotation));

        //Skapa hitbox
        Fiendehitbox = new Rectangle((int)pos.X, (int)pos.Y, 70, 70);

        Blob = Dot; // för test
    }

    public Rectangle getFiendeHitbox() { return Fiendehitbox; }

    public void Update() {
        pos -= direction * speed; // Gå rakt mot spelaren (direction) som är förutbestämd i konstruktorn
        Fiendehitbox.X = (int)pos.X - 35; // -35 för att få hitboxen imitten av fienden 
        Fiendehitbox.Y = (int)pos.Y - 35; // -35 för att få hitboxen imitten av fienden 
    }

    public void Draw(SpriteBatch spriteBatch) {
        //Rita ut skottet samma som overloads som spelaren (för att kunna använda rotation)
        spriteBatch.Draw(texture, pos, null, null, orgin, Rotation);

        // Visualisera hitboxen med koden under
        //spriteBatch.Draw(Blob,new Vector2(Fiendehitbox.X - 35,Fiendehitbox.Y - 35),Color.Red);
    }
}