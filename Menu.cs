using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.IO;

namespace Vault_Prisoner
{
    //Represents a menu to be displayed in game
    class Menu
    {
        //Holds the size of the menu as set in the constructor
        private Rectangle menuSize;
        public Texture2D menuImage;
        public Texture2D buttonImg;

        private SpriteFont titleFont;
        private SpriteFont bodyFont;

        //Holds references to its buttons and allows other classes to read the buttons
        private List<Button> buttons;
        public List<Button> Buttons
        {
            get { return buttons; }
        }

        //References to the positions of the text on the buttons it has
        private List<Vector2> buttonTextPositions;

        //Locations for text elements
        private Vector2 headerTxtPos;
        private Vector2 bodyTxtPos;
        private Vector2 actionTxtPos;
        private Vector2 reactionTxtPos;
        private Vector2 conclusionTxtPos;

        /// <summary>
        /// Creates a menu given the filepath/filename.txt with the corresponding menu information. Look at SampleMenuInfo.txt for formatting
        /// </summary>
        public Menu(string filename, Texture2D menuImage, Texture2D buttonImg, SpriteFont titleFont, SpriteFont bodyFont)
        {
            this.titleFont = titleFont;
            this.bodyFont = bodyFont;

            this.menuImage = menuImage;
            this.buttonImg = buttonImg;

            buttons = new List<Button>();
            buttonTextPositions = new List<Vector2>();

            StreamReader reader = new StreamReader(filename);

            int[] menuDims = GetReaderData(reader);
            menuSize = new Rectangle(menuDims[0], menuDims[1], menuDims[2], menuDims[3]);

            //Set the text position values
            headerTxtPos = GetPositionValues(reader);
            bodyTxtPos = GetPositionValues(reader);
            actionTxtPos = GetPositionValues(reader);
            reactionTxtPos = GetPositionValues(reader);
            conclusionTxtPos = GetPositionValues(reader);

            //Reads through button information per button - assigns dimentions with that button and the 
            //text on the button, then adds to the list of buttons
            while (reader.EndOfStream == false)
            {
                string text = reader.ReadLine();
                string[] numbers = text.Split(',');
                int[] buttonDims = new int[6];
                for(int i = 0; i < 4; i++)
                {
                    buttonDims[i] = int.Parse(numbers[i]);
                }

                buttonTextPositions.Add(new Vector2(int.Parse(numbers[4]), int.Parse(numbers[5])));
                Button button = new Button(buttonDims[0], buttonDims[1], buttonDims[2], buttonDims[3]);

                button.text = reader.ReadLine();
                buttons.Add(button);
            }

            reader.Close();

        }

        //Draws the menu given its graphic and the spritebatch object. Uses the size set in the constructor
        public void DrawMenu(SpriteBatch sb)
        {
            sb.Draw(menuImage, menuSize, Color.White);
        }

        /// <summary>
        /// Writes the given text on the menu in its appropriate format. Takes a number of arguments 
        /// and two font styles to use in presenting the text and the spritebatch object used in the game
        /// </summary>
        /// <param name="header">The center-aligned title of the menu</param>
        /// <param name="body">The description of the situation</param>
        /// <param name="action">The action followed by a colon and a value</param>
        /// <param name="reaction">the reaction followed by a colon and a value</param>
        /// <param name="conclusion">the concluding statement of an action</param>
        public void DrawText(string header, string body,
            string action, string reaction, string conclusion, SpriteBatch sb)
        {
            sb.DrawString(titleFont, header, headerTxtPos, Color.Black);
            sb.DrawString(bodyFont, body, bodyTxtPos, Color.Black);
            sb.DrawString(bodyFont, action, actionTxtPos, Color.Black);
            sb.DrawString(bodyFont, reaction, reactionTxtPos, Color.Black);
            sb.DrawString(bodyFont, conclusion, conclusionTxtPos, Color.Black);
        }

        //Draws the menu's buttons and enables them
        public void DrawMenuButtons(SpriteBatch sb)
        {
            //Draws its buttons
            for (int i = 0; i < buttons.Count; i++)
            {
                buttons[i].DrawButton(buttonImg, titleFont, buttonTextPositions[i], sb);
                buttons[i].isEnabled = true;
            }
        }

        private int[] GetReaderData(StreamReader reader)
        {
            string line = reader.ReadLine();
            string[] readDims = line.Split(',');
            int[] dimensions = new int[readDims.Length];

            for (int i = 0; i < readDims.Length; i++)
            {
                dimensions[i] = int.Parse(readDims[i]);
            }

            return dimensions;
        }

        //Returns a vector2 with the values read in from the reader
        private Vector2 GetPositionValues(StreamReader reader)
        {
            int[] data = GetReaderData(reader);
            return new Vector2(data[0], data[1]);
        }
    }
}
