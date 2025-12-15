using System;
using GfEngine.Visuals;
using GfEngine.Visuals.Events;
using GfEngine.Battles.Entities;

namespace GfEngine.Battles.Interfaces
{
    // 엔진이 전투 맵(던전)에서 클라이언트에게 바라는 요구사항
    public interface IBattleVisualizer : IVisualizer
    {
        void Initialize();
    }
}