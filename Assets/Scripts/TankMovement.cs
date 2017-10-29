using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankMovement : MonoBehaviour {

	public int m_PlayerNumber = 1;
	public float m_MovementSpeed = 25f;
	public float m_TurnSpeed = 90f;

	private string m_MovementAxisName;
	private string m_TurnAxisName;
	private float m_MovementInputValue;
	private float m_TurnInputValue;

	// Use this for initialization
	void Start () {
		m_MovementAxisName = "Vertical " + m_PlayerNumber;
		m_TurnAxisName = "Horizontal " + m_PlayerNumber;
	}
	
	// Update is called once per frame
	void Update () {
		m_MovementInputValue = Input.GetAxis (m_MovementAxisName);
		m_TurnInputValue = Input.GetAxis (m_TurnAxisName);
	}

	void FixedUpdate() {
		// Move and turn tank.
		transform.Translate (0f, 0f, m_MovementInputValue * m_MovementSpeed * Time.deltaTime);
		transform.Rotate (0f, m_TurnInputValue * m_TurnSpeed * Time.deltaTime, 0f);
	}
		
}
