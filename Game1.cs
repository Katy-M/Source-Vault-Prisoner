using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;

/*Change log - started recording 11/17/17
 11/17/17 - Created subroutines in Grid.cs for the creation of tiles and their colliders
 12/22/17 - Setup variables and methods for GUI creation. Updated situation classes to contain a string type that can be checked in Game1.cs 
 12/23/17 - Game States added to the game. Some menu textures added to the game and linked in the GUI (textures not loaded yet)
 12/31/17 - Menu textures added and loaded (start, end, and trap menus need to be added). Situations activate.
          - menus adjusted so they hold references to their fonts and the fonts are loaded into the gui
 01/01/18 - All drawing on menus is done in Game1.Draw() instead of Situation.Activate(). Health menu functional.
 01/02/18 - Armor, weapon, and mishap menus functional. Some button properties changed to have a global scope
          - Added stats menu and fixed bug relating to gaining health after a mishap.
 01/03/18 - Fixed a typo in Grid.cs that caused a tile to be occupied in the first row.
          - Tiles are hidden by default and accessible tiles are revealed when pressing enter. There is a max number of times tiles can be revealed per grid
 01/15/18 - Trap situation implemented. Mishap and trap situations buggy - player can hold button down instead of click it once
 01/31/18 - Situation button-hold fixed. Added a key tile graphic
 02/02/18 - Monster attack situation implemented.
 02/16/17 - Fixed bug regarding monster situation auto-solving.
          - Key situation functional
 02/26/17 - Implemented stopwatch
 02/28/17 - Began implementing grid sequence and key distribution
 03/01/18 - Grid sequence and key distribution complete. Auto-solve bug presented in a specific case after getting a key
 03/02/18 - Player can move between grids correctly. Key bug fixed.
          - Main menu implemented; start state partially implemented
 03/03/18 - Minor changes to time and attacking; rearraged functions in main code
 03/04/18 - Minor balance changes to situation generation
 03/11/18 - Game over state and menu implemented
 03/13/18 - Implemented game instructions that are accessible from start menu
 03/15/18 - Player wins game after two keys are found and they reach the exit. Win menu needs to be implemented. Timer displays during situations
          - Results: Change health to blue colors. Put controls and exit game in stats menu. Include key and no key icons in menu
 03/25/18 - All game states implemented
 03/30/18 - Implemented grid backround setup and placeholder background images.
 03/31/18 - Changed health color from red to blue. Included player, arrow, and cyan tile graphics.
 04/04/18 - Began creating grids. Modified some tile graphics
 04/24/18 - Created second grid and modified player tile graphic

        NEXT STEPS: CREATE BACKGROUND AND REMAINING TILE ART
                    PLAYTEST AND IMPLEMENT AUDIO/SOUND EFFECTS
 */

namespace Vault_Prisoner
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

    //--------------------------------- VARIABLES FOR GAME/KEYBOARD STATES, TIME MANAGEMENT, TEXTURES, AND GRID SETUP ---------------------------------
    #region GameComponents
        private Player player;

        private Grid grid;
        private int gridNum;
        private const int GRID_MAX_COUNT = 5;
        private Grid[] allMaps;

        //Gamestate values
        private bool startState;
        private bool gameState;
        private bool situationState;
        private bool overState;
        private bool winState;

        //Keeps track of the number of frames passed in the game
        private int framesPassed;
        private Stopwatch timer;
        private Vector2 timerTextPos;
        private const long MAX_TIME = 360;

        //Random number generator used to assgin situations to the tiles
        private Random rng;

        //Tile images
        private Texture2D tileImg;
        private Texture2D tileImgCyan;
        private Texture2D playerTileImg;
        private Texture2D arrowTileImg;
        private Texture2D healthImg;
        private Texture2D weaponImg;
        private Texture2D armorImg;
        private Texture2D monsterImg;
        private Texture2D mishapImg;
        private Texture2D trappedImg;
        private Texture2D keyImg;

        //Menu images
        private Texture2D healthMenuImg;
        private Texture2D weaponMenuImg;
        private Texture2D armorMenuImg;
        private Texture2D monsterMenuImg;
        private Texture2D mishapMenuImg;
        private Texture2D trappedMenuImg;
        private Texture2D statsMenuImg;
        private Texture2D keyMenuImg;
        private Texture2D mainMenuImg;
        private Texture2D overMenuImg;
        private Texture2D winMenuImg;

        private Texture2D instructMenuImg;
        private bool instructOn;

        private KeyboardState kStatePrev;

        //Fonts used
        private SpriteFont bodyFont;
        private SpriteFont titleFont;

        private Situation[] gameSituations;

        //Gui used in the game
        private Gui gui;

        //Stores a reference to the tile that triggered a situation
        private Tile triggeredTile;

        //private Button button;
        private Texture2D buttonTexture;

        //Determines player starting tile
        private int[] playerStartIndex;
        private int relativePlayerPos;

        //Variables used for revealing tiles accessible to the player ("investigating")
        private int exploreUses;
        private const int MAX_EXPLORE = 4;
        private List<Tile> nearbyTiles;

        //Stores an ordered list of grid background images
        List<Texture2D> gridTextureMaps;
        Rectangle gridSpace = new Rectangle(342, 0, 1024, 768);

    #endregion
    //-------------------------------------------------------------------------------------------------------------------------------------------------

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            //Resize the application to a better resolution
            //Source: https://stackoverflow.com/questions/11283294/how-to-resize-window-using-xna
            graphics.PreferredBackBufferWidth = 1366;
            graphics.PreferredBackBufferHeight = 768;
            //graphics.IsFullScreen = true;
            //End source
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            instructOn = false;

            rng = new Random();
            kStatePrev = Keyboard.GetState();
            allMaps = new Grid[GRID_MAX_COUNT];

            gridTextureMaps = new List<Texture2D>();

            framesPassed = 0;
            timer = new Stopwatch();
            timerTextPos = new Vector2(22, 65);

            //The player will start the game on one of these tiles (first row)
            playerStartIndex = new int[] {0, 8, 16, 24, 32, 40};

            gridNum = 0;

            startState = true;
            exploreUses = MAX_EXPLORE;

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            tileImg = this.Content.Load<Texture2D>("tileImg.png");
            tileImgCyan = this.Content.Load<Texture2D>("tileImgCyan.png");
            arrowTileImg = this.Content.Load<Texture2D>("arrowTileImg.png");
            playerTileImg = this.Content.Load<Texture2D>("playerImg.png");
            healthImg = this.Content.Load<Texture2D>("health.png");
            weaponImg = this.Content.Load<Texture2D>("weapon.png");
            armorImg = this.Content.Load<Texture2D>("armor.png");
            monsterImg = this.Content.Load<Texture2D>("monster.png");
            mishapImg = this.Content.Load<Texture2D>("mishap.png");
            trappedImg = this.Content.Load<Texture2D>("trapped.png");
            keyImg = this.Content.Load<Texture2D>("key.png");

            titleFont = this.Content.Load<SpriteFont>("mainFont");
            bodyFont = this.Content.Load<SpriteFont>("bodyFont");

            //Load the menu textures
            mainMenuImg = this.Content.Load<Texture2D>("MainMenuAlpha.png");
            instructMenuImg = this.Content.Load<Texture2D>("InstructionsMenu.png");
            overMenuImg = this.Content.Load<Texture2D>("GameOverMenu.png");
            winMenuImg = this.Content.Load<Texture2D>("winMenu.png");

            monsterMenuImg = this.Content.Load<Texture2D>("attackMenu.png");
            healthMenuImg = this.Content.Load<Texture2D>("healthMenu.png");
            armorMenuImg = this.Content.Load<Texture2D>("armorMenu.png");
            mishapMenuImg = this.Content.Load<Texture2D>("mishapMenu.png");
            weaponMenuImg = this.Content.Load<Texture2D>("weaponMenu.png");
            statsMenuImg = this.Content.Load<Texture2D>("statsMenu.png");
            trappedMenuImg = this.Content.Load<Texture2D>("trapMenu.png");
            keyMenuImg = this.Content.Load<Texture2D>("keyMenu.png");

            buttonTexture = this.Content.Load<Texture2D>("button.png");

            //Load the grid background textures
            List<Texture2D> maps = new List<Texture2D>();

            maps.Add(this.Content.Load<Texture2D>("grid1.png"));
            maps.Add(this.Content.Load<Texture2D>("grid2.png"));
            maps.Add(this.Content.Load<Texture2D>("grid3.png"));
            maps.Add(this.Content.Load<Texture2D>("grid4.png"));
            maps.Add(this.Content.Load<Texture2D>("grid5.png"));

            //Create an array with instances of all situations in the game. The number of situations created 
            //affects the probablility of the situations appearing on the grid
            gameSituations = new Situation[] {
                //Monster attacks
                new MonsterAttack(monsterImg, rng),
                new MonsterAttack(monsterImg, rng),
                new MonsterAttack(monsterImg, rng),
                new MonsterAttack(monsterImg, rng),
                //Trap
                new Trap(trappedImg, rng),
                new Trap(trappedImg, rng),
                //Mishap
                new Mishap(mishapImg, rng),
                new Mishap(mishapImg, rng),
                //Armor
                new ArmorPickup(armorImg, rng),
                new ArmorPickup(armorImg, rng),
                //Health
                new HealthPickup(healthImg, rng),
                new HealthPickup(healthImg, rng),
                //Weapon
                new WeaponPickup(weaponImg, rng),
                new WeaponPickup(weaponImg, rng),};

            SetupGameGrids();

            grid = allMaps[gridNum];
            relativePlayerPos = playerStartIndex[rng.Next(0, playerStartIndex.Length)];

            player = new Player(playerTileImg, grid.Tiles[relativePlayerPos]);

            //Make the gui in the game
            gui = new Gui(mainMenuImg,monsterMenuImg, armorMenuImg, 
                weaponMenuImg, healthMenuImg, 
                mishapMenuImg,trappedMenuImg, keyMenuImg, overMenuImg, 
                winMenuImg, buttonTexture, titleFont, bodyFont);

            gridTextureMaps = GridTextures.SetupGridMaps(maps, GRID_MAX_COUNT, rng);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            IsMouseVisible = true;
            // TODO: Add your update logic here

            KeyboardState kStateCurr = Keyboard.GetState();

            if (player.Health <= 0)
            {
                timer.Stop();
                gameState = false;
                situationState = false;
                overState = true;
            }

            //If the instructions are on and the player presses backspace, exit the instructions
            if(instructOn && kStateCurr.IsKeyDown(Keys.Back) 
                && kStatePrev.IsKeyDown(Keys.Back) == false)
            {
                instructOn = false;
                if (timer.IsRunning == false 
                    && (gameState || situationState))
                {
                    timer.Start();
                }
            }
        //--------------------------------------------- GAMESTATE ------------------------------------------------
        #region GameState
            if (gameState)
            {
                //The player moves to the previous grid if pressing space on an entry tile
                if(player.PlayerTile.isEntry)
                {
                    if (kStateCurr.IsKeyDown(Keys.Space)
                        && kStatePrev.IsKeyDown(Keys.Space) == false)
                    {
                        //Reset the visible tiles in the current grid
                        foreach (Tile tile in grid.Tiles)
                        {
                            tile.isVisible = false;
                        }

                        exploreUses = MAX_EXPLORE;
                        gridNum--;
                        grid = allMaps[gridNum];
                        player.PlayerTile = grid.Tiles[23];
                        relativePlayerPos = 23;
                    }
                }
                else if(player.PlayerTile.isExit)
                {
                    if (kStateCurr.IsKeyDown(Keys.Space)
                    && kStatePrev.IsKeyDown(Keys.Space) == false)
                    {
                        foreach (Tile tile in grid.Tiles)
                        {
                            tile.isVisible = false;
                        }

                        exploreUses = MAX_EXPLORE;
                        gridNum++;
                        grid = allMaps[gridNum];
                        player.PlayerTile = grid.Tiles[16];
                        relativePlayerPos = 16;
                    }
                }
                else if (player.PlayerTile.isDoor && player.keys == 2)
                {
                    timer.Stop();
                    gameState = false;
                    winState = true;
                }

                //Check to see if the player is investigating the nearby tiles
                if (kStateCurr.IsKeyDown(Keys.Enter)
                    && kStatePrev.IsKeyDown(Keys.Enter) != true)
                {
                    RevealNearbyTiles();
                }

                movePlayer(kStateCurr, kStatePrev);
            }
        #endregion
        //--------------------------------------------------------------------------------------------------------

            kStatePrev = kStateCurr;
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);
            // TODO: Add your drawing code here

            spriteBatch.Begin();
            //----  BEGIN SPRITEBATCH------------------------------------------

            if (startState)
            {
                Menu main = gui.MainMenu;
                main.DrawMenu(spriteBatch);
                main.DrawMenuButtons(spriteBatch);

                //Depending on the buttons clicked:
                //Play the game
                if (main.Buttons[0].isClicked)
                {
                    startState = false;
                    gameState = true;
                    timer.Start();
                    main.Buttons[0].isClicked = false;
                }
                else if (main.Buttons[1].isClicked) //How to play
                {
                    instructOn = true;
                }
                else if (main.Buttons[2].isClicked) //Credits
                {

                }
                else if (main.Buttons[3].isClicked) //Quit
                {
                    Exit();
                }

                //Display a different graphic
            }
            //If the game is in the situation state or the game state
            if (gameState || situationState)
            {
                spriteBatch.Draw(gridTextureMaps[gridNum], gridSpace, Color.White);
                grid.DrawTiles(tileImg, keyImg, tileImgCyan, arrowTileImg, spriteBatch);
                player.Draw(spriteBatch, Color.White);
            }

            //If there is a triggered situation active
            if (situationState)
            {
                player.Draw(spriteBatch, Color.White);

                Situation current = triggeredTile.situation;

                //Draw the menu that is triggered
                current.menu.DrawMenu(spriteBatch);
                current.menu.DrawText(current.header, current.body, current.action,
                    current.reaction, current.conclusion, spriteBatch);
                current.menu.DrawMenuButtons(spriteBatch);

            #region Situation Button Configurations
                //Display until the player clicks the continue button and passes - change of gameState
                if (current.menu.Buttons[0].isClicked)
                {
                    if (triggeredTile.hasKey == true)
                    {
                        //Reset the status of the monster and change to key situation
                        current.status = "";
                        current.monsterHasKey = false;
                        triggeredTile.situation = new Key(keyImg, rng);
                        current = triggeredTile.situation;

                        //Reset the status of the key
                        current.status = "";

                        framesPassed = 0;
                        player.playerImg = keyImg;
                        current.Activate(player, gui);
                        triggeredTile.hasKey = false;
                    }
                    else
                    {
                        current.status = "";
                        player.playerImg = playerTileImg;
                        triggeredTile = null; //Clear the tile
                        situationState = false;
                        gameState = true;
                    }
                }

                //Enable the continue button if the situation is resolved
                if(current.Type == "key" && framesPassed >= 60) current.status = "pass";

                if (current.status == "pass")
                {
                    current.menu.Buttons[0].isEnabled = true;
                    if(current.menu.Buttons.Count > 1)
                    {
                        current.menu.Buttons[1].isEnabled = false;
                    }
                }
                else
                {
                    current.menu.Buttons[0].isEnabled = false;
                }
                if (current.Type == "mishap")
                {
                    if (current.menu.Buttons[1].isClicked)
                    {
                        current.status = "clicked";
                        current.Activate(player, gui);
                        current.menu.Buttons[1].isEnabled = false;
                    }
                }
                else if(current.Type == "trap" || current.Type == "monster")
                {

                    if (framesPassed < 180)
                    {
                        current.menu.Buttons[1].isEnabled = false;
                    }
                    else if (current.menu.Buttons[1].isClicked)
                    {
                        current.status = "clicked";
                        current.Activate(player, gui);
                        framesPassed = 0;
                    }
                    else if (current.Type == "monster" && framesPassed == 180
                        && current.status != "pass")
                    {
                        current.Activate(player, gui);
                    }
                }
             #endregion
            }

            else if (gameState)
            {
                DrawStatsMenu();
                framesPassed = 180; //Reset frame count
            }
            else if (overState)
            {
                //Draw the game over menu and draw the text
                Menu over = gui.OverMenu;
                over.DrawMenu(spriteBatch);
                over.DrawMenuButtons(spriteBatch);

                spriteBatch.DrawString(titleFont, player.Health.ToString(), 
                    gui.overMenuTextPos[0], Color.White);
                spriteBatch.DrawString(titleFont, GetTimeLeft(),
                    gui.overMenuTextPos[1], Color.White);
                spriteBatch.DrawString(titleFont, player.keys.ToString(),
                    gui.overMenuTextPos[2], Color.White);

                if (over.Buttons[0].isClicked) //Play again
                {
                    overState = false;

                    //Reset values
                    allMaps = new Grid[GRID_MAX_COUNT];
                    framesPassed = 0;
                    SetupGameGrids();
                    gridNum = 0;
                    grid = allMaps[gridNum];

                    relativePlayerPos = playerStartIndex[rng.Next(0, playerStartIndex.Length)];
                    player = new Player(playerTileImg, grid.Tiles[relativePlayerPos]);
                    exploreUses = MAX_EXPLORE;

                    timer.Reset();
                    timer.Start();
                    gameState = true;
                }
                else if (over.Buttons[1].isClicked) //Quit game
                {
                    Exit();
                }
            }
            else if (winState)
            {
                //Draw the win menu and the text
                Menu win = gui.WinMenu;
                win.DrawMenu(spriteBatch);
                win.DrawMenuButtons(spriteBatch);

                spriteBatch.DrawString(titleFont, player.Health.ToString(),
                    gui.winMenuTextPos[0], Color.White);
                spriteBatch.DrawString(titleFont, GetTimeLeft(),
                    gui.winMenuTextPos[1], Color.White);

                if (win.Buttons[0].isClicked) //Play again
                {
                    winState = false;

                    //Reset values
                    allMaps = new Grid[GRID_MAX_COUNT];
                    framesPassed = 0;
                    SetupGameGrids();
                    gridNum = 0;
                    grid = allMaps[gridNum];

                    relativePlayerPos = playerStartIndex[rng.Next(0, playerStartIndex.Length)];
                    player = new Player(playerTileImg, grid.Tiles[relativePlayerPos]);
                    exploreUses = MAX_EXPLORE;

                    timer.Reset();
                    timer.Start();
                    gameState = true;
                }
                else if (win.Buttons[1].isClicked) //Quit game
                {
                    Exit();
                }
            }

            framesPassed += 1;

            //Display timer info
            if (gameState || situationState)
            {
                if ((MAX_TIME - timer.ElapsedMilliseconds / 1000) < 31)
                {
                    spriteBatch.DrawString(bodyFont, "Time left: " + GetTimeLeft(),
                    timerTextPos, Color.Red);
                }
                else if (gameState)
                {
                    spriteBatch.DrawString(bodyFont, "Time left: " + GetTimeLeft(),
                    timerTextPos, Color.Cyan);
                }
                else if (situationState)
                {
                    spriteBatch.DrawString(bodyFont, "Time left: " + GetTimeLeft(),
                    timerTextPos, Color.DarkCyan);

                }
            }

            if ((MAX_TIME - timer.ElapsedMilliseconds / 1000) <= 0)
            {
                timer.Stop();

                //End the game
                gameState = false;
                situationState = false;
                overState = true;
            }

            //If the instructions are up, draw the instructions menu
            if (instructOn)
            {
                spriteBatch.Draw(instructMenuImg, new Rectangle(0, 0, 1366, 768), Color.White);
                if(timer.IsRunning) timer.Stop();
            }

            //----  END SPRITEBATCH-----------------------------------------------
            spriteBatch.End();
            base.Draw(gameTime);
        }

        //--------------------------------------------- HELPER FUNCTIONS ------------------------------------------------
        #region Helper Functions

        //Creates a set of grids for this playthrough
        private void SetupGameGrids()
        {
            List<int[,]> locations = new List<int[,]>();

            //Ensure there are more than 3 total monster situations
            while(locations.Count < 3)
            {
                for (int i = 0; i < allMaps.Length; i++)
                {
                    allMaps[i] = new Grid(rng, gameSituations);

                    for (int j = 0; j < allMaps[i].Tiles.Length; j++)
                    {
                        //Set the tiles that indicate where the player can move between maps
                        if (j == 16 && i != 0)
                        {
                            allMaps[i].Tiles[j].isEntry = true;
                        }
                        else if (j == 23 && i != GRID_MAX_COUNT - 1)
                        {
                            allMaps[i].Tiles[j].isExit = true;
                        }
                        else if (j == 23 && i == GRID_MAX_COUNT - 1)
                        {
                            allMaps[i].Tiles[j].isDoor = true;
                        }

                        //Keep track of the number of monster situations across all created grids
                        if (allMaps[i].Tiles[j].hasData &&
                            allMaps[i].Tiles[j].situation.Type == "monster")
                        {
                            locations.Add(new int[,] { { i, j } });
                        }
                    }
                }
            }

            //Assign exactly two keys to random monster situations
            for (int i = 0; i < 2; i++)
            {
                //Get the location of a random monster tile
                int rand = rng.Next(0, locations.Count);
                int[,] location = locations[rand];
                int mapNum = location[0, 0];
                int tileNum = location[0, 1];

                //Assign a key to it
                allMaps[mapNum].Tiles[tileNum].hasKey = true;

                //Remove from the list
                locations.RemoveAt(rand);
            }
        }

        //Handles player movement in the game given the current keyboard state
        private void movePlayer(KeyboardState kStateCurr, KeyboardState kStatePrev)
        {
            //Move right
            if (kStateCurr.IsKeyDown(Keys.Right))
            {
                if (kStatePrev.IsKeyDown(Keys.Right) != true
                    && relativePlayerPos + 1 < grid.Tiles.Length
                    && grid.Tiles[relativePlayerPos].lastTile != true)
                {
                    TriggerSituation(relativePlayerPos + 1);

                    player.PlayerTile = grid.Tiles[relativePlayerPos + 1];
                    relativePlayerPos++;
                }
            }
            //Move left
            else if (kStateCurr.IsKeyDown(Keys.Left))
            {
                if (kStatePrev.IsKeyDown(Keys.Left) != true
                    && relativePlayerPos - 1 >= 0
                    && grid.Tiles[relativePlayerPos].firstTile != true)
                {
                    TriggerSituation(relativePlayerPos - 1);

                    player.PlayerTile = grid.Tiles[relativePlayerPos - 1];
                    relativePlayerPos -= 1;
                }
            }
            //Move down
            else if (kStateCurr.IsKeyDown(Keys.Down))
            {
                if (kStatePrev.IsKeyDown(Keys.Down) != true
                    && relativePlayerPos + 8 < grid.Tiles.Length)
                {
                    TriggerSituation(relativePlayerPos + 8);

                    player.PlayerTile = grid.Tiles[relativePlayerPos + 8];
                    relativePlayerPos += 8;
                }
            }
            //Move up
            else if (kStateCurr.IsKeyDown(Keys.Up))
            {
                if (kStatePrev.IsKeyDown(Keys.Up) != true
                    && relativePlayerPos - 8 >= 0)
                {
                    TriggerSituation(relativePlayerPos - 8);

                    player.PlayerTile = grid.Tiles[relativePlayerPos - 8];
                    relativePlayerPos -= 8;
                }
            }
        }

        //Checks to see if the tile at a given position has a situation attached. If so, activate the situation
        private void TriggerSituation(int gridPos)
        {
            if (grid.Tiles[gridPos].hasData)
            {
                triggeredTile = grid.Tiles[gridPos];

                player.playerImg = grid.Tiles[gridPos].situation.Image;

                gameState = false;
                situationState = true;

                if (triggeredTile.hasKey)
                    triggeredTile.situation.monsterHasKey = true;

                triggeredTile.situation.Activate(player, gui);
            }
        }

        //Draws the stats menu and its text
        private void DrawStatsMenu()
        {
            spriteBatch.Draw(statsMenuImg, new Rectangle(0, 0, 342, 768), Color.White);

            //Health - center align with box
            if (player.Health < 10)
            {
                Vector2 pos = new Vector2(gui.statsMenuTextPos["health"].X + 7,
                    gui.statsMenuTextPos["health"].Y);

                spriteBatch.DrawString(bodyFont, player.Health.ToString(), pos, Color.White);
            }
            else
            {
                spriteBatch.DrawString(bodyFont, player.Health.ToString(),
                    gui.statsMenuTextPos["health"], Color.White);
            }

            //Attack min and max
            spriteBatch.DrawString(bodyFont, player.AttackMin.ToString(),
                gui.statsMenuTextPos["aMin"], Color.White);
            spriteBatch.DrawString(bodyFont, player.AttackMax.ToString(),
                gui.statsMenuTextPos["aMax"], Color.White);

            //Defense min and max
            spriteBatch.DrawString(bodyFont, player.DefenseMin.ToString(),
                gui.statsMenuTextPos["dMin"], Color.White);

            //Center align with box
            if (player.DefenseMax >= 10)
            {
                Vector2 pos = new Vector2(gui.statsMenuTextPos["dMax"].X - 4,
                    gui.statsMenuTextPos["dMax"].Y);

                spriteBatch.DrawString(bodyFont, player.DefenseMax.ToString(), pos, Color.White);
            }
            else
            {
                spriteBatch.DrawString(bodyFont, player.DefenseMax.ToString(),
                    gui.statsMenuTextPos["dMax"], Color.White);
            }

            //Luck min and max
            spriteBatch.DrawString(bodyFont, player.luckMin.ToString(),
                gui.statsMenuTextPos["lMin"], Color.White);
            spriteBatch.DrawString(bodyFont, player.luckMax.ToString(),
                gui.statsMenuTextPos["lMax"], Color.White);

            //Number of keys
            spriteBatch.DrawString(bodyFont, player.keys.ToString(),
                gui.statsMenuTextPos["keys"], Color.White);

            //Inspections
            if(exploreUses > 0)
            {
                spriteBatch.DrawString(bodyFont, exploreUses.ToString(),
                gui.statsMenuTextPos["inspections"], Color.White);
            }
            else
            {
                spriteBatch.DrawString(bodyFont, exploreUses.ToString(),
                gui.statsMenuTextPos["inspections"], Color.Red);
            }
        }

        private void RevealNearbyTiles()
        {
            nearbyTiles = new List<Tile>();

            /* Reference all the existing tiles the player can access from their current location
             * and check if they contain a situation */
            if (grid.Tiles[relativePlayerPos].lastTile == false
                && grid.Tiles[relativePlayerPos + 1].hasData)
            {
                nearbyTiles.Add(grid.Tiles[relativePlayerPos + 1]);
            }
            if (grid.Tiles[relativePlayerPos].firstTile == false
                && grid.Tiles[relativePlayerPos - 1].hasData)
            {
                nearbyTiles.Add(grid.Tiles[relativePlayerPos - 1]);
            }
            if (relativePlayerPos + 8 < grid.Tiles.Length
                && grid.Tiles[relativePlayerPos + 8].hasData)
            {
                nearbyTiles.Add(grid.Tiles[relativePlayerPos + 8]);
            }
            if (relativePlayerPos - 8 > 0
                && grid.Tiles[relativePlayerPos - 8].hasData)
            {
                nearbyTiles.Add(grid.Tiles[relativePlayerPos - 8]);
            }

            for (int i = 0; i < nearbyTiles.Count; i++)
            {
                if (exploreUses > 0)
                {
                    nearbyTiles[i].isVisible = true;
                }
            }

            //Subtract from the explore uses if tiles with data are revealed
            if (nearbyTiles.Count > 0)
            {
                exploreUses -= 1;
                if (exploreUses < 0)
                {
                    exploreUses = 0;
                }
            }
        }

        private string GetTimeLeft()
        {
            long timeLeft = MAX_TIME - (timer.ElapsedMilliseconds / 1000);
            long minutes = timeLeft / 60;
            long seconds = timeLeft % 60;

            if(seconds < 10)
                return minutes + ".0" + seconds;
            else
                return minutes + "." + seconds;
        }
        #endregion
        //---------------------------------------------------------------------------------------------------------------
    }
}
