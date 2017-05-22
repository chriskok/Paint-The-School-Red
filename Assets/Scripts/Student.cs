using UnityEngine;
using System.Collections;

public class Student : MonoBehaviour {

	public GameObject[] particles;

	public static int noRed = 0;
	public static int noGreen = 0;
	public static int totalStudents = 0;

	public float speed;
	public int infected = 0;
	public int yellowFrequency = 5;

	Renderer rend;

	// Use this for initialization
	void Start () {
		rend = GetComponent<Renderer> ();
		++totalStudents;
		if (totalStudents % yellowFrequency != 0) {
			SetSpeed (1);
		} else if (totalStudents % yellowFrequency == 0) {
			SetSpeed (2);
		}
	}
	
	// Update is called once per frame
	void Update () {
		transform.position += transform.forward * speed * Time.deltaTime;
	}

	void SwitchDirection(float yRot){
		Vector3 rotationVector = transform.rotation.eulerAngles;
		rotationVector.y = yRot + Random.Range (45, 135);
		transform.rotation = Quaternion.Euler(rotationVector);
	}

	void OnCollisionEnter(Collision col){
		if (col.collider.tag == "Wall" || col.collider.tag == "Player") {
			SwitchDirection (transform.eulerAngles.y);
		} 

		if (infected == 1 && col.collider.tag == "Student") {
			Student s = col.transform.GetComponent<Student> ();
			if (s.infected == 0) {
				s.Infection (infected);
			} else if (s.infected == 2) {
				s.Infection (0);
				Infection (0);
				--noRed;
				--noGreen;
			}
			SwitchDirection (transform.eulerAngles.y);
		}

		if (infected == 2 && col.collider.tag == "Student") {
			Student s = col.transform.GetComponent<Student> ();
			if (s.infected == 0) {
				s.Infection (infected);
			}
			SwitchDirection (transform.eulerAngles.y);
		}
	}

	void OnCollisionStay(Collision col){
		if (col.collider.tag == "Wall" || col.collider.tag == "Student") {
			SwitchDirection (transform.eulerAngles.y);
		}
	}

	public void Infection (int infectID){
		if (infected == 0) {
			if (infectID == 1) {
				rend.material.color = Color.red;
				noRed++;
				Instantiate (particles [0], transform.position + new Vector3 (0, 0.5f, 0), Quaternion.identity);
			} else if (infectID == 2) {
				rend = GetComponent<Renderer> ();
				rend.material.color = Color.green;
				noGreen++;
				Instantiate (particles [1], transform.position + new Vector3 (0, 0.5f, 0), Quaternion.identity);
			}
			infected = infectID;
			//Debug.Log (noInfected);
		} 

		if (infectID == 0) {
			rend.material.color = Color.white;
			infected = 0;
		}
			
	}

	public void SetSpeed (int studentType){
		switch (studentType) {
		case 1:
			speed = Random.Range (2f, 5f);
			break;
		case 2:
			speed = Random.Range (5f, 9f);
			for (int i = 1; i < GetComponentsInChildren<Renderer> ().Length; i++) {
				Renderer eyeRend = GetComponentsInChildren<Renderer> () [i];
				eyeRend.material.color = Color.yellow;
			}
			break;
		}
	}
}
