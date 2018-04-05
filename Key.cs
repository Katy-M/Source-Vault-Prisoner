using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace Vault_Prisoner
{
    class Key : Situation
    {
        private Texture2D image;
        private Random rng;

        public Key(Texture2D image, Random rng) 
            : base(image, rng)
        {
            type = "key";
            this.image = image;
            this.rng = rng;
            status = "";
        }

        protected override void SetDefaultValues()
        {
            header = "Key";
            body = "You've found a key!\nNice work!";
            action = "";
            reaction = "";
            conclusion = "Keys in inventory: ";
        }

        public override void Activate(Player player, Gui gui)
        {
            SetDefaultValues();
            menu = gui.allMenus["key"];

            if (menu != null)
            {
                player.keys++;
                conclusion += player.keys + "/2";
            }
        }
    }
}
