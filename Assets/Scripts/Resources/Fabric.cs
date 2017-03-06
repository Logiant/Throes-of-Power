using UnityEngine;
using System.Collections;

public class Fabric : ResourceObject {

	enum FABRIC {
		wool, flax, papyrus, linen, SIZE
	}

	FABRIC fabric;

	// Use this for initialization
	void Start () {
		type = TYPE.Mineral;
		fabric = (FABRIC) Random.Range(0, (int)FABRIC.SIZE);
	}
	
	// Update is called once per frame
	void Update () {

	}
}
