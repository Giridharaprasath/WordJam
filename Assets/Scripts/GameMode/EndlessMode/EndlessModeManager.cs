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
                GridSize = new(4, 4)
            };

            base.Awake();
        }
    }
}
