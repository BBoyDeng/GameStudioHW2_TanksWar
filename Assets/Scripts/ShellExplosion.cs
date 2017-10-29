using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellExplosion : MonoBehaviour {

	public LayerMask m_TankMask;
	public ParticleSystem m_ExplosionParticle;
	public float m_ExplosionRadius = 5f;
	public float m_ExplosionForce = 100f;
	public float m_MaxDamage = 50f;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnCollisionEnter (Collision other) {

		Collider[] colliders = Physics.OverlapSphere (transform.position, m_ExplosionRadius, m_TankMask);

		for (int i = 0; i < colliders.Length; i++) {
			Rigidbody targetRigidbody = colliders [i].gameObject.GetComponent<Rigidbody> ();

			if (!targetRigidbody)
				continue;

			targetRigidbody.AddExplosionForce (m_ExplosionForce, transform.position, m_ExplosionRadius);

			TankHealth targetHealth = targetRigidbody.GetComponent<TankHealth> ();

			if (!targetHealth)
				continue;

			float damage = CalculateDamage(targetRigidbody.position);
			targetHealth.TakeDamage (damage);
		}

		ParticleSystem explosionInstance = Instantiate (m_ExplosionParticle) as ParticleSystem;
		explosionInstance.transform.position = transform.position;

		explosionInstance.GetComponent<AudioSource> ().Play ();

		Destroy (explosionInstance.gameObject, explosionInstance.main.duration);
		Destroy (gameObject);
	}

	private float CalculateDamage(Vector3 targetPos) {
		Vector3 explosionToTarget = targetPos - transform.position;

		float explosionDistance = explosionToTarget.magnitude;
		float relativeDistance = (m_ExplosionRadius - explosionDistance) / m_ExplosionRadius;
		float damage = relativeDistance * m_MaxDamage;
		damage = Mathf.Max (0f, damage);

		return damage;
	}

	public void IsExplosion() {

	}

}
