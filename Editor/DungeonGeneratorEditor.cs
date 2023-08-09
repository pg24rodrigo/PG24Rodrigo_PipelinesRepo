using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using RFerzuli.DungeonGenerator.Runtime;
using UnityEditor.SceneManagement;
using UnityEditor.IMGUI.Controls;

namespace RFerzuli.DungeonGenerator.Editor
{
    [CustomEditor(typeof(Runtime.DungeonGenerator))]
    public class DungeonGeneratorEditor : UnityEditor.Editor
    {
        private BoxBoundsHandle _boundsHandles = new BoxBoundsHandle();
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            // cast generic target object to our used class
            Runtime.DungeonGenerator dungeonGenerator = (Runtime.DungeonGenerator)target;

            if (dungeonGenerator.DungeonData == null) return;

            if(GUILayout.Button("Regenerate"))
            {
                ClearAll(dungeonGenerator.transform);
                dungeonGenerator.GenerateAll();
                EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
            }

            // add the buttons on the gui
            if(GUILayout.Button("AddRoom"))
            {
                dungeonGenerator.AddRoom();

                // make sure that unity saves the changes
                EditorUtility.SetDirty(dungeonGenerator.DungeonData);

                // scene also gets saved
                EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
            }
            // add the buttons on the gui
            if (GUILayout.Button("RemoveRoom"))
            {
                dungeonGenerator.RemoveRoom();

                // make sure that unity saves the changes
                EditorUtility.SetDirty(dungeonGenerator.DungeonData);

                // scene also gets saved
                EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
            }
        }

        private void OnSceneGUI()
        {
            Runtime.DungeonGenerator dungeonGenerator = (Runtime.DungeonGenerator)target;
            if (dungeonGenerator.DungeonData == null) return;
            
            for(int i = 0; i<dungeonGenerator.Rooms.Count; i++)
            {
                Bounds room = dungeonGenerator.Rooms[i];

                // starts and saves the state for the unchanged assets 
                // if there are changes it adds 
                EditorGUI.BeginChangeCheck();
                _boundsHandles.center = room.center;
                _boundsHandles.size = room.size;
                _boundsHandles.DrawHandle();

                if(EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(dungeonGenerator.DungeonData, "Resize Room");
                    dungeonGenerator.Rooms[i] = new Bounds(_boundsHandles.center, _boundsHandles.size);
                }
            }

            // for saving we mark the data as dirty
            EditorUtility.SetDirty(dungeonGenerator.DungeonData);

        }
        
        public void ClearAll(Transform transform)
        {
            List<Transform> toDestroy = new List<Transform>();

            for (int i = transform.childCount - 1; i >= 0; i--)
            {
                Transform container = transform.GetChild(1);

                for (int j = 0; j < container.childCount; j++)
                {
                    toDestroy.Add(container.GetChild(j));
                }
            }

            foreach (Transform t in toDestroy)
            {
                DestroyImmediate(t.gameObject);
            }
        }   
    }
}