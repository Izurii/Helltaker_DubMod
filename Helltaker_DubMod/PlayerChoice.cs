using BepInEx;
using HarmonyLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Helltaker_DubMod
{
    [HarmonyPatch(typeof(GoalSprite), "Update")]
    class PlayerChoice : BaseUnityPlugin
    {
        private static bool waitingForChoice;
        private static bool submitBlock;
        private static string playerChoiceIndex;
        static void Prefix(GoalSprite __instance)
        {

            var txtDir = __instance.GetType().GetField("txtDir", BindingFlags.Public | BindingFlags.Instance);
            var txtDirValue = txtDir.GetValue(__instance);
            var txtName = txtDirValue.ToString().Split('\\')[2].Split('.')[0];
            
            /*
             * Files starting with 'm', '10' or that contains 'h', don't contain any choices that needs to be dubbed
             * or don't have any choices at all
             * '6h' is an exception
             */
            if ((txtName.ToLower().Contains('h') && txtName.ToLower() != "6h") ||
                txtName.StartsWith("10") || txtName.StartsWith("m")
            )
                return;

            var waitingForChoiceField = __instance.GetType().GetField("waitingForChoice", BindingFlags.NonPublic | BindingFlags.Instance);
            waitingForChoice = (bool)waitingForChoiceField.GetValue(__instance);

            var submitBlockField = __instance.GetType().GetField("submitBlock", BindingFlags.NonPublic | BindingFlags.Instance);
            submitBlock = (bool)submitBlockField.GetValue(__instance);

            if (waitingForChoice)
            {
                if (Input.GetButton("Submit"))
                {
                    if (!submitBlock)
                    {
                        
                        //Debug.LogWarning("Player choice index: "+ playerChoiceIndex);

                        var dialogueIndexField = __instance.GetType().GetField("dialogueIndex", BindingFlags.NonPublic | BindingFlags.Instance);
                        playerChoiceIndex = dialogueIndexField.GetValue(__instance).ToString();
                        
                        var voiceAudioPath = Main.currentDirectory + "\\audio\\" + txtName + "\\answer_" + playerChoiceIndex + ".wav";
                        Main.instance.playAudio(voiceAudioPath);

                    }
                }
            }
        }

        
    }
}
