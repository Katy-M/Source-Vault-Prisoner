/*Programmer: Katy Mollenkopf 
 * ---------------------------- */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace Vault_Prisoner
{
    class MonsterAttack : Situation
    {
        private Texture2D image;
        private Random rng;

        private int monsterHealth;
        private const int MAX_HEALTH = 8;

        //Maximum and minimum damage the monster can inflict or protect against
        private const int A_MAX_VALUE = 8;
        private const int A_MIN_VALUE = 1;

        private const int D_MAX_VALUE = 6;
        private const int D_MIN_VALUE = 0;

        //Is the monster attacking or defending?
        private string monsterState;

        private bool firstTime;

        public MonsterAttack(Texture2D image, Random rng) 
            : base(image, rng)
        {
            type = "monster";
            this.image = image;
            this.rng = rng;


            monsterHealth = MAX_HEALTH;
            firstTime = true;
            status = "";
        }

        protected override void SetDefaultValues()
        {
            if (monsterHasKey) header = "Attack - Key";
            else header = "    Attack";

            body = "You've encountered\none of the prison's\ndeadly monsters.\n";
            action = "";
            reaction = "";
            conclusion = "";
        }

        public override void Activate(Player player, Gui gui)
        {
            SetDefaultValues();
            menu = gui.allMenus["attack"];

            if (menu != null)
            {
                if (firstTime == true) monsterState = "attacking";

                if(monsterState == "attacking")
                {
                    body += "He begins his attack!\nClick \"ACT\" to protect\nyourself.";
                    action = "Attack value:      ";
                    reaction = "You protected:   ";
                }
                else if(monsterState == "defending")
                {
                    body += "You fight back!\nClick \"ACT\" to throw\nyourself at the\nmonster!";
                    action = "Attack value:      ";
                    reaction = "It protected:        ";
                }

                if(status == "clicked" && monsterState == "attacking")
                {
                    //Determine what attack value the monster will attack with
                    int attack = rng.Next(A_MIN_VALUE, A_MAX_VALUE + 1);
                    action += attack;

                    //Determine player's defend value
                    int defend = rng.Next(player.DefenseMin, player.DefenseMax + 1);
                    reaction += defend;

                    //Subtract from player health the difference of monster attack and player defend
                    if(attack - defend > 0)
                    {
                        player.Health -= (attack - defend);
                    }

                    conclusion += "Your health: " + player.Health + "/20";
                    monsterState = "defending";
                    status = "";

                    firstTime = false;
                    return;
                }
                else if (status == "clicked" && monsterState == "defending")
                {
                    //Determine player's attack value
                    int attack = rng.Next(player.AttackMin, player.AttackMax + 1);
                    action += attack;

                    //Determine what value the monster will defend with
                    int defend = rng.Next(D_MIN_VALUE, D_MAX_VALUE + 1);
                    reaction += defend;

                    //Subtract from monster health the difference of player attack and monster defend
                    if (attack - defend > 0)
                    {
                        monsterHealth -= (attack - defend);
                    }

                    if(monsterHealth <= 0)
                    {
                        monsterHealth = 0;

                        SetDefaultValues();
                        status = "pass";
                        monsterState = "";
                        body += "You defeated the\nmonster!";
                        monsterHealth = MAX_HEALTH;
                        firstTime = true;
                        return;
                    }
                    conclusion += "Monster health: " + monsterHealth + "/" + MAX_HEALTH;
                    monsterState = "attacking";
                    status = "";

                    firstTime = false;
                    return;
                }
            }
        }
    }
}
