
// Type: GameManager.Systems.ProfileRenderSystem
// Assembly: GameManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D5B7C06C-E91C-42CC-A488-0F11359C3CAC
// Modded by [M]edia[E]xplorer

using BlueJay.Component.System.Interfaces;
using BlueJay.Core.Containers;
using BlueJay.Utils;
using Microsoft.Xna.Framework;
using GameManager.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

#nullable enable
namespace GameManager.Systems
{
    public class ProfileRenderSystem : IDrawSystem, ISystem
    {
        private readonly ProfileService _profileService;
        private readonly ISpriteBatchContainer _spriteBatch;
        private readonly ISpriteFontContainer _font;

        public ProfileRenderSystem(
          ProfileService profileService,
          ISpriteBatchContainer spriteBatch,
          IContentManagerContainer content)
        {
            this._profileService = profileService;
            this._spriteBatch = spriteBatch;
            this._font = content.Load<ISpriteFontContainer>("TestFont");
        }

        public void OnDraw()
        {
            this._spriteBatch.Begin();
            int y = 10;
            foreach (KeyValuePair<string, double> keyValuePair 
                in (IEnumerable<KeyValuePair<string, double>>)Enumerable.OrderBy<KeyValuePair<string, double>, 
                string>((IEnumerable<KeyValuePair<string, double>>)this._profileService.Profile, 
                (Func<KeyValuePair<string, double>, string>)(x => x.Key)))
            {
                ISpriteBatchContainer spriteBatch = this._spriteBatch;
                ISpriteFontContainer font = this._font;
                string text = $"{keyValuePair.Key} (ms): {keyValuePair.Value}";
                Vector2 position = new Vector2(10f, (float)y);
                Color white = Color.White;
                spriteBatch.DrawString(font, text, position, white);
                y += 20;
            }
            this._spriteBatch.End();
        }
    }
}
