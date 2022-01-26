using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using TMPro;

public class MenuManager : MonoBehaviour
{
    private string m_timerModeInfo;

    [SerializeField] private GameObject m_startScreen;
    [SerializeField] private GameObject m_modeSelectionScreen;
    [Space]
    [SerializeField] private AnswerBox m_timerModeBox;
    [SerializeField] private TextMeshProUGUI m_timerModeInfoText;
    private AnswerBox[] m_CategoryBoxes;
    [Space]
    [SerializeField] private GameObject m_answerBoxPrefab;
    [SerializeField] private Transform m_categoriesHolder;

    private List<string> m_UsedCategories = new List<string>();

    private void Start()
    {
        m_timerModeInfoText.text = $"If timed mode is enabled, " +
            $"you will have to answer as many questions as possible in " +
            $"{StaticData.timerDuration} seconds.";
        m_timerModeBox.singleChoice = false;
        LoadCaterogies();
        StartScreen();
    }

    public bool SetGameData()
    {
        foreach (AnswerBox _cBox in m_CategoryBoxes)
        {
            if (_cBox.isChecked)
                m_UsedCategories.Add(_cBox.Text);
        }
        if (m_UsedCategories.Count == 0)
            return false;
        StaticData.QuestionCategories = m_UsedCategories;
        StaticData.isTimedMode = m_timerModeBox.isChecked;
        return true;
    }

    public void StartScreen()
    {
        m_startScreen.SetActive(true);
        m_modeSelectionScreen.SetActive(false);
    }

    public void ModeSelectionScreen()
    {
        m_startScreen.SetActive(false);
        m_modeSelectionScreen.SetActive(true);
    }

    public void MoveToGameScene()
    {
        if (SetGameData())
            SceneManager.LoadScene("Game");
        else
            Debug.LogWarning("No categories chosen");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private void LoadCaterogies()
    {
        object[] _questionLists = Resources.LoadAll("", typeof(QuestionList));
        m_CategoryBoxes = new AnswerBox[_questionLists.Length];
        for (int i = 0; i < _questionLists.Length; i++)
        {
            m_CategoryBoxes[i] = Instantiate(m_answerBoxPrefab, m_categoriesHolder).GetComponent<AnswerBox>();
            m_CategoryBoxes[i].singleChoice = false;
            m_CategoryBoxes[i].Text = ((QuestionList)_questionLists[i]).name;
        }
    }
}
