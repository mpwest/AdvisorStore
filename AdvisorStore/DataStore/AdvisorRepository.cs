namespace AdvisorStore.DataStore
{
    public interface AdvisorRepository
    {
        public IEnumerable<Advisor> GetList(string? search, int limit, int skip);
        public Advisor? Get(string id);
        public Advisor Create(Advisor advisor);
        public Advisor Update(Advisor advisor);
        public Boolean Delete(string id);
    }
}
