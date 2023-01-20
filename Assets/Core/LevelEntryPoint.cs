using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Afterlife.Core
{
    public class LevelEntryPoint : MonoBehaviour
    {
        [field: SerializeField]
        public string Id { get; private set; }

        [field: SerializeField]
        public Transform SpawnOffset { get; private set; }
    }
}
