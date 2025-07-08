using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WordJam
{
    public class LevelModeManager : GameModeManager
    {
        protected override void Awake()
        {
            CurrentGameMode = GameModeEnum.LevelMode;

            CurrentLevelData = new()
            {
                GridSize = new(3, 4)
            };

            base.Awake();
        }
    }
}
