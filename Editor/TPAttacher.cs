using System;
using System.Linq;
using JetBrains.Annotations;
using System.Collections.Generic;
using UnityEngine;
using nadena.dev.modular_avatar.core;
using VRC.SDK3.Avatars.Components;

namespace moe.kyre.tool4tp
{
    public static class TPAttacher
    {
        public static void BoneProxy (GameObject obj)
        {
            return;
        }

        public static void BlendShapeSync (GameObject local, GameObject reference)
        {
            GameObject avatar = nadena.dev.ndmf.runtime.RuntimeUtil.FindAvatarInParents(reference.transform).gameObject;
            string relativePath = nadena.dev.ndmf.runtime.RuntimeUtil.RelativePath(avatar, local);
            
            List<BlendshapeBinding> bindings = TPWindow.blendShapes
                .Select(blendshape => new BlendshapeBinding
                {
                    ReferenceMesh = new AvatarObjectReference
                    {
                        referencePath = relativePath
                    },
                    Blendshape = blendshape.name,
                    LocalBlendshape = blendshape.name
                }).ToList();

            var component = local.AddComponent<ModularAvatarBlendshapeSync>();

            if (component != null)
            {
                component.Bindings = bindings;
            }

            return;
        }

        public static void MergeAnimator (GameObject obj)
        {
            return;
        }
    }
}
