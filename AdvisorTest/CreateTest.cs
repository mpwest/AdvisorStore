using AdvisorStore;
using AdvisorStore.DataStore.InMemory;

namespace AdvisorTest
{
    public class CreateTest
    {
        // Masking occurs in controller
        [Fact]
        public void CreateMinimum()
        {
            var name = "Test Advisor";
            var sin = "000000000";
            var advisor = new Advisor { Name = name, SIN = sin };
            var advisorRepository = new InMemoryAdvisorRepository();
            var response = advisorRepository.Create(advisor);

            using var db = new AdvisorContext();
            var result = db.Advisors.Where(a => a.Id == response.Id)
                .FirstOrDefault();

            Assert.NotNull(result);
            Assert.Equal(name, result.Name);
            Assert.Equal(sin, result.SIN);
        }

        [Fact]
        public void CreateWithOptionalFields()
        {
            var name = "Test Advisor";
            var sin = "000000000";
            var address = "123 Any St";
            var phone = "12345678";
            var advisor = new Advisor { Name = name, SIN = sin, Address = address, Phone = phone };
            var advisorRepository = new InMemoryAdvisorRepository();
            var response = advisorRepository.Create(advisor);

            using var db = new AdvisorContext();
            var result = db.Advisors.Where(a => a.Id == response.Id)
                .FirstOrDefault();

            Assert.NotNull(result);
            Assert.Equal(name, result.Name);
            Assert.Equal(sin, result.SIN);
            Assert.Equal(address, result.Address);
            Assert.Equal(phone, result.Phone);
        }

        [Fact]
        public void CreateWithValidationErrors()
        {
            // ...
            // 
        }

    }
}