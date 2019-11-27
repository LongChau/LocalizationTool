using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ultility.Localization
{
    public class FakePlayerData : MonoBehaviour
    {
        [SerializeField]
        private LocalizationText _txtPlayerData;

        // Start is called before the first frame update
        void Start()
        {
            _txtPlayerData.DictValues.Add(0, "200");
            _txtPlayerData.DictValues.Add(1, "300");
            _txtPlayerData.DictValues.Add(2, "LongChauTest");

            _txtPlayerData.UpdateUI();
        }
    }
}
