using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

class Skott {

    Texture2D texture;
    Vector2 pos;
    float Rotation;
    Vector2 orgin = new Vector2(23f, 13f); //-- Hitta mitten av bilden för att vera var skottet ska rotera runt.
    Vector2 direction;
    float speed = 6;
    List<Skott> skottlista;

    //Hitbox
    Rectangle skotthitbox;

    public Skott(Texture2D texture, Vector2 position, float rot, List<Skott> slista) {
        //När konstruktion kallas vill vi "Få ut variablerna"
        this.texture = texture;
        pos = position;
        Rotation = rot;
        skottlista = slista;

        //Skapa hitbox
        skotthitbox = new Rectangle((int)pos.X, (int)pos.Y, 8, 8);
    }

    public Rectangle getskottHitbox() { return skotthitbox; }

    public void Update(int i) {
        // Gör om spelarens nuvarande rotation till en riktning som sedan används för att flytta skotten.
        direction = new Vector2((float)Math.Cos(Rotation), (float)Math.Sin(Rotation));
        //Flytta skotten och en variabel för fart för att lätt kunna ändra det.
        pos -= direction * speed;
        skotthitbox.X = (int)pos.X - 4;
        skotthitbox.Y = (int)pos.Y - 4;

        //Avståndsformeln *Roten ur*(x2 − x1)^2 + (y2 − y1)^2 | används för att kolla om skottet är utanför kartan (1500 är utanför kartan med marginal) för att då ta bort det. (För bättre prestanda)
        if (Math.Sqrt(Math.Pow(pos.X, 2) + Math.Pow(pos.Y, 2)) > 1500) {
            skottlista.RemoveAt(i);
        }

    }

    public void Draw(SpriteBatch spriteBatch) {
        //Rita ut skottet samma som overloads som spelaren
        spriteBatch.Draw(texture, pos, null, null, orgin, Rotation);
    }

}
