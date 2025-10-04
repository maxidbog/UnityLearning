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


    private void Awake()
    {
        gameModel.OnValuesChanged.AddListener(UpdateValues);
        diceCountField.onEndEdit.AddListener(ChangeDiceCount);
        winTargetField.onEndEdit.AddListener(ChangeMinWinValue);
        drawTargetField.onEndEdit.AddListener(ChangeMinDrawValue);
        rollButton.onClick.AddListener(gameModel.StartRoll);

        UpdateValues();
    }

    private void UpdateValues()
    {
        var totalScore = gameModel.TotalScore;
        var minWinValue = gameModel.MinWinValue;
        var minDrawValue = gameModel.MinDrawValue;
        diceCountField.text = gameModel.DicesCount.ToString();
        winTargetField.text = gameModel.MinWinValue.ToString();
        drawTargetField.text = gameModel.MinDrawValue.ToString();
        if (totalScore >= minDrawValue)
        {
            if (totalScore < minWinValue)
                scoreText.color = Color.yellow;
            else scoreText.color = Color.green;
        }
        else scoreText.color = Color.red;
        scoreText.text = $"Score: {gameModel.TotalScore}";
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
