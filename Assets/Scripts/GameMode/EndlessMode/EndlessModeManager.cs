using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WordJam
{
    public class EndlessModeManager : GameModeManager
    {
        protected override void Awake()
        {
            CurrentGameMode = GameModeEnum.EndlessMode;

            CurrentLevelData = new()
            {
                gridSize = new(4, 4)
            };

            base.Awake();
        }
    }
}
