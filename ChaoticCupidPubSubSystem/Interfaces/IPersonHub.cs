namespace ChaoticCupidPubSubSystem.Interfaces
{
    public interface IPersonHub
    {
        public Task InitSinglePerson(string username, string city, int age, string phoneNumber);
        public Task BlockPerson(string username, string blockedUsername);
        public Task ConfirmReceived(string username);
    }
}
