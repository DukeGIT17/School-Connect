namespace SchoolConnect_RepositoryLayer.Interfaces
{
    public interface IGroup
    {
        Task<Dictionary<string, object>> AddActorToGroup(long actorId, long schoolId, string groupName);
    }
}
