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

        public Action<Tile> OnTileSelected;

        public GameObject BonusBug;
        public GameObject[] PointsObject;

        private int tileScorePoints = 1;
        private bool hasBonus = false;

        public void OnPointerEnter(PointerEventData eventData)
        {
            OnTileSelected?.Invoke(this);
        }

        public void SetTileText(GridData gridData)
        {
            LetterIndex = GameCommonData.GetLetterIndex(gridData.letter);
            if (LetterIndex < 1 || LetterIndex > 26)
            {
                Debug.LogError($"Invalid letter index: {LetterIndex} for letter: {gridData.letter}");
                return;
            }
            LetterText.text = gridData.letter.ToUpper();

            tileScorePoints = UnityEngine.Random.Range(1, 4);
            for (int i = 1; i <= tileScorePoints; i++)
            {
                PointsObject[i - 1]?.SetActive(true);
            }

            if (gridData.tileType == 1)
            {
                hasBonus = true;
                BonusBug?.SetActive(true);
            }
        }

        public void SetTileTextRandom()
        {
            LetterIndex = UnityEngine.Random.Range(1, 26);
            var alphabet = GameCommonData.AlphabetToWordMap[LetterIndex];
            LetterText.text = alphabet;

            tileScorePoints = UnityEngine.Random.Range(1, 3);
            for (int i = 1; i <= tileScorePoints; i++)
            {
                PointsObject[i - 1]?.SetActive(true);
            }
        }

        public int GetTileScorePoints()
        {
            if (hasBonus)
            {
                return (tileScorePoints * 10) + 100;
            }
            return tileScorePoints * 10;
        }
    }
}
