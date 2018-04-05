using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Vault_Prisoner
{
    /// <summary>
    /// Base class for all the situations in the game
    /// </summary>
    abstract class Situation
    {
        //Strings used for the text on the menus that will be called in Activate
        public string header;
        public string body;
        public string action;
        public string reaction;
        public string conclusion;

        //Status of the current situation - pass or ongoing
        public string status;

        public Menu menu;
        public bool monsterHasKey;

        protected string type;
        public string Type { get { return type; } } //Read only

        private Texture2D image;
        public Texture2D Image
        {
            get { return image; }
            set { image = value; }
        }

        /// <summary>
        /// Sets default values for menu text strings
        /// </summary>
        protected abstract void SetDefaultValues();

        //Constructor that takes an image and a random number generator that is used throughout the program
        public Situation(Texture2D image, Random rng)
        {
            header = "";
            body = "";
            action = "";
            reaction = "";
            conclusion = "";

            monsterHasKey = false;
            this.image = image;
            status = "";
        }

        /// <summary>
        /// Method that activates the situation when the player sprite collides with the tile sprite
        /// </summary>
        public abstract void Activate(Player player, Gui gui);
    }
}
