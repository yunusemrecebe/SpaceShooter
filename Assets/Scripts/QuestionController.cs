using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using System.Runtime.CompilerServices;
using NUnit.Framework.Internal;
using UnityEngine.UI;

public class QuestionController : MonoBehaviour
{
    public Text questionText;
    public Text questionTopic;
    public Text scoreText;
    public Text informationText;
    public GameObject answer1;
    public GameObject answer2;
    public GameObject answer3;
    public GameObject answer4;
    public GameObject restartButton;
    public Image flagImage;

    internal int questionId;
    internal int correctAnswer;
    internal bool answerIsTrue;

    private static int lastScore;
    private string lastScoreText;
    private int lastQuestionId;
    private System.Random random;

    List<Question> questions = new List<Question>();
    List<InCorrectAnswer> inCorrectAnswers = new List<InCorrectAnswer>();
    GameController gameController = new GameController();
    

    void Start()
    {
        answer1.SetActive(false);
        answer2.SetActive(false);
        answer3.SetActive(false);
        answer4.SetActive(false);
        flagImage.enabled = false;
        answerIsTrue = false;

        informationText.text = "";
        questionTopic.text = "";
        questionText.text = "";
        lastQuestionId = -1;
    }
    
    public void RandomQuestion()
    {
        random = new System.Random();
        CreateQuestion();
        informationText.text = "Eğer soruya doğru cevap verirseniz \n kaldığınız yerden devam edeceksiniz.";

    Repeat:
        questionId = random.Next(questions.Count);
        try
        {
            if (PlayerPrefs.GetInt("lastQuestionId") != questionId)
            {
                var s = questions.Find(x => x.Id == questionId);
                questionTopic.text = s.Query;
                PlayerPrefs.SetInt("lastQuestionId", questionId);
                PlayerPrefs.Save();
                //lastQuestionId = questionId;
            }
            else
            {
                goto Repeat;
            }
        }
        catch (Exception)
        {
            goto Repeat;
        }
    }

    public void ShowAnswerButtons()
    {
        answer1.SetActive(true);
        answer2.SetActive(true);
        answer3.SetActive(true);
        answer4.SetActive(true);
        restartButton.SetActive(true);
        flagImage.enabled = true;
        CreateInCorrectAnswer();

        random = new System.Random();
        int correctAnswersButton = random.Next(4);

        var buttons = new[] { answer1, answer2, answer3, answer4};
        var answer = questions.Find(x => x.Id == questionId);
        flagImage.sprite = Resources.Load<Sprite>(answer.Picture);
        buttons[correctAnswersButton].GetComponentInChildren<Text>().text = answer.Answer;
        correctAnswer = answer.Id;

        List<int> oldInCorrectAnswers = new List<int>();

        for (int i = 0; i < buttons.Length; i++)
        {
            if (i == correctAnswersButton)
            {
                continue;//doğru cevabın olduğu butonu atlamak için
            }
            Test:
            int InCorrectAnswerId = random.Next(inCorrectAnswers.Count);
            foreach (int oldInCorrectAnswerId in oldInCorrectAnswers)
            {
                if (oldInCorrectAnswerId == InCorrectAnswerId)
                {
                    goto Test;
                }
            }
            var falseAnswers = inCorrectAnswers.Find(x => x.Id == InCorrectAnswerId);
            oldInCorrectAnswers.Add(InCorrectAnswerId);
            buttons[i].GetComponentInChildren<Text>().text = falseAnswers.Answer;
        }
    }

    public void TakeAnswer(GameObject buton)
    {
        string correctAnswer = buton.GetComponentInChildren<Text>().text;

        if (questions[questionId].Answer.Contains(correctAnswer))
        {
            StartCoroutine(TrueAnswer());
        }
        else
        {
            FalseAnswer();
        }
    }

    IEnumerator TrueAnswer()
    {
        answer1.SetActive(false);
        answer2.SetActive(false);
        answer3.SetActive(false);
        answer4.SetActive(false);
        restartButton.SetActive(false);

        lastScoreText = scoreText.text.Replace("Puan: ", null);
        lastScore = Convert.ToInt32(lastScoreText);

        questionText.text = "Tebrikler! \n Doğru bildiniz. \n Oyun yeniden başlatılıyor...";

        PlayerPrefs.SetInt("score", lastScore);
        PlayerPrefs.Save();

        yield return new WaitForSeconds(3);

        Application.LoadLevel(Application.loadedLevel);
    }

    void FalseAnswer()
    {
        var answer = questions.Find(x => x.Id == questionId);
        questionText.text = "Yanlış Cevap! \n Oyuna yeniden başlayabilirsiniz. \n Cevap, '" + answer.Answer + "' olmalıydı!";

        answer1.SetActive(false);
        answer2.SetActive(false);
        answer3.SetActive(false);
        answer4.SetActive(false);
    }

    void CreateQuestion()
    {
        questions.Add(new Question() { Id = 0, Query = "Bu ülkenin başkenti hangisidir?", Answer = "Ankara", Picture = "flagTurkey" });
        questions.Add(new Question() { Id = 1, Query = "Bu ülkenin başkenti hangisidir?", Answer = "Washington DC", Picture = "flagUnitedStates" });
        questions.Add(new Question() { Id = 2, Query = "Bu ülkenin başkenti hangisidir?", Answer = "Berlin", Picture = "flagGermany" });
        questions.Add(new Question() { Id = 3, Query = "Bu ülkenin başkenti hangisidir?", Answer = "Madrid", Picture = "flagSpain" });
        questions.Add(new Question() { Id = 4, Query = "Bu ülkenin başkenti hangisidir?", Answer = "Atina", Picture = "flagGreece" });
        questions.Add(new Question() { Id = 5, Query = "Bu ülkenin başkenti hangisidir?", Answer = "Sofya", Picture = "flagBulgaria" });
        questions.Add(new Question() { Id = 6, Query = "Bu ülkenin başkenti hangisidir?", Answer = "Panama", Picture = "flagPanama" });
        questions.Add(new Question() { Id = 7, Query = "Bu ülkenin başkenti hangisidir?", Answer = "Bogota", Picture = "flagColombia" });
        questions.Add(new Question() { Id = 8, Query = "Bu ülkenin başkenti hangisidir?", Answer = "Paris", Picture = "flagFrance" });
        questions.Add(new Question() { Id = 9, Query = "Bu ülkenin başkenti hangisidir?", Answer = "Roma", Picture = "flagItaly" });
        questions.Add(new Question() { Id = 10, Query = "Bu ülkenin başkenti hangisidir?", Answer = "Londra", Picture = "flagUnitedKingdom" });
        questions.Add(new Question() { Id = 11, Query = "Bu ülkenin başkenti hangisidir?", Answer = "Cape Town", Picture = "flagSouthAfrica" });
    }
        
    void CreateInCorrectAnswer()
    {
        inCorrectAnswers.Add(new InCorrectAnswer() { Id = 0, Answer = "Punom Pen" });
        inCorrectAnswers.Add(new InCorrectAnswer() { Id = 1, Answer = "Bangkok" });
        inCorrectAnswers.Add(new InCorrectAnswer() { Id = 2, Answer = "Singapur" });
        inCorrectAnswers.Add(new InCorrectAnswer() { Id = 3, Answer = "Abu Dabi" });
        inCorrectAnswers.Add(new InCorrectAnswer() { Id = 4, Answer = "Doha" });
        inCorrectAnswers.Add(new InCorrectAnswer() { Id = 5, Answer = "Kahire" });
        inCorrectAnswers.Add(new InCorrectAnswer() { Id = 6, Answer = "Tahran" });
        inCorrectAnswers.Add(new InCorrectAnswer() { Id = 7, Answer = "Tokyo" });
        inCorrectAnswers.Add(new InCorrectAnswer() { Id = 8, Answer = "Bakü" });
        inCorrectAnswers.Add(new InCorrectAnswer() { Id = 9, Answer = "Üsküp" });
    }
}

public class Question
{
    public int Id { get; set; }
    public string Query { get; set; }
    public string Answer { get; set; }
    public string Picture { get; set; }
}

public class InCorrectAnswer
{
    public int Id { get; set; }
    public string Answer { get; set; }
}

