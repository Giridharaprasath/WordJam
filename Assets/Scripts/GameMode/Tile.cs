using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace WordJam
{
    public class Tile : MonoBehaviour, IPointerEnterHandler
    {
        public int TileIndex;
        public int LetterIndex;
        public TMP_Text LetterText;
        public bool IsSelected;

        public Action<Tile> OnTileSelected;

        public void OnPointerEnter(PointerEventData eventData)
        {
            OnTileSelected?.Invoke(this);
        }

        public void SetTileText(string letter)
        {
            LetterIndex = GameCommonData.GetLetterIndex(letter);
            if (LetterIndex < 1 || LetterIndex > 26)
            {
                Debug.LogError($"Invalid letter index: {LetterIndex} for letter: {letter}");
                return;
            }
            LetterText.text = letter.ToUpper();
        }

        public void SetTileTextRandom()
        {
            // TODO : SET LETTER INDEX FROM LEVEL DATA
            LetterIndex = UnityEngine.Random.Range(1, 26);
            var alphabet = GameCommonData.AlphabetToWordMap[LetterIndex];
            LetterText.text = alphabet;
        }
    }
}
