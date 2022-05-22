﻿using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace CompassNavigatorPro {

	public enum MINIMAP_STYLE {
		TornPaper = 0,
		SolidBox = 1,
		SolidCircle = 2,
		Custom = 100
	}

	public enum MINIMAP_LOCATION {
		TopLeft,
		TopCenter,
		TopRight,
		MiddleLeft,
		MiddleCenter,
		MiddleRight,
		BottomLeft,
		BottomCenter,
		BottomRight,
		Custom
	}


	public enum MINIMAP_CAMERA_MODE {
		Perspective = 0,
		Orthographic = 1
	}


	public enum MINIMAP_CAMERA_SNAPSHOT_FREQUENCY {
		Continuous = 0,
		TimeInterval = 1,
		DistanceTravelled = 2
	}


	public partial class CompassPro : MonoBehaviour {


		#region Events

		/// <summary>
		/// Event fired when this POI appears in the Mini-Map.
		/// </summary>
		public Action<CompassProPOI> OnPOIVisibleInMiniMap;

		/// <summary>
		/// Event fired when the POI disappears from the Mini-Map
		/// </summary>
		public Action<CompassProPOI> OnPOIHidesInMiniMap;

		/// <summary>
		/// Event fired when full screen mode changes
		/// </summary>
		public Action<bool> OnMiniMapChangeFullScreenState;

		#endregion

		#region Public MiniMap properties


		[SerializeField]
		bool _showMiniMap = false;

		/// <summary>
		/// Show/Hide minimap 
		/// </summary>
		public bool showMiniMap {
			get { return _showMiniMap; }
			set {
				if (value != _showMiniMap) {
					_showMiniMap = value;
					SetupMiniMap ();
					isDirty = true;
				}
			}
		}


		[SerializeField]
		MINIMAP_LOCATION _miniMapLocation = MINIMAP_LOCATION.BottomRight;

		/// <summary>
		/// Minimap screen location
		/// </summary>
		public MINIMAP_LOCATION miniMapLocation {
			get { return _miniMapLocation; }
			set {
				if (value != _miniMapLocation) {
					_miniMapLocation = value;
					SetupMiniMap ();
					isDirty = true;
				}
			}
		}



		[SerializeField]
		Vector2 _miniMapLocationOffset;

		/// <summary>
		/// Minimap screen location offset
		/// </summary>
		public Vector2 miniMapLocationOffset {
			get { return _miniMapLocationOffset; }
			set {
				if (value != _miniMapLocationOffset) {
					_miniMapLocationOffset = value;
					SetupMiniMap ();
					isDirty = true;
				}
			}
		}


		[SerializeField]
		bool _miniMapKeepStraight;

		/// <summary>
		/// Keep the mini-map oriented to North
		/// </summary>
		public bool miniMapKeepStraight {
			get { return _miniMapKeepStraight; }
			set {
				if (value != _miniMapKeepStraight) {
					_miniMapKeepStraight = value;
					if (cameraCompass != null) {
						cameraCompass.eulerAngles = new Vector3 (0, 0, 180f);
					}
                    SetupMiniMap(true);
					isDirty = true;
				}
			}
		}



		[SerializeField]
		[Range(0, 90)]
		float _miniMapCameraTilt;

		/// <summary>
		/// Mini-map camera rotation. By default it looks straight down.
		/// </summary>
		public float miniMapCameraTilt {
			get { return _miniMapCameraTilt; }
			set {
				if (value != _miniMapCameraTilt) {
					_miniMapCameraTilt = value;
					UpdateMiniMapContents();
					isDirty = true;
				}
			}
		}


		[SerializeField]
		float _miniMapSize = 0.25f;

		/// <summary>
		/// The screen size of the mini-map
		/// </summary>
		public float miniMapSize {
			get { return _miniMapSize; }
			set {
				if (value != _miniMapSize) {
					_miniMapSize = value;
					SetupMiniMap ();
					isDirty = true;
				}
			}
		}

		/// <summary>
		/// Where to center the mini map
		/// </summary>
		public Transform miniMapFollow;


		/// <summary>
		/// Optional mini-map mask texture
		/// </summary>
		[SerializeField]
		Sprite _miniMapMaskSprite;

		/// <summary>
		/// The sprite for the mini-map mask
		/// </summary>
		public Sprite miniMapMaskSprite {
			get { return _miniMapMaskSprite; }
			set {
				if (value != _miniMapMaskSprite) {
					_miniMapMaskSprite = value;
					SetupMiniMap ();
					isDirty = true;
				}
			}
		}

		/// <summary>
		/// Optional mini-map border texture
		/// </summary>
		[SerializeField]
		Texture2D _miniMapBorderTexture;

		/// <summary>
		/// Show/Hide minimap 
		/// </summary>
		public Texture2D miniMapBorderTexture {
			get { return _miniMapBorderTexture; }
			set {
				if (value != _miniMapBorderTexture) {
					_miniMapBorderTexture = value;
					SetupMiniMap ();
					isDirty = true;
				}
			}
		}


		[SerializeField]
		MINIMAP_STYLE _miniMapStyle = MINIMAP_STYLE.TornPaper;

		/// <summary>
		/// Style of mini-map
		/// </summary>
		public MINIMAP_STYLE miniMapStyle {
			get { return _miniMapStyle; }
			set {
				if (value != _miniMapStyle) {
					_miniMapStyle = value;
					SetupMiniMap ();
					isDirty = true;
				}
			}
		}


		[SerializeField]
		Sprite _miniMapMaskSpriteFullScreenMode;

		/// <summary>
		/// The sprite for the mini-map mask in full-screen mode
		/// </summary>
		public Sprite miniMapMaskSpriteFullScreenMode {
			get { return _miniMapMaskSpriteFullScreenMode; }
			set {
				if (value != _miniMapMaskSpriteFullScreenMode) {
					_miniMapMaskSpriteFullScreenMode = value;
					SetupMiniMap();
					isDirty = true;
				}
			}
		}

		[SerializeField]
		Texture2D _miniMapBorderTextureFullScreenMode;

		/// <summary>
		/// Optional mini-map border texture when in full-screen mode
		/// </summary>
		public Texture2D miniMapBorderTextureFullScreenMode {
			get { return _miniMapBorderTextureFullScreenMode; }
			set {
				if (value != _miniMapBorderTextureFullScreenMode) {
					_miniMapBorderTextureFullScreenMode = value;
					SetupMiniMap();
					isDirty = true;
				}
			}
		}

		[SerializeField]
		MINIMAP_STYLE _miniMapStyleFullScreenMode = MINIMAP_STYLE.SolidBox;

		/// <summary>
		/// Style for the mini-map when in full-screen mode
		/// </summary>
		public MINIMAP_STYLE miniMapStyleFullScreenMode {
			get { return _miniMapStyleFullScreenMode; }
			set {
				if (value != _miniMapStyleFullScreenMode) {
					_miniMapStyleFullScreenMode = value;
					SetupMiniMap();
					isDirty = true;
				}
			}
		}

		[SerializeField]
		int _miniMapResolutionNormalSize = 256;

		/// <summary>
		/// The capture resolution when minimap is in full-screen mode.
		/// </summary>
		public int miniMapResolutionNormalSize {
			get { return _miniMapResolutionNormalSize; }
			set {
				if (value != _miniMapResolutionNormalSize) {
					_miniMapResolutionNormalSize = value;
					SetupMiniMap ();
					isDirty = true;
				}
			}
		}


		[SerializeField]
		RectTransform _miniMapFullScreenPlaceholder;

		/// <summary>
		/// Optionally assign an UI rect transform which will be used to render the mini-map in full-screen mode
		/// </summary>
		public RectTransform miniMapFullScreenPlaceholder {
			get { return _miniMapFullScreenPlaceholder; }
			set {
				if (value != _miniMapFullScreenPlaceholder) {
					if (_miniMapZoomState) {
						MiniMapZoomToggle (false);
					} else {
						SetupMiniMap ();
					}
					_miniMapFullScreenPlaceholder = value;
					isDirty = true;
				}
			}
		}



		[SerializeField]
		float _miniMapFullScreenSize = 0.9f;

		/// <summary>
		/// The percentage of screen size when minimap is in full screen mode.
		/// </summary>
		public float miniMapFullScreenSize {
			get { return _miniMapFullScreenSize; }
			set {
				if (value != _miniMapFullScreenSize) {
					_miniMapFullScreenSize = value;
					SetupMiniMap ();
					isDirty = true;
				}
			}
		}



		[SerializeField]
		bool _miniMapKeepAspectRatio = true;

		/// <summary>
		/// Keep aspect ration in full screen mode
		/// </summary>
		public bool miniMapKeepAspectRatio {
			get { return _miniMapKeepAspectRatio; }
			set {
				if (value != _miniMapKeepAspectRatio) {
					_miniMapKeepAspectRatio = value;
					SetupMiniMap ();
					isDirty = true;
				}
			}
		}

		[SerializeField] MINIMAP_CAMERA_MODE _miniMapCameraMode = MINIMAP_CAMERA_MODE.Orthographic;

		/// <summary>
		/// Mini-map projection mode
		/// </summary>
		public MINIMAP_CAMERA_MODE miniMapCameraMode {
			get { return _miniMapCameraMode; }
			set {
				if (value != _miniMapCameraMode) {
					_miniMapCameraMode = value;
					SetupMiniMap ();
					isDirty = true;
				}
			}
		}


		[SerializeField]
		MINIMAP_CAMERA_SNAPSHOT_FREQUENCY _miniMapCameraSnapshotFrequency = MINIMAP_CAMERA_SNAPSHOT_FREQUENCY.DistanceTravelled;

		/// <summary>
		/// How often the mini-map camera will capture the scene
		/// </summary>
		public MINIMAP_CAMERA_SNAPSHOT_FREQUENCY miniMapCameraSnapshotFrequency {
			get { return _miniMapCameraSnapshotFrequency; }
			set {
				if (value != _miniMapCameraSnapshotFrequency) {
					_miniMapCameraSnapshotFrequency = value;
					SetupMiniMap ();
					isDirty = true;
				}
			}
		}

		[SerializeField]
		float _miniMapCaptureSize = 256f;

		/// <summary>
		/// The orthographic camera size
		/// </summary>
		public float miniMapCaptureSize {
			get { return _miniMapCaptureSize; }
			set {
				if (value != _miniMapCaptureSize) {
					_miniMapCaptureSize = value;
                    UpdateMiniMapContents(); 
					isDirty = true;
				}
			}
		}


		[SerializeField]
		float _miniMapSnapshotInterval = 10f;

		/// <summary>
		/// The time interval between minimap camera shots
		/// </summary>
		public float miniMapSnapshotInterval {
			get { return _miniMapSnapshotInterval; }
			set {
				if (value != _miniMapSnapshotInterval) {
					_miniMapSnapshotInterval = value;
					isDirty = true;
				}
			}
		}


		[SerializeField]
		float _miniMapSnapshotDistance = 10f;

		/// <summary>
		/// The distance interval between minimap camera shots
		/// </summary>
		public float miniMapSnapshotDistance {
			get { return _miniMapSnapshotDistance; }
			set {
				if (value != _miniMapSnapshotDistance) {
					_miniMapSnapshotDistance = value;
					isDirty = true;
				}
			}
		}


		[SerializeField]
		float _miniMapContrast = 1.02f;

		/// <summary>
		/// Contrast of the mini-map image
		/// </summary>
		public float miniMapContrast {
			get { return _miniMapContrast; }
			set {
				if (value != _miniMapContrast) {
					_miniMapContrast = value;
					miniMapMaterialRefresh = true;
					isDirty = true;
				}
			}
		}


		[SerializeField]
		float _miniMapBrightness = 1.05f;

		/// <summary>
		/// Brightness of the mini-map image
		/// </summary>
		public float miniMapBrightness {
			get { return _miniMapBrightness; }
			set {
				if (value != _miniMapBrightness) {
					_miniMapBrightness = value;
					miniMapMaterialRefresh = true;
					isDirty = true;
				}
			}
		}


		[SerializeField]
		bool _miniMapEnableShadows = false;

		/// <summary>
		/// Enables/disables shadow casting when rendering mini-map
		/// </summary>
		public bool miniMapEnableShadows {
			get { return _miniMapEnableShadows; }
			set {
				if (value != _miniMapEnableShadows) {
					_miniMapEnableShadows = value;
					isDirty = true;
				}
			}
		}





		[SerializeField, Range (0, 1)]
		float _miniMapZoomMin = 0.01f;

		/// <summary>
		/// The orthographic minimum size for the camera
		/// </summary>
		public float miniMapZoomMin {
			get { return _miniMapZoomMin; }
			set {
				if (value != _miniMapZoomMin) {
					_miniMapZoomMin = value;
                    UpdateMiniMapContents(); 
					isDirty = true;
				}
			}
		}

		[SerializeField, Range (0, 1)]
		float _miniMapZoomMax = 1f;

		/// <summary>
		/// The orthographic maximum size for the camera
		/// </summary>
		public float miniMapZoomMax {
			get { return _miniMapZoomMax; }
			set {
				if (value != _miniMapZoomMax) {
					_miniMapZoomMax = value;
                    UpdateMiniMapContents();
					isDirty = true;
				}
			}
		}


		[SerializeField, Range (0, 1f)]
		float _miniMapZoomLevel = 0.5f;

		/// <summary>
		/// The current mini-map zoom based on the min/max size (orthographic mode) or altitude (perspective mode)
		/// </summary>
		public float miniMapZoomLevel {
			get { return _miniMapZoomLevel; }
			set {
				float clampedValue = Mathf.Clamp(value, _miniMapZoomMin, _miniMapZoomMax);
				if (clampedValue != _miniMapZoomLevel) {
					_miniMapZoomLevel = clampedValue;
                    UpdateMiniMapContents();
					isDirty = true;
				}
			}
		}


		[SerializeField]
		float _miniMapCameraMinAltitude = 10;

		/// <summary>
		/// The min distance from the camera to the following target
		/// </summary>
		public float miniMapCameraMinAltitude {
			get { return _miniMapCameraMinAltitude; }
			set {
				if (value != _miniMapCameraMinAltitude) {
					_miniMapCameraMinAltitude = value;
                    UpdateMiniMapContents();
					isDirty = true;
				}
			}
		}


		[SerializeField]
		float _miniMapCameraMaxAltitude = 100f;

		/// <summary>
		/// The max distance from the camera to the following target
		/// </summary>
		public float miniMapCameraMaxAltitude {
			get { return _miniMapCameraMaxAltitude; }
			set {
				if (value != _miniMapCameraMaxAltitude) {
					_miniMapCameraMaxAltitude = value;
                    UpdateMiniMapContents();
					isDirty = true;
				}
			}
		}



		[SerializeField]
		float _miniMapCameraHeightVSFollow = 200f;

		/// <summary>
		/// When mini-map is in orthographic projection, an optional height for the camera with respect to the main camera or followed item
		/// </summary>
		public float miniMapCameraHeightVSFollow {
			get { return _miniMapCameraHeightVSFollow; }
			set {
				if (value != _miniMapCameraHeightVSFollow) {
					_miniMapCameraHeightVSFollow = value;
                    UpdateMiniMapContents();
					isDirty = true;
				}
			}
		}


		[SerializeField] int _miniMapLayerMask = -1;

		/// <summary>
		/// The layer mask for the mini-map camera
		/// </summary>
		public int miniMapLayerMask {
			get { return _miniMapLayerMask; }
			set {
				if (value != _miniMapLayerMask) {
					_miniMapLayerMask = value;
					SetupMiniMap ();
					isDirty = true;
				}
			}
		}

		[SerializeField]
		float _miniMapIconSize = 0.5f;

		/// <summary>
		/// The size for the icons on the mini-map
		/// </summary>
		public float miniMapIconSize {
			get { return _miniMapIconSize; }
			set {
				if (value != _miniMapIconSize) {
					_miniMapIconSize = value;
					isDirty = true;
				}
			}
		}



		[SerializeField]
		float _miniMapPlayerIconSize = 1f;

		/// <summary>
		/// Scale multiplier for player's icon
		/// </summary>
		public float miniMapPlayerIconSize {
			get { return _miniMapPlayerIconSize; }
			set {
				if (value != _miniMapPlayerIconSize) {
					_miniMapPlayerIconSize = value;
					isDirty = true;
				}
			}
		}

		[SerializeField]
		float _miniMapClampBorder = 0.02f;

		/// <summary>
		/// The distance to the edge for the clamped icons on the minimap
		/// </summary>
		public float miniMapClampBorder {
			get { return _miniMapClampBorder; }
			set {
				if (value != _miniMapClampBorder) {
					_miniMapClampBorder = value;
					needUpdateBarContents = true;
					isDirty = true;
				}
			}
		}

		[SerializeField]
		bool _miniMapClampBorderCircular;

		/// <summary>
		/// Enable if the mini-map uses a circular shape
		/// </summary>
		public bool miniMapClampBorderCircular {
			get { return _miniMapClampBorderCircular; }
			set {
				if (value != _miniMapClampBorderCircular) {
					_miniMapClampBorderCircular = value;
					needUpdateBarContents = true;
					isDirty = true;
				}
			}
		}
		


		[SerializeField]
		float _miniMapAlpha = 1.0f;

		/// <summary>
		/// The alpha (transparency) of the mini-map.
		/// </summary>
		public float miniMapAlpha {
			get { return _miniMapAlpha; }
			set {
				if (value != _miniMapAlpha) {
					_miniMapAlpha = value;
					isDirty = true;
				}
			}
		}

		[SerializeField]
		bool _miniMapShowButtons;

		public bool miniMapShowButtons {
			get { return _miniMapShowButtons; }
			set {
				if (value != _miniMapShowButtons) {
					_miniMapShowButtons = value;
					isDirty = true;
					SetupMiniMap ();
				}
			}
		}


		[SerializeField]
		float _miniMapButtonsScale = 1.0f;

		/// <summary>
		/// The size of the buttons of the mini-map
		/// </summary>
		public float miniMapButtonsScale {
			get { return _miniMapButtonsScale; }
			set {
				if (value != _miniMapButtonsScale) {
					_miniMapButtonsScale = value;
					isDirty = true;
					SetupMiniMap ();
				}
			}
		}

		[SerializeField]
		bool _miniMapIconEvents;

		public bool miniMapIconEvents {
			get { return _miniMapIconEvents; }
			set {
				if (_miniMapIconEvents != value) {
					_miniMapIconEvents = value;
				}
			}
		}


		public void MiniMapZoomIn (float speed = 1f) {
			miniMapZoomLevel += Time.deltaTime * speed;
		}

		public void MiniMapZoomOut (float speed = 1f) {
			miniMapZoomLevel -= Time.deltaTime * speed;
		}

		bool _miniMapZoomState;

		/// <summary>
		/// Sets mini-map in full-screen mode or normal mode
		/// </summary>
		public bool miniMapZoomState {
			get {
				return _miniMapZoomState; 
			}
			set {
				if (_miniMapZoomState != value) {
					if (OnMiniMapChangeFullScreenState != null) {
						OnMiniMapChangeFullScreenState (value);
					}
					MiniMapZoomToggle (value);
				}
			}
		}

		/// <summary>
		/// Forces an update of mini-map contents
		/// </summary>
		public void UpdateMiniMapContents (int numberOfFramesToRefresh = 1) {
			if (needMiniMapShot == 0) {
				needMiniMapShot += numberOfFramesToRefresh;
			}
			needUpdateBarContents = true;
		}


		/// <summary>
		/// Returns true if mouse pointer is over the mini-map
		/// </summary>
		/// <returns><c>true</c> if this instance is pointer over mini map; otherwise, <c>false</c>.</returns>
		public bool IsMouseOverMiniMap () {
			if (miniMapUIRootRT == null)
				return false;
			return RectTransformUtility.RectangleContainsScreenPoint (miniMapUIRootRT, Input.mousePosition);
		}

		public Vector3 miniMapFullScreenWorldCenter;
		public Vector3 miniMapFullScreenWorldSize = new Vector3(1000,0,1000);
		public bool miniMapFullScreenWorldCenterFollows;
		public bool miniMapFullScreenFreezeCamera = true;

		#endregion


		
	}

}



