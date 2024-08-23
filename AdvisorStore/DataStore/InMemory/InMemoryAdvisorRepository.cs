
namespace AdvisorStore.DataStore.InMemory
{
    public class InMemoryAdvisorRepository : AdvisorRepository
    {
        public Advisor Create(Advisor advisor)
        {
            if (advisor != null)
            {
                using var db = new AdvisorContext();
                var result = db.Add(advisor);
                db.SaveChanges();

                return result.Entity;
            }
            throw new InvalidOperationException("Missing advisor data in create");
        }

        public Boolean Delete(string id)
        {
            using var db = new AdvisorContext();
            try
            {
                var advisor = db.Advisors.Where(a => a.Id == new Guid(id)).FirstOrDefault();
                if (advisor == null)
                {
                    return false;
                }
                var result = db.Remove(advisor);
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public Advisor? Get(string id)
        {
            using var db = new AdvisorContext();
            var guid = new Guid(id);
            return db.Advisors.Where(a => a.Id == guid)
                .FirstOrDefault();
        }

        public IEnumerable<Advisor> GetList(string? search, int limit, int skip)
        {
            var searchProperties = Advisor.SearchableProperties;

            using var db = new AdvisorContext();
            var result = db.Advisors.Where(a =>
                search == null ||
                search == string.Empty ||
                searchProperties.Any(prop =>
                // Coalescing to handle null without suppressing warning
                prop != null && ((prop.GetValue(a) ?? "").ToString() ?? "").Contains(search, StringComparison.OrdinalIgnoreCase))
            ).Skip(skip)
            .Take(limit)
            .ToList();

            return result;
        }

        public Advisor Update(Advisor advisor)
        {
            if (advisor != null)
            {
                using var db = new AdvisorContext();
                var result = db.Update(advisor).Entity;
                db.SaveChanges();
                return result;
            }
            throw new InvalidOperationException("Missing advisor data in update");
        }
    }
}
