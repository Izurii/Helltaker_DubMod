using BepInEx;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Helltaker_DubMod
{
    [HarmonyPatch(typeof(Manager), "VolumeChange")]
    class VolumeChange : BaseUnityPlugin
    {
        static void Postfix(Manager __instance, int type, int lvl)
        {
            if (type==1)
            {
                var muteField = __instance.GetType().GetField("mute", BindingFlags.Public | BindingFlags.Instance);
                float mute = (float)muteField.GetValue(__instance);

                float num = (float)lvl / 3f;
                num *= mute;
                if (lvl == 2 || lvl == 1)
                {
                    num += 0.04f;
                }
                Main.audioSource.volume = num;
            }
        }

    }
}
