using System.Collections.Generic;
using System.Diagnostics;
using GGCustomToolbar;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using Debug = UnityEngine.Debug;
using System.IO;

namespace WOE_Common.Base_Editor
{
    public static class EditorCustomToolbar
    {
        private static bool IsCustomPlayMode = false;

        [EditorToolbarButton("Folder Icon", "Open Project Folder", 1, EditorToolbarPosition.LeftLeft)]
        public static void OpenProjectFolder()
        {
            EditorUtility.RevealInFinder(Application.dataPath);
        }

        [EditorToolbarButton("Scene", "Open Scene", 0, EditorToolbarPosition.LeftRight, true)]
        public static void OpenScene()
        {
            var a = new GenericMenu();
            List<string> scenes = new();
            foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes)
            {
                int slash = scene.path.LastIndexOf('/');
                string sceneName = slash >= 0 ? scene.path[(slash + 1)..] : scene.path;
                scenes.Add(sceneName);
                a.AddItem(new GUIContent(sceneName), false, () =>
                {
                    if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                    {
                        Debug.Log($"Opening scene: {scene.path}");
                        EditorSceneManager.OpenScene(scene.path);
                    }
                });
            }
            a.ShowAsContext();
        }

        [EditorToolbarButton("Animation Icon", "Play From Start", 0, EditorToolbarPosition.RightLeft, true)]
        public static void PlayFromStart()
        {
            if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            {
                IsCustomPlayMode = true;
                EditorApplication.EnterPlaymode();
            }
        }

        [InitializeOnLoadMethod]
        public static void OnEditorStarted()
        {
            EditorApplication.playModeStateChanged += OnPlayModeEntered;
        }

        private static void OnPlayModeEntered(PlayModeStateChange state)
        {
            if (state == PlayModeStateChange.ExitingEditMode)
            {
                if (IsCustomPlayMode)
                {
                    EditorSceneManager.playModeStartScene = AssetDatabase.LoadAssetAtPath<SceneAsset>(EditorBuildSettings.scenes[0].path);
                }
            }
            else if (state == PlayModeStateChange.EnteredEditMode)
            {
                if (IsCustomPlayMode)
                {
                    IsCustomPlayMode = false;
                    EditorSceneManager.playModeStartScene = null;
                }
            }
        }
    }
}
