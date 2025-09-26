{
  description = "A very basic flake";

  inputs = {
    nixpkgs.url = "github:nixos/nixpkgs?ref=nixos-unstable";
  };

  outputs = { self, nixpkgs }:
    let
      system = "x86_64-linux";
      pkgs = nixpkgs.legacyPackages.${system};
    in
    {
      devShells = {
        ${system} =
          let
            watch = pkgs.writeShellScriptBin "watch" ''
              watchexec --restart --exts cs --debounce 500ms --watch ./ -- "dotnet run --project Labyrinth"
            '';
          in
          {
            default = pkgs.mkShell
              {
                buildInputs = with pkgs;[
                  dotnetCorePackages.sdk_9_0-bin
                  omnisharp-roslyn
                  watchexec

                  watch
                ];
              };
          };
      };
    };
}
