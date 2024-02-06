using HarmonyLib;
using UnityEngine;
using Photon.Pun;
using GorillaTag;

namespace AntiSodaPatch
{
    [HarmonyPatch(typeof(ScienceExperimentPlatformGenerator), "SpawnSodaBubbleLocal")]
    internal class AntiSodaPatch
	{
		private static bool Prefix()
		{
            return false;
        }
	}

    [HarmonyPatch(typeof(ScienceExperimentPlatformGenerator), "SpawnSodaBubbleRPC")]
    internal class GetSenderName
    {
        private static void Postfix(PhotonMessageInfo info)
        {
            Debug.LogError(info.Sender.ToString() + "is attempting to crash your game!");
        }
    }
}