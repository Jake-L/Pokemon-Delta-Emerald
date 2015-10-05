using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pokemon_delta_emerald
{
    public class Pokemon
    {
        //information about the pokemon
        public int pokemonID;
        public int xp;
        public int level;
        public int currentHP;
        public string nature = "Serious";
        public string[] moves = new string[4];
        int[] evs = new int[6];
        double[] natureMultiplier = new double[6] { 1, 1, 1, 1, 1, 1 }; //still need to add this to stat calculation
        public int[] stats = new int[6];

        Random rand = new Random();

        //write to text files
        System.IO.StreamWriter file;
        System.IO.StreamReader read;
        string address = "";
        string filename = "";
        DateTime nowDateTime;

        //create wild / trainer's Pokemon
        public Pokemon(int num, int exp, bool trainer, bool wild)
        {
            //only trained Pokemon have EVs
            if (wild == false)
            {
                if (trainer == true)
                {
                    randomEV();
                }

                randomNature();
            }         

            pokemonID = num;
            xp = exp;
            level = Convert.ToInt32(Math.Ceiling(Math.Pow(xp, (1.00 / 3.00))));

            getMoves();
            calculateStats();
            currentHP = stats[0];
        }

        //create the player's Pokemon
        public Pokemon(string handler)
        {
            filename = handler;
        }

        //generate random EVs for the enemy trainer's Pokemon
        private void randomEV()
        {
            int i = 0;
            int maxEV = Convert.ToInt32(Math.Ceiling(Math.Pow(xp, (1.00 / 3.00))) * 2.5);
            int currentEV = 0;

            while (currentEV < maxEV)
            {
                evs[i] += rand.Next(maxEV / 2);

                if (evs[i] > maxEV - currentEV)
                {
                    evs[i] = maxEV - currentEV;
                }

                currentEV += evs[i];

                i++;

                if (i > 5)
                {
                    i = 0;
                }
            }
        }

        //generate the Pokemon's nature
        private void randomNature()
        {
            int num = rand.Next(25);

            if (num == 0)
            {
                nature = "Hardy";
            }
            else if (num == 1)
            {
                nature = "Lonely";
            }
            else if (num == 2)
            {
                nature = "Brave";
            }
            else if (num == 3)
            {
                nature = "Adamant";
            }
            else if (num == 4)
            {
                nature = "Naughty";
            }
            else if (num == 5)
            {
                nature = "Bold";
            }
            else if (num == 6)
            {
                nature = "Docile";
            }
            else if (num == 7)
            {
                nature = "Relaxed";
            }
            else if (num == 8)
            {
                nature = "Impish";
            }
            else if (num == 9)
            {
                nature = "Lax";
            }
            else if (num == 10)
            {
                nature = "Timid";
            }
            else if (num == 11)
            {
                nature = "Hasty";
            }
            else if (num == 12)
            {
                nature = "Serious";
            }
            else if (num == 13)
            {
                nature = "Jolly";
            }
            else if (num == 14)
            {
                nature = "Naive";
            }
            else if (num == 15)
            {
                nature = "Modest";
            }
            else if (num == 16)
            {
                nature = "Mild";
            }
            else if (num == 17)
            {
                nature = "Quiet";
            }
            else if (num == 18)
            {
                nature = "Bashful";
            }
            else if (num == 19)
            {
                nature = "Rash";
            }
            else if (num == 20)
            {
                nature = "Calm";
            }
            else if (num == 21)
            {
                nature = "Gentle";
            }
            else if (num == 22)
            {
                nature = "Sassy";
            }
            else if (num == 23)
            {
                nature = "Careful";
            }
            else if (num == 24)
            {
                nature = "Quirky";
            }
        }
        
        //generate the Pokemon's moves
        private void getMoves()
        {
            int i = 0; //count the moves
            int j = 0; //count the slot the move goes in

            while (i < 20 && Global.pkmnMovesLvl[pokemonID, i] != 0 && Global.pkmnMovesLvl[pokemonID, i] <= level)
            {
                moves[j] = Global.pokemonMoves[pokemonID, i];

                if (j == 4)
                {
                    j = 0;
                }

                j++;
                i++;
            }
        }

        //returns the slot of a random move
        public int randomMove()
        {
            string move = null;
            int value = 0;

            while (move == null)
            {
                value = rand.Next(4);
                move = moves[value];
            }
            
            return value;
        }

        //generates the pokemon's stats
        private void calculateStats()
        {
            //HP
            stats[0] = 10 + Convert.ToInt32(((2 * Global.hp[pokemonID]) + (evs[0] / 4) + 100) * level / 100.00);

            //Attack
            stats[1] = 5 + Convert.ToInt32((2 * Global.attack[pokemonID] + (evs[1] / 4)) / 100.00 * level);

            //Defense
            stats[2] = 5 + Convert.ToInt32((2 * Global.attack[pokemonID] + (evs[1] / 4)) / 100.00 * level);

            //Sp. Attack
            stats[3] = 5 + Convert.ToInt32((2 * Global.attack[pokemonID] + (evs[1] / 4)) / 100.00 * level);

            //Sp. Defense
            stats[4] = 5 + Convert.ToInt32((2 * Global.attack[pokemonID] + (evs[1] / 4)) / 100.00 * level);

            //Speed
            stats[5] = 5 + Convert.ToInt32((2 * Global.attack[pokemonID] + (evs[1] / 4)) / 100.00 * level);
        }

        //save pokemon to a text file
        public string savePokemon()
        {
            //check if the pokemon has been saved before
            if (filename == null)
            {
                //create the filename
                nowDateTime = DateTime.Now;
                filename = nowDateTime.Year.ToString();
                filename += nowDateTime.Month.ToString();
                filename += nowDateTime.Day.ToString();
                filename += nowDateTime.Hour.ToString();
                filename += nowDateTime.Minute.ToString();
                filename += nowDateTime.Second.ToString();
            }

            //prepare text writing variables
            address = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

            file = new System.IO.StreamWriter(address + filename + ".txt");
            file.WriteLine(pokemonID);
            file.WriteLine(nature);

            for (int i = 0; i < evs.Length; i++)
            {
                file.WriteLine(evs[i]);
            }

            for (int i = 0; i < moves.Length; i++)
            {
                file.WriteLine(moves[i]);
            }

            file.Close();

            return filename;
        }
    }
}
