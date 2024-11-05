using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolConnect_RepositoryLayer.Interfaces
{
    public interface IChats
    {
        Task<Dictionary<string, object>> GetChatsAsync();
    }
}
