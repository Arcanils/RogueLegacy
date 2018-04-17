using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameplayManager : MonoBehaviour {

	public GameObject PrefabBoss;
	public GameObject PrefabPawnPlayer;
	public GameObject PrefabControllerPlayer;

	private PatternHandler.EDifficulty _difficulty;

	private bool _isFinish;

	private void Awake()
	{
		_difficulty = (PatternHandler.EDifficulty)UIMain.GlobalParamSet;
	}

	private IEnumerator Start()
	{
		yield return null;
		var player = CreatePlayer();
		CreateBoss(player);

		yield return null;
		player.transform.position = Vector3.zero;
	}

	private void Update()
	{
		if (Input.GetButtonUp("Replay"))
		{
			SceneManager.LoadScene(1);
		}
	}

	private void CreateBoss(PawnComponent pawn)
	{
		var boss = GameObject.Instantiate(PrefabBoss, Vector3.zero, Quaternion.identity).GetComponent<BossController>();
		boss.OnFinishPattern += () => EndGame(true);
		boss.Init(pawn, _difficulty);
	}

	private PawnComponent CreatePlayer()
	{
		var pawn = GameObject.Instantiate(PrefabPawnPlayer, Vector3.zero, Quaternion.identity).GetComponent<PawnComponent>();
		var controller = GameObject.Instantiate(PrefabControllerPlayer).GetComponent<PlayerController>();

		controller.Init(pawn);
		pawn.OnDeath += () => EndGame(false);
		return pawn;
	}

	private void EndGame(bool victory = false)
	{
		if (_isFinish)
			return;
		_isFinish = true;

		int currentScore;
		int bestScore;

		SaveScore(victory, out currentScore, out bestScore);


		DisplayEndGame(currentScore, bestScore, victory);
	}

	private void SaveScore(bool isVictory, out int currentScore, out int bestScore)
	{
		currentScore = FindObjectOfType<BossController>().GetCurrentIndexPattern();

		var key = "Score_" + (int)_difficulty;
		var keyVictory = "Victory_" + (int)_difficulty;
		bestScore = PlayerPrefs.GetInt(key, int.MinValue); 

		if (currentScore > bestScore)
		{
			PlayerPrefs.SetInt(key, currentScore);
			PlayerPrefs.SetString(keyVictory, isVictory ? "Victory" : "Dead");
		}
		else if (currentScore == bestScore && isVictory)
		{
			PlayerPrefs.SetString(keyVictory, "Victory");
		}

	}

	private void DisplayEndGame(int currentScore, int bestScore, bool isVictory)
	{
		var keyVictory = "Victory_" + (int)_difficulty;
		var isBestAVictory = PlayerPrefs.GetString(keyVictory, "Dead");

		var newScoreTxt = "Result : "  + (isVictory ? "Victory" : "Dead") + " on the " + currentScore + " patterns";
		var bestScoreTxt = "Best : " + isBestAVictory + " on the " + bestScore + " patterns";
		FindObjectOfType<UIEndScreen>().PlayEnd(newScoreTxt, bestScoreTxt);
	}
}
