using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Sirenix.OdinInspector;
using System.Text.RegularExpressions;

namespace Ultility.Localization
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class LocalizationText : SerializedMonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI _curText;

        [SerializeField, Required("This key is required")]
        [ValueDropdown("GetKeyInDict", IsUniqueList = true)]
        private string _key;

        [Space(30)]

        [InfoBox("Check this if you have dynamic values")]
        [SerializeField]
        private bool _isContainsDynamicValue;

        private static IEnumerable GetKeyInDict()
        {
            if (StaticLocalization.DictStaticLocalizationData.Count != 0)
                return StaticLocalization.DictStaticLocalizationData.Keys;

            return null;
        }

        [SerializeField, ShowIf("_isContainsDynamicValue", true)]
        [DictionaryDrawerSettings(KeyLabel = "Key", ValueLabel = "Data")]
        private Dictionary<int, string> _dictValues = new Dictionary<int, string>();

        public Dictionary<int, string> DictValues { get => _dictValues; set => _dictValues = value; }

        private void OnValidate()
        {
            _curText = GetComponent<TextMeshProUGUI>();

            UpdateUI();
        }

        [Button("UpdateUI", ButtonSizes.Medium)]
        private void BtnUpdateUI()
        {
            UpdateUI();
        }

        public void UpdateUI()
        {
            if (StaticLocalization.DictStaticLocalizationData.Count != 0 &&
                StaticLocalization.DictStaticLocalizationData.ContainsKey(_key))
            {
                SetupText(StaticLocalization.DictStaticLocalizationData);
            }
        }

        private void Start()
        {
            SetupText(Localization.Instance.DictLocalizationData);
            Localization.Instance.OnChangeLocalization += Handle_OnChangeLocalization;
        }

        private void Handle_OnChangeLocalization()
        {
            SetupText(Localization.Instance.DictLocalizationData);
        }

        public void SetupText(Dictionary<string, string> dictInput)
        {
            if (dictInput.ContainsKey(_key))
            {
                var value = dictInput[_key];

                if (_isContainsDynamicValue && _dictValues != null && _dictValues.Count != 0)
                {
                    string[] passParams = new string[_dictValues.Count];
                    for (int index = 0; index < _dictValues.Count; index++)
                    {
                        passParams[index] = _dictValues[index];
                    }
                    value = string.Format(value, passParams);
                }

                _curText.SetText(value);
            }
            else
                Debug.LogError($"On {name} Key: {_key} is not contained in Localization");
        }

        /// <summary>
        /// Editor setup for test only
        /// without hit play mode
        /// </summary>
        public void EditorSetupText()
        {
            if (StaticLocalization.DictStaticLocalizationData.ContainsKey(_key))
            {
                var value = StaticLocalization.DictStaticLocalizationData[_key];
                _curText.SetText(value);
            }
            else
                Debug.LogError($"On {name} Key: {_key} is not contained in Localization");
        }
    }
}
