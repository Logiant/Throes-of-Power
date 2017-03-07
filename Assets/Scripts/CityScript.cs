using UnityEngine;
using System.Collections;

public class CityScript : MonoBehaviour {

	TerrainMaster terrain;

	string cityName;

	//float harvest_radius???
	//list of child resources???

	// Use this for initialization
	void Start () {
	
		terrain = GameObject.FindGameObjectWithTag ("Terrain").GetComponent<TerrainMaster>();


		Vector3 pos = transform.position;
		pos.y = terrain.Sample (pos.x, pos.z);

		transform.position = pos;

		cityName = "Village";

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public string getName() {
		return cityName;
	}
}
