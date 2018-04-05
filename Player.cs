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
    /// Contains all the information specific to the player
    /// </summary>
    class Player
    {
        //Player sprite
        public Texture2D playerImg;

        //Player tile
        private Tile playerTile;
        public Tile PlayerTile
        {
            get { return playerTile; }
            set
            {
                playerTile = value;
                PlayerTile.hasData = false;
            }
        }

        //Defaults to zero if set less than - cannot be greater than 20
        private int health;
        public int Health
        {
            get { return health; }
            set
            {
                if(value < 0) { value = 0; }
                if (value > 20) { value = 20; }

                health = value;
            }
        }

        //Highest and lowest attack values the player can land on an enemy. Determined by weapon quality
        private int attackMin;
        public int AttackMin
        {
            get { return attackMin; }
            set
            {
                if(value < 0) { value = 0;}
                attackMin = value;
            }
        }

        private int attackMax;
        public int AttackMax
        {
            get { return attackMax; }
            set
            {
                if (value < 0) { value = 0; }

                attackMax = value;
            }
        }

        public int weaponValue;

        //Highest and lowest defense values the player can shield from an enemy. Cannot be less than 4 or greater than 12
        private int defenseMin;
        public int DefenseMin
        {
            get { return defenseMin; }
            set
            {
                if (value < 0) { value = 0; }
                if (value > 4) { value = 4; }

                defenseMin = value;
            }
        }

        private int defenseMax;
        public int DefenseMax
        {
            get { return defenseMax; }
            set
            {
                if (value < 0) { value = 0;}
                if(value > 12) { value = 12;}

                defenseMax = value;
            }
        }

        //Affects escaping from traps
        public int luckMin;
        public int luckMax;

        //The player wins the game when they collect two keys
        public int keys;

        /// <summary>
        /// Creates the Player the user will control. Takes an image and a tile to display on screen.
        /// </summary>
        public Player(Texture2D img, Tile tile)
        {
            health = 20;
            attackMin = 1;
            attackMax = 5;
            defenseMin = 0;
            defenseMax = 8;
            luckMin = 0;
            luckMax = 8;
            keys =  0;
            weaponValue = 0;

            playerImg = img;
            playerTile = tile;
        }

        public void Draw(SpriteBatch sb, Color color)
        {
            sb.Draw(playerImg, playerTile.TileDims, color);
        }
    }
}
