using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Question : ICloneable
{
    public bool SingleChoice = true;
    public string Text;
    public Answer[] Answers;

    public Question()
    {

    }

    public Question(Question question)
    {
        Text = question.Text;
        Answers = new Answer[question.Answers.Length];
        SingleChoice = question.SingleChoice;
        for (int i = 0; i < Answers.Length; i++)
        {
            Answers[i] = question.Answers[i];
        }
    }

    public object Clone()
    {
        Question _clone = new Question();
        _clone.Text = Text;
        _clone.Answers = new Answer[Answers.Length];
        for (int i = 0; i < Answers.Length; i++)
        {
            _clone.Answers[i] = Answers[i];
        }
        return _clone;
    }
}
