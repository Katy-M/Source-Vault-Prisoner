using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace Vault_Prisoner
{
    class Mishap : Situation
    {
        private Texture2D image;
        private Random rng;

        //Variable stores how many total health points a player can lose
        private int damage;

        public Mishap(Texture2D image, Random rng) 
            : base(image, rng)
        {
            type = "mishap";
            this.image = image;
            this.rng = rng;
        }

        protected override void SetDefaultValues()
        {
            header = "Mishap";
            body = "You've crashed at\nthe bottom of a\npitfall!\n\nClick\"CHECK\" to see\nhow badly you're\ninjured.";
            action = "Max injury:          ";
            reaction = "You protected:   ";
            conclusion = "";
        }

        public override void Activate(Player player, Gui gui)
        {
            SetDefaultValues();
            menu = gui.allMenus["mishap"];

            if(menu != null)
            {
                damage = 8;

                int saved = 0;
                int healthLost = 0;

                //Use random number generator to see how many health points the player will save (depending on defense)
                if (status == "clicked")
                {
                    saved = rng.Next(player.DefenseMin, player.DefenseMax + 1);
                    healthLost = damage - saved;

                    if (healthLost < 0)
                    {
                        healthLost = 0;
                    }

                    player.Health -= healthLost;

                    action += damage;
                    reaction += saved;
                    conclusion = "You lost " + healthLost + " health.";
                    status = "pass";
                }
            }
        }
    }
}
