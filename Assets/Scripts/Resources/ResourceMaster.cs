using UnityEngine;
using System.Collections;

public class ResourceMaster : MonoBehaviour {

	//terrain parent
	TerrainRender terrain;

	// types of resources: mineral, fabric, food, luxury
	//prefabs for resources:
	public GameObject mineralPrefab;
	public GameObject fabricPrefab;
	public GameObject foodPrefab;
	public GameObject luxuryPrefab;


	// Awake is called before start - use it to set up "this" object
	void Awake () {

	}

	void Start() {
		terrain = GetComponentInParent<TerrainRender> ();
		//spawn some prefabs!
		for (int i = 0; i < 3; i++) {
			// position is (1,3)  to (8,6)
			Vector3 spawnPt = new Vector3 (Random.Range(1f, 8f), 0.0f, Random.Range(3f, 6f));
			spawnPt.y = terrain.Sample (spawnPt.x, spawnPt.z);
			Instantiate (mineralPrefab, spawnPt, new Quaternion ());

			spawnPt = new Vector3 (Random.Range(1f, 8f), 0.0f, Random.Range(3f, 6f));
			spawnPt.y = terrain.Sample (spawnPt.x, spawnPt.z);
			Instantiate (fabricPrefab, spawnPt, new Quaternion ());

			spawnPt = new Vector3 (Random.Range(1f, 8f), 0.0f, Random.Range(3f, 6f));
			spawnPt.y = terrain.Sample (spawnPt.x, spawnPt.z);
			Instantiate (foodPrefab, spawnPt, new Quaternion ());

			spawnPt = new Vector3 (Random.Range(1f, 8f), 0.0f, Random.Range(3f, 6f));
			spawnPt.y = terrain.Sample (spawnPt.x, spawnPt.z);
			Instantiate (luxuryPrefab, spawnPt, new Quaternion ());
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
