/*Programmer: Katy Mollenkopf 
 * ---------------------------- */
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
    //Connects all of the tiles needed to form a grid in the game.
    class Grid
    {
        //Random number generator used to assgin situations to the tiles as declared in Game1.cs
        private Random rng;
        private const int MAX_DATA_TILES = 4; //percentage of tile containing data

        //Contains all the tile information
        private Tile[] tiles;
        public Tile[] Tiles
        {
            get { return tiles; }
            set { tiles = value; }
        }
        private Rectangle[] tileColliders;
        private bool[] values;
        private Situation[] situations;

        ///<summary>
        ///Creates a new grid. Sets up 8 rows and 6 columns of tiles
        ///</summary>
        public Grid(Random rng, Situation[] situations)
        {
            values = new bool[48];
            this.rng = rng;
            //The first and last columns will not have situations assigned to them.
            tiles = new Tile[48];

            Setup();

            //Pull the situations from Game1.cs and store them in here
            this.situations = situations;

            Assign();
        }

        //Assigns all information regarding tile data and tile dimensions in that grid.
        private void Setup()
        {
            //Determines how many tiles will contain data on initialization
            for(int i = 0; i < values.Length; i++)
            {
                int num = rng.Next(0, 11);
                if (num < MAX_DATA_TILES)
                {
                    values[i] = true;
                }
                else
                {
                    values[i] = false;
                }
            }

            //Setup the dimensions for the tiles in the grid
            tileColliders = new Rectangle[48];

            CreateTileColliders(0, 8, 0); //First row
            CreateTileColliders(8, 16, 128); //Second row
            CreateTileColliders(16, 24, 256); //Third row
            CreateTileColliders(24, 32, 384); //Fourth row
            CreateTileColliders(32, 40, 512); //Fifth row
            CreateTileColliders(40, 48, 640); //Sixth row

            //Setup the tiles in the grid
            CreateRow(0, 7); //Row 1
            CreateRow(8, 15); //Row 2
            CreateRow(16, 23); //Row 3
            CreateRow(24, 31); //Row 4
            CreateRow(32, 39); //Row 5
            CreateRow(40, 47); //Row 6
        }

        //Assigns situations to the tiles that contain data
        private void Assign()
        {
            for(int i = 0; i < tiles.Length; i++)
            {
                if (tiles[i].hasData == true)
                {
                    //Use the random number to pick an array index and assign it to that tile
                    int index = rng.Next(0, situations.Length);
                    tiles[i].situation = situations[index];
                }
            }
        }

        //Subroutine sets up the tiles in the grid given a starting tile
        private void CreateRow(int firstTile, int lastTile)
        {
            tiles[firstTile] = new Tile(false, tileColliders[firstTile]);
            tiles[firstTile].firstTile = true;

            for (int i = firstTile + 1; i < lastTile; i++)
            {
                tiles[i] = new Tile(values[i], tileColliders[i]);
            }

            tiles[lastTile] = new Tile(false, tileColliders[lastTile]);
            tiles[lastTile].lastTile = true;
        }

        //Subroutine that sets dimensions for the tiles in the grid
        private void CreateTileColliders(int firstTile, int lastTile, int yValue)
        {
            int mult = 1;
            tileColliders[firstTile] = new Rectangle(342, yValue, 128, 128);

            for (int i = firstTile + 1; i < lastTile; i++)
            {
                tileColliders[i] = new Rectangle(342 + (128 * mult), yValue, 128, 128);
                mult++;
            }
        }

        /// <summary>
        /// Draws the tiles in this grid to the application
        /// </summary>
        public void DrawTiles(Texture2D tileImg, Texture2D keyImage, 
            Texture2D tileImgCyan, Texture2D arrowTileImg, Texture2D doorImg, SpriteBatch sb)
        {
            for (int i = 0; i < tiles.Length; i++)
            {
                if (tiles[i].isExit)
                {
                    sb.Draw(arrowTileImg, tiles[i].TileDims, Color.White);
                }
                else if (tiles[i].isEntry)
                {
                    sb.Draw(arrowTileImg, tiles[i].TileDims, null, Color.White, 0, 
                        new Vector2(0, 0), SpriteEffects.FlipHorizontally, 0);
                }
                else if (tiles[i].isDoor)
                {
                    sb.Draw(doorImg, tiles[i].TileDims, Color.White);
                }
                else if (tiles[i].hasData == true)
                {
                    if (tiles[i].isVisible)
                    {
                        sb.Draw(tiles[i].situation.Image, tiles[i].TileDims, Color.White);

                        //If the tile has a key and is visible, draw a different image
                        if(tiles[i].hasKey == true)
                        {
                            sb.Draw(keyImage, tiles[i].TileDims, Color.White);
                        }
                    }
                    else
                    {
                        sb.Draw(tileImgCyan, tiles[i].TileDims, Color.White);
                    }
                }
                else
                {
                    sb.Draw(tileImg, tiles[i].TileDims, Color.White);
                }
            }
        }
    }
}
