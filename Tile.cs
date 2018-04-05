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
    //Represents a tile on the grid that the player can move to
    class Tile
    {
        //Stores whether a tile has a situation inside of it.
        public bool hasData;

        //The situation of a tile if it has one
        public Situation situation;

        //Stores the dimensions (x pos, y pos, width, height) of the tile
        private Rectangle tileDims;
        public Rectangle TileDims
        {
            get { return tileDims; }
        }

        //Is the tile in the far right or left column of the grid
        public bool firstTile;
        public bool lastTile;

        public bool isVisible;
        public bool hasKey;

        //True if the tile marks the end
        public bool isDoor;
        public bool isEntry;
        public bool isExit;

        /// <summary>
        /// Constructor for a tile.
        /// </summary>
        /// <param name="hasData">If this tile contains a Situation (see Situation.cs)</param>
        /// <param name="tileDims">The dimensions (x pos, y pos, width, height) of the tile</param>
        public Tile(bool hasData, Rectangle tileDims)
        {
            situation = null;
            hasKey = false;

            this.hasData = hasData;
            this.tileDims = tileDims;
            isVisible = false;
        }
    }
}
