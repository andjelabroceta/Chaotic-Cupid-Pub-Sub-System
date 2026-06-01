namespace ChaoticCupidPubSubSystem.Model
{
    public class Person
    {
        public string Username { get; set; }
        public string City { get; set; }
        public int Age { get; set; }
       
        public string PhoneNumber { get; set; }
        public string ConnectionId { get; set; } = string.Empty;
        
        public bool ConfirmLetter { get; set; }
        public List<string> BlockedPersons { get; set; }

        public Person(string username, string city, int age, string phoneNumber)
        {
            this.Username = username;
            this.City = city;
            this.Age = age;
            this.PhoneNumber = phoneNumber;
            this.ConnectionId = ConnectionId;
            this.ConfirmLetter = true;
            this.BlockedPersons = new List<string>();
        }
    }
}
