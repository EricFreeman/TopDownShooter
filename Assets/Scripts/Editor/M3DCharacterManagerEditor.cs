using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using MORPH3D.COSTUMING;
using MORPH3D;

namespace MORPH3D.EDITORS
{
	[CustomEditor (typeof(M3DCharacterManager))]
	public class M3DCharacterManagerEditor : Editor
	{
		protected M3DCharacterManager data = null;

		protected M3DBlendshapeNames names = null;
		protected string[] namesList = null;
		
		protected bool showAllBlendShapes = false;
		protected string selectedBlendShape = "";
		protected bool showAllBlendshapesGroups = false;
		protected bool[] showBlendshapeGroups = null;
		//protected int selectedBlendshapeName = 0;
		protected string[] selectedBlendshapeNames = null;

		protected bool showContentPacks = false;

		protected bool showAllClothing = false;
		protected bool showAllClothingGroups = false;
		protected bool[] showClothingGroups = null;
		protected int selectedClothingName = 0;
	
		protected bool showAttachmentPoints = false;
		protected bool[] showAttachmentPointsGroups = null;
		//protected int[] selectedProps = null;
		protected string[] selectedPropsNames = null;

		//protected GameObject selectedNewAttachmentPoint = null;
		protected string selectedNewAttachmentPointName = "";

		protected bool showHair = false;

		public override void OnInspectorGUI()
		{
			#region just_stuff
			serializedObject.Update ();
			if(data == null)
				data = (M3DCharacterManager)target;
			if (names == null){
//				Debug.Log("NAMES SCIRPTZABLE OBJECT NULL IN EDITOR");
				names = (M3DBlendshapeNames)Resources.Load ("M3D_BlendshapesNames");
			}
			#endregion just_stuff

			
			#region LOD
			float lod;
			lod = EditorGUILayout.Slider("LOD", data.currentLODLevel, 0, 1);
			if(lod != data.currentLODLevel)
			{
				Undo.RecordObject(data, "Change LOD");
				data.setCharacterLODLevel(lod);
				EditorUtility.SetDirty(data);
			}
			EditorGUILayout.Space();
			#endregion LOD

			#region blendshapes
			List<MORPH3D.FOUNDATIONS.CoreBlendshape> shapes = data.GetAllBlendshapes();
			if(shapes.Count  == 0){//this check houldnt be needed. it's now included in the bendshape model itself
				Debug.Log("NO BLEnDSHAPES VIA EDITOR");
				data.InitBlendshapeModel();
				shapes = data.GetAllBlendshapes();
			}


			showAllBlendShapes = EditorGUILayout.Foldout (showAllBlendShapes, "All Blendshapes");
			if(showAllBlendShapes)
			{
				EditorGUI.indentLevel++;

				GUILayout.BeginHorizontal();
				selectedBlendShape = GUILayout.TextField(selectedBlendShape, GUI.skin.FindStyle("ToolbarSeachTextField"));
				if (GUILayout.Button("", GUI.skin.FindStyle("ToolbarSeachCancelButton")))
				{
					selectedBlendShape = "";
					GUI.FocusControl(null);
				}
				GUILayout.EndHorizontal();
				EditorGUILayout.Space();

				foreach(MORPH3D.FOUNDATIONS.CoreBlendshape shape in shapes)
				{
					if(selectedBlendShape != "" && names.GetLabelFromDisplayName(shape.displayName).IndexOf(selectedBlendShape, System.StringComparison.OrdinalIgnoreCase) < 0)
						continue;

					bool tempLock;
					float temp = DisplayBlendShape(shape, out tempLock);
					
					if(tempLock != shape.isLocked)
					{
						Undo.RecordObject(data, "Lock Blendshape");
						if(tempLock)
							data.LockBlendshape(shape.displayName);
						else
							data.UnlockBlendshape(shape.displayName);
						EditorUtility.SetDirty(data);
					}
					if(temp != shape.currentValue)
					{
						Undo.RecordObject(data, "Change Blendshape");
						data.SetBlendshapeValue(shape.displayName, temp);
						EditorUtility.SetDirty(data);
					}
				}
				EditorGUI.indentLevel--;
			}

			showAllBlendshapesGroups = EditorGUILayout.Foldout (showAllBlendshapesGroups, "Blendshape Groups");
			if(showAllBlendshapesGroups)
			{
				namesList = names.GetAllNames();
				List<MORPH3D.FOUNDATIONS.CoreBlendshapeGroup> groups = data.GetAllBlendshapeGroups();
				if(showBlendshapeGroups == null || showBlendshapeGroups.Length != groups.Count)
					showBlendshapeGroups = new bool[groups.Count];
				if(selectedBlendshapeNames == null || selectedBlendshapeNames.Length != groups.Count)
					selectedBlendshapeNames = new string[groups.Count];

				EditorGUI.indentLevel++;

				int remove = -1;
				for(int i = 0; i < groups.Count; i++)
				{
					showBlendshapeGroups[i] = EditorGUILayout.Foldout (showBlendshapeGroups[i], groups[i].groupName);
					if(showBlendshapeGroups[i])
					{
						EditorGUI.indentLevel++;
						if(!groups[i].isLocked)
							groups[i].groupName = EditorGUILayout.TextField("Name", groups[i].groupName);

						if(groups[i].bsCount > 0)
						{
							float groupVal = EditorGUILayout.Slider("Group Weight", groups[i].groupValue, 0, 100);
							if(groups[i].groupValue != groupVal)
							{
								Undo.RecordObject(data, "Change Group Value");
								data.SetGroupValue(groups[i].groupName, groupVal);
								EditorUtility.SetDirty(data);
							}
							EditorGUILayout.Space();
						}

						List<MORPH3D.FOUNDATIONS.CoreBlendshape> groupShapes = groups[i].GetAllBlendshapes();
						int deleteFromGroup = -1;
						for(int x = 0; x < groupShapes.Count; x++)
						{
							bool tempLock;
							bool delete;
							float temp = (groups[i].isLocked) ? DisplayBlendShape(groupShapes[x], out tempLock) : DisplayBlendShape(groupShapes[x], out tempLock, out delete);

							if(delete)
								deleteFromGroup = x;

							if(tempLock != groupShapes[x].isLocked)
							{
								Undo.RecordObject(data, "Lock Blendshape");
								if(tempLock)
									data.LockBlendshape(groupShapes[x].displayName);
								else
									data.UnlockBlendshape(groupShapes[x].displayName);
								EditorUtility.SetDirty(data);
							}
							if(temp != groupShapes[x].currentValue)
							{
								Undo.RecordObject(data, "Change Blendshape");
								data.SetBlendshapeValue(groupShapes[x].displayName, temp);
								EditorUtility.SetDirty(data);
							}
						}
						if(deleteFromGroup >= 0)
						{	
							Undo.RecordObject(data, "Delete Blendshape from Group");
							groupShapes.RemoveAt(deleteFromGroup);
							EditorUtility.SetDirty(data);
						}

						if(!groups[i].isLocked)
						{
							if(namesList.Length > 0)
							{
								EditorGUILayout.Space();
								EditorGUILayout.BeginHorizontal();								
								EditorGUILayout.LabelField("Add Blendshape:", GUILayout.Width(150));
								EditorGUILayout.LabelField(selectedBlendshapeNames[i], GUILayout.Width(150));
								MORPH3D.FOUNDATIONS.CoreBlendshape tempBlendshape = data.GetBlendshapeByName(names.GetDisplayName(selectedBlendshapeNames[i]));
								if(selectedBlendshapeNames[i] != "" && selectedBlendshapeNames[i] != null && tempBlendshape == null)
									selectedBlendshapeNames[i] = "";
								if(GUILayout.Button("Search"))
								{
									int num = i;
									SearchableWindow.Init(delegate(string newName) { selectedBlendshapeNames[num] = newName; }, namesList);
								}
								if(selectedBlendshapeNames[i] != "" && selectedBlendshapeNames[i] != null && tempBlendshape != null && GUILayout.Button("Add"))
								{
									Undo.RecordObject(data, "Add Blendshape to Group");
									groups[i].AddBlendshapeToGroup(tempBlendshape);
									EditorUtility.SetDirty(data);
									selectedBlendshapeNames[i] = "";
								}
								GUILayout.FlexibleSpace();
								EditorGUILayout.EndHorizontal();
							}
							
							/*
							EditorGUILayout.Space();
							EditorGUILayout.BeginHorizontal();
							GUILayout.FlexibleSpace();
							selectedBlendshapeName = EditorGUILayout.Popup(selectedBlendshapeName, namesList);
							if(GUILayout.Button("Add Shape", GUILayout.Width(100)))
							{
								MORPH3D.FOUNDATIONS.CoreBlendshape shape = data.GetBlendshapeByName(names.GetDisplayName(namesList[selectedBlendshapeName]));
								if(shape != null && !groups[i].ContainsBlendshape(shape))
								{
									Undo.RecordObject(data, "Add Blendshape to Group");
									groups[i].AddBlendshapeToGroup(shape);
									EditorUtility.SetDirty(data);
									selectedBlendshapeName = 0;
								}
							}
							GUILayout.FlexibleSpace();
							EditorGUILayout.EndHorizontal();
							*/
							
							EditorGUILayout.Space();
							EditorGUILayout.BeginHorizontal();
							GUILayout.FlexibleSpace();
							if(GUILayout.Button("Delete Group", GUILayout.Width(100)))
								remove = i;
							GUILayout.FlexibleSpace();
							EditorGUILayout.EndHorizontal();	
						}
						EditorGUI.indentLevel--;
					}
				}
				if(remove >= 0)
				{
					Undo.RecordObject(data, "Remove Blendshape Group");
					groups.RemoveAt(remove);
					EditorUtility.SetDirty(data);
				}
				EditorGUILayout.Space();
				EditorGUILayout.BeginHorizontal();
				GUILayout.FlexibleSpace();
				if(GUILayout.Button("Add Group", GUILayout.Width(100)))
				{
					Undo.RecordObject(data, "Create Blendshape Group");
					data.AddBlendShapeGroup("New Group");
					EditorUtility.SetDirty(data);
				}
				GUILayout.FlexibleSpace();
				EditorGUILayout.EndHorizontal();
				EditorGUI.indentLevel--;
			}
			EditorGUILayout.Space();
			#endregion blendshapes

			#region contentPacks
			showContentPacks = EditorGUILayout.Foldout (showContentPacks, "Show ContentPacks");
			if(showContentPacks)
			{
				EditorGUI.indentLevel++;
				ContentPack[] allPacks = data.GetAllContentPacks();
				for(int i = 0; i < allPacks.Length; i++)
				{
					EditorGUILayout.BeginHorizontal();
					EditorGUILayout.LabelField(allPacks[i].name);
					if(GUILayout.Button("X"))
					{
						Undo.RecordObject(data, "Remove Bundle");
						data.UnloadContentPackFromFigure(allPacks[i]);
						data.RemoveContentPackFromModel(allPacks[i].RootGameObject, true);
						EditorUtility.SetDirty(data);
					}
					EditorGUILayout.EndHorizontal();
				}
				GameObject tempPack;
				tempPack = (GameObject)EditorGUILayout.ObjectField("New", null, typeof(GameObject), false);
				if(tempPack != null)
				{
					ContentPack packScript = new ContentPack(tempPack);
					Undo.RecordObject(data, "Add Bundle");
					data.AddContentPackToModel(packScript);
					ContentPack[] all_packs = data.GetAllContentPacks ();
					foreach (ContentPack pack in all_packs)
						data.LoadContentPackToFigure (pack);
					EditorUtility.SetDirty(data);
				}
				EditorGUI.indentLevel--;
			}
			EditorGUILayout.Space();
			#endregion contentPacks
			
			#region hair
			showHair = EditorGUILayout.Foldout (showHair, "Hair");
			if(showHair)
			{
				EditorGUI.indentLevel++;
				List<MORPH3D.COSTUMING.CIhair> allHair = data.GetAllHairItems();
				foreach(MORPH3D.COSTUMING.CIhair mesh in allHair)
				{
					if(DisplayHair(mesh))
					{
						Undo.RecordObject(data, "Toggle Hair");
						data.SetVisibilityOnHairItem(mesh.displayName, !mesh.isVisible);
						EditorUtility.SetDirty(data);
					}
				}
				EditorGUI.indentLevel--;
			}
			EditorGUILayout.Space();
			#endregion hair

			#region clothing
			showAllClothing = EditorGUILayout.Foldout (showAllClothing, "Clothing");
			if(showAllClothing)
			{
				EditorGUI.indentLevel++;

				List<CIclothing> allClothing = null;
				allClothing = data.GetAllLoadedClothingItems();
				foreach(CIclothing mesh in allClothing)
				{
					bool tempLock;
					bool temp = DisplayClothingMesh(mesh, out tempLock);

					if(tempLock != mesh.isLocked)
					{
						Undo.RecordObject(data, "Lock Clothing");
						if(tempLock)
							data.LockClothingItem(mesh.displayName);
						else
							data.UnlockClothingItem(mesh.displayName);
						EditorUtility.SetDirty(data);
					}

					if(temp)
					{
						Undo.RecordObject(data, "Toggle Clothing");
						data.SetClothingVisibility(mesh.displayName, !mesh.isVisible);
						EditorUtility.SetDirty(data);
					}
				}
				EditorGUI.indentLevel--;
			}
			EditorGUILayout.Space();
			#endregion clothing

			#region props
			CIattachmentPoint[] attachmentPoints = data.GetAllAttachmentPoints();
//			Debug.Log("AP LENGTH:"+attachmentPoints.Length);
			if(showAttachmentPointsGroups == null || showAttachmentPointsGroups.Length != attachmentPoints.Length)
				showAttachmentPointsGroups = new bool[attachmentPoints.Length];
			/*
			if(selectedProps == null || selectedProps.Length != attachmentPoints.Length)
				selectedProps = new int[attachmentPoints.Length];
			*/	
			if(selectedPropsNames == null || selectedPropsNames.Length != attachmentPoints.Length)
				selectedPropsNames = new string[attachmentPoints.Length];
			List<CIprop> props = data.GetAllLoadedProps();
			string[] propsNames = new string[]{};
			if(props != null){
				propsNames = new string[props.Count];
			}
			for(int i = 0; i < propsNames.Length; i++)
				propsNames[i] = props[i].displayName;

			showAttachmentPoints = EditorGUILayout.Foldout (showAttachmentPoints, "Attachment Points");
			if(showAttachmentPoints)
			{
				int deleteAttachment = -1;
				EditorGUI.indentLevel++;
				for(int i = 0; i < attachmentPoints.Length; i++)
				{ 
					EditorGUILayout.BeginHorizontal();
					showAttachmentPointsGroups[i] = EditorGUILayout.Foldout (showAttachmentPointsGroups[i], attachmentPoints[i].attachmentPointName);
					GUILayout.FlexibleSpace();
					if(GUILayout.Button("X", GUILayout.Width(45)))
						deleteAttachment = i;
					GUILayout.FlexibleSpace();
					EditorGUILayout.EndHorizontal();
					if(showAttachmentPointsGroups[i])
					{
						EditorGUI.indentLevel++;
						CIprop[] activeProps = attachmentPoints[i].getAttachmentArray();
						int destroyProp = -1;
						for(int x = 0; x < activeProps.Length; x++)
						{
							if(DisplayProp(activeProps[x]))
								destroyProp = x;
						}
						if(destroyProp >= 0)
						{
							Undo.RecordObject(data, "Destroy Prop");
							data.DetachPropFromAttachmentPoint(activeProps[destroyProp].displayName, attachmentPoints[i].attachmentPointName);
							EditorUtility.SetDirty(data);
						}
//						Debug.Log("GF");
						if(propsNames.Length > 0)
						{
//							Debug.Log("FDFG");
							EditorGUILayout.Space();
							EditorGUILayout.BeginHorizontal();
							EditorGUILayout.LabelField("Add Prop:", GUILayout.Width(150));
							EditorGUILayout.LabelField(selectedPropsNames[i], GUILayout.Width(150));
							if(selectedPropsNames[i] != "" && selectedPropsNames[i] != null && data.GetLoadedPropByName(selectedPropsNames[i]) == null)
								selectedPropsNames[i] = "";
							if(GUILayout.Button("Search"))
							{
								int num = i;
								SearchableWindow.Init(delegate(string newName) { selectedPropsNames[num] = newName; }, propsNames);
							}
							if(selectedPropsNames[i] != "" && selectedPropsNames[i] != null && GUILayout.Button("Add"))
							{
								Undo.RecordObject(data, "Attach Prop");
								data.AttachPropToAttachmentPoint(selectedPropsNames[i], attachmentPoints[i].attachmentPointName);
								EditorUtility.SetDirty(data);
								selectedPropsNames[i] = "";
							}
							GUILayout.FlexibleSpace();
							EditorGUILayout.EndHorizontal();
							/*
							EditorGUILayout.Space();
							EditorGUILayout.BeginHorizontal();
							selectedProps[i] = EditorGUILayout.Popup (selectedProps[i], propsNames, GUILayout.Width(150));
							if(GUILayout.Button("Add"))
							{
								Undo.RecordObject(data, "Attach Prop");
								data.AttachPropToAttachmentPoint(propsNames[selectedProps[i]], attachmentPoints[i].attachmentPointName);
								EditorUtility.SetDirty(data);
								selectedProps[i] = 0;
							}
							GUILayout.FlexibleSpace();
							EditorGUILayout.EndHorizontal();
							*/
						}
						EditorGUILayout.Space();
						EditorGUI.indentLevel--;
					}
				}

				if(deleteAttachment >= 0)
				{
					Undo.RecordObject(attachmentPoints[deleteAttachment], "Delete Attachment Point");
					data.DeleteAttachmentPoint(attachmentPoints[deleteAttachment].attachmentPointName);
				}

				EditorGUILayout.Space();
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.LabelField("New Point:", GUILayout.Width(150));
				EditorGUILayout.LabelField(selectedNewAttachmentPointName, GUILayout.Width(150));
				Transform tempBone = data.GetBoneByName (selectedNewAttachmentPointName);
				if(selectedNewAttachmentPointName != "" && selectedNewAttachmentPointName != null && tempBone == null)
					selectedNewAttachmentPointName = "";
				if(GUILayout.Button("Search"))
				{
					SearchableWindow.Init(delegate(string newName) { selectedNewAttachmentPointName = newName; }, data.GetAllBonesNames());
				}
				if(selectedNewAttachmentPointName != "" && selectedNewAttachmentPointName != null && tempBone != null && GUILayout.Button("Add"))
				{
					Undo.RecordObject(tempBone.gameObject, "New Attachment Point");
					data.CreateAttachmentPointOnBone(selectedNewAttachmentPointName);
					selectedNewAttachmentPointName = "";
				}
				GUILayout.FlexibleSpace();
				EditorGUILayout.EndHorizontal();

				/*
				EditorGUILayout.Space();
				EditorGUILayout.BeginHorizontal();
				selectedNewAttachmentPointName = EditorGUILayout.TextField("New Point Bone Name", selectedNewAttachmentPointName);
				if(GUILayout.Button("Add") && selectedNewAttachmentPointName != "")
				{
					Transform bone = data.boneService.getBoneByName (selectedNewAttachmentPointName);
					if(bone != null)
					{
						Undo.RecordObject(bone.gameObject, "New Attachment Point");
						data.CreateAttachmentPointOnBone(selectedNewAttachmentPointName);
					}
					selectedNewAttachmentPointName = "";
				}
				EditorGUILayout.EndHorizontal();

				EditorGUILayout.BeginHorizontal();
				selectedNewAttachmentPoint = (GameObject)EditorGUILayout.ObjectField("New Attachemnt Point", selectedNewAttachmentPoint, typeof(GameObject), true);
				if(selectedNewAttachmentPoint != null && !selectedNewAttachmentPoint.activeInHierarchy)
					selectedNewAttachmentPoint = null;
				if(GUILayout.Button("Add") && selectedNewAttachmentPoint != null)
				{
					if(selectedNewAttachmentPoint.GetComponent<CIattachmentPoint>() == null)
					{
						Undo.RecordObject(selectedNewAttachmentPoint, "New Attachment Point");
						data.CreateAttachmentPointFromGameObject(selectedNewAttachmentPoint);
					}
					selectedNewAttachmentPoint = null;
				}
				EditorGUILayout.EndHorizontal();
				*/

				EditorGUI.indentLevel--;
			}
			EditorGUILayout.Space();
			#endregion props
		}

		#region blendshapes_display
		protected float DisplayBlendShape(MORPH3D.FOUNDATIONS.CoreBlendshape shape, out bool lockShape)
		{
			float result;
			EditorGUILayout.BeginHorizontal();



			result = EditorGUILayout.Slider(GetLabelToDisplay(shape), shape.currentValue, 0f, 100f);
			lockShape = EditorGUILayout.Toggle(shape.isLocked);
			
			EditorGUILayout.EndHorizontal();
			
			return result;
		}

		protected string GetLabelToDisplay(MORPH3D.FOUNDATIONS.CoreBlendshape shape){
			string name_to_display = names.GetLabelFromDisplayName(shape.displayName);
			if (string.IsNullOrEmpty (name_to_display) == true)
				name_to_display = shape.displayName;
			return name_to_display;
		}

		protected float DisplayBlendShape(MORPH3D.FOUNDATIONS.CoreBlendshape shape, out bool lockShape, out bool delete)
		{
			float result;
			EditorGUILayout.BeginHorizontal();

			string showName = GetLabelToDisplay (shape);//names.GetLabelFromDisplayName (shape.displayName);
			result = EditorGUILayout.Slider((string.IsNullOrEmpty(showName)) ? shape.displayName : showName, shape.currentValue, 0f, 100f);
			lockShape = EditorGUILayout.Toggle(shape.isLocked);
			delete = GUILayout.Button ("X");
			
			EditorGUILayout.EndHorizontal();
			
			return result;
		}
		#endregion blendshapes_display

		#region clothing_display
		protected bool DisplayClothingMesh(MORPH3D.COSTUMING.CIclothing mesh, out bool lockItem)
		{
			bool result;
			EditorGUILayout.BeginHorizontal();

			EditorGUILayout.LabelField (mesh.displayName, GUILayout.Width(150));
			if(mesh.isVisible)
				GUILayout.Space (60);
			result = GUILayout.Button ((mesh.isVisible) ? "Disable" : "Enable", GUILayout.Width(60));
			if(!mesh.isVisible)
				GUILayout.Space (60);
			if (mesh.isVisible)
				lockItem = EditorGUILayout.Toggle (mesh.isLocked);
			else
				lockItem = mesh.isLocked;

			EditorGUILayout.EndHorizontal();

			return result;
		}

		#endregion clothing_display

		#region props_display
		protected bool DisplayProp(MORPH3D.COSTUMING.CIprop prop)
		{
			bool result;
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField (prop.displayName, GUILayout.Width(180));
			GUILayout.Space (60);
			result = GUILayout.Button ("Disable", GUILayout.Width(60));
			EditorGUILayout.EndHorizontal();
			
			return result;
		}
		#endregion props_display
		
		#region hair_display
		protected bool DisplayHair(MORPH3D.COSTUMING.CIhair mesh)
		{
			bool result;
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField (mesh.displayName, GUILayout.Width(150));
			if(mesh.isVisible)
				GUILayout.Space (60);
			result = GUILayout.Button ((mesh.isVisible) ? "Disable" : "Enable", GUILayout.Width(60));
			if(!mesh.isVisible)
				GUILayout.Space (60);
			EditorGUILayout.EndHorizontal();
			
			return result;
		}
		#endregion hair_display
	}
}
