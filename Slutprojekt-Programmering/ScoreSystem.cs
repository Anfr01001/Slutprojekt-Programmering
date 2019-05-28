using System.Collections.Generic;
using System.IO;

class ScoreSystem {

    /// <summary>  
    /// Här söts allt gällande topplistan som kan ses i slutet.
    ///Eftersom jag vill att spelarens resultat ska bli kvar även efter omstart av spel sparas listan i ett textdokument.
    ///Alla resultat sparas och sedan tas de bästa ut med hjälp av koden.  
    ///Koden tar ut resultaten med hjälp at "|" täcken som är plaserade efter värje nytt score. så själva listan ser ut ex. |56|87|8|12|77|
    /// </summary>

    string tempScoreboard;
    List<int> BestScore = new List<int>();
    int StartIndex, EndIndex;
    string tempscore;

    // --- lägg till nytt score i filen
    public void UpdateraScoreLista(string NewScore) {
        // Scoreboard sparas som en textfil för att kunna används även efter det att spelat startats om.

        // kopierar filen in i en string
        tempScoreboard = File.ReadAllText("Scoreboard.txt");
        //Lägger till nytt score separerat med | för att göra det lättare att hitta senare
        tempScoreboard = tempScoreboard + NewScore + "|";
        //Skriver över texten i filen igen.
        File.WriteAllText("Scoreboard.txt", tempScoreboard);
    }


    // --- Få ut en lista med alla scores i nummerårdning.
    public List<int> getScoreLista() {
        //nollställer listan och index samt läser av scoreboardfilen.
        StartIndex = 0;
        EndIndex = 0;
        BestScore.RemoveRange(0, BestScore.Count); // Ränsa listan.
        tempScoreboard = File.ReadAllText("Scoreboard.txt");

        while (EndIndex < tempScoreboard.Length - 1) {

            // Leta upp första | efter var senaste sökning avslutades.
            StartIndex = tempScoreboard.IndexOf('|', EndIndex);
            // Hitta | efter den förra för då vet vi att "scoren" står där emellan.
            EndIndex = tempScoreboard.IndexOf('|', StartIndex + 1);
            // börja från start (senaste |) och eftersom Substring vill ha längen blir den slutet minus början (-1 pga att vi inte vill ha med den senaste "|")
            tempscore = tempScoreboard.Substring(StartIndex + 1, EndIndex - StartIndex - 1);

            if (tempscore != null)
                BestScore.Add(int.Parse(tempscore));

        }

        //Sortera listan med alla scores och vänd sedan på den så störst blir först.
        BestScore.Sort();
        BestScore.Reverse();

        return BestScore;
    }

}
