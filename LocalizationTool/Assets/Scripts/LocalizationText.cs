using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Sirenix.OdinInspector;

namespace Ultility.Localization
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class LocalizationText : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI _curText;

        [SerializeField, Required("This key is required")]
        [ValueDropdown("GetKeyInDict", IsUniqueList = true)]
        private string _key;

        private static IEnumerable GetKeyInDict()
        {
            if (StaticLocalization.DictStaticLocalizationData.Count != 0)
                return StaticLocalization.DictStaticLocalizationData.Keys;

            return null;
        }

        private void OnValidate()
        {
            _curText = GetComponent<TextMeshProUGUI>();

            if (StaticLocalization.DictStaticLocalizationData.Count != 0 &&
                StaticLocalization.DictStaticLocalizationData.ContainsKey(_key))
            {
                _curText.SetText(StaticLocalization.DictStaticLocalizationData[_key]);
            }
        }

        private void Start()
        {
            SetupText();
            Localization.Instance.OnChangeLocalization += Handle_OnChangeLocalization;
        }

        private void Handle_OnChangeLocalization()
        {
            SetupText();
        }

        public void SetupText()
        {
            if (Localization.Instance.DictLocalizationData.ContainsKey(_key))
            {
                var value = Localization.Instance.DictLocalizationData[_key];
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
