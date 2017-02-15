using UnityEngine;
using System.Collections;

public class CityScript : MonoBehaviour {

	TerrainRender terrain;

	// Use this for initialization
	void Start () {
	
		terrain = GameObject.FindGameObjectWithTag ("Terrain").GetComponent<TerrainRender>();


		Vector3 pos = transform.position;
		pos.y = terrain.Sample (pos.x, pos.z);

		transform.position = pos;


	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
