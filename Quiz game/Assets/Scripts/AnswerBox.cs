using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class AnswerBox : MonoBehaviour
{
    [HideInInspector] public bool isChecked;
    
    public static bool isLocked = false;

    private static AnswerBox m_checkedAnswerBox;

    private const float _maxDiff = .01f;

    [SerializeField] private GameObject m_roundCheckBox;
    [SerializeField] private GameObject m_squareCheckBox;
    [SerializeField] private GameObject m_roundCheckmark;
    [SerializeField] private GameObject m_squareCheckmark;
    [Space]
    [SerializeField] private TextMeshProUGUI m_text;
    [Space]
    [SerializeField] private Image[] m_edges;
    [SerializeField] private Color m_green;
    [SerializeField] private Color m_red;
     private Color m_target;

    private bool m_singleChoice = true;
    private Answer m_answer;
    private bool m_changeColor = false;

    public bool changeColor
    {
        set
        {
            m_changeColor = value;
            m_target = isCorrect ? m_green : m_red;
        }
    }

    public bool singleChoice
    {
        set
        {
            m_singleChoice = value;
            m_roundCheckBox.SetActive(value);
            m_squareCheckBox.SetActive(!value);
        }
    }

    public bool isCorrect
    {
        get
        {
            return m_answer.IsCorrect;
        }
    }

    public Answer AnswerData
    {
        set
        {
            m_answer = value;
        }
    }

    public string Text
    {
        get
        {
            return m_text.text;
        }
        set
        {
            m_text.text = value;
        }
    }

    private void Awake()
    {
        m_checkedAnswerBox = null;
        isChecked = false;
        m_roundCheckmark.SetActive(false);
        m_squareCheckmark.SetActive(false);
    }

    public void AnswerClicked()
    {
        if (!isLocked)
        {
            // Add PlaySound
            if (m_singleChoice)
            {
                if (!isChecked)
                {
                    SetSelection(true);
                    if (m_checkedAnswerBox != this)
                    {
                        m_checkedAnswerBox?.SetSelection(false);
                        m_checkedAnswerBox = this;
                    }
                }
            }
            else
            {
                SetSelection(!isChecked);
            }
        }        
    }

    public void SetSelection(bool _value)
    {
        isChecked = _value;
        if (m_singleChoice)
            m_roundCheckmark.SetActive(_value);
        else
            m_squareCheckmark.SetActive(_value);
    }

    private void ChangeColor()
    {
        float _increment = .02f;
        bool _changesMade = false;
        for (int i = 0; i < m_edges.Length; i++)
        {
            int _rSign = (int)Mathf.Sign(m_target.r - m_edges[0].color.r);
            int _gSign = (int)Mathf.Sign(m_target.g - m_edges[0].color.g);
            int _bSign = (int)Mathf.Sign(m_target.b - m_edges[0].color.b);

            float _newR = SameColor(m_edges[0].color.r, m_target.r) 
                ? m_edges[0].color.r : m_edges[0].color.r + _rSign * _increment;
            float _newG = SameColor(m_edges[0].color.g, m_target.g)
                ? m_edges[0].color.g : m_edges[0].color.g + _gSign * _increment;
            float _newB = SameColor(m_edges[0].color.b, m_target.b)
                ? m_edges[0].color.b : m_edges[0].color.b + _bSign * _increment;

            if (_newR != m_edges[0].color.r || _newG != m_edges[0].color.g || _newB != m_edges[0].color.b)
            {
                _changesMade = true;
                m_edges[i].color = new Color(
                    m_edges[i].color.r + _rSign * _increment,
                    m_edges[i].color.g + _gSign * _increment,
                    m_edges[i].color.b + _bSign * _increment,
                    1);
            }                
        }
        if (!_changesMade)
            m_changeColor = false;
    }

    private bool SameColor(float _colorL, float _colorR)
    {
        return Mathf.Abs(_colorR - _colorL) <= _maxDiff;
    }

    private void Update()
    {
        if (m_changeColor)
            ChangeColor();
    }
}
