using UnityEngine;
using System.Collections;

public class Luxury : ResourceObject {

	enum LUXURY {
		salt, grapes, oil, SIZE
	}

	LUXURY luxury;

	// Use this for initialization
	void Start () {
		type = TYPE.Mineral;
		luxury = (LUXURY) Random.Range(0, (int)LUXURY.SIZE);
	}
	
	// Update is called once per frame
	void Update () {

	}
}
