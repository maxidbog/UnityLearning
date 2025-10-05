using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [Header("Model")]
    [SerializeField] private DiceGame gameModel;
    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TMP_InputField drawTargetField;
    [SerializeField] private TMP_InputField winTargetField;
    [SerializeField] private TMP_InputField diceCountField;
    [SerializeField] private Button rollButton;
    [SerializeField] private TextMeshProUGUI finalText;

    private CanvasGroup canvasGroup;


    private void Awake()
    {
        gameModel.OnValuesChanged.AddListener(UpdateValues);
        diceCountField.onEndEdit.AddListener(ChangeDiceCount);
        winTargetField.onEndEdit.AddListener(ChangeMinWinValue);
        drawTargetField.onEndEdit.AddListener(ChangeMinDrawValue);
        rollButton.onClick.AddListener(gameModel.StartRoll);
        canvasGroup = GetComponent<CanvasGroup>();
        UpdateValues();
    }

    private void UpdateValues()
    {
        var totalScore = gameModel.TotalScore;
        var minWinValue = gameModel.MinWinValue;
        var minDrawValue = gameModel.MinDrawValue;
        var isRolling = gameModel.IsRolling;
        Color color;
        string textFinal;
        diceCountField.text = gameModel.DicesCount.ToString();
        winTargetField.text = gameModel.MinWinValue.ToString();
        drawTargetField.text = gameModel.MinDrawValue.ToString();
        if (totalScore >= minDrawValue)
        {
            if (totalScore < minWinValue)
            {
                color = Color.yellow;
                textFinal = "Ничья";
            }
            else
            {
                color = Color.green;
                textFinal = "Победа";
            }
        }
        else
        {
            color = Color.red;
            textFinal = "Поражение";
        }
        scoreText.color = color;
        scoreText.text = $"Score: {gameModel.TotalScore}";

        if (totalScore > 0 && !isRolling)
        {
            finalText.color = color;
            finalText.text = textFinal;
        }
        else finalText.text = string.Empty;

        canvasGroup.interactable = isRolling ? false : true;
    }

    private void ChangeDiceCount(string input)
    {
        if (int.TryParse(input, out int value))
            gameModel.DicesCount = value;
        else UpdateValues();
    }

    private void ChangeMinWinValue(string input)
    {
        if (int.TryParse(input, out int value))
            gameModel.MinWinValue = value;
        else UpdateValues();
    }

    private void ChangeMinDrawValue(string input)
    {
        if (int.TryParse(input, out int value))
            gameModel.MinDrawValue = value;
        else UpdateValues();
    }

}
