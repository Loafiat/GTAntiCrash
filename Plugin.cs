using System;
using System.IO;
using BepInEx;
using BepInEx.Configuration;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utilla;

namespace AntiCrash
{
	[ModdedGamemode]
	[BepInDependency("org.legoandmars.gorillatag.utilla")]
	[BepInPlugin("Lofiat.AntiCrash", "AntiCrash", "1.0.0")]
	public class Plugin : BaseUnityPlugin
	{
        public static bool basementLoaded = false;
        void Start()
        {
            Utilla.Events.GameInitialized += OnGameInitialized;
            SceneManager.sceneLoaded += BasementToggle;
            HarmonyPatches.ApplyHarmonyPatches();
            CrashConfig.Initialize();
            if (CrashConfig.Debug.Value)
            {
                Application.wantsToQuit += NoQuitRightAngleBracketColonOpenParenthesis;
            }
        }
        void OnGameInitialized(object sender, EventArgs e)
        {
            HarmonyPatches.ApplyHarmonyPatches();
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

        private void BasementToggle(Scene scene, LoadSceneMode mode)
        {
            if (scene.name == "Basement")
            {
                basementLoaded = true;
            }
            else
            {
                basementLoaded = false;
            }
        }
        
        public static class CrashConfig
        {
            public static ConfigFile ConfigFile { get; private set; }
            public static ConfigEntry<bool> Debug;
            public static ConfigEntry<bool> ReportUserName;
            public static void Initialize()
            {
                ConfigFile = new ConfigFile(Path.Combine(Paths.ConfigPath, "AntiCrash.cfg"), true);
                Debug = ConfigFile.Bind("Configuration", "Enable Debug Features", false, "Enables Debug Features.");
                ReportUserName = ConfigFile.Bind("Configuration", "Report Crasher Username", false, "Outputs Crasher Username to console. (CAUSES SPAM!)");
            }
        }
    }
}