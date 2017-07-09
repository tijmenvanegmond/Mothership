using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public enum NodeName
    {
        prisma = 0,
        cube = 1,
        cylinder = 2,
        slope = 3
    }

    public class NodeController : MonoBehaviour
    {
    

        [Serializable]
        public struct NamedGameObject
        {
            public string name;
            public GameObject GO;
        }
        public NamedGameObject[] NamedGameObjectArray;
        public static Dictionary<string, GameObject> nodes = new Dictionary<string, GameObject>();



        public void Awake()
        {
            if (nodes.Count == 0)
            {
                foreach (NamedGameObject namedGO in NamedGameObjectArray)
                {
                    nodes.Add(namedGO.name, namedGO.GO);
                }
            }
        }
    }
}