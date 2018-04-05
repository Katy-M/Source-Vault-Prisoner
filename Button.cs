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
    class Button
    {
        //Rectangle for the width, height, starting x position, and starting y position
        public Rectangle dimensions;

        //Was the button previously clicked?
        //public bool prevClicked;

        //Variable for holding the mouse state
        private MouseState mState;

        //Boolean variable and get property for determining if the button was clicked
        public bool isClicked;
        public string text;

        //Boolean and property for determining if the button is clickable
        public bool isEnabled;

        //Constructor for a button sets its width and height from values given
        public Button(int xStartPos, int yStartPos, int width, int height)
        {
            dimensions = new Rectangle(xStartPos, yStartPos, width, height);
            //Set later in the program
            text = null;
        }

        //Button draws itself given an image, font, position of text, and a spritebatch object
        public void DrawButton(Texture2D image, SpriteFont textFont, Vector2 textPos, SpriteBatch sb)
        {

            mState = Mouse.GetState();

            //If the mouse is intersecting with the button's rectangle coordinates, draw it in a different color
            if (dimensions.Contains(mState.Position) && isEnabled)
            {
                sb.Draw(image, dimensions, Color.White);
                sb.DrawString(textFont, text, textPos, Color.White);

                //If the button is pressed while the mouse is inside its dimensions
                if (mState.LeftButton == ButtonState.Pressed)
                {
                    isClicked = true;
                    return;
                }
            }
            else if (isEnabled)
            {
                sb.Draw(image, dimensions, Color.White);
                sb.DrawString(textFont, text, textPos, Color.Black);
            }
            else
            {
                sb.Draw(image, dimensions, Color.IndianRed);
                sb.DrawString(textFont, text, textPos, Color.Black);
            }

            isClicked = false;
        }
    }
}
