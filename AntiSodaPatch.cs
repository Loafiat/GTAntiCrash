using HarmonyLib;
using UnityEngine;
using Photon.Pun;
using GorillaTag;
using AntiCrash;

namespace AntiSodaPatch
{
    [HarmonyPatch(typeof(ScienceExperimentPlatformGenerator), "SpawnSodaBubbleLocal")]
    internal class AntiSodaPatch
	{
		private static bool Prefix()
		{
            return Plugin.basementLoaded;
        }
	}

    [HarmonyPatch(typeof(ScienceExperimentPlatformGenerator), "SpawnSodaBubbleRPC")]
    internal class GetSenderName
    {
        private static void Postfix(PhotonMessageInfo info)
        {
            if (Plugin.CrashConfig.ReportUserName.Value)
            {
                if (!Plugin.basementLoaded)
                {
                    Debug.LogError(info.Sender.NickName + " is attempting to crash your game!");
                }
            }
        }
    }
}