using UnityEngine;
using TMPro;

public class InformationSetter : MonoBehaviour
{

    private ScenarioInfo scenarioInfo;
    private string[] lineInfo;
    private TextMeshPro[] lines;

    // Start is called before the first frame update
    void Start()
    {
        scenarioInfo = GameObject.Find("Scenario Info").GetComponent<ScenarioInfo>();
        lineInfo = scenarioInfo.GetInfo();
        lines = new TextMeshPro[9];
        for (int i = 0; i < 9; i++)
        {
            lines[i] = transform.GetChild(i).GetComponent<TextMeshPro>();
        }
        SetText();
    }

    private void SetText()
    {
        for (int i = 0; i < 9; i++)
        {
            if (lineInfo[i] != "")
            {
                lines[i].text = lines[i].name + ": " + lineInfo[i];
            }
            else
            {
                lines[i].text = "";
            }
        }
    }
}
