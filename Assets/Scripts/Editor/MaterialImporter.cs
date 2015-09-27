using UnityEngine; 
using UnityEditor; 
using System.Collections;
using System.Collections.Generic;
using System;
using System.Reflection;

using System.IO;

using System.Text.RegularExpressions;

using MORPH3D.CONSTANTS;
using MORPH3D.FOUNDATIONS;
using MORPH3D.COSTUMING;
using MORPH3D.UTILITIES;
using MORPH3D.CORESERVICES;
using MORPH3D;

public class MaterialImporter : AssetPostprocessor {
	
	
	#region MATERIALS
	Material OnAssignMaterialModel (Material material, Renderer renderer ) {
		//			Debug.Log ("ASSGINING MAT");
		string path = StripFileName (assetPath);
		
		
		if(assetPath.Contains("/MORPH3D/")){
			
			string geometry_name = renderer.name;
			if (geometry_name.Contains (".")){
				
				geometry_name = geometry_name.Substring (0, geometry_name.LastIndexOf ("."));//all lods share material
				
				//exception for hair
				if(renderer.name.Contains("Shape_opaque")){
					geometry_name += "_opaque";
				}
				if(renderer.name.Contains("Shape_feathered")){
					geometry_name += "_feathered";
				}
				
			}
			
			string go_name = GetFilename(assetPath);
			//				Debug.Log(go_name);
			
			Material existing_material = LoadExistingMaterial(material.name, geometry_name, go_name);
			if(existing_material == null){
				return CreatePersistentMaterial(geometry_name, path, material);
			}else{
				return existing_material;
			}
			
			
		}else{


			//FOR NON MORPH3D ASSETS
			var materialPath = path +"/Materials/"+ material.name + ".mat";
			
			if(AssetDatabase.IsValidFolder(path+"/Materials") == false)
				AssetDatabase.CreateFolder(path, "Materials");
			
			if (AssetDatabase.LoadAssetAtPath<Material>(materialPath))
				return AssetDatabase.LoadAssetAtPath<Material>(materialPath) as Material;
			
			material.shader = Shader.Find("Standard");
			AssetDatabase.CreateAsset(material, materialPath);


			
		}
		return material;
	}
	#endregion



	
	Material CreatePersistentMaterial(string geometry_name, string current_path, Material material){
		//			Debug.Log ("CREATIN AMTERIAL");
		string mat_ext = ".mat";
		string material_folder = "/M3DMaterials";
		if (AssetDatabase.IsValidFolder (current_path + material_folder) == false)
			AssetDatabase.CreateFolder (current_path, "M3DMaterials");
		string material_path = current_path + material_folder + "/" +geometry_name + mat_ext;
		material.shader = Shader.Find("Standard");
		AssetDatabase.CreateAsset(material, material_path);
		return material;
	}
	
	Material LoadExistingMaterial(string mat_name, string geometry_name, string go_name){
		Material loaded_material = null;
		string mat_ext = ".mat";
		string path = StripFileName (assetPath);
		string local_materials_folder = path + "/M3DMaterials/";
		string shared_materials_folder = "Assets/MORPH3D/SharedMaterials/";
		//local materials mat name
		//			Debug.Log("LAODING MAT FROM LOCAL Mat");
		
		loaded_material = AssetDatabase.LoadAssetAtPath<Material>(local_materials_folder + mat_name + mat_ext);
		//shared materials mat name
		if (loaded_material == null) {
			//				Debug.Log("LOADING MAT FROM Shared MAT");
			loaded_material = AssetDatabase.LoadAssetAtPath<Material> (shared_materials_folder + mat_name + mat_ext);
		}
		//local materials geometry name
		if (loaded_material == null) {
			//				Debug.Log("LOADING MAT FROM LOCAL GEOMETRY");
			loaded_material = AssetDatabase.LoadAssetAtPath <Material> (local_materials_folder + geometry_name + mat_ext);
		}
		//shared materials geometry name
		if (loaded_material == null) {
			//				Debug.Log("LOADING MAT FROM SHARED GEOMETRY");
			loaded_material = AssetDatabase.LoadAssetAtPath <Material> (shared_materials_folder + geometry_name + mat_ext);
		}
		//local materials gameobject name
		if (loaded_material == null) {
			//				Debug.Log("LOADING MAT FROM LOCAL GAMEOBJECT");
			
			loaded_material = AssetDatabase.LoadAssetAtPath <Material> (local_materials_folder + go_name + mat_ext);
		}
		//shared materials gamobejct name
		if (loaded_material == null) {
			//				Debug.Log("LOADING MAT FROM SHARED GAMEOBJECT");
			loaded_material = AssetDatabase.LoadAssetAtPath <Material> (shared_materials_folder + go_name + mat_ext);
		}
		
		
		return loaded_material;
	}
	
	



	string StripFileName(string path){
		return path.Substring (0, path.LastIndexOf ("/"));
	}
	
	
	string GetFilename(string path){
		string file_name = path.Substring (path.LastIndexOf ("/")+1);
		file_name = file_name.Substring (0, file_name.LastIndexOf ("."));
		return file_name;
	}

}

