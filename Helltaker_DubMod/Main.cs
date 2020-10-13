using System.IO;
using System.Threading;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System;
using BepInEx;
using System.Linq;
using HarmonyLib;
using System.Reflection;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Helltaker_DubMod
{
    [BepInPlugin("com.hootoh.helltaker_mod.nsfw_dub_mod", "Helltaker Dub Mod", "1.0.0.0")]
    public class Main : BaseUnityPlugin
    {
        public static Main instance;
        public static string[] excludedDialogueFiles;
        public static string currentDirectory;
        public static AudioSource audioSource;

        void Awake()
        {
            instance = this;
            currentDirectory = Directory.GetCurrentDirectory();

            /*
            * 'm' closing credits
            * 'm2' file contains ONLY chapters names
            */
            string[] excludedDialogueFilesArray = { "m", "m2" };
            excludedDialogueFiles = excludedDialogueFilesArray;

            var harmony = new Harmony("com.hootoh.helltaker_mod.audio_mod");
            var assembly = Assembly.GetExecutingAssembly();
            harmony.PatchAll(assembly);

            this.gameObject.AddComponent<AudioSource>();
            audioSource = GetComponent<AudioSource>();
        }

        IEnumerator LoadAudio(string path)
        {
            if (System.IO.File.Exists(path))
            {
                using (var www = new WWW(path))
                {
                    yield return www;
                    AudioClip voiceDialogue = www.GetAudioClip();
                    audioSource.clip = voiceDialogue;
                    audioSource.Play();
                }
            } else
            {
                Debug.LogError("Couldn't find the audio file. File: "+path);
            }
        }
        internal void playAudio(string path)
        {
            StartCoroutine(LoadAudio(path));
        }

    }
}