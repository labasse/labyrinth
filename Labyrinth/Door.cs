namespace Labyrinth
{
    public class Door : Tile
    {
        private bool _isLocked;
        public Key Key { get; }
        
        public Door()
        {
            Key = new Key();
            _isLocked = true;
        }
        
        public bool IsLocked => _isLocked;
        public override bool IsTraversable => !_isLocked;
        public void Lock() => _isLocked = true;
        
        public bool TryUnlock(Key key)
        {
            if (key.Id == Key.Id)
            {
                _isLocked = false;
                return true;
            }
            return false;
        }
        
        public override void Pass()
        {
            if (_isLocked) throw new InvalidOperationException("Door is locked.");
        }
    }
}
