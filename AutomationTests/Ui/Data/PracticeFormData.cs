using Bogus;

namespace AutomationTests.Ui.Data
{
    public class PracticeFormData
    {
       public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
        public string Mobile { get; set; }
        public string BirthDay { get; set; }
        public string BirthMonth { get; set; }
        public string BirthYear { get; set; }
        public string Subject { get; set; }
        public string[] Hobbies { get; set; }
        public string Address { get; set; }
        public string State { get; set; }
        public string City { get; set; }

        public static PracticeFormData GetDefaultData()
        {
            return new PracticeFormData
            {
                FirstName = "Ivan",
                LastName = "Petrov",
                Email = "ivan.petrov@example.com",
                Gender = "Male",
                Mobile = "0888123456",
                BirthDay = "01",
                BirthMonth = "January",
                BirthYear = "1990",
                Subject = "Maths",
                Hobbies = new[] { "Sports", "Reading" },
                Address = "123 Test Street",
                State = "NCR",
                City = "Delhi"
            };
        }

        private static readonly Dictionary<string, List<string>> StateCityMap = new()
        {
            { "NCR", new List<string> { "Delhi", "Gurgaon", "Noida" } },
            { "Uttar Pradesh", new List<string> { "Agra", "Lucknow", "Meerut" } },
            { "Haryana", new List<string> { "Karnal", "Panipat" } },
            { "Rajasthan", new List<string> { "Jaipur", "Jaisalmer" } }
        };

        public static PracticeFormData GetRandomData()
        {
            var faker = new Faker();
            
            var firstName = faker.Name.FirstName();
            var lastName = faker.Name.LastName();
            
            var randomGender = faker.PickRandom("Male", "Female", "Other");
            
            var randomMonth = faker.PickRandom(
                "January", "February", "March", "April", "May", "June",
                "July", "August", "September", "October", "November", "December"
            );

            var randomSubject = faker.PickRandom("Maths", "English", "Chemistry", "Physics", "History");
            var randomHobbies = new[] { faker.PickRandom("Sports", "Reading", "Music") };

            var states = StateCityMap.Keys.ToList();
            var randomState = faker.PickRandom(states);
            var citiesForState = StateCityMap[randomState];
            var randomCity = faker.PickRandom(citiesForState);

            return new PracticeFormData
            {
                FirstName = firstName,
                LastName = lastName,
                Email = faker.Internet.Email(firstName.ToLower(), lastName.ToLower()),
                Gender = randomGender,
                Mobile = faker.Phone.PhoneNumber("##########"),
                BirthDay = faker.Random.Number(1, 28).ToString("00"),
                BirthMonth = randomMonth,
                BirthYear = faker.Random.Number(1980, 2005).ToString(),
                Subject = randomSubject,
                Hobbies = randomHobbies,
                Address = faker.Address.StreetAddress(),
                State = randomState,
                City = randomCity
            };
        }
    }
}