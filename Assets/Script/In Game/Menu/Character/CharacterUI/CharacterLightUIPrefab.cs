using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace BleizEntertainment
{
    public class CharacterLightUIPrefab :MonoBehaviour
    {
        [SerializeField]TextMeshProUGUI characterName;
        [SerializeField]Image ActiveBackground,characterProfile;
        [SerializeField] Slider healthBar;
        private int UltPercentage,Index;
        private bool activate = false;
        Color BaseColor;

        public void Init(PlayerSO charactergeneral, int Index)
        {
            characterName.SetText(charactergeneral.CharacterInfoData.ChatacterName);
            UltPercentage = 0;
            this.gameObject.name = this.gameObject.name+charactergeneral.CharacterInfoData.ChatacterName+Index;
            healthBar.value = 1;
            CharacterOffHandler.healthPercetange += ChangeDisplayedHealth;
            if (Index == 0) { activate = true;return; }
            BaseColor = ActiveBackground.color;
            BaseColor.a = 0;
            ActiveBackground.color = BaseColor;
        }
        private void OnDestroy()
        {
            CharacterOffHandler.healthPercetange -= ChangeDisplayedHealth;
        }
        public void Deactivate()
        {
            activate=false;
            BaseColor = ActiveBackground.color;
            BaseColor.a = 0;
            ActiveBackground.color = BaseColor;
        }
        public void Activate()
        {
            activate = true;
            BaseColor = ActiveBackground.color;
            BaseColor.a = 0.4705882f;
            ActiveBackground.color = BaseColor;
        }

        void ChangeDisplayedHealth(float healthPercetange,bool healOverwrite = false)
        {
            if(!activate && !healOverwrite) return;
            Debug.Log(healthPercetange / 100);
            healthBar.value = healthPercetange / 100;
        }
    }
}
