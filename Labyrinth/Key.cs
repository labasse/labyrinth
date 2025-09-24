public class Key : ICollectable
{
	public string KeyId { get; private set; }
	public Key(string keyId)
	{
		KeyId = keyId;
	}
}