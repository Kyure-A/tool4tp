using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;
using nadena.dev.modular_avatar.core;
using VRC.SDK3.Avatars.Components;

namespace moe.kyre.tool4tp
{
    public class TPLipSyncWindow : EditorWindow
    {
        private SkinnedMeshRenderer local = null;
        public static List<BlendShape> blendShapes = new List<BlendShape>();
        
        [MenuItem("Tools/tool4tp/LipSync")]
        public static void ShowWindow()
        {
            GetWindow<TPLipSyncWindow>("tool4tp/LipSync");
        }
        
        private void OnGUI()
        {
            var editorLocal = (SkinnedMeshRenderer)EditorGUILayout.ObjectField("舌ピアスなど", (SkinnedMeshRenderer)local, typeof(SkinnedMeshRenderer), true);

            if (editorLocal != null && local == null) local = editorLocal;
            
            if (local != null)
            {
                blendShapes = TPBlendShapes.GetBlendShapes(local);

                var visemes = blendShapes.Where(s => TPBlendShapes.isViseme(s)).ToList();
                
                foreach (var bs in visemes)
                {
                    EditorGUILayout.BeginHorizontal();
                    
                    bool state = EditorGUILayout.Toggle(true, GUILayout.Width(20));
                    
                    EditorGUILayout.LabelField(bs.name);
                    
                    EditorGUILayout.EndHorizontal();
                }
            }
        }
    }
}
