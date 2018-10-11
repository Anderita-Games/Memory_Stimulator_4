using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMaster : MonoBehaviour {
	public GameObject Cell;
	public int Size = 4;
	int Border = 5;
	public int Cells_Active = 0;
	public GameObject[] Active;
	int Cells_Matched = 0;
	public int Moves_Made = 0;
	Color32[] Colors;
	int[] Colors_Used;
	public GameObject Moves_Made_Text;
	public GameObject Highscore_Text;

	void Start () {
		Active = new GameObject[2];
		Colors = new Color32[] {
			new Color32(0,180,229,255), new Color32(17,136,102,255),
			new Color32(127,63,81,255), new Color32(153,34,68,255),
			new Color32(136,51,17,255), new Color32(61,73,76,255),
			new Color32(242,121,155,255), new Color32(170,221,204,255),
		};
		Colors_Used = new int[Colors.Length];

		float Width = Screen.width / Size - 5;
		for (int a = Size; a > 0; a--) {
			for (int b = 0; b < Size; b++) {
				GameObject Clone = Instantiate(Cell);
				Clone.transform.SetParent(GameObject.Find("Canvas").transform);
				Clone.name = "Cell";
				Clone.GetComponent<RectTransform>().sizeDelta = new Vector2(Width, Width);
				Clone.GetComponent<RectTransform>().localPosition = new Vector3((Width + Border) * (b + .5f) - Screen.width / 2f,(0 - (Width + Border) * (a - .5f - Size / 2f)), 0);
				while (Clone.GetComponent<Cell>().Revealed_State.Equals(new Color32(0,0,0,255))) { //TODO: Dont make random
					int Rando = Random.Range(0, Colors_Used.Length);
					if (Colors_Used[Rando] < 2) {
						Clone.GetComponent<Cell>().Revealed_State = Colors[Rando];
						Colors_Used[Rando] += 1;
					}
				}
			}
		}

		Moves_Made_Text.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width, (Screen.height - Width * Size) / 2f - 10);
		Moves_Made_Text.GetComponent<RectTransform>().localPosition = new Vector3(0, Screen.height / 2f - Moves_Made_Text.GetComponent<RectTransform>().sizeDelta.y / 2f);
		Highscore_Text.GetComponent<RectTransform>().sizeDelta = Moves_Made_Text.GetComponent<RectTransform>().sizeDelta;
		Highscore_Text.GetComponent<RectTransform>().localPosition = new Vector3(0, Moves_Made_Text.GetComponent<RectTransform>().sizeDelta.y / 2f - Screen.height / 2f);
		if (PlayerPrefs.GetInt("Highscore") != 0) {
			Highscore_Text.GetComponent<UnityEngine.UI.Text>().text = "Highscore: " + PlayerPrefs.GetInt("Highscore");
		}else {
			Highscore_Text.GetComponent<UnityEngine.UI.Text>().text = "Highscore: None";
		}
	}

	void Update () {
		Moves_Made_Text.GetComponent<UnityEngine.UI.Text>().text = "Moves Made: " + Moves_Made;
		if (Cells_Matched >= Size * Size && Input.GetMouseButtonDown(0)) {
			SceneManager.LoadScene("Main");
			if (PlayerPrefs.GetInt("Highscore") == 0 || PlayerPrefs.GetInt("Highscore") > Moves_Made) {
				PlayerPrefs.SetInt("Highscore", Moves_Made);
			}
			Debug.Log("Resarting");
		}
	}

	public void Add_To_Active (GameObject Cell) { //tested with no color variation
		if (Active[0] == null) {
			Active[0] = Cell;
		}else if (Active[1] == null) {
			Active[1] = Cell;
			if (Active[0].GetComponent<UnityEngine.UI.RawImage>().color == Active[1].GetComponent<UnityEngine.UI.RawImage>().color) {
				Destroy(Active[0].GetComponent<Cell>());
				Destroy(Active[1].GetComponent<Cell>());
				Cells_Matched += 2;
				Active[0] = null;
				Active[1] = null;
			}
		}else {
			Active[0].GetComponent<Cell>().Flip(false);
			Active[1].GetComponent<Cell>().Flip(false);
			Active[0] = Cell;
			Active[1] = null;
		}
	}

	public void Remove_From_Active (GameObject Cell) {
		for (int i = 0; i < Active.Length; i++) {
			if (Active[i] == Cell) {
				Active[i] = null;
			}
		}
	}
}
