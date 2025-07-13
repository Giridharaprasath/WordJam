using System.Collections.Generic;

namespace WordJam
{
    public static class GameCommonData
    {
        public static IDictionary<int, string> AlphabetToWordMap { get; private set; } = new Dictionary<int, string>
        {
            { 1, "A" }, { 2, "B" }, { 3, "C" }, { 4, "D" }, { 5, "E" }, { 6, "F" }, { 7, "G" }, { 8, "H" }, { 9, "I" }, { 10, "J" },
            { 11, "K" }, { 12, "L" }, { 13, "M" }, { 14, "N" }, { 15, "O" }, { 16, "P" }, { 17, "Q" }, { 18, "R" }, { 19, "S" }, { 20, "T" },
            { 21, "U" }, { 22, "V" }, { 23, "W" }, { 24, "X" }, { 25, "Y" }, { 26, "Z" }
        };

        public static WeightedRandom WeightedRandom { get; private set; } = new();

        public static int GetLetterIndex(string letter)
        {
            if (AlphabetToWordMap.Values.Contains(letter))
            {
                foreach (var kvp in AlphabetToWordMap)
                {
                    if (kvp.Value.Equals(letter, System.StringComparison.OrdinalIgnoreCase))
                    {
                        return kvp.Key;
                    }
                }
            }

            return -1;
        }

        public static void ShowDebugLog(string message, int logType)
        {
#if UNITY_EDITOR
            switch (logType)
            {
                case 0:
                    UnityEngine.Debug.Log(message);
                    break;
                case 1:
                    UnityEngine.Debug.LogWarning(message);
                    break;
                case 2:
                    UnityEngine.Debug.LogError(message);
                    break;
                default:
                    UnityEngine.Debug.Log(message);
                    break;
            }
#elif UNITY_DEVELOPMENT
            UnityEngine.Debug.Log(message);
#endif

        }
    }
}
