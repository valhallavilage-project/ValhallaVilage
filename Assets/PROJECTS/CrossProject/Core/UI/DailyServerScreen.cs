using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace CrossProject.Core
{
    public class DailyServerScreen : MonoBehaviour
    {
        [SerializeField] private TMP_Dropdown serverChosse;

        private int index;

        private void Awake()
        {
            serverChosse.onValueChanged.AddListener((int value) =>
            {
                index = value;
            });
        }

        public void Open()
        {
            gameObject.SetActive(true);
        }

        public void Close()
        {
            gameObject.SetActive(false);
        }

        public async UniTask<int> GetIndex(ServerList[] serverLists)
        {
            index = -1;
            List<TMP_Dropdown.OptionData> options = new List<TMP_Dropdown.OptionData>();
            foreach (var item in serverLists)
            {
                options.Add(new TMP_Dropdown.OptionData(item.name));
            }
            serverChosse.options = options;

            while(index == -1)
            {
                await UniTask.Yield();
            }

            return index;
        }
    }
}
