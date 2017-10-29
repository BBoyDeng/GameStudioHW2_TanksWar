using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankShooting : MonoBehaviour {

	public int m_PlayerNumber = 1;
	public float m_MinLaunchForce = 30f;
	public float m_MaxLaunchForce = 50f;
	public Rigidbody m_Shell;
	public Transform m_ShellTransform;
	public AudioSource m_ShootingAudio;
	public AudioClip m_FireClip;

	private string m_FireButton;

	// Use this for initialization
	void Start () {
		m_FireButton = "Fire " + m_PlayerNumber;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown (m_FireButton)) {
			Rigidbody shellInstance = Instantiate (m_Shell, m_ShellTransform.position, m_ShellTransform.rotation) as Rigidbody;

			float launchForce = Random.Range (m_MinLaunchForce, m_MaxLaunchForce);
			shellInstance.velocity = launchForce * m_ShellTransform.forward;

			m_ShootingAudio.clip = m_FireClip;
			m_ShootingAudio.Play ();
		}
	}


}
