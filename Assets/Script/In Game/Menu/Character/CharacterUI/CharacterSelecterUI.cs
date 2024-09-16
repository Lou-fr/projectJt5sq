using UnityEngine;

namespace BleizEntertainment
{
    public class CharacterSelecterUI : MonoBehaviour
    {
        private int MaxCharacter = 1;
        [SerializeField] private CharacterLightUIPrefab LightUIPrefab;
        [SerializeField] private Transform CharacterContainer;
        private CharacterLightUIPrefab[] InitiatedPrefab = new CharacterLightUIPrefab[4];

        public void InitLightUI(CharacterSO[] charactersInfo, int i)
        {
            foreach (Transform child in CharacterContainer)
            {
                Destroy(child);
            }
            for (int index = 0; index < i; index++)
            {
                CharacterSO character = charactersInfo[index];
                InitiatedPrefab[index] = Instantiate(LightUIPrefab, CharacterContainer);
                InitiatedPrefab[index].Init(character, index);
            }
        }
        public void ChangeLightUI(int lastCharacter, int newCharacter)
        {
            InitiatedPrefab[lastCharacter].Deactivate();
            InitiatedPrefab[newCharacter].Activate();
        }
    }
}
