using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace jp.gulti.ColorBlind
{

	public class ColorBlindnessSimulator : jp.gulti.PostEffectsBase
	{
		public enum ColorBlindMode
		{
			Protanope,
			Deuteranope,
		}
		[SerializeField]
		public ColorBlindMode BlindMode = ColorBlindMode.Protanope;

		[SerializeField]
		public float BlindIntensity = 1.0f;

        [SerializeField]
		public Shader ColorBlindShader;
		protected Material ColorBlindMat;

		public enum ColorBlindAlgorithm
		{
			GULTI,
			IntSim,
		}
		public ColorBlindAlgorithm BlindAlgorithm = ColorBlindAlgorithm.GULTI;

		#region Overrides
		
		protected override bool CheckResources ()
		{
			CheckSupport(false);
			ColorBlindShader = Shader.Find("Hidden/GULTI/ColorBlindSimulator");
			ColorBlindMat = CreateMaterial (ColorBlindShader, ColorBlindMat);

			return ColorBlindMat != null;
		}
		
		#endregion

		#region Monobehavior

		void OnDisable()
		{
			if(ColorBlindMat != null)
			{
#if UNITY_EDITOR
				if(!UnityEditor.EditorApplication.isPlaying)
					DestroyImmediate(ColorBlindMat, true);
				else
#endif
				Destroy(ColorBlindMat);
			}
		}

		void OnRenderImage(RenderTexture _src, RenderTexture _dst)
		{
			if(ColorBlindMat == null)
			{
				if(!CheckResources())
				{
					NotSupported();
					return;
				}
			}

			switch (BlindMode)
			{
				case ColorBlindMode.Protanope:
					ColorBlindMat.shaderKeywords = new string[] { "CB_TYPE_ONE" };
					break;
				case ColorBlindMode.Deuteranope:
					ColorBlindMat.shaderKeywords = new string[] { "CB_TYPE_TWO" };
					break;
			}

			switch (BlindAlgorithm)
			{
			case ColorBlindAlgorithm.GULTI:
				ColorBlindShader = Shader.Find("Hidden/GULTI/ColorBlindSimulator");
				break;
			case ColorBlindAlgorithm.IntSim:
				ColorBlindShader = Shader.Find ("Hidden/GULTI/CVDSimulator");
				break;
			}
			ColorBlindMat.shader = ColorBlindShader;

			// Intensity Set
			ColorBlindMat.SetFloat("_BlindIntensity", BlindIntensity);

			Graphics.Blit(_src, _dst, ColorBlindMat);
		}

		#endregion
	}
}