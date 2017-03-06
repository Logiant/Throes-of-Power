using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CityInfoPanel : MonoBehaviour {

	Text textbox;

	public void Start() {
		textbox = GetComponentInChildren<Text> ();
		setText ("none");
	}

	public void setText(string name) {
		textbox.text = name;
	}
}
