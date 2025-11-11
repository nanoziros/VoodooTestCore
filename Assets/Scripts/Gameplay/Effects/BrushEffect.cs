using UnityEngine;

namespace Gameplay.Effects
{
	public abstract class BrushEffect : MonoBehaviour
	{
		public virtual void Play()
		{
		}

		public virtual void Stop()
		{
		}

		public virtual void SetColor(Color _Color)
		{
		}

		protected void SetParticleSysColor(ParticleSystem _Sys, Color _Color)
		{
			ParticleSystem.MainModule main = _Sys.main;
			ParticleSystem.MinMaxGradient grad = main.startColor;
			grad.color = _Color;
			main.startColor = grad;
		}
	}
}
