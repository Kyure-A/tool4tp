using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using nadena.dev.modular_avatar.core;
using VRC.SDK3.Avatars.Components;

namespace moe.kyre.tool4tp
{
    public class TPWindow : EditorWindow
    {
        private GameObject local;
        private Vector2 scrollPos = Vector2.zero;
        
        public struct BlendShape
        {
            public string name;
            public int index;
        }
        
        public static List<BlendShape> blendShapes = new List<BlendShape>();
        
        public void UpdateBlendShapeNames(GameObject local)
        {
            blendShapes = new List<BlendShape>();
            
            var renderer = local.GetComponent<SkinnedMeshRenderer>();
            if (renderer == null) return;
            
            Mesh mesh = renderer.sharedMesh;
            if (mesh == null) return;

            // for でかくのださいからもうちょい map とか使いたいけど
            for (int i = 0; i < mesh.blendShapeCount; i++)
            {
                string name = mesh.GetBlendShapeName(i);
                
                blendShapes.Add(new BlendShape
                {
                    name = name,
                    index = i,
                });
            }
        }

        public void RemoveBlendShapeNames(string name)
        {
            blendShapes.RemoveAll(x => x.name == name);
        }        
        
        [MenuItem("Tools/Tool for make Tongue Piercing")]
        public static void ShowWindow()
        {
            GetWindow<TPWindow>("tool4tp");
        }
    
        private void OnGUI()
        {
            EditorGUILayout.BeginHorizontal();        
            var editorLocal = (GameObject)EditorGUILayout.ObjectField("Object", (GameObject)local, typeof(GameObject), true);        
            EditorGUILayout.EndHorizontal();
            
            if (local != null)
            {
                if (local != editorLocal)
                {
                    local = editorLocal;
                    UpdateBlendShapeNames(local);
                }

                scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
                for (int i = 0; i < blendShapes.Count; i++)
                {
                    EditorGUILayout.BeginHorizontal();
            
                    bool state = EditorGUILayout.Toggle(true, GUILayout.Width(20));
            
                    if (!state)
                    {
                        RemoveBlendShapeNames(blendShapes[i].name);
                        continue;
                    }

                    EditorGUILayout.LabelField(blendShapes[i].name);
            
                    EditorGUILayout.EndHorizontal();
                }
                EditorGUILayout.EndScrollView();
            }
        }
    }
}
