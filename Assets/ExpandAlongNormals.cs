using UnityEngine;
using System.Collections;
using MORPH3D.FOUNDATIONS;

[ExecuteInEditMode]
public class ExpandAlongNormals : MonoBehaviour {

	[Range(-5f, 5f)]
	public float expand_value = 3f;

	private float MToMMConversion = .001f;
	private float last_expand_value;
	
	//we edit the "shareddmesh" aka the mesh all items share on disk sooo.....
	//we need reference to a way to put things back;
	[SerializeField][HideInInspector]
	Mesh shared_mesh;
	[SerializeField][HideInInspector]
	Vector3[] source_verts;
	[SerializeField][HideInInspector]
	Vector3[] source_normals;
	
	//need modifiable vert container to make changes into
	Vector3[] vert_buffer;



	// Use this for initialization
	void Start () {
		init ();
		if(Application.isPlaying == true){
				expandVertsAlongNormals();
		}

	}

	// Update is called once per frame
	void Update () {
		
		#if UNITY_EDITOR
		if (vert_buffer == null || source_verts == null) {
			init ();
		}
		#endif
		if(Application.isPlaying == true){
			if (expand_value != last_expand_value) {
				expandVertsAlongNormals();
			}
		}

		
	}

	


	void init(){

		//always have defaults
		last_expand_value = expand_value;
		if (source_verts == null || source_normals == null) {
			//make a backup so you can manipulate the mesh without fear
			createBackup ();
		}

		if (vert_buffer == null) {
			//create swap buffer
			Mesh mesh = GetComponent<SkinnedMeshRenderer> ().sharedMesh;
			vert_buffer = new Vector3[mesh.vertices.Length];
		}

		shared_mesh = GetComponent<CoreMesh>().GetRuntimeMesh();//our equivelent to the mesh instance - changes at runtime do not persist at applciation end
	}
	

	void createBackup(){

		SkinnedMeshRenderer metal_skinned_mesh_renderer = GetComponent<SkinnedMeshRenderer>();
		if(source_verts == null)
			source_verts = (Vector3[])metal_skinned_mesh_renderer.sharedMesh.vertices.Clone ();
		if(source_normals == null)
			source_normals = (Vector3[])metal_skinned_mesh_renderer.sharedMesh.normals.Clone ();

	}
	
	void restoreSkinnedMeshBackup(){
		shared_mesh.vertices = source_verts;
	}
	
	

	
	void expandVertsAlongNormals(){
		Debug.Log("EXPANDING");
		float expand_amount = expand_value * MToMMConversion;

		int i = 0;
		for(;i<source_verts.Length;i++){
			vert_buffer[i] = source_verts[i] + (source_normals[i] * expand_amount);
		}
		
		//these probably get shoved into a hardware buffer on the gpu or something
		shared_mesh.vertices = vert_buffer;
		last_expand_value = expand_value;
	}
	
	//on IOS you must tick the "exit on quit" or use the on pause...
	//http://docs.unity3d.com/ScriptReference/MonoBehaviour.OnApplicationQuit.html
	void OnApplicationQuit(){
		restoreSkinnedMeshBackup ();
	}
	
	
}
