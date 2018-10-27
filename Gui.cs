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
    //Handles and stores all the menus in the game
    class Gui
    {
        //Public dictionary that stores string keys and Menu values. Accessed by key value
        public Dictionary<string,Menu> allMenus;

        private Menu mainMenu;
        public Menu MainMenu { get { return mainMenu; } }

        //Game over menu info
        private Menu overMenu;
        public Menu OverMenu { get { return overMenu; } }
        public Vector2[] overMenuTextPos = new Vector2[]
        {
            new Vector2(962, 220),new Vector2(945, 310), new Vector2(962, 400)
        };

        //Win menu info
        private Menu winMenu;
        public Menu WinMenu { get { return winMenu; } }
        public Vector2[] winMenuTextPos = new Vector2[]
        {
            new Vector2(962, 220),new Vector2(945, 310)
        };

        //Holds and stores references to the positions of the numbers on the stats menu
        public Dictionary<string, Vector2> statsMenuTextPos;

        /// <summary>
        /// Creates all menus that will be present in the game given the menu textures and fonts, then populates the dictionary with these menus
        /// </summary>
        public Gui(Texture2D mainMenuImg, Texture2D attackMenuImg, Texture2D armorMenuImg, Texture2D weaponMenuImg,
            Texture2D healthMenuImg, Texture2D mishapMenuImg, Texture2D trapMenuImg, 
            Texture2D keyMenuImg, Texture2D overMenuImg, Texture2D winMenuImg, Texture2D buttonImg, 
            SpriteFont title, SpriteFont body)
        {
            allMenus = new Dictionary<string, Menu>();
            statsMenuTextPos = new Dictionary<string, Vector2>();

            //Create the main menu, game over menu, and win menu
            mainMenu = new Menu("Data/mainMenuInfo.txt", mainMenuImg, buttonImg, title, body);
            overMenu = new Menu("Data/overMenuInfo.txt", overMenuImg, buttonImg, title, body);
            winMenu = new Menu("Data/winMenuInfo.txt", winMenuImg, buttonImg, title, body);

            //Add the attack menu
            allMenus.Add("attack", new Menu("Data/attackMenuInfo.txt", attackMenuImg, buttonImg, title, body));
            //Add the health menu
            allMenus.Add("health", new Menu("Data/healthMenuInfo.txt", healthMenuImg, buttonImg, title, body));
            //Add the armor menu
            allMenus.Add("armor", new Menu("Data/armorMenuInfo.txt", armorMenuImg, buttonImg, title, body));
            //Add the weapon menu
            allMenus.Add("weapon", new Menu("Data/weaponMenuInfo.txt", weaponMenuImg, buttonImg, title, body));
            //Add the mishap menu
            allMenus.Add("mishap", new Menu("Data/mishapMenuInfo.txt", mishapMenuImg, buttonImg, title, body));
            //Add the trap menu
            allMenus.Add("trap", new Menu("Data/trapMenuInfo.txt", trapMenuImg, buttonImg, title, body));
            //Add the key menu
            allMenus.Add("key", new Menu("Data/keyMenu.txt", keyMenuImg, buttonImg, title, body));

            SetupStatsMenu();
        }

        //Add stats menu number positions
        private void SetupStatsMenu()
        {
            statsMenuTextPos.Add("health", new Vector2(239, 130));
            statsMenuTextPos.Add("aMin", new Vector2(206, 250));
            statsMenuTextPos.Add("aMax", new Vector2(286, 250));
            statsMenuTextPos.Add("dMin", new Vector2(206, 370));
            statsMenuTextPos.Add("dMax", new Vector2(286, 370));
            statsMenuTextPos.Add("lMin", new Vector2(206, 490));
            statsMenuTextPos.Add("lMax", new Vector2(286, 490));
            statsMenuTextPos.Add("keys", new Vector2(246, 610));
            statsMenuTextPos.Add("inspections", new Vector2(246, 694));
        }
    }
}
