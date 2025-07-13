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
        public GameObject RockObject;

        private int tileScorePoints = 1;
        private bool hasBonus = false;
        private bool hasRocks = false;

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (hasRocks) return;
            
            OnTileSelected?.Invoke(this);
        }

        public void SetTileText(GridData gridData)
        {
            LetterIndex = GameCommonData.GetLetterIndex(gridData.letter);
            if (LetterIndex < 1 || LetterIndex > 26)
            {
                GameCommonData.ShowDebugLog($"Invalid letter index: {LetterIndex} for letter: {gridData.letter}", 2);
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

            if (gridData.tileType == 2)
            {
                hasRocks = true;
                RockObject?.SetActive(true);
            }
        }

        public void SetTileTextRandom()
        {
            // LetterIndex = UnityEngine.Random.Range(1, 26);
            LetterIndex = GameCommonData.WeightedRandom.GetRandomItem();
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

        public void SetRockState(bool Value)
        {
            if (!hasRocks) return;

            hasRocks = Value;
            RockObject?.SetActive(Value);
        }
    }
}
