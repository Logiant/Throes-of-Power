using UnityEngine;
using System.Collections;

public class Food : ResourceObject {

	enum FOOD {
		grain, fruits, SIZE
	}

	FOOD food;

	// Use this for initialization
	void Start () {
		type = TYPE.Mineral;
		food = (FOOD) Random.Range(0, (int)FOOD.SIZE);
	}
	
	// Update is called once per frame
	void Update () {

	}
}
