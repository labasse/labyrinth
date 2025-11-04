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


# Rapport Étape 2 – Explorateur

---

Implémenter une classe `Explorer` capable de parcourir un labyrinthe de manière aléatoire, à partir d’un `ICrawler`, jusqu’à atteindre une tuile de type `Outside` ou après un nombre maximal d’actions défini.
Cette étape introduit également une abstraction du générateur aléatoire pour rendre les tests unitaires déterministes.


## Explorer.cs

La classe `Explorer` reçoit au constructeur :

- un `ICrawler` (position courante dans le labyrinthe),
- un générateur aléatoire optionnel (`IRandomSource`), injecté pour permettre le mock en test.

```csharp
public class Explorer(ICrawler crawler, IRandomSource? rng = null)
```

Si aucun générateur n’est fourni, la classe utilise par défaut un `RandomSource` basé sur `Random.Shared`.

#### Méthode principale : `GetOut(int n)`

- Exécute jusqu’à *n* actions aléatoires parmi :
  - **Walk** : tente d’avancer d’une case (intercepte les exceptions en cas d’obstacle),
  - **TurnLeft / TurnRight** : modifie la direction du crawler.
- S’arrête dès qu’une tuile de type `Outside` est atteinte ou lorsque le nombre maximal d’actions est atteint.
- Lève une `ArgumentOutOfRangeException` si *n* est négatif.
- Retourne `true` si la sortie est trouvée, `false` sinon.

Le `catch` d’`InvalidOperationException` a été encapsulé dans un bloc court, uniquement pour ignorer les déplacements impossibles (on se prend techniquement le mur ou la bordure). Dans l'optique de faire une méthode un "peu plus intelligente dans la recherche de la sortie" on pourrait imaginer tourner aléatoirement à droite ou à gauche si on ne peut pas avancer, mais ce n'est pas demandé ici.

Une énumération interne `Act` a été introduite pour éviter les valeurs magiques `0/1/2` et clarifier les actions possibles.

---

## Gestion de la génération aléatoire

### Interface `IRandomSource`

```csharp
public interface IRandomSource
{
    int Next(int minInclusive, int maxExclusive);
}
```

Cette interface définit un contrat simple et permet d’injecter des implémentations différentes selon le contexte.

### Implémentation par défaut : `RandomSource`

```csharp
public sealed class RandomSource : IRandomSource
{
    public int Next(int minInclusive, int maxExclusive)
        => Random.Shared.Next(minInclusive, maxExclusive);
}
```

- Utilise `Random.Shared` pour garantir la thread-safety et éviter la recréation d’instances de `Random`.
- Le comportement aléatoire standard.
---

## Tests unitaires

Les tests ont été rédigés dans le fichier `LabyrinthTest/Crawl`.
Ils valident la logique d’exploration tout en remplaçant le générateur aléatoire par une version déterministe.

### Mock du générateur aléatoire

Une classe `SequenceRandom` implémente `IRandomSource` pour renvoyer une séquence fixe de valeurs, permettant de reproduire les comportements de test.

```csharp
private sealed class SequenceRandom(params int[] seq) : IRandomSource
{
    private readonly Queue<int> _queue = new(seq);
    public int Next(int minInclusive, int maxExclusive) {
        var value = _queue.Dequeue();
        _queue.Enqueue(value);
        return value;
    }
}
```

### Scénarios testés

- **Déjà face à une sortie** → retourne `true` sans effectuer d’action.
- **Tourne à droite puis avance** → atteint une sortie et retourne `true`.
- **Tourne à gauche puis avance** → atteint une sortie et retourne `true`.
- **Aucune sortie trouvée** après *n* actions → retourne `false`.

Les tests utilisent des cartes ASCII minimales pour vérifier le comportement dans des situations simples.
Tous les tests passent avec succès.