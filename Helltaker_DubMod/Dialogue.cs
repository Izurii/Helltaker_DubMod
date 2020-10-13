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
    [HarmonyPatch(typeof(GoalSprite), "Dialogue")]
    public class Dialogue : BaseUnityPlugin
    {
        private static string filename;
        private static dialogueElement[] dialogueArray;
        static void Prefix(GoalSprite __instance, int a)
        {

            //TO-DO
            // List of excluded audio indexes, lines with nothing wrote just, dots or something like that
            // '9_4' file index 8,9,10 is just '...' I don't think this need to be dubbed

            var txtDir = __instance.GetType().GetField("txtDir", BindingFlags.Public | BindingFlags.Instance);
            var txtDirValue = txtDir.GetValue(__instance);
            var txtName = txtDirValue.ToString().Split('\\')[2].Split('.')[0];

            if (Main.excludedDialogueFiles.Contains(txtName))
                return;

            var dialogueArrayField = __instance.GetType().GetField("dialogueArray", BindingFlags.Public | BindingFlags.Instance);
            dialogueArray = (dialogueElement[])dialogueArrayField.GetValue(__instance);
            
            if (dialogueArray[a].talkText[0] != -1)
            {

                if (dialogueArray[a].talkText.Length > 1)
                {
                    filename = dialogueArray[a].talkText[0] + "_" + dialogueArray[a].talkText[1];
                    //Debug.LogWarning(dialogueArray[a].talkText[0] + " & " + dialogueArray[a].talkText[1]);
                }
                else
                {
                    filename = dialogueArray[a].talkText[0].ToString();
                    //Debug.LogWarning(dialogueArray[a].talkText[0]);
                }
            }
            else
            {
                return;
            }

            var voiceAudioPath = Main.currentDirectory + "\\audio\\" + txtName + "\\" + filename + ".wav";
            Main.instance.playAudio(voiceAudioPath);

        }
    }
}
