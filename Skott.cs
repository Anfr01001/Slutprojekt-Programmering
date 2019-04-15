using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

    class Skott {

        Texture2D texture;
        Vector2 pos;
        float Rotation;
        Vector2 orgin = new Vector2(23f, 13f); //-- Hitta mitten av bilden för att vera var skottet ska rotera runt.
        MouseState MouseState;
        Vector2 direction;
        float speed = 6;

        //Hitbox
        Rectangle skotthitbox;

        public Skott(Texture2D texture, Vector2 position, float rot, MouseState MouseState) {
            //När konstruktion kallas vill vi "Få ut variablerna"
            this.texture = texture;
            pos = position;
            Rotation = rot;
            MouseState = MouseState;

            //Skapa hitbox
            skotthitbox = new Rectangle((int)pos.X, (int)pos.Y, 8, 8);
        }

        public Rectangle getskottHitbox() { return skotthitbox; }

        public void Update() {
            // Gör om spelarens nuvarande rotation till en riktning som sedan används för att flytta skotten.
            direction = new Vector2((float)Math.Cos(Rotation), (float)Math.Sin(Rotation));
            //Flytta skotten och en variabel för fart för att lätt kunna ändra det.
            pos -= direction * speed;
            skotthitbox.X = (int)pos.X - 4;
            skotthitbox.Y = (int)pos.Y - 4;

        }

        public void Draw(SpriteBatch spriteBatch) {
            //Rita ut skottet samma som overloads som spelaren
            spriteBatch.Draw(texture, pos, null, null, orgin, Rotation);
        }

    }