using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace Vault_Prisoner
{
    class HealthPickup : Situation
    {
        private Texture2D image;
        private Random rng;
        private int healthToGain;

        public HealthPickup(Texture2D image, Random rng) 
            : base(image, rng)
        {
            type = "healthPickup";
            this.image = image;
            this.rng = rng;
        }

        protected override void SetDefaultValues()
        {
            header = "health";
            body = "You've discovered a \nfirst aid pack!\n\n";
            action = "Health Gained      ";
            reaction = "";
            conclusion = "Current health: ";
        }

        public override void Activate(Player player, Gui gui)
        {
            SetDefaultValues();
            menu = gui.allMenus["health"];

            if(menu != null)
            {
                healthToGain = rng.Next(1, 7);

                //Display cap at the player's max health
                if (player.Health + healthToGain >= 20)
                {
                    conclusion += "20/20";
                }
                else
                {
                    conclusion += (player.Health + healthToGain) + "/" + "20";
                }

                player.Health += healthToGain;

                //Set variation for the text strings
                action += healthToGain.ToString();

                if (healthToGain >= 1 && healthToGain < 3) //low yield
                {
                    body += "There's not very\nmuch in here...";
                }
                else if (healthToGain >= 3 && healthToGain < 5) //mid yield
                {
                    body += "Some things in here\ncan help.";
                }
                else if (healthToGain >= 5 && healthToGain < 7) //high yield
                {
                    body += "This one is actually\nuseful!";
                }

                status = "pass";
            }
        }
    }
}
