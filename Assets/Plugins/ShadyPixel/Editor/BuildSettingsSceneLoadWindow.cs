using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace ShadyPixel.Editor
{
    public class BuildSettingsSceneLoadWindow : EditorWindow
    {
        private Vector2 _scrollPosition;

        [MenuItem("Window/Shady Pixel/Scene Load Window")]
        internal static void Init()
        {
            var window = (BuildSettingsSceneLoadWindow)GetWindow(typeof(BuildSettingsSceneLoadWindow), false, "Scene Load Window");
            window.position = new Rect(window.position.xMin + 100f, window.position.yMin + 100f, 200f, 400f);
        }

        internal void OnGUI()
        {
            EditorGUILayout.BeginVertical();
            _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition, false, false);

            GUILayout.Label("Scenes In Build", EditorStyles.boldLabel);
            for (var i = 0; i < EditorBuildSettings.scenes.Length; i++)
            {
                var scene = EditorBuildSettings.scenes[i];
                if (scene.enabled)
                {
                    var sceneName = Path.GetFileNameWithoutExtension(scene.path);
                    var pressed = GUILayout.Button(i + ": " + sceneName, new GUIStyle(GUI.skin.GetStyle("Button")) { alignment = TextAnchor.MiddleLeft });
                    if (pressed)
                    {
                        if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                        {
                            EditorSceneManager.OpenScene(scene.path);
                        }
                    }
                }
            }

            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndVertical();
        }
    }
}