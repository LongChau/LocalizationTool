using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System.IO;
using System;
using UnityEditor;

namespace Ultility.Localization
{
    public class Localization : SerializedMonoBehaviour
    {
        [FilePath, SerializeField, Required]
        private string _languageFolderPath;
        [FilePath, SerializeField]
        private string _exportAssetPath;

        [SerializeField, HideInInspector]
        private ELocalizationLanguage _localizationLanguage;

        [SerializeField]
        private LocalizationData _localizationData;

        [ReadOnly]
        [SerializeField, DictionaryDrawerSettings(KeyLabel = "Key", ValueLabel = "Data"), PropertyOrder(Order = 99)]
        private Dictionary<string, string> _dictLocalizationData = new Dictionary<string, string>();

        [ShowInInspector]
        public ELocalizationLanguage LocalizationLanguage
        {
            get => _localizationLanguage;
            set
            {
                //Debug.Log($"Change language to {value}");
                _localizationLanguage = value;

                // load new localization files
                LoadLocalizationFiles();

#if UNITY_EDITOR
                //Use this for quick test without play the game
                if (!EditorApplication.isPlaying)
                {
                    var customTexts = Resources.FindObjectsOfTypeAll<LocalizationText>();
                    foreach (var item in customTexts)
                    {
                        item.EditorSetupText();
                    }

                    return;
                }
#endif

                OnChangeLocalization?.Invoke();
            }
        }

        private static Localization _instance;
        public static Localization Instance { get => _instance; set => _instance = value; }

        public Dictionary<string, string> DictLocalizationData => _dictLocalizationData;

        public event Action OnChangeLocalization;

        private void Awake()
        {
            if (_instance == null)
                _instance = this;
            else if (_instance != this)
                Destroy(this);

            DontDestroyOnLoad(gameObject);
        }

        // for running on editor only
        private void OnValidate()
        {
            LoadLocalizationFiles();
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        [Button("LoadLocalizationFiles", ButtonSizes.Medium)]
        public void LoadLocalizationFiles()
        {
            if (_localizationData == null)  // check if data is null
            {
                _localizationData = AssetDatabase.LoadAssetAtPath<LocalizationData>(_exportAssetPath);
                if (_localizationData == null)  // check if cannot find that path
                {
                    // tell user to import again
                    Debug.LogError($"Cannot find language asset at path: {_exportAssetPath}");
                    Debug.Log($"Please check path {_languageFolderPath}");
                    return;
                }
            }

            if (_dictLocalizationData == null)
                _dictLocalizationData = new Dictionary<string, string>();

            GetLocalizationValueBaseOnKey(_localizationLanguage);
        }

        private void GetLocalizationValueBaseOnKey(ELocalizationLanguage langType)
        {
            _dictLocalizationData.Clear();

            foreach (var item in _localizationData.ArrItems)
            {
                //TODO: If there are more languages.
                // please add here
                switch (langType)
                {
                    case ELocalizationLanguage.EN:
                        _dictLocalizationData.Add(item.ID, item.EN);
                        break;
                    case ELocalizationLanguage.VN:
                        //_dictLocalizationData.Add(item.ID, item.VN);
                        break;
                    case ELocalizationLanguage.DEU:
                        //_dictLocalizationData.Add(item.ID, item.DEU);
                        break;
                    case ELocalizationLanguage.JP:
                        _dictLocalizationData.Add(item.ID, item.JP);
                        break;
                    case ELocalizationLanguage.RU:
                        _dictLocalizationData.Add(item.ID, item.RU);
                        break;
                    case ELocalizationLanguage.CH:
                        _dictLocalizationData.Add(item.ID, item.CH);
                        break;
                    case ELocalizationLanguage.FR:
                        _dictLocalizationData.Add(item.ID, item.FR);
                        break;
                    case ELocalizationLanguage.KO:
                        _dictLocalizationData.Add(item.ID, item.KO);
                        break;
                    default:
                        break;
                }
            }

            if (_dictLocalizationData.Count == 0)
                Debug.LogError($"Cannot find this language {langType.ToString()}");
            else
            {
#if UNITY_EDITOR
                StaticLocalization.DictStaticLocalizationData = _dictLocalizationData;
#endif
            }
        }

        [Button("Clear localization data", ButtonSizes.Medium)]
        public void ClearDict()
        {
            DictLocalizationData.Clear();
            _localizationData = null;
            _languageFolderPath = "";
            _exportAssetPath = "";
        }

#if UNITY_EDITOR
        [EnableIf("@this._localizationData == null")]
        [Button("ConvertCSVToAsset", ButtonSizes.Medium)]
        public void ConvertCSVToAsset()
        {
            Debug.Log("ConvertCSVToAsset()");

            if (_languageFolderPath.Contains(".csv"))
            {
                TextAsset data = AssetDatabase.LoadAssetAtPath<TextAsset>(_languageFolderPath);

                string assetfile = _languageFolderPath.Replace(".csv", ".asset");

                _localizationData = AssetDatabase.LoadAssetAtPath<LocalizationData>(assetfile);

                if (_localizationData == null)
                {
                    _localizationData = new LocalizationData();
                }

                _localizationData.ArrItems = CSVSerializer.Deserialize<LocalizationData.Item>(data.text);

                if (string.IsNullOrEmpty(_exportAssetPath))
                    AssetDatabase.CreateAsset(_localizationData, assetfile);
                else
                    AssetDatabase.CreateAsset(_localizationData, _exportAssetPath);

                EditorUtility.SetDirty(_localizationData);
                AssetDatabase.SaveAssets();

                Debug.Log("Converted success)");
            }
        }
#endif
    }
}
