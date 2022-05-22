using UnityEngine;
using UnityEngine.UI;
using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;


namespace CompassNavigatorPro {

	public partial class CompassPro : MonoBehaviour {

		[NonSerialized]
		public bool needFogOfWarUpdate, needFogOfWarTextureUpdate;

		const string FOG_OF_WAR_LAYER = "FogOfWarLayer";
		Texture2D fogOfWarTexture;
		Color32[] fogOfWarColorBuffer;
		Material fogOfWarMaterial;
		int fogOfWarAutoClearLastPosX, fogOfWarAutoClearLastPosZ;

		#region Fog Of War

		void UpdateFogOfWarOnLoadScene(Scene scene, LoadSceneMode loadMode) {
			if (loadMode == LoadSceneMode.Single) {
				UpdateFogOfWar ();
			}
		}


		void UpdateFogOfWarTexture () {

			if (miniMapCamera == null)
				return;
			
			Transform fogOfWarLayer = transform.Find (FOG_OF_WAR_LAYER);
			if (fogOfWarLayer != null) {
				DestroyImmediate (fogOfWarLayer.gameObject);
			}
			
			if (!_fogOfWarEnabled) return;

			if (fogOfWarTexture == null || fogOfWarTexture.width != _fogOfWarTextureSize || fogOfWarTexture.height != _fogOfWarTextureSize) {
				fogOfWarTexture = new Texture2D (_fogOfWarTextureSize, _fogOfWarTextureSize, TextureFormat.Alpha8, false);
				fogOfWarTexture.hideFlags = HideFlags.DontSave;
				fogOfWarTexture.filterMode = FilterMode.Bilinear;
				fogOfWarTexture.wrapMode = TextureWrapMode.Clamp;
			}

			// Update bounds
			ResetFogOfWar (_fogOfWarDefaultAlpha);
			CompassProFogVolume[] fv = FindObjectsOfType<CompassProFogVolume> ();
			Array.Sort (fv, VolumeComparer);
			for (int k = 0; k < fv.Length; k++) {
				Collider collider = fv [k].GetComponent<Collider> ();
				if (collider != null && collider.gameObject.activeInHierarchy) {
					SetFogOfWarAlpha (collider.bounds, fv [k].alpha, fv [k].border);
				}
			}
			needFogOfWarTextureUpdate = true;
		}

		void UpdateFogOfWarPosition () {
			
			if (!_fogOfWarEnabled)
				return;
			
			if (needFogOfWarUpdate) {
				needFogOfWarUpdate = false;
				UpdateFogOfWarTexture ();
			}

			if (_fogOfWarAutoClear && miniMapFollow != null) {
				Vector3 pos = miniMapFollow.transform.position;
				int x = (int)pos.x;
				int z = (int)pos.z;
				if (x != fogOfWarAutoClearLastPosX || z != fogOfWarAutoClearLastPosZ) {
					fogOfWarAutoClearLastPosX = x;
					fogOfWarAutoClearLastPosZ = z;
					SetFogOfWarAlpha (pos, _fogOfWarAutoClearRadius, 0, 1f);
				}
			}

			if (needFogOfWarTextureUpdate) {
				needFogOfWarTextureUpdate = false;
				if (fogOfWarTexture != null) {
                    fogOfWarTexture.SetPixels32(fogOfWarColorBuffer);
					fogOfWarTexture.Apply ();
                    miniMapMaterialRefresh = true;
				}
			}
		}

		int VolumeComparer (CompassProFogVolume v1, CompassProFogVolume v2) {
			if (v1.order < v2.order) {
				return -1;
			} else if (v1.order > v2.order) {
				return 1;
			} else {
				return 0;
			}
		}

		/// <summary>
		/// Changes the alpha value of the fog of war at world position creating a transition from current alpha value to specified target alpha. It takes into account FogOfWarCenter and FogOfWarSize.
		/// Note that only x and z coordinates are used. Y (vertical) coordinate is ignored.
		/// </summary>
		/// <param name="worldPosition">in world space coordinates.</param>
		/// <param name="radius">radius of application in world units.</param>
		/// <param name="fogNewAlpha">target alpha value.</param>
		/// <param name="border">value that determines the hardness of the border.</param>
		public void SetFogOfWarAlpha (Vector3 worldPosition, float radius, float fogNewAlpha, float border) {
			if (fogOfWarTexture == null)
				return;

			float tx = (worldPosition.x - _fogOfWarCenter.x) / _fogOfWarSize.x + 0.5f;
			if (tx < 0 || tx > 1f)
				return;
			float tz = (worldPosition.z - _fogOfWarCenter.z) / _fogOfWarSize.z + 0.5f;
			if (tz < 0 || tz > 1f)
				return;

			int tw = fogOfWarTexture.width;
			int th = fogOfWarTexture.height;
			int px = Mathf.Clamp ((int)(tx * tw), 0, tw - 1);
			int pz = Mathf.Clamp ((int)(tz * th), 0, th - 1);
			int colorBufferPos = pz * tw + px;
			byte newAlpha8 = (byte)(fogNewAlpha * 255);
			float tr = radius / _fogOfWarSize.z;
			int delta = (int)(th * tr);
			int deltaSqr = delta * delta;
			for (int r = pz - delta; r <= pz + delta; r++) {
				if (r >= 0 && r < th) {
					for (int c = px - delta; c <= px + delta; c++) {
						if (c >= 0 && c < tw) {
							int distanceSqr = (pz - r) * (pz - r) + (px - c) * (px - c);
							if (distanceSqr <= deltaSqr) {
								colorBufferPos = r * tw + c;
								Color32 colorBuffer = fogOfWarColorBuffer [colorBufferPos];
								float t = distanceSqr * border / deltaSqr;
								if (t > 1f) {
									t = 1f;
								}
                                byte targetAlpha = (byte)(newAlpha8 * (1.0 - t) + colorBuffer.a * t); // Mathf.Lerp (newAlpha8, colorBuffer.a, t);
								colorBuffer.a = targetAlpha;
								fogOfWarColorBuffer [colorBufferPos] = colorBuffer;
								needFogOfWarTextureUpdate = true;
							}
						}
					}
				}
			}
		}


		/// <summary>
		/// Changes the alpha value of the fog of war within bounds creating a transition from current alpha value to specified target alpha. It takes into account FogOfWarCenter and FogOfWarSize.
		/// Note that only x and z coordinates are used. Y (vertical) coordinate is ignored.
		/// </summary>
		/// <param name="bounds">in world space coordinates.</param>
		/// <param name="fogNewAlpha">target alpha value.</param>
		/// <param name="border">value that determines the hardness of the border.</param>
		public void SetFogOfWarAlpha (Bounds bounds, float fogNewAlpha, float border) {
			if (fogOfWarTexture == null)
				return;

			Vector3 worldPosition = bounds.center;
			float tx = (worldPosition.x - _fogOfWarCenter.x) / _fogOfWarSize.x + 0.5f;
			if (tx < 0 || tx > 1f)
				return;
			float tz = (worldPosition.z - _fogOfWarCenter.z) / _fogOfWarSize.z + 0.5f;
			if (tz < 0 || tz > 1f)
				return;

			int tw = fogOfWarTexture.width;
			int th = fogOfWarTexture.height;
			int px = Mathf.Clamp ((int)(tx * tw), 0, tw - 1);
			int pz = Mathf.Clamp ((int)(tz * th), 0, th - 1);
			int colorBufferPos = pz * tw + px;
			byte newAlpha8 = (byte)(fogNewAlpha * 255);
			float trx = bounds.extents.x / _fogOfWarSize.x;
			float trz = bounds.extents.z / _fogOfWarSize.z;
			int deltax = (int)(tw * trx);
			int deltaz = (int)(th * trz);
			for (int r = pz - deltaz; r <= pz + deltaz; r++) {
				if (r >= 0 && r < th) {
                    int distancez = pz - r;
                    if (distancez < 0) distancez = -distancez;
                    if (distancez > deltaz) continue;
                    float dz = (deltaz - distancez + 1) / (deltaz * border + 0.0001f);
                    for (int c = px - deltax; c <= px + deltax; c++) {
						if (c >= 0 && c < tw) {
                            int distancex = px - c;
                            if (distancex < 0) distancex = -distancex;
							if (distancex <= deltax) {
								colorBufferPos = r * tw + c;
								Color32 colorBuffer = fogOfWarColorBuffer [colorBufferPos];
								float dx = (deltax - distancex + 1) / (deltax * border + 0.0001f);
								float t = dx * dz;
								if (t > 1f) {
									t = 1f;
								}
                                byte targetAlpha = (byte)(colorBuffer.a * (1f - t) + newAlpha8 * t); // Mathf.Lerp (colorBuffer.a, newAlpha8, t);
								colorBuffer.a = targetAlpha;
								fogOfWarColorBuffer [colorBufferPos] = colorBuffer;
								needFogOfWarTextureUpdate = true;
							}
						}
					}
				}
			}
		}

		/// <summary>
		/// Fill fog of war with a value
		/// </summary>
		public void ResetFogOfWar (float alpha = 1f) {
			if (fogOfWarTexture == null)
				return;
			int h = fogOfWarTexture.height;
			int w = fogOfWarTexture.width;
			int newLength = h * w;
			if (fogOfWarColorBuffer == null || fogOfWarColorBuffer.Length != newLength) {
				fogOfWarColorBuffer = new Color32[newLength];
			}
			byte a8 = (byte)(alpha * 255f);
			Color32 opaque = new Color32 (a8, a8, a8, a8);
			for (int k = 0; k < newLength; k++) {
				fogOfWarColorBuffer [k] = opaque;
			}
			isDirty = true;
		}



		/// <summary>
		/// Gets the current alpha value of the Fog of War at a given world position
		/// </summary>
		/// <returns>The fog of war alpha.</returns>
		/// <param name="worldPosition">World position.</param>
		public float GetFogOfWarAlpha (Vector3 worldPosition) {
			if (fogOfWarColorBuffer == null)
				return 1f;

			float tx = (worldPosition.x - _fogOfWarCenter.x) / _fogOfWarSize.x + 0.5f;
			if (tx < 0 || tx > 1f)
				return 1f;
			float tz = (worldPosition.z - _fogOfWarCenter.z) / _fogOfWarSize.z + 0.5f;
			if (tz < 0 || tz > 1f)
				return 1f;

			int tw = fogOfWarTexture.width;
			int th = fogOfWarTexture.height;
			int px = (int)(tx * tw);
			int pz = (int)(tz * th);
			int colorBufferPos = pz * tw + px;
			if (colorBufferPos < 0 || colorBufferPos >= fogOfWarColorBuffer.Length)
				return 1f;
			return fogOfWarColorBuffer [colorBufferPos].a / 255f;
		}

		#endregion
	}


}



