using System;
using System.Collections.Generic;
using System.Linq;
using Data.Static;
using UnityEngine;

namespace Components
{
    public class CharacterInfoController : MonoBehaviour
    {
        public Data.Static.CharacterInfo info;

        [NonSerialized] public Dictionary<CharacterActionType, CharacterActionBase> actionsIndex =
            new Dictionary<CharacterActionType, CharacterActionBase>();

        private void Awake()
        {
            actionsIndex = info.actions.ToDictionary(a => a.type, a => a.action);
        }
    }
}