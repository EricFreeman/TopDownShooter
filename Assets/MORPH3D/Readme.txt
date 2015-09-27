Welcome To The Morph3D Character System!


********************************************
************** GETTING STARTED *************
********************************************

1) If this is the first time you've imported A Morph3D Package, you'll need to reimport the "MORPH3D" folder. The reason for this is that unity will not always import the Morph3D DLL before it imports the content. Our DLL file sets up the content for you on import. It's worth the wait. It saves hours of time, and connects all the amgic together so things work properly.

2) Drag a Male or Female Character into a scene. He or she should be textured and have a component called "M3D Character Manager" on the root gameobject. Reimport the figure if the component is missing or the figures are not textured.

3) Test the blendshapes by toggling down the blendshapes menu on the character manager, and use sliders to make something you like.

4) Hair, Props, and Clothing are called "costume items" in the Morph3D Character System. To add one to your figure, drag a MCS fbx prefab into the "Show Content Packs" slot in the character manager. You will then have access to hiding and showing the costume item (hair, clothing and props) via the appropriate dropdown in the character manager GUI.




********************************************
****************** CREDITS *****************
********************************************

The "Vendor" contains shaders provided by the Unity Blacksmith project found here : https://www.assetstore.unity3d.com/en/#!/content/39941
If you haven't explored the source code for that project, you should, it's enlightening stuff.

Morph3D has had to make a few changes to those shaders and the GUIs for them so that our system can work as needed. Moving forward, we will be migrating into truly unique shaders inspired by the Volund shaders, but, for now, we've moved them into this vendor folder so credit can be given properly, and so the differences in them will not interfere with the originals, should you be using them.