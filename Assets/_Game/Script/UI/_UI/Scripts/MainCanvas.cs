using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainCanvas : UICanvas
{
    [Header("UI References")]
    [SerializeField] private Text timerText;
    [SerializeField] private Text questionText;
    [SerializeField] private Button[] answerButtons;

    private float timer = 0f;
    private int correctAnswer;
    private bool isGameOver = false;
    private bool isStarted = false;
    private bool hasAnsweredCorrectly = false; // để check "lần đầu đúng"

    private void Start()
    {
        UIManager.Ins.mainCanvas = this;

        GenerateQuestion();
        UpdateTimerUI();
    }

    private void Update()
    {
        if (isGameOver) return;
        if (!isStarted) return;

        // Timer tăng dần
        timer += Time.deltaTime;
        UpdateTimerUI();
    }

    private void UpdateTimerUI()
    {
        timerText.text = timer.ToString("0.0");
    }

    private void GenerateQuestion()
    {
        int a = Random.Range(0, 10);
        int b = Random.Range(0, 10);
        char[] ops = { '+', '-', '*' };
        char op = ops[Random.Range(0, ops.Length)];

        switch (op)
        {
            case '+': correctAnswer = a + b; break;
            case '-': correctAnswer = a - b; break;
            case '*': correctAnswer = a * b; break;
        }

        questionText.text = a + " " + op + " " + b;

        List<int> answers = new List<int> { correctAnswer };

        while (answers.Count < 4)
        {
            int wrongAns = correctAnswer + Random.Range(-3, 4);
            if (!answers.Contains(wrongAns))
            {
                answers.Add(wrongAns);
            }
        }

        // Xáo trộn
        for (int i = 0; i < answers.Count; i++)
        {
            int rnd = Random.Range(0, answers.Count);
            int temp = answers[rnd];
            answers[rnd] = answers[i];
            answers[i] = temp;
        }

        // Gán vào nút
        for (int i = 0; i < answerButtons.Length; i++)
        {
            int val = answers[i];
            answerButtons[i].GetComponentInChildren<Text>().text = val.ToString();
            answerButtons[i].onClick.RemoveAllListeners();
            answerButtons[i].onClick.AddListener(() => OnAnswerClicked(val));
        }
    }

    private void OnAnswerClicked(int chosenAnswer)
    {
        if (isGameOver) return;

        if (chosenAnswer == correctAnswer)
        {
            AudioManager.Ins.PlaySFX(AudioManager.Ins.point);

            Debug.Log("Correct!");

            if (!isStarted)
            {
                isStarted = true;
                timer = 0f;
                UpdateTimerUI();
            }

            hasAnsweredCorrectly = true; // đã có ít nhất 1 lần đúng

            // gọi Rabbit.Fly()
            if (LevelManager.Ins.curLevel != null && LevelManager.Ins.curLevel.rabbit != null)
            {
                LevelManager.Ins.curLevel.rabbit.Fly();
            }

            GenerateQuestion();
        }
        else
        {
            Debug.Log("Wrong!");

            if (!hasAnsweredCorrectly) // chỉ xử lý thua khi sai lần đầu tiên
            {
                isGameOver = true;

                UIManager.Ins.TransitionUI<ChangeUICanvas, MainCanvas>(0.6f,
                    () =>
                    {
                        LevelManager.Ins.DespawnLevel();
                        UIManager.Ins.OpenUI<MainCanvas>();
                        LevelManager.Ins.SpawnLevel();
                    });
            }
            else
            {
                // Sai sau khi đã đúng ít nhất 1 lần -> bỏ qua, không thua
                Debug.Log("Ignored wrong answer (already started).");
            }
        }
    }

    public void ResetUI()
    {
        timer = 0f;
        isStarted = false;
        isGameOver = false;
        hasAnsweredCorrectly = false;

        UpdateTimerUI();
        GenerateQuestion();
    }
}
