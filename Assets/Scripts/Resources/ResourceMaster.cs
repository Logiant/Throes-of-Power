using UnityEngine;
using System.Collections;

public class ResourceMaster : MonoBehaviour {

	//terrain parent
	TerrainMaster terrain;

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
		terrain = GetComponentInParent<TerrainMaster> ();
		//spawn some prefabs!
		//this should use height && slope data to position the resources
		//for various resources:
		/*  
		 *  Fabric: flat/hilly
		 *  Food:   flat/hilly
		 *  Luxury:	mostly flat
		 *  Mineral:mostly steep
		 * 
		 */ 
		for (int i = 0; i < 3; i++) {
			// position is (1,3)  to (8,6)
			Vector3 spawnPt = terrain.getTileOfType(TerrainMaster.TILE_TYPES.GRASSLAND);
		//	spawnPt.y = terrain.Sample (spawnPt.x, spawnPt.z);
			Instantiate (mineralPrefab, spawnPt, new Quaternion ());

			spawnPt = terrain.getTileOfType(TerrainMaster.TILE_TYPES.GRASSLAND);
		//	spawnPt.y = terrain.Sample (spawnPt.x, spawnPt.z);
			Instantiate (fabricPrefab, spawnPt, new Quaternion ());

			spawnPt = terrain.getTileOfType(TerrainMaster.TILE_TYPES.GRASSLAND);
		//	spawnPt.y = terrain.Sample (spawnPt.x, spawnPt.z);
			Instantiate (foodPrefab, spawnPt, new Quaternion ());

			spawnPt = terrain.getTileOfType(TerrainMaster.TILE_TYPES.GRASSLAND);
		//	spawnPt.y = terrain.Sample (spawnPt.x, spawnPt.z);
			Instantiate (luxuryPrefab, spawnPt, new Quaternion ());
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
