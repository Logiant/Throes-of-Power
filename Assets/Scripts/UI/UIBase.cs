using UnityEngine;
using System.Collections;

public class UIBase : MonoBehaviour {

	GameObject selected = null;

	//child UI Objects
	CityInfoPanel cityInfo;

	// Use this for initialization
	void Start () {
		cityInfo = GetComponentInChildren<CityInfoPanel> ();
	}
	
	// Update is called once per frame
	void Update () {
	
		if (Input.GetMouseButtonUp (0)) {
			Debug.Log ("Click");
			RaycastHit hitInfo;
			if (Physics.Raycast (Camera.main.ScreenPointToRay (Input.mousePosition), out hitInfo)) {
				// what are we clicking?
				selected = hitInfo.collider.gameObject;

				if (selected.GetComponentInParent<CityScript> () != null) { //if it is a city
					cityInfo.setText (selected.GetComponentInParent<CityScript> ().getName ());
				} else {
					cityInfo.setText ("None");
				}


			}
		}

	}
}
