using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    private const string M_CORRECT = "Your answer was correct!";
    private const string M_INCORRECT = "Your answer was incorrect!";
    private const float M_TRANSITION_DURATION = 1.5f;

    [SerializeField] private QuestionList[] m_chosenQuestions;
    [SerializeField] private TextMeshProUGUI m_question;
    [SerializeField] private TextMeshProUGUI m_result;
    [SerializeField] private TextMeshProUGUI m_currentScore;
    [SerializeField] private TextMeshProUGUI m_timerText;
    [SerializeField] private Transform m_answerHolder;
    [SerializeField] private GameObject m_answerBoxPrefab;
    [SerializeField] private GameObject m_timerBoxObject;

    private Container<Question> m_remainingQuestions = new Container<Question>();
    private Container<AnswerBox> m_currentAnswers = new Container<AnswerBox>(5);
    private Question m_lastQuestion;
    private bool m_transitionInProgress = false;
    private bool m_isTimedMode;
    private float m_timer;
    private int m_totalQuestions;
    private int m_totalQuestionsAnswered = 0;
    private int m_answeredQuestions;
    private int m_correctlyAnsweredQuestions;

    private void Awake()
    {
        GetQuestionList();
        GameSetup();
    }

    private void Start()
    {
        SelectQuestion();
    }

    public void CheckAnswer()
    {
        m_totalQuestionsAnswered++;
        if (!m_isTimedMode)
            m_answeredQuestions++;
        else
            m_totalQuestions++;
        bool _correctAnswer = true;
        for (int i = 0; i < m_currentAnswers.Count; i++)
        {
            AnswerBox _aBox = m_currentAnswers.Get(i);
            if (_aBox.isCorrect != _aBox.isChecked)
            {
                _aBox.changeColor = true;
                _correctAnswer = false;
            }
            else if (_aBox.isCorrect)
                _aBox.changeColor = true;
        }
        m_result.gameObject.SetActive(true);
        m_result.text = _correctAnswer ? M_CORRECT : M_INCORRECT;
        if (_correctAnswer)
        {
            m_correctlyAnsweredQuestions++;
            if (m_isTimedMode)
            {
                m_answeredQuestions++;
            }
        }
        m_transitionInProgress = true;
        StartCoroutine(QuestionTransition());
    }

    private void GameSetup()
    {
        m_isTimedMode = StaticData.isTimedMode;
        if (m_isTimedMode)
        {
            m_answeredQuestions = 0;
            m_totalQuestions = 0;
            m_timer = StaticData.timerDuration;
            FillQuestions();
        }
        else
        {
            m_answeredQuestions = 1;
            m_timerBoxObject.SetActive(false);
            m_totalQuestions = StaticData.questionAmount;
            FillQuestions(m_totalQuestions);
        }
        m_result.gameObject.SetActive(false);
        m_currentScore.text = m_answeredQuestions + " / " + m_totalQuestions;
    }

    private void GetQuestionList()
    {
        m_chosenQuestions = new QuestionList[StaticData.QuestionCategories.Count];
        for (int i = 0; i < StaticData.QuestionCategories.Count; i++)
        {
            m_chosenQuestions[i] = (QuestionList)Resources.Load(StaticData.QuestionCategories[i]);
        }
    }

    private void FillQuestions(int fixedLength = -1)
    {
        bool _isFull = false;
        int _index = 0;

        while (!_isFull)
        {
            _isFull = true;
            for (int i = 0; i < m_chosenQuestions.Length; i++)
            {
                if (_index < m_chosenQuestions[i].Questions.Length)
                {
                    Question _question = m_chosenQuestions[i].Questions[_index];
                    m_remainingQuestions.Add(new Question(_question));
                    if (fixedLength != -1 && m_remainingQuestions.Count >= fixedLength)
                        break;                    
                    _isFull = false;
                }
            }
            _index++;
            if (_isFull && fixedLength != -1 && m_remainingQuestions.Count < fixedLength)
            {
                _isFull = false;
                _index = 0;
            }
        }
    }

    private void SelectQuestion()
    {
        if (m_remainingQuestions.Count == 0)
        {
            FillQuestions();
        }
        if (m_currentAnswers.Count > 0)
        {
            for (int i = m_currentAnswers.Count - 1; i >= 0; i--)
            {
                Destroy(m_currentAnswers.Get(i).gameObject);
            }
        }
        m_currentAnswers = new Container<AnswerBox>();
        int _index = 0;
        do
        {
            _index = Random.Range(0, m_remainingQuestions.Count);
        } while (m_lastQuestion != null && m_lastQuestion.Text == m_remainingQuestions.Get(_index).Text 
        && m_remainingQuestions.Count > 1);
        Question _selectedQuestion = m_lastQuestion = m_remainingQuestions.RemoveByIndex(_index);
        m_question.text = _selectedQuestion.Text;
        AnswerBox.isLocked = false;
        foreach (Answer _answer in _selectedQuestion.Answers)
        {
            AnswerBox _answerBox = Instantiate(m_answerBoxPrefab, m_answerHolder).GetComponent<AnswerBox>();
            _answerBox.singleChoice = _selectedQuestion.SingleChoice;
            _answerBox.AnswerData = _answer;
            _answerBox.Text = _answer.Text;
            m_currentAnswers.Add(_answerBox);
        }

        float height =
            m_currentAnswers.Get(0).GetComponent<RectTransform>().rect.height * m_currentAnswers.Count
            + 40 * (m_currentAnswers.Count - 1) + 20;
        m_answerHolder.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
    }

    private IEnumerator QuestionTransition()
    {
        while (true)
        {
            if (m_transitionInProgress)
            {
                AnswerBox.isLocked = true;
                yield return new WaitForSeconds(M_TRANSITION_DURATION);
                m_currentScore.text = m_answeredQuestions + " / " + m_totalQuestions;
                m_transitionInProgress = false;
            }
            else
            {
                if (!m_isTimedMode && m_answeredQuestions - 1 >= m_totalQuestions)
                {
                    EndGame();
                    yield break;
                }
                SelectQuestion();
                m_result.gameObject.SetActive(false);
                yield break;
            }
        }        
    }

    private void EndGame()
    {
        AnswerBox.isLocked = false;
        StaticData.correctAnswerCount = m_correctlyAnsweredQuestions;
        StaticData.answeredQuestionCount = m_totalQuestionsAnswered;
        SceneManager.LoadScene("Result");
    }

    private void Update()
    {
        if (m_isTimedMode)
        {
            if (!m_transitionInProgress && m_timer > 0f)
            {
                m_timer -= Time.deltaTime;
                m_timerText.text = ((int)m_timer).ToString();
            }
            else if (m_timer <= 0f)
                EndGame();
        }
    }
}
