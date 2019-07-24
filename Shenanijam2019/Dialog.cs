using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shenanijam2019
{
    public class Dialog
    {
        public List<string> lines;
        public int CurrentLine { get; set; }
        public bool IsScrolling { get; set; }
        public bool DrawDialog { get; set; } //Is being drawn to the screen
        private readonly float _scrollSpeed; //should be a small float < 0.1 or so
        private string _scrollLine; //drawn to the screen to make the scrolling effect
        private int _currChar; //current character in the _scrollLine string
        private int _tickCount; //number of frames between scrolls

        public Dialog(float scrollSpeed = 0.05f)
        {
            lines = new List<string>();
            CurrentLine = -1;
            DrawDialog = false;
            IsScrolling = false;
            _scrollSpeed = scrollSpeed;
            _scrollLine = "";
            _currChar = 1;
            _tickCount = 0;
        }

        /// <summary>
        /// Add a line of dialog to the list
        /// </summary>
        /// <param name="message"></param>
        public void AddDialog(string message)
        {
            this.lines.Add(message);
        }

        /// <summary>
        /// Update method, mainly used for controlling scroll calls
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            _tickCount++;

            if(DrawDialog && IsScrolling && (_tickCount * dt > _scrollSpeed))
            {
                Scroll();
                _tickCount = 0;
            }
        }

        /// <summary>
        /// Builds the string character by character to make it appear to scroll
        /// </summary>
        private void Scroll()
        {
            _scrollLine = lines[CurrentLine].Substring(0, _currChar);
            _currChar++;

            if(_currChar > lines[CurrentLine].Length)
            {
                IsScrolling = false;
                _currChar = 1;
            }
        }

        /// <summary>
        /// Draws the dialog line
        /// </summary>
        /// <param name="sb">Sprite Batch</param>
        /// <param name="font"></param>
        /// <param name="position"></param>
        /// <param name="color"></param>
        public void Draw(SpriteBatch sb, SpriteFont font, Vector2 position, Color color)
        {
            sb.Begin();
            sb.DrawString(font, _scrollLine,position, color);
            if(!IsScrolling)
            {
                sb.DrawString(font, "Press E to continue...", new Vector2(975, 680), Color.Purple);
            }
            sb.End();
        }

        /// <summary>
        /// Updates the current line to the next one in the list
        /// </summary>
        public void UpdateDialogLine()
        {
            CurrentLine++;

            if (CurrentLine >= lines.Count)
            {
                ResetDialog();
            }
        }

        /// <summary>
        /// resets the line to the beginning
        /// </summary>
        public void ResetDialog()
        {
            DrawDialog = false;
            IsScrolling = false;
            CurrentLine = -1;
            _scrollLine = "";
            _currChar = 1;
        }
    }
}
