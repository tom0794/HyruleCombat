using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace HyruleCombat.Scenes
{
    /// <summary>
    /// Template for GameScenes that allows child classes to possess,
    /// draw and update game elements.
    /// </summary>
    public abstract class GameScene : DrawableGameComponent
    {
        public List<GameComponent> Components { get; set; }

        public GameScene(Game game) : base(game)
        {
            Components = new List<GameComponent>();
            Hide();
        }

        public virtual void Show()
        {
            this.Visible = true;
            this.Enabled = true;
        }

        public virtual void Hide()
        {
            this.Visible = false;
            this.Enabled = false;
        }

        public override void Draw(GameTime gameTime)
        {
            DrawableGameComponent comp = null;
            foreach (GameComponent item in Components)
            {
                if (item is DrawableGameComponent)
                {
                    comp = (DrawableGameComponent)item;
                    if (comp.Visible)
                    {
                        comp.Draw(gameTime);
                    }
                }
            }
            base.Draw(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            foreach (GameComponent item in Components.ToList())
            {
                if (item.Enabled)
                {
                    item.Update(gameTime);
                }
            }
            base.Update(gameTime);
        }
    }
}
