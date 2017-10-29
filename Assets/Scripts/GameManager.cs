using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	public int m_NumRoundsToWin = 3;
	public float m_StratDelay = 3f;
	public float m_EndDelay = 3f;
	public GameObject[] m_Tanks;
	public Color[] m_TankColors;
	public Transform[] m_SpawnPoints;
	public Text m_MessageText;
	public AudioSource m_AudioSource;
	public AudioClip m_RoundStartingBGM;
	public AudioClip m_GameOverBGM;

	private int m_RoundNum = 0;
	private WaitForSeconds m_StartWait;
	private WaitForSeconds m_EndWait;
	private TankMovement[] m_TankMovements = new TankMovement[2];
	private TankShooting[] m_TankShootings = new TankShooting[2];
	private int[] m_TankWins = {0, 0};
	private int m_RoundWinner;
	private int m_GameWinner;

	// Use this for initialization
	void Start () {
		m_StartWait = new WaitForSeconds (m_StratDelay);
		m_EndWait = new WaitForSeconds (m_EndDelay);

		for (int i = 0; i < m_Tanks.Length; i++) {
			m_TankMovements [i] = m_Tanks [i].GetComponent<TankMovement> ();
			m_TankShootings [i] = m_Tanks [i].GetComponent<TankShooting> ();
		}

		StartCoroutine (GameLoop ());
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Escape))
			SceneManager.LoadScene (0);
	}

	private IEnumerator GameLoop() {
		yield return StartCoroutine (RoundStarting ());
		yield return StartCoroutine (RoundPlaying ());
		yield return StartCoroutine (RoundEnding ());

		if (m_GameWinner != -1) {
			SceneManager.LoadScene (1);
		} else {
			StartCoroutine (GameLoop ());
		}
	}

	private IEnumerator RoundStarting() {
		ResetAllTanks ();
		DisableTankCtrl ();

		m_RoundNum++;
		m_MessageText.text = "Round " + m_RoundNum;

		m_AudioSource.clip = m_RoundStartingBGM;
		m_AudioSource.Play ();

		yield return m_StartWait;
	}

	private IEnumerator RoundPlaying() {
		EnableTankCtrl ();

		m_MessageText.text = string.Empty;

		while (!OneTankLeft ())
			yield return null;
	}

	private IEnumerator RoundEnding() {
		DisableTankCtrl ();

		m_RoundWinner = GetRoundWinner ();
		if (m_RoundWinner != -1)
			m_TankWins [m_RoundWinner]++;

		m_GameWinner = GetGameWinner ();
		if (m_GameWinner != -1) {
			m_AudioSource.clip = m_GameOverBGM;
			m_AudioSource.Play ();
		}

		m_MessageText.text = EndingMessage ();

		yield return m_EndWait;
	}

	private bool OneTankLeft() {
		int numTankLeft = 0;

		for (int i = 0; i < m_Tanks.Length; i++) {
			if (m_Tanks [i].activeSelf)
				numTankLeft++;
		}

		return numTankLeft <= 1;
	}

	private void ResetAllTanks() {
		for (int i = 0; i < m_Tanks.Length; i++) {
			m_Tanks [i].transform.position = m_SpawnPoints [i].position;
			m_Tanks [i].transform.rotation = m_SpawnPoints [i].rotation;
			m_Tanks [i].GetComponent<TankHealth> ().Relive ();
		}
	}
		
	private void EnableTankCtrl() {
		for (int i = 0; i < m_Tanks.Length; i++) {
			m_TankMovements [i].enabled = true;
			m_TankShootings [i].enabled = true;
		}
	}

	private void DisableTankCtrl() {
		for (int i = 0; i < m_Tanks.Length; i++) {
			m_TankMovements [i].enabled = false;
			m_TankShootings [i].enabled = false;
		}
	}

	private int GetRoundWinner() {
		for (int i = 0; i < m_Tanks.Length; i++) {
			if (m_Tanks [i].activeSelf)
				return i;
		}

		return -1;
	}

	private int GetGameWinner() {
		for (int i = 0; i < m_Tanks.Length; i++) {
			if (m_TankWins [i] == m_NumRoundsToWin)
				return i;
		}

		return -1;
	}

	private string EndingMessage() {
		string message = "DRAW!";

		if (m_RoundWinner != -1)
			message = "<color=#" + ColorUtility.ToHtmlStringRGB(m_TankColors[m_RoundWinner]) + ">PLAYER " + (m_RoundWinner + 1) + "</color>" + " WINS THIS ROUND!";

		message += "\n\n\n";

		for (int i = 0; i < m_Tanks.Length; i++) {
			message += "<color=#" + ColorUtility.ToHtmlStringRGB(m_TankColors[i]) + ">PLAYER " + (i + 1) + "</color>" + " : " + m_TankWins [i] +  " WINS\n";
		}

		if (m_GameWinner != -1)
			message = "<color=#" + ColorUtility.ToHtmlStringRGB(m_TankColors[m_GameWinner]) + ">PLAYER " + (m_GameWinner + 1) + "</color>" +  " WINS THE GAME!";

		return message;
	}

}
