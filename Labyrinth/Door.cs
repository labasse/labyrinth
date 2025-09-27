using System;

public class Door : Tile
{
    private Key key; // clé qui ouvre cette porte
    public bool IsOpen { get; private set; } = false;

    public override bool IsTraversable => IsOpen;

    public Door(Key k)
    {
        key = k;
    }

    public void Unlock(Key k)
    {
        if (k.Name == key.Name)
        {
            IsOpen = true;
        }
        else
        {
            throw new Exception("Mauvaise clé !");
        }
    }

    public void Lock()
    {
        IsOpen = false;
    }

    public override void Pass()
    {
        if (!IsOpen)
            throw new Exception("La porte est fermée !");
    }
}
