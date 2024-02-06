using System;
using System.IO;
using BepInEx;
using BepInEx.Configuration;
using Photon.Pun;
using UnityEngine;
using Utilla;

namespace AntiCrash
{
	[ModdedGamemode]
	[BepInDependency("org.legoandmars.gorillatag.utilla")]
	[BepInPlugin("Lofiat.AntiCrash", "AntiCrash", "1.0.0")]
	public class Plugin : BaseUnityPlugin
	{
        bool bubbleInit = true;
        GameObject Basement;
        void Start()
        {
            Utilla.Events.GameInitialized += OnGameInitialized;
            CrashConfig.Initialize();
            if (CrashConfig.Debug.Value)
            {
                Application.wantsToQuit += NoQuitRightAngleBracketColonOpenParenthesis;
            }
        }
        void OnGameInitialized(object sender, EventArgs e)
        {
            Basement = GameObject.Find("Environment Objects/LocalObjects_Prefab/Basement").gameObject;
        }

        static bool NoQuitRightAngleBracketColonOpenParenthesis()
        {
            if (PhotonNetwork.InRoom)
            {
                Debug.Log("Prevented crash handler.");
                return false;
            }
            else
            {
                return true;
            }
        }

        private void DisableBubbles()
        {
            if (bubbleInit)
            {
                bubbleInit = false;
                HarmonyPatches.ApplyHarmonyPatches();
            }
        }

        void Update()
        {
            if (!Basement.activeSelf)
            {
                DisableBubbles();
            }
            else if (Basement.activeSelf)
            {
                EnableBubbles();
            }
        }

        private void EnableBubbles()
        {
            if (!bubbleInit)
            {
                bubbleInit = true;
                HarmonyPatches.RemoveHarmonyPatches();
            }
        }
        public static class CrashConfig
        {
            public static ConfigFile ConfigFile { get; private set; }
            public static ConfigEntry<bool> Debug;
            public static void Initialize()
            {
                ConfigFile = new ConfigFile(Path.Combine(Paths.ConfigPath, "AntiCrash.cfg"), true);
                Debug = ConfigFile.Bind("Configuration", "Enable Debug Features", false, "Enables Debug Features.");
            }
        }
    }
}