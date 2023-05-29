using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Loader
{
    public enum Scene
    {
        MainMenuScene,
        Testing, //NEED TO SWAP TO GameScene¹N
        LoadingScene
    }
    private static Scene targetScene;

    public static void Load(Scene targetSceneName)
    {
        Loader.targetScene = targetScene;

        SceneManager.LoadScene(Scene.LoadingScene.ToString());

        SceneManager.LoadScene(targetSceneName.ToString());
    }

    public static void LoaderCallback()
    {
        SceneManager.LoadScene(targetScene.ToString());
    }
}
