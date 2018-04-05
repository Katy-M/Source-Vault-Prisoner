using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace Vault_Prisoner
{
    class ArmorPickup : Situation
    {
        private Texture2D image;
        private Random rng;
        private int pointsToIncrement;

        public ArmorPickup(Texture2D image, Random rng) 
            : base(image, rng)
        {
            type = "armorPickup";
            this.image = image;
            this.rng = rng;
        }

        protected override void SetDefaultValues()
        {
            header = "Armor";
            body = "You've found junk you\ncan use to protect\nyourself!\n\n";
            action = "Armor Gained:    ";
            reaction = "";
            conclusion = "Armor rating\n(min-max): ";
        }

        public override void Activate(Player player, Gui gui)
        {
            SetDefaultValues();
            menu = gui.allMenus["armor"];

            if(menu != null)
            {
                //Use random number generator to see what defense points the player will increment
                pointsToIncrement = rng.Next(0, 3);
                player.DefenseMin += pointsToIncrement;
                player.DefenseMax += pointsToIncrement;

                if (player.DefenseMax == 12 && player.DefenseMin == 4) //Capped
                {
                    body += "You're fully armed,\nso you put it back.";
                    pointsToIncrement = 0;
                }
                else
                {
                    //Change the body text depending on the quality of the armor
                    switch (pointsToIncrement)
                    {
                        case 0:
                            body += "It doesn't cover\nanything, so you\nput it back.";
                            break;
                        case 1:
                            body += "It covers some\nareas.";
                            break;
                        case 2:
                            body += "It can block some\nattacks.";
                            break;
                    }
                }

                action += pointsToIncrement;
                conclusion += player.DefenseMin + " - " + player.DefenseMax;

                status = "pass";
            }
        }
    }
}
