using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;

namespace moe.kyre.tool4tp
{
    public static class TPLipSyncAnim
    {
        private static void CreateClip(string rendererPath, string dirPath, List<BlendShape> visemes)
        {
            for (int i = 0; i < visemes.Count; i++)
            {
                AnimationClip clip = new AnimationClip();

                for (int j = 0; j < visemes.Count; j++)
                {
                    var curve = new AnimationCurve();

                    if (i == j) curve.AddKey(0f, 100f);
                    else curve.AddKey(0f, 0f);

                    clip.SetCurve(rendererPath, typeof(SkinnedMeshRenderer), $"blendShape.{visemes[j].name}", curve);
                }

                AnimationClipSettings settings = AnimationUtility.GetAnimationClipSettings(clip);
                settings.loopTime = false;
                AnimationUtility.SetAnimationClipSettings(clip, settings);
                
                AssetDatabase.CreateAsset(clip, Path.Combine(dirPath, $"{visemes[i].name}.anim").Replace("\\", "/"));
            }

            AssetDatabase.SaveAssets();
        }
        
        public static string Create(string rendererPath, string dirPath, List<BlendShape> visemes)
        {
            CreateClip(rendererPath, dirPath, visemes);

            string createPath = Path.Combine(dirPath, "LipSync.controller").Replace("\\", "/");
            
            AnimatorController ac = AnimatorController.CreateAnimatorControllerAtPath(createPath);
            ac.AddParameter("Viseme", AnimatorControllerParameterType.Int);
            ac.AddParameter("Voice", AnimatorControllerParameterType.Float);
            
            AnimatorStateMachine stateMachine = ac.layers[0].stateMachine;

            foreach (var viseme in visemes)
            {
                AnimationClip clip = AssetDatabase.LoadAssetAtPath<AnimationClip>(Path.Combine(dirPath, $"{viseme.name}.anim").Replace("\\", "/"));
                AnimatorState state = stateMachine.AddState(TPBlendShapes.Visemes.FirstOrDefault(v => viseme.name.Contains(v)));
                
                state.motion = clip;
                
                // Setting Motion Time to "Voice"
                state.timeParameter = "Voice";
                state.timeParameterActive = true;
                state.writeDefaultValues = false;
                
                // 黄色いやつを設定
                if (viseme.name.Contains("sil")) stateMachine.defaultState = state;

                stateMachine.AddEntryTransition(state);
                state.AddExitTransition();
            }

            AssetDatabase.SaveAssets();

            return createPath; 
        }
    }
}
