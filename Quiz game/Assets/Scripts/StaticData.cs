using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StaticData
{
    // Pre start
    public static List<string> QuestionCategories;
    public static int timerDuration = 30;
    public static int questionAmount = 15;
    public static bool isTimedMode = false;

    // Result
    public static int correctAnswerCount = 0;
    public static int answeredQuestionCount = 0;
}
