using System;
using System.Linq;
using System.Numerics;
using Dalamud.Interface.Windowing;
using ImGuiNET;
using PokemonAstraUmbra.Core.Database;
using PokemonAstraUmbra.Core.Models;
using PokemonAstraUmbra.Services;

namespace PokemonAstraUmbra.Windows;

public class DebugWindow : Window, IDisposable
{
    public DebugWindow() : base("Debug Window")
    {
        Size = new Vector2(320, 192);
        SizeCondition = ImGuiCond.FirstUseEver;
    }

    public override async void Draw()
    {
        ImGui.Text($"Delta Time: {DeltaTimeService.Instance.DeltaTime}");
        ImGui.Text($"Framerate: {DeltaTimeService.Instance.FrameRate}");

        ImGui.Separator();

        Vector3? playerPos = DalamudService.ClientState.LocalPlayer?.Position;
        ImGui.Text($"Player pos: ({playerPos?.X:0.00}, {playerPos?.Y:0.00}, {playerPos?.Z:0.00})");
        ImGui.Text($"Egg cycles: {PositionService.Instance.EggCycles}");

        ImGui.Separator();

        ImGui.Text($"Region: {PositionService.GetPlaceName(PositionService.Instance.RegionId)} ({PositionService.Instance.RegionId})");
        ImGui.Text($"Zone: {PositionService.GetPlaceName(PositionService.Instance.ZoneId)} ({PositionService.Instance.ZoneId})");
        ImGui.Text($"Area: {PositionService.GetPlaceName(PositionService.AreaId)} ({PositionService.AreaId})");
        ImGui.Text($"SubArea: {PositionService.GetPlaceName(PositionService.SubAreaId)} ({PositionService.SubAreaId})");
        ImGui.Text($"Sanctuary: {PositionService.IsInSanctuary}");
        ImGui.Text($"Time: {PositionService.EorzeaTime:HH:mm}");

        if (PositionService.Instance.WildPokemon != null)
        {
            ImGui.Separator();

            await using PokemonDbContext db = new();
            foreach (WildPokemon wildPokemon in PositionService.Instance.WildPokemon.Pool)
            {
                PokemonSpecies? species = db.PokemonSpecies.FirstOrDefault(x => x.Id == wildPokemon.SpeciesId);
                if (species == null) continue;

                string levelRange = wildPokemon.MinLevel.ToString();
                if (wildPokemon.MinLevel != wildPokemon.MaxLevel) levelRange = $"{wildPokemon.MinLevel}-{wildPokemon.MaxLevel}";

                ImGui.Text($"{species.Name}: Lv. {levelRange} ({wildPokemon.Rate * 100f:0}%%)");
            }

            if (ImGui.Button("Get encounter"))
            {
                WildPokemon wildPokemon = PositionService.Instance.WildPokemon.GetWildPokemon();
                PokemonSpecies? species = db.PokemonSpecies.FirstOrDefault(x => x.Id == wildPokemon.SpeciesId);
                if (species != null)
                {
                    Pokemon pokemon = Pokemon.GetWildPokemon(species, wildPokemon.MinLevel, wildPokemon.MaxLevel);
                    DalamudService.Log.Info("A wild {Species} appeared! (Lv. {Level})", pokemon.Species.Name, pokemon.Level);
                }
            }
        }

        ImGui.Separator();

        if (ImGui.Button("Pokemon Generator")) WindowService.Instance.ShowPokemonGeneratorWindow();

        // Vector2 pos = ImGui.GetCursorPos();
        // ImGui.Text($"X Y: {pos.X}, {pos.Y}");
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }
}