using System;
using System.Linq;
using System.Numerics;
using Dalamud.Interface.Textures.TextureWraps;
using Dalamud.Interface.Windowing;
using ImGuiNET;
using PokemonAstraUmbra.Core.Models;
using PokemonAstraUmbra.Core.Utility;
using PokemonAstraUmbra.Services;

namespace PokemonAstraUmbra.Windows;

public class PokemonGeneratorWindow : Window, IDisposable
{
    private int _pokemonSpeciesIdInput = 700;
    private int _pokemonLevelInput = 5;
    private Pokemon? _pokemon;

    public PokemonGeneratorWindow() : base("Pokemon Generator")
    {
        Size = new Vector2(320, 192);
        SizeCondition = ImGuiCond.FirstUseEver;
    }

    public override async void Draw()
    {
        ImGui.InputInt("Pokemon No.", ref _pokemonSpeciesIdInput);
        ImGui.InputInt("Lv.", ref _pokemonLevelInput);

        if (ImGui.Button("Generate"))
        {
            _pokemon = await PokemonGenerationUtility.CreateRandomPokemon(_pokemonSpeciesIdInput, _pokemonLevelInput);
        }

        ImGui.Separator();

        if (_pokemon == null) return;

        ImGui.Text(_pokemon.Species.Name);
        ImGui.Text($"Lv. {_pokemon.Level}");
        ImGui.Text(Enum.GetName(_pokemon.Gender));
        ImGui.Text(Enum.GetName(_pokemon.Nature));
        ImGui.Text($"Ability: {_pokemon.Ability?.Name}");
        ImGui.Text($"Moves: {string.Join(", ", _pokemon.Moves.Select(x => x.Name))}");

        string sprite = _pokemon.IsShiny ? _pokemon.Species.ShinyFrontSprite : _pokemon.Species.FrontSprite;

        IDalamudTextureWrap? image = DalamudService.TextureProvider
            .GetFromFile(sprite)
            .GetWrapOrDefault();

        if (image != null)
        {
            // ImGui.SetCursorPos();
            ImGui.Image(image.ImGuiHandle, new Vector2(image.Width, image.Height));
        }
        else
        {
            ImGui.Text("Image not found.");
        }
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
    }
}