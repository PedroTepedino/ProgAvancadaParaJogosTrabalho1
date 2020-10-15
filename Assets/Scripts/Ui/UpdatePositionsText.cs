
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpdatePositionsText : MonoBehaviour
{
    private Dictionary<int, string> specialPositionsDictionary = new Dictionary<int, string>
    {
        {1, "1st:"},
        {2, "2nd:"},
        {3, "3rd:"},
    };
    [SerializeField] private TextMeshProUGUI _text;

    private void Update()
    {
        if (GameManager.Instance.RaceFinished)
        {
            string positions = null;
            for (var index = 0; index < 8; index++)
            {
                if (index < GameManager.Instance.FinalRunnersPositions.Length)
                {
                    var runner = GameManager.Instance.FinalRunnersPositions[index];
                    if (specialPositionsDictionary.ContainsKey(index + 1))
                    {
                        positions += $"{specialPositionsDictionary[index + 1]} {runner.gameObject.name}\n";
                    }
                    else
                    {
                        positions += $"{index + 1}th: {runner.gameObject.name}\n";
                    }
                }
                else
                {
                    if (specialPositionsDictionary.ContainsKey(index + 1))
                    {
                        positions += $"{specialPositionsDictionary[index + 1]}\n";
                    }
                    else
                    {
                        positions += $"{index + 1}th:\n";
                    }
                }
            }
            _text.text = positions;
        }
    }

    private void OnValidate()
    {
        if (_text == null)
        {
            _text = this.GetComponent<TextMeshProUGUI>();
        }
    }
}