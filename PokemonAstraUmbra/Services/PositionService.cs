using System;
using System.Linq;
using Dalamud.Game.ClientState.Conditions;
using Dalamud.Plugin.Services;
using FFXIVClientStructs.FFXIV.Client.Game.UI;
using FFXIVClientStructs.FFXIV.Client.System.Framework;
using FFXIVClientStructs.FFXIV.Common.Math;
using Lumina.Excel.Sheets;
using PokemonAstraUmbra.Core.Database.SeededData;
using PokemonAstraUmbra.Core.Models;

namespace PokemonAstraUmbra.Services;

public unsafe class PositionService : IDisposable
{
    public static PositionService Instance { get; private set; } = new();

    // TODO: make this a delegate or something
    public uint EggCycles;
    public WildPokemonPool? WildPokemon;
    public uint? RegionId => _currentTerritory?.PlaceNameRegion.RowId;
    public uint? ZoneId => _currentTerritory?.PlaceName.RowId;
    public static uint AreaId => TerritoryInfo.Instance()->AreaPlaceNameId;
    public static uint SubAreaId => TerritoryInfo.Instance()->SubAreaPlaceNameId;
    public static bool IsInSanctuary => TerritoryInfo.Instance()->InSanctuary;
    public static DateTimeOffset EorzeaTime => DateTimeOffset.FromUnixTimeSeconds(Framework.Instance()->ClientTime.EorzeaTime);

    private const uint StepsPerEggCycle = 128;

    private TerritoryType? _currentTerritory;

    private Vector3? _lastPos;
    private Vector3? _currentPos;
    private float _eggCycleSteps = 0;
    private bool _isJumping = false;
    private bool _isBetweenAreas = false;

    private PositionService()
    {
        _lastPos = DalamudService.ClientState.LocalPlayer?.Position;

        DalamudService.Condition.ConditionChange += OnConditionChange;
        DalamudService.Framework.Update += OnFrameworkUpdate;
    }

    public void Dispose()
    {
        DalamudService.Condition.ConditionChange -= OnConditionChange;
        DalamudService.Framework.Update -= OnFrameworkUpdate;

        GC.SuppressFinalize(this);
    }

    private void OnConditionChange(ConditionFlag flag, bool value)
    {
        switch (flag)
        {
            case ConditionFlag.Mounted:
                break;
            case ConditionFlag.ChocoboRacing:
                break;
            case ConditionFlag.PlayingMiniGame:
                break;
            case ConditionFlag.PlayingLordOfVerminion:
                break;
            case ConditionFlag.ParticipatingInCustomMatch:
                break;
            case ConditionFlag.Fishing:
                break;
            case ConditionFlag.BetweenAreas:
                _isBetweenAreas = value;
                break;
            case ConditionFlag.Jumping:
                _isJumping = value;
                break;
            case ConditionFlag.InFlight:
                break;
            case ConditionFlag.Swimming:
                break;
            case ConditionFlag.Diving:
                break;
            default:
                return;
        }
    }

    private void OnFrameworkUpdate(IFramework framework)
    {
        UpdateDistanceTraveled();
        UpdateCurrentTerritory();
    }

    public static string? GetPlaceName(PlaceName placeName) => GetPlaceName(placeName.RowId);

    public static string? GetPlaceName(uint? placeId)
    {
        if (placeId == null) return null;

        bool rowFound = DalamudService.DataManager.GetExcelSheet<PlaceName>()
            .TryGetRow(placeId.Value, out PlaceName place);
        return rowFound ? place.Name.ExtractText() : null;
    }

    private void UpdateDistanceTraveled()
    {
        if (_isBetweenAreas)
        {
            _lastPos = null;
            return;
        }

        _currentPos = DalamudService.ClientState.LocalPlayer?.Position;
        if (_currentPos == null) return;

        if (_lastPos == null)
        {
            _lastPos = _currentPos;
            return;
        }

        Vector3 currentPosAdjusted = _currentPos.Value;
        if (_isJumping) currentPosAdjusted.Y = _lastPos.Value.Y;

        _eggCycleSteps += Vector3.Distance(_lastPos.Value, currentPosAdjusted);
        if (_eggCycleSteps >= StepsPerEggCycle)
        {
            _eggCycleSteps -= StepsPerEggCycle;
            EggCycles += 1;
        }
        _lastPos = currentPosAdjusted;
    }

    private void UpdateCurrentTerritory()
    {
        bool currentTerritoryExists = DalamudService.DataManager.Excel
            .GetSheet<TerritoryType>()
            .TryGetRow(DalamudService.ClientState.TerritoryType, out TerritoryType currentTerritory);
        if (!currentTerritoryExists) return;

        _currentTerritory = currentTerritory;

        WildArea? wildArea = WildAreas.Areas.FirstOrDefault(x => x.XivPlaceId == SubAreaId);
        wildArea ??= WildAreas.Areas.FirstOrDefault(x => x.XivPlaceId == AreaId);
        wildArea ??= WildAreas.Areas.FirstOrDefault(x => x.XivPlaceId == ZoneId);
        wildArea ??= WildAreas.Areas.FirstOrDefault(x => x.XivPlaceId == RegionId);

        if (wildArea == null)
        {
            WildPokemon = null;
            return;
        }

        WildPokemon = EorzeaTime.Hour switch
        {
            >= 4 and < 10 => new WildPokemonPool(wildArea.MorningWildPokemon),
            >= 10 and < 18 => new WildPokemonPool(wildArea.DayWildPokemon),
            _ => new WildPokemonPool(wildArea.NightWildPokemon)
        };
    }
}