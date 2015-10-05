using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Pokemon_delta_emerald
{
    class BattleClass
    {
        //visuals of users pokemon
        Texture2D[] userPokemonSprite;
        Rectangle userPokemonRec;
                
        //hold the pokemon's HP
        int oldHP; //oldHP is the amount of HP currently shown by the health bar
        
        //hold the enemy's pokemon's HP
        int oldHPEnemy;

        //hold the pokemon's stats
        int currentPokemon;
        Pokemon[] pokemonTeamEnemy = new Pokemon[6];
        int currentPokemonEnemy;

        //temporary variables used for damage calculation
        double tempMultiplier;
        double tempMultiplierEnemy;

        //the slot of the move the pokemon uses
        int tempMove = -1;
        int tempMoveEnemy;

        //the number of the move the pokemon uses
        int tempMoveNum;
        int tempMoveNumEnemy;

        int higherSpeed;
        int tempDamage;
        int tempDamageEnemy;
        string tempString;

        //visuals of the enemies pokemon
        Texture2D[] enemyPokemonSprite;
        Rectangle enemyPokemonRec;

        //trainer images
        Texture2D[] trainerSprite = new Texture2D[5];
        Rectangle trainerRec;
        //Texture2D enemyTrainerSprite;
        //Rectangle enemyTrainerRec;
        //string enemyTrainer;
        bool trainerBattle;
        int enemyBattle;        
        
        //counters
        int counter; //used for animated pokemon sprites
        int counter2; //used for intro animations
        int counter3 = 2000; //used for battles
        bool enemyDefeated; //check if battle has been won

        //pokeballs
        Texture2D[] pokeball = new Texture2D[4];
        Rectangle pokeballRec;

        //background image
        Rectangle battleBackgroundRec;
        Texture2D battleBackground;

        Rectangle textboxRec;
        Texture2D textboxTex;

        Rectangle[] healthBarRec = new Rectangle[2]; //[0] is the user's rec
        Texture2D[] healthBarSprite = new Texture2D[2]; //[1] is the enemy's rec

        Rectangle[] healthBarColourRec = new Rectangle[2];
        Texture2D[] healthBarColourSprite = new Texture2D[4];
        
        ContentManager Content;
        Text textClass;

        Random rand = new Random();

        Option option;

        public BattleClass(bool trainerBttle, int enemyBttle, ContentManager cnt)
        {
            trainerBattle = trainerBttle;
            enemyBattle = enemyBttle;
            Content = cnt;

            Initialize();
        }

        private void Initialize()
        {
            setEnemyPokemon();
            enemyPokemonSprite = new Texture2D[Global.animationLength[pokemonTeamEnemy[currentPokemonEnemy].pokemonID]];
            userPokemonSprite = new Texture2D[Global.animationLengthBack[Global.team[currentPokemon].pokemonID]];

            //load the trainer
            for (int i = 0; i < trainerSprite.Length; i++)
            {
                trainerSprite[i] = Content.Load<Texture2D>("Trainers/" + Global.trainer + i);
            }

            trainerRec = new Rectangle(80 - trainerSprite[0].Width / 2, 112 - (trainerSprite[0].Height), (trainerSprite[0].Width), trainerSprite[0].Height);
            trainerRec = Global.changeRecSize(trainerRec);

            //load the pokeball the trainer throws
            for (int i = 0; i < pokeball.Length; i++)
            {
                pokeball[i] = Content.Load<Texture2D>("pokeball" + i);
            }

            //load the enemies pokemon
            for (int j = 0; j < enemyPokemonSprite.Length; j++)
            {
                if (pokemonTeamEnemy[currentPokemonEnemy].pokemonID < 9)
                {
                    enemyPokemonSprite[j] = Content.Load<Texture2D>("Pokemon" + "/00" + (pokemonTeamEnemy[currentPokemonEnemy].pokemonID + 1) + " (" + (j + 1) + ")");
                }
                else if (pokemonTeamEnemy[currentPokemonEnemy].pokemonID < 99)
                {
                    enemyPokemonSprite[j] = Content.Load<Texture2D>("Pokemon" + "/0" + (pokemonTeamEnemy[currentPokemonEnemy].pokemonID + 1) + " (" + (j + 1) + ")");
                }
                else
                {
                    enemyPokemonSprite[j] = Content.Load<Texture2D>("Pokemon" + "/" + (pokemonTeamEnemy[currentPokemonEnemy].pokemonID + 1) + " (" + (j + 1) + ")");
                }
            }

            enemyPokemonRec = new Rectangle((176 - enemyPokemonSprite[0].Width / 4), 66 - (enemyPokemonSprite[0].Height / 2), enemyPokemonSprite[0].Width / 2, enemyPokemonSprite[0].Height / 2);
            enemyPokemonRec = Global.changeRecSize(enemyPokemonRec);

            //load the users pokemon
            for (int j = 0; j < userPokemonSprite.Length; j++)
            {
                if (Global.team[currentPokemon].pokemonID < 9)
                {
                    userPokemonSprite[j] = Content.Load<Texture2D>("PokemonBack" + "/00" + (Global.team[currentPokemon].pokemonID + 1) + "b (" + (j + 1) + ")");
                }
                else if (Global.team[currentPokemon].pokemonID < 99)
                {
                    userPokemonSprite[j] = Content.Load<Texture2D>("PokemonBack" + "/0" + (Global.team[currentPokemon].pokemonID + 1) + "b (" + (j + 1) + ")");
                }
                else 
                {
                    userPokemonSprite[j] = Content.Load<Texture2D>("PokemonBack" + "/" + (Global.team[currentPokemon].pokemonID + 1) + "b (" + (j + 1) + ")");
                }
            }

            userPokemonRec = new Rectangle(62 - userPokemonSprite[0].Width / 4, 112 - userPokemonSprite[0].Height / 2, userPokemonSprite[0].Width / 2, userPokemonSprite[0].Height / 2);
            userPokemonRec = Global.changeRecSize(userPokemonRec);

            //display background
            battleBackground = Content.Load<Texture2D>("background");
            battleBackgroundRec = new Rectangle(0, 0, battleBackground.Width, battleBackground.Height);
            battleBackgroundRec = Global.changeRecSize(battleBackgroundRec);

            //display textbox
            textboxTex = Content.Load<Texture2D>("Text/box4");
            textboxRec = new Rectangle(0, 160 - textboxTex.Height, textboxTex.Width, textboxTex.Height);
            textboxRec = Global.changeRecSize(textboxRec);

            //display health bars
            healthBarSprite[0] = Content.Load<Texture2D>("healthbar1");
            healthBarRec[0] = new Rectangle(240 - healthBarSprite[0].Width, 112 - healthBarSprite[0].Height, healthBarSprite[0].Width, healthBarSprite[0].Height);
            healthBarRec[0] = Global.changeRecSize(healthBarRec[0]);

            healthBarSprite[1] = Content.Load<Texture2D>("healthbar2");
            healthBarRec[1] = new Rectangle(0, 0, healthBarSprite[1].Width, healthBarSprite[1].Height);
            healthBarRec[1] = Global.changeRecSize(healthBarRec[1]);

            healthBarColourSprite[0] = Content.Load<Texture2D>("healthbarred");
            healthBarColourSprite[1] = Content.Load<Texture2D>("healthbaryellow");
            healthBarColourSprite[2] = Content.Load<Texture2D>("healthbargreen");
            healthBarColourSprite[3] = Content.Load<Texture2D>("healthbarblue");

            healthBarColourRec[0] = new Rectangle(184, 91, 48, 3);
            healthBarColourRec[0] = Global.changeRecSize(healthBarColourRec[0]);
            healthBarColourRec[1] = new Rectangle(50, 23, 48, 3);
            healthBarColourRec[1] = Global.changeRecSize(healthBarColourRec[1]);

            oldHP = Global.team[currentPokemon].currentHP;
            oldHPEnemy = pokemonTeamEnemy[currentPokemonEnemy].currentHP;

            option = new Option(trainerBattle, Global.team[currentPokemon].moves, Content);
        }

        public void Update(GameTime gameTime)
        {
            if (gameTime.TotalGameTime.Milliseconds % 2 == 0)
            {
                counter++;                
            }
            if (counter2 < 5)
            {
                if (counter > 50)
                {
                    trainerRec.X -= counter2 * Global.unit;

                    if (gameTime.TotalGameTime.Milliseconds % 200 == 0)
                    {
                        counter2++;
                    }
                }
            }

            else if (Global.team[currentPokemon].currentHP > 0 && pokemonTeamEnemy[currentPokemonEnemy].currentHP > 0 && counter2 >= 5)
            {
                battle();

                if (option != null)
                {
                    option.Update(gameTime);
                }
                else
                {
                    counter3++;
                }
            }
            else if (oldHP <= 0)
            {
                //pokemonHP[currentPokemon] = 0;

                if (teamAlive(Global.team) == false && counter3 > 120)
                {
                    Global.gameOver = true;
                }

                counter3++;

            }
            else if (oldHPEnemy <= 0)
            {
                //pokemonHPEnemy[currentPokemonEnemy] = 0;

                if (teamAlive(pokemonTeamEnemy) == false && counter3 > 120)
                {
                    enemyDefeated = true;                    
                }

                counter3++;
            }
        }

        //decided which attacks each user will use, and what order they attack in
        private void battle()
        {
            if (option == null && counter3 > 240)
            {
                option = new Option(trainerBattle, Global.team[currentPokemon].moves, Content);
                tempMove = -1;
            }

            if (option != null && option.GetSelection() != -1)
            {
                tempMove = option.GetSelection();
                option = null;
                counter3 = 0;
            }

            if (counter3 == 0 && tempMove != -1)
            {
                if (Global.team[currentPokemon].stats[5] > pokemonTeamEnemy[currentPokemonEnemy].stats[5])
                {
                    higherSpeed = 1; //1 means user is faster
                }
                else if (Global.team[currentPokemon].stats[5] < pokemonTeamEnemy[currentPokemonEnemy].stats[5])
                {
                    higherSpeed = 0; //0 means enemy is faster
                }
                else
                {
                    higherSpeed = rand.Next(2);
                }

                tempDamage = damage(true, tempMove);
                tempDamageEnemy = damage(false, pokemonTeamEnemy[currentPokemonEnemy].randomMove());

                if (higherSpeed == 1)
                {
                    oldHPEnemy = pokemonTeamEnemy[currentPokemonEnemy].currentHP;
                    pokemonTeamEnemy[currentPokemonEnemy].currentHP -= tempDamage;

                    tempString = Global.pokemonName[Global.team[currentPokemon].pokemonID] + " used " + Global.team[currentPokemon].moves[tempMove];
                    textClass = new Text(tempString, 1, Content);
                }
                else
                {
                    oldHP = Global.team[currentPokemon].currentHP;
                    Global.team[currentPokemon].currentHP -= tempDamageEnemy;

                    tempString = Global.pokemonName[pokemonTeamEnemy[currentPokemonEnemy].pokemonID] + " used " + pokemonTeamEnemy[currentPokemonEnemy].moves[tempMoveEnemy];
                    textClass = new Text(tempString, 1, Content);
                }
            }

            else if (counter3 == 120 && tempMove != -1)
            {
                if (higherSpeed == 0)
                {
                    oldHPEnemy = pokemonTeamEnemy[currentPokemonEnemy].currentHP;
                    pokemonTeamEnemy[currentPokemonEnemy].currentHP -= tempDamage;

                    tempString = Global.pokemonName[Global.team[currentPokemon].pokemonID] + " used " + Global.team[currentPokemon].moves[tempMove];
                    textClass = null;
                    textClass = new Text(tempString, 1, Content);
                }
                else
                {
                    oldHP = Global.team[currentPokemon].currentHP;
                    Global.team[currentPokemon].currentHP -= tempDamageEnemy;

                    tempString = Global.pokemonName[pokemonTeamEnemy[currentPokemonEnemy].pokemonID] + " used " + pokemonTeamEnemy[currentPokemonEnemy].moves[tempMoveEnemy];
                    textClass = null;
                    textClass = new Text(tempString, 1, Content);
                }
            }
        }

        //calculate how much damage an attack does
        private int damage(bool user, int move)
        {
            int damage = 0;
            int userPokemon = Global.team[currentPokemon].pokemonID;
            int enemyPokemon = pokemonTeamEnemy[currentPokemonEnemy].pokemonID;

            if (user == true)
            {
                tempMoveNum = Global.getMoveNumber(Global.team[currentPokemon].moves[move]);
                
                if (Global.moveDamage[tempMoveNum] > 0)
                {
                    tempMultiplier = Global.getMultiplier(tempMoveNum, userPokemon, enemyPokemon);

                    if (Global.movePhysical[tempMoveNum])
                    {
                        damage = Convert.ToInt32(((((2 * Global.team[currentPokemon].level / 5 + 2) * Global.team[currentPokemon].stats[1] * Global.moveDamage[tempMoveNum] / pokemonTeamEnemy[currentPokemonEnemy].stats[2]) / 50) + 2) * tempMultiplier);
                    }
                    else
                    {
                        damage = Convert.ToInt32(((((2 * Global.team[currentPokemon].level / 5 + 2) * Global.team[currentPokemon].stats[3] * Global.moveDamage[tempMoveNum] / pokemonTeamEnemy[currentPokemonEnemy].stats[4]) / 50) + 2) * tempMultiplier);
                    }
                }
            }
            else
            {
                tempMoveNumEnemy = Global.getMoveNumber(pokemonTeamEnemy[currentPokemonEnemy].moves[move]);
                
                if (Global.moveDamage[tempMoveNumEnemy] > 0)
                {
                    tempMultiplierEnemy = Global.getMultiplier(tempMoveNum, enemyPokemon, userPokemon);

                    if (Global.movePhysical[tempMoveNum])
                    {
                        damage = Convert.ToInt32(((((2 * pokemonTeamEnemy[currentPokemonEnemy].level / 5 + 2) * pokemonTeamEnemy[currentPokemonEnemy].stats[1] * Global.moveDamage[tempMoveNumEnemy] / Global.team[currentPokemon].stats[2]) / 50) + 2) * tempMultiplierEnemy);
                    }
                    else
                    {
                        damage = Convert.ToInt32(((((2 * pokemonTeamEnemy[currentPokemonEnemy].level / 5 + 2) * pokemonTeamEnemy[currentPokemonEnemy].stats[3] * Global.moveDamage[tempMoveNumEnemy] / Global.team[currentPokemon].stats[4]) / 50) + 2) * tempMultiplierEnemy);
                    }
                }
            }

            return damage;
        }

        //set what Pokemon the enemy has
        private void setEnemyPokemon()
        {
            if (trainerBattle == true)
            {
                if (enemyBattle == 200)
                {

                    if (Global.starterPokemon == 0)
                    {
                        pokemonTeamEnemy[0] = new Pokemon(3, Convert.ToInt32((Math.Pow(5, 3))), true, false);
                    }
                    else if (Global.starterPokemon == 3)
                    {
                        pokemonTeamEnemy[0] = new Pokemon(6, Convert.ToInt32((Math.Pow(5, 3))), true, false);
                    }
                    else
                    {
                        pokemonTeamEnemy[0] = new Pokemon(0, Convert.ToInt32((Math.Pow(5, 3))), true, false);
                    }
                }
            }
            else
            {
                pokemonTeamEnemy[0] = Global.getWildPokemon(enemyBattle - 500);
            }
        }
        
        //check if any pokemon on a team are still alive
        private bool teamAlive(Pokemon[] team)
        {
            bool alive = false;
            int i = 0;

            while (i < team.Length && alive == false)
            {
                if (team[i] != null)
                {
                    if (team[i].currentHP > 0)
                    {
                        alive = true;
                    }
                }

                i++;
            }

            return alive;
        }

        //check if a battle has ended
        public bool isDefeated()
        {
            return enemyDefeated;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(battleBackground, battleBackgroundRec, Color.White);
            spriteBatch.Draw(enemyPokemonSprite[counter % enemyPokemonSprite.Length], enemyPokemonRec, Color.White);
            spriteBatch.Draw(userPokemonSprite[counter % userPokemonSprite.Length], userPokemonRec, Color.White);
            spriteBatch.Draw(textboxTex, textboxRec, Color.White);

            if (counter2 < 5)
            {
                spriteBatch.Draw(trainerSprite[counter2], trainerRec, Color.White);
            }
            else
            {
                spriteBatch.Draw(healthBarSprite[0], healthBarRec[0], Color.White);
                spriteBatch.Draw(healthBarSprite[1], healthBarRec[1], Color.White);

                if (oldHP > Global.team[currentPokemon].currentHP)
                {
                    oldHP--;
                }
                double percentHP = (double)(oldHP) / (double)(Global.team[currentPokemon].stats[0]);
                healthBarColourRec[0].Width = Convert.ToInt32(Math.Ceiling(percentHP * 48 * Global.unit));

                if (percentHP > 0.5)
                {
                    spriteBatch.Draw(healthBarColourSprite[2], healthBarColourRec[0], Color.White);
                }
                else if (percentHP > 0.2)
                {
                    spriteBatch.Draw(healthBarColourSprite[1], healthBarColourRec[0], Color.White);
                }
                else
                {
                    spriteBatch.Draw(healthBarColourSprite[0], healthBarColourRec[0], Color.White);
                }

                if (oldHPEnemy > pokemonTeamEnemy[currentPokemonEnemy].currentHP)
                {
                    oldHPEnemy--;
                }
                double percentHPEnemy = (double)(oldHPEnemy) / (double)(pokemonTeamEnemy[currentPokemonEnemy].stats[0]);
                healthBarColourRec[1].Width = Convert.ToInt32(Math.Ceiling(percentHPEnemy * 48 * Global.unit));

                if (percentHPEnemy > 0.5)
                {
                    spriteBatch.Draw(healthBarColourSprite[2], healthBarColourRec[1], Color.White);
                }
                else if (percentHPEnemy > 0.2)
                {
                    spriteBatch.Draw(healthBarColourSprite[1], healthBarColourRec[1], Color.White);
                }
                else
                {
                    spriteBatch.Draw(healthBarColourSprite[0], healthBarColourRec[1], Color.White);
                }
            }

            if (counter2 >= 5)
            {
                if (option != null)
                {
                    option.Draw(spriteBatch);
                }

                if (textClass != null && counter3 < 240)
                {
                    textClass.Draw(spriteBatch);
                }
            }
        }

    }
}
