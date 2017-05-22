using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

	[Header ("GameObject Arrays")]
	public GameObject[] students;
	public GameObject[] walls;

	[Header ("Scene Variables")]
	public Text studentText;
	public Text timeText;
	public Text endText;
	public Text startText;
	public Text instructText;

	public Slider playerShoot;
	public Camera mainCam;
	public GameObject player;

	public Button startBut;
	public Button cusBut;
	public Button doneBut;

	public GameObject in1;
	public GameObject in2;
	public GameObject in3;
	public GameObject restartBut;
	public GameObject exitBut;

	[Header ("Gameplay Variables")]
	public int firstWave;
	public int secondWave;
	public int waveNumber = 0;
	public int enemyNumber;

	private float timeCount = 0;

	void Awake() {
		studentText.enabled = false;
		timeText.enabled = false;
		endText.enabled = false;
		startText.enabled = true;
		instructText.enabled = true;
		playerShoot.gameObject.SetActive(false);
		player.SetActive (false);
		mainCam.gameObject.SetActive (true);
		startBut.gameObject.SetActive (true);
		cusBut.gameObject.SetActive (true);
		doneBut.gameObject.SetActive (false);
		in1.SetActive (false);
		in2.SetActive (false);
		in3.SetActive (false);
		restartBut.SetActive (false);
		Time.timeScale = 0;
	}

	void Start(){
		Cursor.lockState = CursorLockMode.Confined;
		Cursor.visible = true;
	}

	void Update(){
		if (Student.noRed == firstWave && waveNumber == 1) {
			SpawnOuterStudents (secondWave);
			DropWalls ();
			waveNumber++;
		} else if (Student.noRed == (secondWave*2 + firstWave) && waveNumber == 2) {
			GameEnd ();
		}

		studentText.text = "Red: " + Student.noRed + "\nGreen: " + Student.noGreen;
		timeCount += Time.deltaTime;
		timeText.text = (Mathf.RoundToInt(timeCount)).ToString();
	}

	public void GameStart (){
		waveNumber++;
		/*firstWave = wave1;
		secondWave = wave2;
		enemyNumber = enemyNo;*/
		SpawnStudents (firstWave);

		studentText.enabled = true;
		timeText.enabled = true;
		startText.enabled = false;
		instructText.enabled = false;
		playerShoot.gameObject.SetActive(true);
		player.SetActive (true);
		mainCam.gameObject.SetActive (false);
		startBut.gameObject.SetActive (false);
		cusBut.gameObject.SetActive (false);
		exitBut.SetActive (false);
		Time.timeScale = 1f;
	}

	public void GameEnd (){
		studentText.enabled = false;
		timeText.enabled = false;
		endText.enabled = true;
		endText.gameObject.SetActive (true);
		playerShoot.gameObject.SetActive(false);
		player.SetActive (false);
		mainCam.gameObject.SetActive (true);
		restartBut.SetActive (true);
		exitBut.SetActive (true);

		endText.text = "You won within: \n" + (Mathf.RoundToInt(timeCount)).ToString() + " seconds";
		waveNumber++;
	}

	public void SpawnStudents (int noStudents){
		for (int i = 1; i <= noStudents; i++) {
			float xVal = Random.Range (-9f, 9f);
			float zVal = Random.Range (-10f, 9f);
			Vector3 pos = new Vector3 (xVal, 1, zVal);

			float yRot = Random.Range (0, 360f);
			Vector3 rot = new Vector3 (0, yRot, 0);
			Instantiate (students [0], pos, Quaternion.Euler(rot));
		}
	}

	public void SpawnOuterStudents (int noStudents){
		for (int i = 1; i <= noStudents; i++) {
			float xVal = Random.Range (-31f, 30f);
			float zVal = Random.Range (12f, 28f);
			Vector3 pos = new Vector3 (xVal, 1, zVal);

			float yRot = Random.Range (0, 360f);
			Vector3 rot = new Vector3 (0, yRot, 0);

			if (i <= noStudents - enemyNumber) {
				Instantiate (students [0], pos, Quaternion.Euler (rot));
			} else if (i >= noStudents - enemyNumber) {
				GameObject greener = Instantiate (students [0], pos, Quaternion.Euler (rot)) as GameObject;
				Student gScript = greener.GetComponent<Student> ();
				gScript.Infection (2);
			}
		}

		for (int i = 1; i <= noStudents; i++) {
			float xVal = Random.Range (-31f, 30f);
			float zVal = Random.Range (-31f, -13f);
			Vector3 pos = new Vector3 (xVal, 1, zVal);

			float yRot = Random.Range (0, 360f);
			Vector3 rot = new Vector3 (0, yRot, 0);

			if (i <= noStudents - enemyNumber) {
				Instantiate (students [0], pos, Quaternion.Euler (rot));
			} else if (i >= noStudents - enemyNumber) {
				GameObject greener = Instantiate (students [0], pos, Quaternion.Euler (rot)) as GameObject;
				Student gScript = greener.GetComponent<Student> ();
				gScript.Infection (2);
			}
		}
	}

	public void DropWalls(){
		for (int i = 0; i <= walls.Length - 1; i++) {
			walls [i].SetActive (false);
		}
	}

	public void CustomSetUp(){
		startBut.gameObject.SetActive (false);
		cusBut.gameObject.SetActive (false);
		doneBut.gameObject.SetActive (true);
		in1.SetActive (true);
		in2.SetActive (true);
		in3.SetActive (true);
	}

	public void ExitCustom(){
		startBut.gameObject.SetActive (true);
		cusBut.gameObject.SetActive (true);
		doneBut.gameObject.SetActive (false);
		in1.SetActive (false);
		in2.SetActive (false);
		in3.SetActive (false);
	}

	public void SetWave1(string userInput){
		firstWave = int.Parse(userInput);
	}

	public void SetWave2(string userInput){
		secondWave = int.Parse(userInput);
	}

	public void SetEnemies(string userInput){
		enemyNumber = int.Parse(userInput);
	}

	public void RestartLevel (){
		Student.noRed = 0;
		Student.noGreen = 0;
		Student.totalStudents = 0;
		SceneManager.LoadScene (0, LoadSceneMode.Single);
	}

	public void ExitGame(){
		Application.Quit ();
	}
}
