using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RFerzuli.DungeonGenerator.Runtime
{
    [CreateAssetMenu(menuName = "Level Design/Dungeon Data")]
    public class DungeonData : ScriptableObject
    {
        [field: SerializeField] public List<Bounds> Rooms { get; private set; } = new List<Bounds>();
    }
}
