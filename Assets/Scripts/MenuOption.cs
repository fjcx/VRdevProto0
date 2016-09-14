using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MenuOption : MonoBehaviour {

	public Text menuText;
	public Image menuImage; 
	public string mode;



	// Use this for initialization
	void Start () {
		//menuText.text = "MenuItem";

		Renderer rend = GetComponent<Renderer>();
		rend.material.SetColor ("_SpecColor", Color.gray);
	}

	// Update is called once per frame
	void Update () {
	
	}
}
