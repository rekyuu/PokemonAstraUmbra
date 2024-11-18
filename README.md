Not working on this for the foreseeable future.

---

Setup

```sh
./eng/pokeapi-init.sh
./eng/pokeapi-update.sh # if updating repo
dotnet ef database update
```

Then call `PokeApiUtility.SeedDatabase();` from wherever