using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TankHealth : MonoBehaviour {

	public float m_StartingHealth = 100f;
	public Slider m_Slider;
	public Image m_FillImage;
	public Color m_FullHealthColor = Color.green;
	public Color m_ZeroHealthColor = Color.red;


	private float m_CurrentHealth;
	private bool m_Dead;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void TakeDamage(float amount) {
		StartCoroutine (DecreasingHealth (amount));
	}

	public void Relive() {
		gameObject.SetActive (true);
		m_CurrentHealth = m_StartingHealth;
		SetHealthUI ();
	}

	private IEnumerator DecreasingHealth(float amount) {
		for (int i = 0; i < amount; i++) {
			m_CurrentHealth--;
			SetHealthUI();

			if (m_CurrentHealth <= 0f)
				OnDeath ();

			yield return null;
		}
	}

	private void SetHealthUI() {
		m_Slider.value = m_CurrentHealth;
		m_FillImage.color = Color.Lerp (m_ZeroHealthColor, m_FullHealthColor, m_CurrentHealth / m_StartingHealth);
	}

	private void OnDeath() {
		gameObject.SetActive (false);
	}

}
