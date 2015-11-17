using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MenuLoad : MonoBehaviour {
	
	public void LoadScene(int level) {
		Application.LoadLevel(level);
		Destroy (GameObject.Find("NetManager"));
	}
}