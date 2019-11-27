using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Ultility.Localization
{
    [CreateAssetMenu(fileName = "LocalizationData", menuName = "Config/Language/LocalizationData")]
    public class LocalizationData : SerializedScriptableObject
    {
        [SerializeField, InfoBox("All Keys, Values in CSV file")]
        private Item[] _arrItems;

        public Item[] ArrItems { get => _arrItems; set => _arrItems = value; }

        [Serializable]
        public class Item
        {
            [SerializeField, ReadOnly]
            private string id;
            [SerializeField, ReadOnly]
            private string en, jp, ru, ch, fr, ko;

            public string ID { get => id; set => id = value; }
            public string EN { get => en; set => en = value; }
            public string JP { get => jp; set => jp = value; }
            public string RU { get => ru; set => ru = value; }
            public string CH { get => ch; set => ch = value; }
            public string FR { get => fr; set => fr = value; }
            public string KO { get => ko; set => ko = value; }
        }
    }
}
