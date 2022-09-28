using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageManager : Singleton<StageManager>
{
    private void SetScene(string nextSceneName)
    {
        SceneManager.LoadScene(nextSceneName);
    }
}
