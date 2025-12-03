using Labyrinth.Build;

namespace LabyrinthTest;

[TestFixture]
public class KeymasterTest
{
    [Test]
    public void CreateDoorThenKeyRoom_PlacesKeyInRoom()
    {
        using var keymaster = new Keymaster();
        var door = keymaster.NewDoor();
        var keyRoom = keymaster.NewKeyRoom();
        
        Assert.That(keyRoom.Pass().HasItems, Is.True);
        Assert.That(door.IsLocked, Is.True);
    }

    [Test]
    public void CreateKeyRoomThenDoor_PlacesKeyInRoom()
    {
        using var keymaster = new Keymaster();
        var keyRoom = keymaster.NewKeyRoom();
        var door = keymaster.NewDoor();
        
        Assert.That(keyRoom.Pass().HasItems, Is.True);
        Assert.That(door.IsLocked, Is.True);
    }

    [Test]
    public void MultipleDoorsAndKeyRooms_AnyOrder_MatchesAll()
    {
        using var keymaster = new Keymaster();
        var door1 = keymaster.NewDoor();
        var room1 = keymaster.NewKeyRoom();
        var room2 = keymaster.NewKeyRoom();
        var door2 = keymaster.NewDoor();

        Assert.That(room1.Pass().HasItems, Is.True);
        Assert.That(room2.Pass().HasItems, Is.True);
        Assert.That(door1.IsLocked, Is.True);
        Assert.That(door2.IsLocked, Is.True);
    }

    [Test]
    public void DisposeWithUnmatched_MultipleDoors_Throws()
    {
        var keymaster = new Keymaster();  // PAS de 'using'
        keymaster.NewDoor();
        
        Assert.Throws<InvalidOperationException>(() => keymaster.Dispose());
    }

    [Test]
    public void DisposeWithUnmatched_MultipleKeyRooms_Throws()
    {
        var keymaster = new Keymaster();  // PAS de 'using'
        keymaster.NewKeyRoom();
        
        Assert.Throws<InvalidOperationException>(() => keymaster.Dispose());
    }

    [Test]
    public void DisposeMatched_NoException()
    {
        var keymaster = new Keymaster();
        keymaster.NewDoor();
        keymaster.NewKeyRoom();
        keymaster.Dispose(); // OK, pas d'exception
    }
}
