using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour {

	[SerializeField]
	private float lookSensitivity = 3f;

	private Rigidbody rb;
	private Vector3 velocity;
	private float shootTimer = 4f;

	public int speed;
	public Camera cam;
	public LayerMask mask;
	public Slider shootSlider;

	void Start () {
		rb = GetComponent<Rigidbody> ();
	}

	void Update () {

		//PLAYER MOVEMENT
		float _xMov = Input.GetAxisRaw("Horizontal");
		float _zMov = Input.GetAxisRaw("Vertical");

		Vector3 _movHorizontal = transform.right * _xMov; //transform.right is (1,0,0)
		Vector3 _movVertical = transform.forward * _zMov; //transform.forward is (0,0,1)
		velocity = (_movHorizontal + _movVertical).normalized * speed;//Final movement vector

		rb.MovePosition (rb.position + velocity * Time.fixedDeltaTime);


		//PLAYER ROTATION
		float _yRot = Input.GetAxisRaw("Mouse X");
		Vector3 _rotation = new Vector3 (0f, _yRot, 0f) * lookSensitivity;
		rb.MoveRotation (rb.rotation * Quaternion.Euler(_rotation));


		//PLAYER SHOOT
		if (Input.GetButtonDown("Fire1") && shootTimer >= 4){
			Infect();
		}

		if (shootTimer < 4) {
			shootTimer += Time.deltaTime;
			shootSlider.value = shootTimer;
		}
	}

	void Infect(){
		RaycastHit _hit;
		Vector3 shootPos = cam.transform.position + new Vector3 (0, 0.2f, 0);

		if (Physics.Raycast(shootPos, cam.transform.forward, out _hit, 1000, mask)) {
			if (_hit.collider.tag == "Student") {
				Student st = _hit.transform.gameObject.GetComponent<Student> ();
				if (st.infected == 0) {
					st.Infection (1);
					shootTimer = 0f;
				} if (st.infected == 2) {
					st.Infection (0);
					shootTimer = 0f;
					Student.noGreen--;
				}
			}
		}

		Debug.DrawLine (shootPos, _hit.point, Color.red, 3f);
	}
}
