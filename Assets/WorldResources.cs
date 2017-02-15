using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WorldResources : MonoBehaviour {

	public GameObject resourceNode;

	int numResources = 5;

	List<GameObject> copper;

	TerrainRender terrain;

	//resources
	/*
	 * tools/weapons (flint, copper/tin/bronze, iron)
	 * lumber (trees)
	 * cloth/fabric (flax, wool, silk, etc)
	 * paper (papyrus)
	 * ceramics (clays, porcelain)
	 * stones (sandstone, granite, marble, etc)
	 * food? -farming and wild animals
	 */


	// Use this for initialization
	void Start () {
		terrain = GetComponentInParent<TerrainRender> ();
		copper = new List<GameObject> ();

		//spawn 5 copper nodes just because we can
		for (int i = 0; i < numResources; i++) {
			Vector3 pos = new Vector3 (Random.Range (1.5f, 8.5f), 0, Random.Range (3f, 6f));
			pos.y = terrain.Sample (pos.x, pos.z);
			copper.Add ((GameObject)Instantiate (resourceNode, pos, transform.rotation));
		}

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
