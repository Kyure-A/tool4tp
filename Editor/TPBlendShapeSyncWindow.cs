using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using nadena.dev.modular_avatar.core;
using VRC.SDK3.Avatars.Components;

namespace moe.kyre.tool4tp
{
    public class TPBlendShapeSyncWindow : EditorWindow
    {
        private SkinnedMeshRenderer local = null;
        private SkinnedMeshRenderer reference = null;
        private Vector2 scrollPos = Vector2.zero;
        
        public struct BlendShape
        {
            public string name;
            public int index;
        }
        
        public static List<BlendShape> blendShapes = new List<BlendShape>();
        
        public void UpdateBlendShapeNames(SkinnedMeshRenderer local)
        {
            blendShapes = new List<BlendShape>();
            
            Mesh mesh = local.sharedMesh;
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
            var editorLocal = (SkinnedMeshRenderer)EditorGUILayout.ObjectField("舌ピアスなど", (SkinnedMeshRenderer)local, typeof(SkinnedMeshRenderer), true);
            var editorReference = (SkinnedMeshRenderer)EditorGUILayout.ObjectField("元メッシュ", (SkinnedMeshRenderer)reference, typeof(SkinnedMeshRenderer), true);

            if (local == null && editorLocal != null)
            {
                local = editorLocal;
                UpdateBlendShapeNames(local);
            }

            if (reference == null && editorReference != null)
            {
                reference = editorReference;
            }
            
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

                if (reference != null)
                {
                    if (GUILayout.Button("設定する"))
                    {
                        TPAttacher.BlendShapeSync(editorLocal, reference);
                    }
                }
            }
        }
    }
}
