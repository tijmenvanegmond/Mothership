using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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
		public List<Node> nodesList;
		public List<ConnectionPointType> connectionPointTypes;

		public static Dictionary<string, GameObject> Nodes = new Dictionary<string, GameObject>();
		public static List<Node> NodeList;
		public static List<ConnectionPointType> ConnectionPointTypeList;

		public void Awake()
        {
            if (Nodes.Count == 0)
            {
                foreach (NamedGameObject namedGO in NamedGameObjectArray)
                {
                    Nodes.Add(namedGO.name, namedGO.GO);
                }
            }

			if (connectionPointTypes.GroupBy(x => x.ID).Any(z => z.Count() > 1))
				throw new Exception("Not all ConnectionPointTypes have a unique id");

			ConnectionPointTypeList = connectionPointTypes;

		}
    }
}