using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

// Denna klassen har hand om explotionerna som sker när en fiende skjuts.

class Explotion {
    Texture2D[] Textures;
    Vector2 pos;
    double SenasteFrameByte = 0;
    double TidMellanFrames = 0.06; // Det finns 16 st explotions bilder totalt för att få explotionen att "Röra sig"
    int frame = 2; // börja på frame 2 för att få explotionen mer direkt.

    public Explotion(Texture2D[] Tex, Vector2 position) {
        Textures = Tex;
        pos = position;
    }

    public int Update(GameTime gameTime) {
        //Kolla om nog med tid har gått för att en ny "frame" ska användas
        if (gameTime.TotalGameTime.TotalSeconds - SenasteFrameByte > TidMellanFrames) {
            SenasteFrameByte = gameTime.TotalGameTime.TotalSeconds;
            frame++;
        }

        // Så jag vet när explotionen ska tas bort (efter 16 frames)
        return frame;
    }

    public void Draw(SpriteBatch spriteBatch) {
        spriteBatch.Draw(Textures[frame], pos, Color.White);
    }

}
