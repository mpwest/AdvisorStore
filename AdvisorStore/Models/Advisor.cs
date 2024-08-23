using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

namespace AdvisorStore
{
    [Index(nameof(SIN), IsUnique = true)]
    public class Advisor
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        [Column("AdvisorId", TypeName = "GUID")]
        public Guid? Id { get; set; }

        [Required]
        [Column("AdvisorName", TypeName = "NVARCHAR(255)")]
        [StringLength(255, ErrorMessage = "{0} length cannot exceed {1}")]
        public required string Name { get; set; }

        [Required]
        [Column("SocialInsuranceNumber", TypeName = "CHAR(9)")]
        [StringLength(9, ErrorMessage = "{0} length must be {1}", MinimumLength = 9)]
        public required string SIN { get; set; }

        [Column(TypeName = "NVARCHAR(255)")]
        [StringLength(255, ErrorMessage ="{0} length cannot exceed {1}")]
        public string? Address { get; set; }

        [Column("SocialInsuranceNumber", TypeName = "CHAR(9)")]
        [StringLength(8, ErrorMessage = "{0} length must be {1}", MinimumLength = 8)]
        public string? Phone { get; set; }

        public Health HealthStatus { get; set; }

        public Advisor() {
            HealthStatus = RandomHealth();
        }

        private static Health RandomHealth()
        {
            var rand = new Random().Next(1, 5);
            return rand == 1 ? Health.Red : rand == 2 ? Health.Yellow : Health.Green;
        }

        internal static IEnumerable<PropertyInfo?> SearchableProperties {
            get
            {
                var info = new List<PropertyInfo?>
                {
                    typeof(Advisor).GetProperty(nameof(Name)),
                    typeof(Advisor).GetProperty(nameof(SIN)),
                    typeof(Advisor).GetProperty(nameof(Address)),
                    typeof(Advisor).GetProperty(nameof(Phone))
                };
                info.RemoveAll(p => p == null);

                return info;
            }
        }
    }

    public enum Health
    {
        Green,
        Yellow,
        Red,
    }
}
