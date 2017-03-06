using UnityEngine;
using System.Collections;

public abstract class ResourceObject : MonoBehaviour {
	
	protected enum TYPE {
		Mineral, Fabric, Food, Luxury
	};

	protected TYPE type;
	protected float quantity;

	protected float timer = 1.5f;
	protected float cooldown = 1.5f;

}
