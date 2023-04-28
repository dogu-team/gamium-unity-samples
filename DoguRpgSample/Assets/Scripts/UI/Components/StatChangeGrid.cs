using System;
using System.Collections.Generic;
using Data;
using Data.Static;
using UnityEngine;
using UnityEngine.UI;
using Util;

namespace UI
{
    public class StatChangeGrid : MonoBehaviour
    {
        public class Row
        {
            public string name;
            public string befValue;
            public string afterValue;
        }

        public VerticalLayoutGroup layoutGroup;
        [NonSerialized] public List<Row> rows = new List<Row>();

        public void Refresh()
        {
            TransformUtil.DestroyChildren(layoutGroup.transform);

            var rowPrefab = UIPrefabs.Instance.statChangeRowPrefab;
            foreach (var row in rows)
            {
                var go = Instantiate(rowPrefab.gameObject, layoutGroup.transform, false);
                var rowComp = go.GetComponent<StatChangeRow>();
                rowComp.fieldNameText.text = row.name;
                rowComp.beforeText.text = row.befValue;
                rowComp.afterText.text = row.afterValue;
            }
        }
    }
}