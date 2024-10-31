using System;
using _Assets.Scripts.Gameplay;
using UnityEngine;

namespace _Assets.Scripts.Services.Configs
{
    [CreateAssetMenu(fileName = "Fruits Config", menuName = "Configs/Fruits")]
    public class FruitsConfig : ScriptableObject
    {
        [SerializeField] private FruitData[] fruits;
        public Fruit GetPrefab(int index) => fruits[index].Prefab;
        public int GetPoints(int index) => fruits[index].Points;
        public bool HasPrefab(int index) => fruits[index].Prefab != null;

        [Serializable]
        public struct FruitData
        {
            public Fruit Prefab;
            public int Points;

            public FruitData(Fruit prefab, int points)
            {
                Prefab = prefab;
                Points = points;
            }
        }
    }
}