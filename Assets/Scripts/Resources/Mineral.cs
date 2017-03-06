using UnityEngine;
using System.Collections;

public class Mineral : ResourceObject {

	enum MINERAL {
		copper, tin, iron, marble, gold, SIZE
	}

	MINERAL mineral;

	// Use this for initialization
	void Start () {
		type = TYPE.Mineral;
		mineral = (MINERAL) Random.Range(0, (int)MINERAL.SIZE);
	}
	
	// Update is called once per frame
	void Update () {

	}
}
