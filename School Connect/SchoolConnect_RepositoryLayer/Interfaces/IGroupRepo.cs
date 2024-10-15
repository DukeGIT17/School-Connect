namespace SchoolConnect_RepositoryLayer.Interfaces
{
    public interface IGroupRepo
    {
        Task<Dictionary<string, object>> AddActorToGroup(string actorId, long schoolId, string groupName);
    }
}
