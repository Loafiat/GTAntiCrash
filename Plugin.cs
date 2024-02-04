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
        bool inRoom, bubbleInit;
        Transform ObjectPools, SodaBubbles, Basement;
        SinglePool BubblePool;
        void Start() => Utilla.Events.GameInitialized += OnGameInitialized;
        void OnGameInitialized(object sender, EventArgs e)
        {
            bubbleInit = true;
            ObjectPools = GameObject.Find("Environment Objects/PersistentObjects_Prefab/GlobalObjectPools").transform;
            BubblePool = ObjectPools.GetComponent<ObjectPools>().GetPoolByObjectType(GameObject.Find("Environment Objects/PersistentObjects_Prefab/GlobalObjectPools/SodaBubble(Clone)"));
            Basement = GameObject.Find("Environment Objects/LocalObjects_Prefab/Basement").transform;
        }

        private void DestroyBubbles()
        {
            if (bubbleInit)
            {
                bubbleInit = false;
                BubblePool.initAmountToPool = 0;
                BubblePool.Initialize(ObjectPools.gameObject);
            }
        }

        void Update()
        {
            if (Basement.gameObject.activeSelf == false)
            {
                DestroyBubbles();
            }
            else if (Basement.gameObject.activeSelf == true)
            {
                CreateBubbles();
            }
        }

        private void CreateBubbles()
        {
            if (!bubbleInit)
            {
                bubbleInit = true;
                SodaBubbles = GameObject.Find("Environment Objects/PersistentObjects_Prefab/GlobalObjectPools/SodaBubble(Clone)").transform;
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
}