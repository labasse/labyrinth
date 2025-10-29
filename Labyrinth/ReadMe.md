# Rapport Étape 1 - Évènement d'initialisation

----

## Build/AsciiParser.cs

Pour le parser les modifications portent sur la gestion de l'évent `StartPositionFound`. Ce dernier leverait un évènement lorsqu'il trouve la position de départ dans le labyrinthe.
- Parse est devenue une méthode d’instance (et non plus statique) afin de pouvoir lever l’évènement. On aurait pu la garder static (en mettant l'event en static et modifiant les param du invoke) mais cela n'autait pas été une bonne pratique (par exemple si on crée en simultané deux labyrinthe ils seraient sur la même instance d'event).

- Suppression du paramètre ref (int X, int Y) start de la méthode Parse qui n'a plus lieu d'être.

 L'évent est déclaré dans le fichier Build/StartEventArgs.cs :
 ```c#
 namespace Labyrinth.Build;

public class StartEventArgs(int x, int y) : EventArgs
{
    public int X { get; } = x;

    public int Y { get; } = y;
}
```

## Labyrinth.cs
Dans le fichier Labyrinth.cs, on s'abonne à l'évent `StartPositionFound` du parser dans le constructeur de Labyrinth. Lorsqu'il est levé, on met à jour les propriétés `StartX` et `StartY` du labyrinthe.

- Création d'un Parser d'instance utilisé dans l'initialisation, comme la méthode Parse n'est plus static.
- Abonnement à l'évent `StartPositionFound` du parser.
- On utilise une lambda stocké dans une variable pour l'abonnement, afin de pouvoir s'en désabonner après l'initialisation du labyrinthe (on aurait aussi pu déclarer une méthode).
- Désabonnement de l'évent après l'initialisation du labyrinthe.


--> Tous les tests unitaires passent avec succès après ces modifications. On est OK.