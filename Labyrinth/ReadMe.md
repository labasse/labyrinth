# Rapport Étape 1 - Évènement d'initialisation

----

## Build/AsciiParser.cs

Pour le parser les modifications portent sur la gestion de l'évent `StartPositionFound`. Ce dernier leverait un évènement lorsqu'il trouve la position de départ dans le labyrinthe.
- Parse est devenue une méthode d’instance (et non plus statique) afin de pouvoir lever l’évènement. On aurait pu la garder static (en mettant l'event en static et modifiant les param du invoke) mais cela n'autait pas été une bonne pratique (par exemple si on crée en simultané deux labyrinthe ils seraient sur la même instance d'event).

- Suppression du paramètre ref (int X, int Y) start de la méthode Parse qui n'a plus lieu d'être.

 L'évent est déclaré dans le fichier Build/StartEventArgs.cs :
 ```csharp
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





# Étape 3 – Couche présentation

---

Permettre d’afficher en **temps réel** les déplacements de l’explorateur dans le labyrinthe **sans modifier** la logique interne de `GetOut`, en s’appuyant sur des **événements** et une simple mise à jour visuelle dans la console.

## Implémentation

### `CrawlingEventArgs`

Création d’une classe d’événement contenant la position et la direction actuelles :

```csharp
public sealed class CrawlingEventArgs(int x, int y, Direction direction) : EventArgs
{
    public int X { get; } = x;
    public int Y { get; } = y;
    public Direction Direction { get; } = direction;
}
```

Elle permet de transmettre les informations nécessaires à l’affichage lors d’un déplacement ou d’un changement d’orientation.

---

### Événements dans `Explorer`

Deux événements ont été ajoutés :

```csharp
public event EventHandler<CrawlingEventArgs>? PositionChanged;
public event EventHandler<CrawlingEventArgs>? DirectionChanged;
```

- **PositionChanged** est déclenché après un `Walk()` réussi (nouvelle position).
- **DirectionChanged** après un `TurnLeft()` ou `TurnRight()`.
- Aucun événement n’est émis si le déplacement échoue (mur) ou si le crawler est déjà face à la sortie.

----

## Programme console

Le programme principal s’abonne aux événements pour **mettre à jour la position du curseur** et afficher la direction du crawler (`^`, `>`, `v`, `<`) :


La fonction `DrawExplorer` efface l’ancienne position et redessine le symbole à la nouvelle coordonnée via `Console.SetCursorPosition`.
Possible d'ajouter un léger délai et rendre l’animation fluide avec un `Thread.Sleep(150);` par exemple.

L'ensemble de l'implémentation de l'affichage et "la logique du jeu" est géré dans le fichier `Program.cs`. À terme on peut imaginer gérer cette logique dans une classe dédiée pour séparer les responsabilités. Dans cette partie il était demandé de faire les actions dans le programme principal j'ai donc respecté cette consigne.

je n’ai pas pu utiliser un switch sur Direction, celle-ci étant une classe et non un enum (j'ai donc fait plusieurs if). Chaque propriété (North, East, etc.) crée une nouvelle instance, ce qui empêche la comparaison par constante --> j’aimerais confirmation que cette interprétation est correcte (si c'est possible d'inclure ça dans la correction merci).

-----

## Tests unitaires

Les tests existants ont été enrichis pour vérifier :

- que les événements sont bien **déclenchés au bon moment**,
- avec les **bons arguments (X, Y, Direction)**,
- et les informations correctes selon les actions effectuées.

En plus, plusieurs fonction "utilitaires" pour les Asserts ont été ajoutées dans le fichier ExplorerTest pour faciliter la vérification des événements et éviter la duplication de code.

Par exemple :
- AssertThatArg : vérifie que les arguments d’un événement correspondent aux valeurs attendues.
- AssertThatOutside : vérifie que l’explorateur est bien en face une tuile de type `Outside` et donc à trouver la sortie.

Tous les tests passent avec succès. La phase de refactoring est OK.
