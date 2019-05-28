using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
/// <summary>  
///  Den här klassen styr allt gällande fiender
///  den har hand om hur de rör sig, skapas och hjälp metoder för att kolla om att de är träffade.
/// </summary>
public class Fiende {
    Texture2D texture;
    Texture2D shield;
    Vector2 pos;
    float Rotation;
    Vector2 orgin = new Vector2(50, 50); //-- Hitta mitten av bilden för att vera var skottet ska rotera runt.
    Vector2 direction;
    float speed = 2;
    double x, y;
    int angle;
    Random r = new Random();
    bool Harshield;


    List<Fiende> Fiendelista;

    //hitbox 
    Rectangle Fiendehitbox;

    Texture2D Blob;

    public Fiende(Texture2D texture, Texture2D Dot, List<Fiende> flista, Texture2D shieldtex, bool SkaHashield) {
        this.texture = texture;
        shield = shieldtex;

        if (SkaHashield) {
            Harshield = true;
        } else {
            Harshield = false;
        }

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

        Fiendelista = flista;

        //Skapa hitbox
        Fiendehitbox = new Rectangle((int)pos.X, (int)pos.Y, 70, 70);

        Blob = Dot; // för test
    }

    public Rectangle getFiendeHitbox() { return Fiendehitbox; }


    public void Update(int i) {
        pos -= direction * speed; // Gå rakt mot spelaren (direction) som är förutbestämd i konstruktorn
        Fiendehitbox.X = (int)pos.X - 35; // -35 för att få hitboxen imitten av fienden 
        Fiendehitbox.Y = (int)pos.Y - 35; // -35 för att få hitboxen imitten av fienden 
    }

    //Används för att veta vrt explotionen ska vara när fiende blir träffad.
    public Vector2 getpos() { return new Vector2(Fiendehitbox.X, Fiendehitbox.Y); }

    public bool Träffad(int i) {
        bool träff = false;
        //Avståndsformeln *Roten ur*(x2 − x1)^2 + (y2 − y1)^2 | Används för att bedömma om en motståndare är nära någ för att anses vara en träff (onödigt att använda hitboxes här)
        // Vi vet att spelarpositonen är fast och är 490, 490
        if (Math.Sqrt(Math.Pow(pos.X - 490, 2) + Math.Pow(pos.Y - 490, 2)) < 50) {
            Fiendelista.RemoveAt(i);
            träff = true;
        }
        return träff;
    }

    public bool Getshield() { return Harshield; }
    public void Removeshield() { Harshield = false; }

    public void Draw(SpriteBatch spriteBatch) {
        //Rita ut skottet samma som overloads som spelaren (för att kunna använda rotation)
        spriteBatch.Draw(texture, pos, null, null, orgin, Rotation);

        //Om Fiende har en sköld ska den ritas ut på samma plats som fienden 
        if (Harshield) {
            spriteBatch.Draw(shield, pos, null, null, orgin, Rotation);
        }

        // Visualisera hitboxen med koden under
        //spriteBatch.Draw(Blob,new Vector2(Fiendehitbox.X - 35,Fiendehitbox.Y - 35),Color.Red);
    }
}
