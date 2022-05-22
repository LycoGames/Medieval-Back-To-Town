************************************
*      COMPASS NAVIGATOR PRO       *
* (C) Copyright 2016-2019 Kronnect * 
*            README FILE           *
************************************


How to use this asset
---------------------

Thanks for purchasing Compass Navigator Pro!

Using Compass Navigator Pro is very easy! Please take a moment to read the Quick Start Guide located in the Documentation folder.



Future updates
--------------

All our assets follow an incremental development process by which a few beta releases are published on our support forum (kronnect.com).
We encourage you to signup and engage our forum. The forum is the primary support and feature discussions medium.

Of course, all updates of Compass Navigator Pro be eventually available on the Asset Store.



Other Cool Assets!
------------------

Check our other assets on the Asset Store publisher page:
https://www.assetstore.unity3d.com/en/#!/search/page=1/sortby=popularity/query=publisher:15018




Version history
---------------

5.6.1
- Added "Style" option for full screen mode
- Added "Border Is Circular" option useful when clamping icons to the edge

5.5.1
- [Fix] Fixed layer 32 not being selectable in Layer Mask property under Mini-Map settings

5.5
- Added "Camera Tilt" option
- Fixes and minor improvements

5.4
- Added "Player Icon Size" property

V5.3
- Added "Letter Spacing" property to Compass Navigator Pro inspector (Text section)

V5.2
- Added "Visible Min Distance" property to Compass Navigator Pro inspector
- Added "Icon Scale" property to POI script
- Added "Min Distance Text" to POI script (hides the distance when object is near)
- Added "Override Visible Min Distance" property to POI script

V5.1.2
- Added a clamp when setting zoom level by code so it keeps between the range defined by mini map zoom min/max values

V5.1
- Added "Custom" to Compass Pro style options (preserves your own sprite changes)
- [Fix] Duplicating a POI in Editor with "Don't Destroy On Load" option checked results in one of the copies removed from scene at runtime
- [Fix] Fixed an issue when dragging prefab on Unity 2019.3

V5.0
- Added "Center Of Followed" option to keep world map focused on followed object

V4.9
- Added Visited Distance Override to CompassProPOI
- Minimap Min/Max zoom levels are now exposed in the inspector when mini-map mode is set to orthographic

V4.8.2
- [Fix] Fixed an issue when restoring mini-map to normal size with custom borders

V4.8.1
- [Fix] Fixed POI discovery issue with radius option

V4.8
- Added world center, world size and option to freeze camera rotation when in full screen mode
- Minimum Unity version upped to 2017.4 (LTS)
- API: added OnMiniMapChangeFullScreenState event

V4.7.3
- [Fix] Fixed mini-map not updating correctly when loading new levels at runtime

V4.7.2
- API: added POIShowBeacon(position)
- [Fix] Fixed camera icon in minimap rotation issue when toggling Keep Straight option at runtime

V4.7
- Minimap: added full screen placeholder option. Allows to fit the minimap within the given UI element in full screen mode
- [Fix] Fixed minimap not rendering Aquas water during startup due to delayed rendering

V4.6
- Added Fade Out Start / Fade Out Width options to fade out compass bar edges
- Compass Icons created on the Compass Bar UI now use the name of the gameobject to which they represent
- Demo scene updated to show tooltips on mini-map icons
- API: added compassIconGameObject to CompassProPOI component. Holds a reference to the compass icon on the navigation bar when the POI icon is created 
- API: added miniMapIconGameObject to CompassProPOI component. Holds a reference to the compass icon on the mini-map when the POI icon is created 
- API: added new events OnPOIMiniMapIconMouseEnter, OnPOIMiniMapIconMouseExit, OnPOIMiniMapIconMouseDown, OnPOIMiniMapIconMouseUp, OnPOIMiniMapIconMouseClick
- API: added GetMiniMapIconScreenRect to CompassProPOI component. Returns the screen rect occupied by the icon on the mini-map (if visible)
- API: added GetCompassIconScreenRect to CompassProPOI component. Returns the screen rect occupied by the icon on the compass bar (if visible)

V4.5.2
- Performance improvements

V4.5.1
- [Fix] Fixed issue with mini-map icon visibility

V4.5
- Added contrast and brightness options to mini-map inspector
- Improved compatibility of mini-map and Gaia water system
- Improved inspector with collapsible sections to reduce clutter

V4.4
- Reworked mini-map internals
- Added: minimap screen location
- Added: minimap snapshot frequency (in orthographic mode). Can choose between: continuous, time interval, distance travelled.

V4.3.1
- Added cardinal vertical offset to fine-adjust cardinal/ordinal points vertical position

V4.3
- Added mini-map buttons for zoom control
- Added mini-map full screen mode
- API: added miniMapZoomState, MiniMapZoomToggle

V4.2
- API: added fogOfWarTextureData to retrieve/set fog of war state between sessions

V4.1.2
- Fog of War now correctly refreshes between scene changes
- Updated documentation with available fog of war methods and scaling tips

V4.1.1
- Fog of War: added Default Amount/Alpha setting

V4.1
- Mini-Map: added min/max zoom ranges. Use MiniMapZoomLevel property to change current zoom.
- Mini-Map: added Keep Straight option
- Fog of War: auto clear option

V4.0
- New Mini-Map fog of war feature!

V3.5.1
- [Fix] Fixed CompassPro singleton not found if gameobject is renamed

V3.5
- Added compass bending option. Demo: https://youtu.be/y-9jNIaU6Wc

V3.2.2
- Resorted Minimap hierarchy to allow background images

V3.2.1
- [Fix] Compass animation text improvements

V3.2
- Added "Text Shadow" and "Title Shadow" options
- [Fix] Fixed text alignment in Edit mode

V3.1
- Added "Idle Update Mode" property to Compass Bar inspector
- Added "Visible Distance Override" property to POI component
- Improved "Torn Paper" mini-map border

V3.0
- Added Mini-Map
- Added tint color to POIs
- Optional color for beacons

V2.5.1
- Minor improvements and fixes

V2.5
- Added POI Visibility property (always visible, when in range, always hidden)

V2.4.1
- POIs retain positions in compass bar when looking up/down

V2.4
- Added North position

V2.3
- Added Label Hot Zone parameter to define the distance from the center of the compass bar where a POI's label can be shown
- [Fix] POI references were lost if compass bar was disabled/enabled

V2.2
- Added "Show Ordinal Points" (intercardinals: NW, NE, SW, SE)
- Added "Show Half Winds"
- Added "Full 360 Degrees" option to World Mapping Mode
- Internal optimizations
- [Fix] Fixed minor issues with cardinals positions

V2.1
- Heartbeat effects can be added to custom POIs. Sound will play at variable interval based on distance!
- Added "String Format" option for the distance shown on top of compass bar
- Added "canBeVisited" property to POIs

V2.0.1
- [Fix] Fixed demo scene namespace compiler issue with Unity 5.6+

V2.0
- Demo scene included.
- Added World Mapping Modes for different horizontal POI positioning on the compass bar
- Added "Scale In Duration" to add a scaling animation for icons when they appear on the compass bar
- Added options to change Font for titles and texts in Compass Bar inspector
- Added option to enable/disable text revealing effect
- Added title visibility parameter

V1.6
- Improved canvas UI handling so it now supports World Space Canvas properly (also useful for VR)

V1.5.1
- [Fix] Fixed Editor warning with DontDestroyOnLoad call

V1.5
- Added DontDestroyOnLoad support to Compass Bar and POIs so they preserve their states between scene changes
- New menu item GameObject > UI > Compass Navigator Pro to quickly add the compass bar to the scene for convenience

V 1.4.3 
- Added public method ShowAnimatedText()
- [Fix] Fixed text shadow positioning

V 1.4.2 Release 10/Feb/2017
- [Fix] Prevented null error if no main camera is found

V 1.4.1	Release 1/Feb/2017
-   Fixed POI icon not being removed when POI gameobject was destroyed

V 1.4	Release – 18/April/2016
-   New POI beacon effect (API: POIShowBeacon).
-   Added hideWhenVisited to POIs.
-   Automatically hide/make visible POI when parent game object is enabled/disabled.
-   Added clampPosition to POIs (forces POI icon to stay in the bar, possible on the edges if it's outside player view).

V 1.3	Release – 18/April/2016
-	Added smooth alpha transition when a POI gets in visible distance range.
-	Added autohide property

V 1.2	Release – 31/March/2016
-	Added compatibility with Gaia for automatic population of POIs
-	Fixes

V 1.1	Release – 31/March/2016
-	Added optional audio clip support
-	New OnPOIVisible and ONPOIHide events

V 1.0	First Release – 30/March/2016








