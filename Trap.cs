using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace Vault_Prisoner
{
    class Trap : Situation
    {
        private Texture2D image;
        private Random rng;
        private int count;
        private int timesRolled;

        //The difficulty of the trap. Does not exceed the player's max luck value so there is always a chance to escape the trap
        private int difficulty;
        private int damage;
        private int luckRolled;

        public Trap(Texture2D image, Random rng) 
            : base(image, rng)
        {
            type = "trap";
            this.image = image;
            this.rng = rng;

            difficulty = 0;

            count = 0;
            timesRolled = 0;
        }

        protected override void SetDefaultValues()
        {
            damage = 3;

            header = "Trap";
            body = "You've stumbled into\na deadly trap!\n\nYour only option is to\nstruggle. Each failed\nstruggle makes you\nweaker...";
            action = "Trap difficulty:   ";
            reaction = "Your luck:            ";
            conclusion = "";
        }

        public override void Activate(Player player, Gui gui)
        {
            SetDefaultValues();
            menu = gui.allMenus["trap"];

            if(menu != null)
            {
                if (count == 0)
                {
                    difficulty = rng.Next(player.luckMin + 2, player.luckMax);
                }

                if (status == "clicked")
                {
                    luckRolled = rng.Next(player.luckMin + timesRolled, player.luckMax + timesRolled + 1);

                    action += difficulty;
                    reaction += luckRolled;

                    //Determine if the player can set themselves free - if difficulty <= luck
                    if (difficulty <= luckRolled)
                    {
                        conclusion = "You escaped!";
                        status = "pass";
                        timesRolled = 0;
                    }
                    else //Subtract damage from the player's health
                    {
                        player.Health -= damage;
                        conclusion = "Your Health: " + player.Health + "/20";
                        status = "";
                        timesRolled += 1;
                    }

                    count = 1;
                }
            }
        }
    }
}
