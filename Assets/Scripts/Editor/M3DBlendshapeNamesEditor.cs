using UnityEngine;
using UnityEditor;
using System;
using System.Text.RegularExpressions;
using MORPH3D;


	[CustomEditor (typeof(M3DBlendshapeNames))]
	public class M3DBlendshapeNamesEditor : Editor
	{
		public override void OnInspectorGUI ()
		{
//			if (GUILayout.Button ("Reset Names")) {
//				ResetNames ();
//				return;
//			}
			EditorGUILayout.Space ();

			M3DBlendshapeNames data;
			data = (M3DBlendshapeNames)target;


			string tempName;
			foreach (M3DBlendshapeNames.NamePair namePair in data.namesTable) {
				tempName = EditorGUILayout.TextField (new GUIContent (namePair.DisplayName, namePair.DazName), namePair.Label);
				if (tempName != namePair.Label) {
					Undo.RecordObject (data, "Change Name");
					namePair.Label = tempName;
				}
			}
		}

		protected void ResetNames ()
		{
			M3DBlendshapeNames data;
			data = (M3DBlendshapeNames)target;
			data.namesTable.Clear ();
			MORPH3D.M3DCharacterManager[] tempManagers = FindObjectsOfType<MORPH3D.M3DCharacterManager> ();
			foreach (MORPH3D.M3DCharacterManager tempManager in tempManagers) {
				data.AddNamesFromBaseFigure(tempManager.gameObject);//this will store the figure for future use AND add names using below procedure

//				MORPH3D.SERVICES.M3DBlendshapeModel temp = tempManager.blendshapeModel;
//				temp.LoadBlendshapesFromCharacter ();
//				foreach (MORPH3D.FOUNDATIONS.CoreBlendshape shape in temp.GetAllBlendshapes()) {
//					if (!data.HasDisplayName (shape.displayName))
//					data.namesTable.Add (new M3DBlendshapeNames.NamePair (shape.dazName, shape.displayName, MORPH3D.UTILITIES.BlendShapes.convertDisplayNameToLabel (shape.displayName)));
//				}
			}
			EditorUtility.SetDirty (data);
		}

	//moved to UTILITIES
//
//		public string convertDisplayNameToLabel (string displayName)
//		{
//
//			string prefix = displayName.Substring (0, 3);
//			string new_suffix = "";
//			switch (prefix) {
//
//			case "CTR":
//				new_suffix = "(Complete)";
//				break;
//			case "FHM":
//				new_suffix = "(Head)";
//				break;
//			case "FBM":
//				new_suffix = "(Body)";
//				break;
//			case "PHM":
//				new_suffix = "(Head)";
//				break;
//			case "PBM":
//				new_suffix = "(Body)";
//				break;
//			case "CBM":
//				new_suffix = "(Complete)";
//				break;
//			case "SCL":
//				new_suffix = "(Proportion)";
//				break;
//			case "VSM":
//				new_suffix = "(Phoneme)";
//				break;
//
//			}
//			string label = displayName;
//			if (string.IsNullOrEmpty (new_suffix) == false)
//				label = displayName.Substring (3);
//			if (displayName.StartsWith ("CTRL"))
//				label = displayName.Substring (4);
//			label = properSpacing (label);
//			if (displayName.StartsWith ("VSM"))
//				label = VSMformat (label);
//			//return string.Format("{0} {1}", label, new_suffix);
//			return string.Format ("{0}", label);
//		}
//
//		protected string properSpacing (string instr)
//		{
//			var r = new Regex (@"
//                (?<=[A-Z])(?=[A-Z][a-z]) |
//                 (?<=[^A-Z])(?=[A-Z]) |
//                 (?<=[A-Za-z])(?=[^A-Za-z])", RegexOptions.IgnorePatternWhitespace);
//
//			return r.Replace (instr, " ");
//		}
//
//		protected string VSMformat (string unformattedVSM)
//		{
//			string retstr = "";
//			char tempchr = ' ';
//			if (unformattedVSM.Length == 1) {
//				tempchr = unformattedVSM [0];
//				retstr = "\"" + tempchr + Char.ToLower (tempchr) + Char.ToLower (tempchr) + "...\"";
//			}
//
//			if (unformattedVSM.Length > 1) {
//				retstr += unformattedVSM [0];
//				for (int i = 1; i < unformattedVSM.Length; i++) {
//					tempchr = Char.ToLower (unformattedVSM [i]);
//					retstr += tempchr;
//				}
//				retstr = "\"" + retstr + tempchr + "...\"";
//			}
//			return retstr;
//		}

	}
