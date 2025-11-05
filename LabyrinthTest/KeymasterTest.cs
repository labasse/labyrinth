using Labyrinth.Build;

namespace LabyrinthTest
{
    [TestFixture(Description = "Keymaster should handle arbitrary distributions of doors and key rooms")]
    public class KeymasterTest
    {
        [Test]
        public void DoorThenRoom_Matches_NoExceptionOnDispose()
        {
            Assert.That(() =>
            {
                using var km = new Keymaster();
                _ = km.NewDoor();
                _ = km.NewKeyRoom(); 
            }, Throws.Nothing);
        }

        [Test]
        public void RoomThenDoor_Matches_NoExceptionOnDispose()
        {
            Assert.That(() =>
            {
                using var km = new Keymaster();
                var r = km.NewKeyRoom();
                var d = km.NewDoor();
            }, Throws.Nothing);   
        }

        [Test]
        public void Alternating_DKDK_Matches_All()
        {
            Assert.That(() =>
            {
                using var km = new Keymaster();
                km.NewDoor();
                km.NewKeyRoom();
                km.NewDoor();
                km.NewKeyRoom(); 
            }, Throws.Nothing);
        }

        [Test]
        public void Batch_DoorsThenRooms_AllMatch()
        {
            Assert.That(() =>
            {
                using var km = new Keymaster();
                km.NewDoor();
                km.NewDoor();
                km.NewKeyRoom();
                km.NewKeyRoom();
            }, Throws.Nothing);
        }

        [Test]
        public void Batch_RoomsThenDoors_AllMatch()
        {
            Assert.That(() =>
            {
                using var km = new Keymaster();
                km.NewKeyRoom();
                km.NewKeyRoom();
                km.NewDoor();
                km.NewDoor();
            }, Throws.Nothing);
            
        }

        [Test]
        public void ManyDoorsThenManyRooms_AllMatch()
        {
            Assert.That(() =>
            {
                using var km = new Keymaster();
                for (int i = 0; i < 10; i++) km.NewDoor();
                for (int i = 0; i < 10; i++) km.NewKeyRoom();
            }, Throws.Nothing);
            
        }

        [Test]
        public void ManyRoomsThenManyDoors_AllMatch()
        {
            Assert.That(() =>
            {
                using var km = new Keymaster();
                for (int i = 0; i < 10; i++) km.NewKeyRoom();
                for (int i = 0; i < 10; i++) km.NewDoor();
            }, Throws.Nothing);
        }

        [Test]
        public void Unmatched_LeftoverDoor_ThrowsOnDispose()
        {
            Assert.That(() =>
            {
                using var km = new Keymaster();
                km.NewDoor();
            }, Throws.InvalidOperationException);
        }

        [Test]
        public void Unmatched_LeftoverRoom_ThrowsOnDispose()
        {
            Assert.That(() =>
            {
                using var km = new Keymaster();
                km.NewKeyRoom();
            }, Throws.InvalidOperationException);
        }

        [Test]
        public void MixedInterleavings_SameCounts_NoException()
        {
            Assert.That(() =>
            {
                using var km = new Keymaster();
                km.NewDoor();
                km.NewDoor();
                km.NewKeyRoom();
                km.NewDoor();
                km.NewKeyRoom();
                km.NewKeyRoom();
                km.NewDoor();
                km.NewKeyRoom();
                km.NewKeyRoom();
                km.NewDoor();
            }, Throws.Nothing);
        }
    }
}
