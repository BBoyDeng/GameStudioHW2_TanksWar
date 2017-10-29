using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour {

	public float m_DampTime = 0.2f;
	public float m_ScreenEdgeBuffer = 15f;
	public float m_MinSize = 20f;
	public Camera m_Camera;
	public Transform[] m_Tanks;


	private Vector3 m_MoveVelocity;
	private Vector3 m_DesiredPosition;
	private float m_ZoomSpeed;
	private float m_DesiredFieldOfView;


	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	}

	void FixedUpdate() {
		Move ();
		Zoom ();
	}


	private void Move() {
		FindAveragePosition ();

		transform.position = Vector3.SmoothDamp (transform.position, m_DesiredPosition, ref m_MoveVelocity, m_DampTime);
	}

	private void FindAveragePosition() {
		Vector3 averagePos = new Vector3 ();
		int numOfTanks = 0;

		for (int i = 0; i < m_Tanks.Length; i++) {
			if (!m_Tanks [i].gameObject.activeSelf)
				continue;

			averagePos += m_Tanks [i].position;
			numOfTanks++;
		}

		averagePos /= numOfTanks;
		averagePos.x -= 45f;
		averagePos.z -= 45f;
		averagePos.y = transform.position.y;

		m_DesiredPosition = averagePos;
	}

	private void Zoom() {
		FindRequiredFieldOfView ();

		m_Camera.fieldOfView = Mathf.SmoothDamp (m_Camera.fieldOfView, m_DesiredFieldOfView, ref m_ZoomSpeed, m_DampTime);
	}

	private void FindRequiredFieldOfView() {
		Vector3 desiredLocalPos = transform.InverseTransformPoint (m_DesiredPosition);
		float size = 0f;

		for (int i = 0; i < m_Tanks.Length; i++) {
			if (!m_Tanks [i].gameObject.activeSelf)
				continue;

			Vector3 tankLocalPos = transform.InverseTransformPoint (m_Tanks [i].position);
			Vector3 desiredPosToTank = tankLocalPos - desiredLocalPos;

			size = Mathf.Max (size, Mathf.Abs (desiredPosToTank.y));
			size = Mathf.Max (size, Mathf.Abs (desiredPosToTank.x) / m_Camera.aspect);
		}

		size += m_ScreenEdgeBuffer;
		size = Mathf.Max (size, m_MinSize);

		m_DesiredFieldOfView = size;
	}
}
