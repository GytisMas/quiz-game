using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "QuestionList", menuName = "ScriptableObject/QuestionList", order = 0)]
public class QuestionList : ScriptableObject
{
    public Question[] Questions;
}
