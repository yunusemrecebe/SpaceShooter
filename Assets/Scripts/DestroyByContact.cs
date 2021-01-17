using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyByContact : MonoBehaviour
{
	public GameObject explosion;
	public GameObject playerExplosion;
	public int scoreValue;

	private GameController gameController;
    private QuestionController questionController;
    private Question question;
    
    internal bool moveLock;

	void Start()
	{
		GameObject gameControllerObject = GameObject.FindWithTag("GameController");
        GameObject gameControllerObject2 = GameObject.FindWithTag("QuestionController");

        moveLock = false;

		if (gameControllerObject != null && gameControllerObject2) {
			gameController = gameControllerObject.GetComponent<GameController>();
            questionController = gameControllerObject2.GetComponent<QuestionController>();
        }
    }

	void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Boundary") || other.CompareTag("Enemy")) {
            return;
		}

		if (explosion != null) {
			Instantiate(explosion, transform.position, transform.rotation);
		}

		if (other.CompareTag("Player")) {
			Instantiate(playerExplosion, other.transform.position, other.transform.rotation);
            moveLock = true;
			gameController.GameOver();
			questionController.RandomQuestion();
			questionController.ShowAnswerButtons();

		} else {
			gameController.AddScore(scoreValue);
		}

		Destroy(other.gameObject); 
		Destroy(gameObject); 
	}
}
