using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour {
	public Color32 Revealed_State;

	public void Flip (bool Player_Chosen) {
			if (this.GetComponent<UnityEngine.UI.RawImage>().color == new Color32(255,255,255,255)) {
				this.GetComponent<UnityEngine.UI.RawImage>().color = Revealed_State;
				GameObject.Find("Canvas").GetComponent<GameMaster>().Add_To_Active(this.gameObject);
			}else {
				this.GetComponent<UnityEngine.UI.RawImage>().color = new Color32(255,255,255,255);
				GameObject.Find("Canvas").GetComponent<GameMaster>().Remove_From_Active(this.gameObject);
			}
			if (Player_Chosen == true) {
				GameObject.Find("Canvas").GetComponent<GameMaster>().Moves_Made++;
			}
	}
}
