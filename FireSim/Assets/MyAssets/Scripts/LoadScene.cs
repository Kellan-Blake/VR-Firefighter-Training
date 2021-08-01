using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{

    [SerializeField] private GameObject scenarioToPass;
    private ScenarioInfo scenarioInfo;
    private Settings settings;

    // Start is called before the first frame update
    void Start()
    {
        if (scenarioToPass != null)
        {
            scenarioInfo = scenarioToPass.GetComponent<ScenarioInfo>();
            settings = FindObjectOfType<Settings>();
            settings.SetInfo(scenarioInfo);
        }
    }

    public void LoadSceneNumber(int index)
    {
        switch (index)
        {
            case 2:
                settings.LoadInternal();
                break;
            case 3:
                settings.LoadExternal();
                break;
            case 4:
                settings.LoadRelay();
                break;
            default:
                break;
        }
        if (scenarioToPass != null)
        {
            DontDestroyOnLoad(scenarioToPass);
        }
        if (index == 0)
        {
            Destroy(GameObject.Find("Scenario Info"));
        }
        SceneManager.LoadScene(index);
    }
}
