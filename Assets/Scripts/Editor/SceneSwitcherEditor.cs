using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcherEditor : EditorWindow
{
    [MenuItem("Window/Scene Switcher")]
    public static void ShowWindow()
    {
        GetWindow<SceneSwitcherEditor>("Scene Switcher");
    }

    [System.Obsolete]
    private void OnGUI() 
    {
        GUILayout.Label("Scenes in Build Settings:", EditorStyles.boldLabel);

        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            string scenePath = SceneUtility.GetScenePathByBuildIndex(i);
            string sceneName = System.IO.Path.GetFileNameWithoutExtension(scenePath);

            if(GUILayout.Button(sceneName))
            {
                if(EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                {
                    EditorSceneManager.OpenScene(scenePath);
                }
            }
        }    
    }
}
