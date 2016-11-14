using System;

namespace MicroOrms_Performance_Tests
{
    public class User
    {
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public int? Age { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public UserStatus UserStatus { get; set; }
    }
}