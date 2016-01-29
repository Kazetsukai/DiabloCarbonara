Blood Spatter Particle System Help File

Files Included:
- Blood Spatter Particle Prefab
- Blood Spatter Texture Sheet
- Test Scene (As seen in video)

To apply blood spatter to scene: 
Drag BloodSplatter.js to the Main Camera from Project Folder > BloodSpatter_StaticNova > Scripts. This script will trigger each time the mouse is clicked.
Edit the variables by dragging GameObjects into the Main Camera component.
1. Drag BloodSplatter_Normal_Prefab into the Blood Prefab slot
2. Drag a GameObject into the Blood Position slot, this determines where the blood is Instantiated (Spawned).
3. Drag a GameObject into the Blood Rotation slot, this determines the rotation of the Blood Prefab.
4. Adjust rotation of the BloodSplatterPrefab if it apears wrong, else leave it at zero.
5. Adjust how many instances of the BloodSplatterPrefab are alowed on stage at once, setting this low will improve performance but might not look as good.

Notes:
Change Max and Min Emmission within BloodSpatter_Normal_Prefab to increase or decrease the amount of blood spatter, this can affect performance.
Also, changing min and max energy will determine how long the particles stay "alive" or visible.

Particle Prefab by StaticNova.
For support or queries email: Support@staticnova.com
