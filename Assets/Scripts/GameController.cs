using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameController : MonoBehaviour
{
	public GameObject[] asteroids;
	public Vector3 spawnValues;
	public int asteroidCount;
	public float startWait;
	public float spawnWait;
	public float waveWait;
	public Text scoreText;
    public GameObject restartButton;
	
    Question question;
    QuestionController questionController;
    private GameObject png;

    internal int lastScore;
	internal int score;
	internal bool gameOver;
	internal bool restart;
	
	void Start()
    {
        lastScore = PlayerPrefs.GetInt("score");
		if (lastScore>0)
        {
            score = lastScore;
        }
        else
        {
            score = 0;
		}

        UpdateScore();

        restart = false;
		restartButton.SetActive(false);

        StartCoroutine(SpawnWaves());
    }

    IEnumerator SpawnWaves()
	{
		yield return new WaitForSeconds(startWait);
        while(true) {
			for (int i = 0; i < asteroidCount; i++) {
				GameObject asteroid = asteroids[Random.Range(0, asteroids.Length)];

				Vector3 spawnPosition = new Vector3(Random.Range(-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z);
				Quaternion spawnRotation = Quaternion.identity;

				Instantiate(asteroid, spawnPosition, spawnRotation);

				yield return new WaitForSeconds(spawnWait);
			}

			yield return new WaitForSeconds(waveWait);
            if (gameOver)
            {
                break;
            }
		}
	}

	void UpdateScore()
	{
		scoreText.text = "Puan: " + score.ToString();
	}

	public void AddScore(int newScoreValue)
	{
		score += newScoreValue;
		UpdateScore();
	}

	public void GameOver()
    {
        gameOver = true;
    }

    public void Restart()
    {
        PlayerPrefs.SetInt("score", 0);
        PlayerPrefs.Save();
		Application.LoadLevel(Application.loadedLevel);
    }
}
