using System;
using System.Threading.Tasks;

namespace web_api.Interfaces
{
    internal interface ILog
    {
        Task Log(Exception ex);

    }
}
