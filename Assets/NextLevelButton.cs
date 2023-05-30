using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevelButton : MonoBehaviour
{
    private void OnEnable()
    {
        //Check next scene kalau tidak ada, sembunyikan button ini
        var currentScene = SceneManager.GetActiveScene();
        int currentLevel = int.Parse(s: currentScene.name.Split(separator: "Level ")[1]);
        int nextLevel = currentLevel + 1;

        var nextSceneBuildIndex = SceneUtility.GetBuildIndexByScenePath(scenePath: "Level " + nextLevel);
        if (nextSceneBuildIndex == -1)
            this.gameObject.SetActive(value: false);
    }

    public void NextLevel()
    {
        var currentScene = SceneManager.GetActiveScene();
        int currentLevel = int.Parse(s: currentScene.name.Split(separator: "Level ")[1]);
        int nextLevel = currentLevel + 1;
        SceneManager.LoadScene("Level " + nextLevel);
    }
}
