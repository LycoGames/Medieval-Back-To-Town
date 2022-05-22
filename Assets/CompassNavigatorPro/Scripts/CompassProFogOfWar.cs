using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace CompassNavigatorPro {


	public partial class CompassPro : MonoBehaviour {

		[SerializeField]
		bool _fogOfWarEnabled;

		public bool fogOfWarEnabled {
			get { return _fogOfWarEnabled; }
			set {
				if (value != _fogOfWarEnabled) {
					_fogOfWarEnabled = value;
					UpdateFogOfWar ();
					isDirty = true;
				}
			}
		}


		[SerializeField]
		Vector3 _fogOfWarCenter;

		public Vector3 fogOfWarCenter {
			get { return _fogOfWarCenter; }
			set {
				if (value != _fogOfWarCenter) {
					_fogOfWarCenter = value;
					UpdateFogOfWar ();
					isDirty = true;
				}
			}
		}

		[SerializeField]
		Vector3 _fogOfWarSize = new Vector3 (1024, 0, 1024);

		public Vector3 fogOfWarSize {
			get { return _fogOfWarSize; }
			set {
				if (value != _fogOfWarSize) {
					if (value.x > 0 && value.z > 0) {
						_fogOfWarSize = value;
						UpdateFogOfWar ();
						isDirty = true;
					}
				}
			}
		}

		[SerializeField, Range (32, 2048)]
		int _fogOfWarTextureSize = 256;

		public int fogOfWarTextureSize {
			get { return _fogOfWarTextureSize; }
			set {
				if (value != _fogOfWarTextureSize) {
					if (value > 16) {
						_fogOfWarTextureSize = value;
						UpdateFogOfWar ();
						isDirty = true;
					}
				}
			}
		}


		[SerializeField]
		Color _fogOfWarColor = new Color (47 / 255f, 47 / 255f, 47 / 255f);

		public Color fogOfWarColor {
			get { return _fogOfWarColor; }
			set {
				if (value != _fogOfWarColor) {
					_fogOfWarColor = value;
                    miniMapMaterialRefresh = true;
					UpdateFogOfWar ();
					isDirty = true;
				}
			}
		}

		[SerializeField]
		bool _fogOfWarAutoClear;

		/// <summary>
		/// Clears fog automatically as player cross it
		/// </summary>
		/// <value><c>true</c> if fog of war auto clear; otherwise, <c>false</c>.</value>
		public bool fogOfWarAutoClear {
			get { return _fogOfWarAutoClear; }
			set {
				if (value != _fogOfWarAutoClear) {
					_fogOfWarAutoClear = value;
					isDirty = true;
				}
			}
		}



		[SerializeField]
		float _fogOfWarAutoClearRadius = 20f;

		public float fogOfWarAutoClearRadius {
			get { return _fogOfWarAutoClearRadius; }
			set {
				if (value != _fogOfWarAutoClearRadius) {
					_fogOfWarAutoClearRadius = value;
					isDirty = true;
				}
			}
		}



		[SerializeField, Range(0,1)]
		float _fogOfWarDefaultAlpha = 1f;

		public float fogOfWarDefaultAlpha {
			get { return _fogOfWarDefaultAlpha; }
			set {
				if (value != _fogOfWarDefaultAlpha) {
					_fogOfWarDefaultAlpha = value;
					UpdateFogOfWar ();
					isDirty = true;
				}
			}
		}


		public void UpdateFogOfWar () {
			if (!fogOfWarEnabled)
				return;
			if (Application.isPlaying) {
				needFogOfWarUpdate = true;
			} else {
				UpdateFogOfWarTexture ();
			}
		}



		/// <summary>
		/// Gets or set fog of war state as a Color32 buffer. The alpha channel stores the transparency of the fog at that position (0 = no fog, 1 = opaque).
		/// </summary>
		public Color32[] fogOfWarTextureData { 
			get { 
				return fogOfWarColorBuffer;
			} 
			set {
				fogOfWarEnabled = true;
				fogOfWarColorBuffer = value;
				if (value == null || fogOfWarTexture == null)
					return;
				if (value.Length != fogOfWarTexture.width * fogOfWarTexture.height)
					return;
				fogOfWarTexture.SetPixels32(fogOfWarColorBuffer);
				fogOfWarTexture.Apply();
                miniMapMaterialRefresh = true;
			}
		}


	}

}



