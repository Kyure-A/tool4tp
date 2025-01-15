using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;

namespace moe.kyre.tool4tp
{
    public static class TPLipSyncAnim
    {
        private static void CreateClip(string dirPath, List<BlendShape> visemes)
        {
            for (int i = 0; i < visemes.Count; i++)
            {
                AnimationClip clip = new AnimationClip();

                for (int j = 0; j < visemes.Count; j++)
                {
                    var curve = new AnimationCurve();

                    if (i == j) curve.AddKey(0f, 100f);
                    else curve.AddKey(0f, 0f);

                    clip.SetCurve("", typeof(SkinnedMeshRenderer), visemes[j].name, curve);
                }

                AnimationClipSettings settings = AnimationUtility.GetAnimationClipSettings(clip);
                settings.loopTime = false;
                AnimationUtility.SetAnimationClipSettings(clip, settings);

                AssetDatabase.CreateAsset(clip, $"{dirPath}/{visemes[i].name}.anim");
            }

            AssetDatabase.SaveAssets();
        }

        public static void Create(string dirPath, List<BlendShape> visemes)
        {
            CreateClip(dirPath, visemes);
            
            AnimatorController ac = AnimatorController.CreateAnimatorControllerAtPath($"{dirPath}/LipSync.controller");
            ac.AddParameter("Viseme", AnimatorControllerParameterType.Int);
            ac.AddParameter("Voice", AnimatorControllerParameterType.Float);
            
            AnimatorStateMachine stateMachine = ac.layers[0].stateMachine;
            
            string sil = visemes.First(v => v.name.Contains("sil")).name;
            
            foreach (var viseme in visemes)
            {
                AnimationClip visemeClip = AssetDatabase.LoadAssetAtPath<AnimationClip>($"{dirPath}/{viseme.name}.anim");
                AnimatorState visemeState = stateMachine.AddState(viseme.name);
                visemeState.motion = visemeClip;
            }
        }
    }
}
