using System;
using BepInEx;
using Photon.Pun.UtilityScripts;
using UnityEngine;
using Utilla;
using static SinglePool;

namespace AntiCrash
{
	[ModdedGamemode]
	[BepInDependency("org.legoandmars.gorillatag.utilla")]
	[BepInPlugin("Lofiat.AntiCrash", "AntiCrash", "1.0.0")]
	public class Plugin : BaseUnityPlugin
	{
        bool inRoom;
        Transform ObjectPools, SodaBubbles, playerTransform;
        SinglePool BubblePool;
        void Start() => Utilla.Events.GameInitialized += OnGameInitialized;
        void OnGameInitialized(object sender, EventArgs e)
        {
            ObjectPools = GameObject.Find("Environment Objects/PersistentObjects_Prefab/GlobalObjectPools").transform;
            playerTransform = GameObject.Find("Player Objects/Local VRRig/Local Gorilla Player").transform;
            SodaBubbles = GameObject.Find("Environment Objects/PersistentObjects_Prefab/GlobalObjectPools/SodaBubble(Clone)").transform;
            BubblePool = ObjectPools.GetComponent<ObjectPools>().GetPoolByObjectType(SodaBubbles.gameObject);
        }

        [ModdedGamemodeJoin]
        public void OnJoin(string gamemode)
        {
            inRoom = true;
            BubblePool.initAmountToPool = 0;
            BubblePool.Initialize(ObjectPools.gameObject);
        }

        void Update()
        {
            if (inRoom)
            {
                if (Time.deltaTime == 2000)
                {
                    ObjectPools.gameObject.SetActive(false);
                }
            }
        }

        [ModdedGamemodeLeave]
		public void OnLeave(string gamemode)
        {
            inRoom = true;
            foreach (Transform Bubbles in SodaBubbles)
            {
                Destroy(Bubbles.gameObject);
            }
            BubblePool.initAmountToPool = 256;
            BubblePool.Initialize(ObjectPools.gameObject);
            SodaBubbles = GameObject.Find("Environment Objects/PersistentObjects_Prefab/GlobalObjectPools/SodaBubble(Clone)").transform;
        }
    }
}