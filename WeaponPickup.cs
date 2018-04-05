using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace Vault_Prisoner
{
    class WeaponPickup : Situation
    {
        private Texture2D image;
        private Random rng;
        private int newWeaponValue;

        public WeaponPickup(Texture2D image, Random rng) 
            : base(image, rng)
        {
            this.image = image;
            this.rng = rng;
        }

        protected override void SetDefaultValues()
        {
            header = "Weapon";
            body = "You've found a\nweapon!\n\n";
            action = "Weapon value:    ";
            reaction = "";
            conclusion = "Attack rating\n(min-max): ";
        }

        public override void Activate(Player player, Gui gui)
        {
            SetDefaultValues();
            menu = gui.allMenus["weapon"];
            
            if(menu != null)
            {
                //Check to see if the player's current weapon is weaker. If so, increment the player's stats with the new weapon
                newWeaponValue = rng.Next(1, 5); //1, 2, 3, 4

                if (newWeaponValue > player.weaponValue)
                {
                    player.weaponValue = newWeaponValue;
                    player.AttackMin = newWeaponValue;
                    player.AttackMax = newWeaponValue + 5; //Starting attack max

                    body += "You replace your old\nweapon with this one.";
                }
                else
                {
                    body += "it's not better\nthan what you\ncurrently have, so\nyou put it back.";
                }

                action += newWeaponValue;
                conclusion += player.AttackMin + " - " + player.AttackMax;

                status = "pass";
            }
        }
    }
}
