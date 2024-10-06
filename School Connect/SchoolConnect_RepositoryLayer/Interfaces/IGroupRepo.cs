namespace SchoolConnect_RepositoryLayer.Interfaces
{
    public interface IGroupRepo
    {
        Task<Dictionary<string, object>> AddActorToGroup(long actorId, long schoolId, string groupName);
    }
}
