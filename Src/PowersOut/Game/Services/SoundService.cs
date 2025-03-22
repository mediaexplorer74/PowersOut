
// Type: GameManager.Services.SoundService
// Assembly: GameManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D5B7C06C-E91C-42CC-A488-0F11359C3CAC
// Modded by [M]edia[E]xplorer

using BlueJay.Utils;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

#nullable enable
namespace GameManager.Services
{
  public class SoundService
  {
    public Song Background { get; private set; }

    public SoundEffectInstance Thunder { get; private set; }

    public SoundEffectInstance Rain { get; private set; }

    public SoundService(IContentManagerContainer content)
    {
      this.Background = content.Load<Song>("Sound/Music/score-v2");
      this.Thunder = content.Load<SoundEffect>("Sound/Effects/weather-sounds/thunder/loud-thunder-192165").CreateInstance();
      this.Rain = content.Load<SoundEffect>("Sound/Effects/weather-sounds/rain/looped-rain").CreateInstance();
    }

    public void Start()
    {
      MediaPlayer.IsRepeating = true;
      MediaPlayer.Volume = 0.035f;
      MediaPlayer.Play(this.Background);
      this.Rain.IsLooped = true;
      this.Rain.Volume = 0.035f;
      this.Rain.Play();
    }

    public void PlayThunder(float modifier = 1f)
    {
      this.Thunder.IsLooped = false;
      this.Thunder.Volume = 0.055f * modifier;
      this.Thunder.Play();
    }
  }
}
