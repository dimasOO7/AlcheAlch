using Godot;
using System;
using System.Collections.Generic;

public partial class RuleRegistry
{
    public List<SimulationRule> rules;

    public RuleRegistry()
    {
        rules = new List<SimulationRule>
        {
            // ─────── ОБЯЗАТЕЛЬНО ПЕРВЫМ ───────
            new CopyToNextRule(),

            // ─────── Огонь ───────
            new FireDecayRule(),

            // ─────── Трава (порядок критичен!) ───────
            new GrassBurnFromFireRule(),      // горение от огня + захват территории
            new GrassAcidIgnitionRule(),      // зажигание кислотой

            // ─────── Почва (отравление перед ростом!) ───────
            new SoilToPoisonedSoilRule(),
            new SoilToGrassRule(),

            // ─────── Вода ───────
            new WaterToAcidRule(),

            // ─────── Начисление очков (самым последним) ───────
            new GrassTickScoreRule(),

            // Новые правила добавляй сюда ↓
        };
    }
}