using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class ResultScreen : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_scoreText;

    private void Awake()
    {
        ScoreText();
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    private void ScoreText()
    {
        string _text;
        string _questionWord = NumberEndsWithOne(StaticData.correctAnswerCount) ? "question" : "questions";
        if (StaticData.isTimedMode)
        {
            float _accuracy = (float)StaticData.correctAnswerCount / StaticData.answeredQuestionCount;
            _text = $"You answered {StaticData.correctAnswerCount} {_questionWord} with " +
                $"{Mathf.Round(_accuracy * 100)} % accuracy";
        }
            
        else
            _text = $"You answered {StaticData.correctAnswerCount} {_questionWord} out of {StaticData.answeredQuestionCount}";
        m_scoreText.text = _text;
    }

    private bool NumberEndsWithOne(int _number)
    {
        while (_number > 10)
            _number %= 10;
        return _number == 1;
    }
}
