using Upgrow.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Upgrow.Application.IServices.Inquiry
{
    public interface IPagedService<Tdto>
    {
        Task<(int total, List<Tdto> data)> GetPagedAsync(DataTableRequest request);
        Task UpdateStatusAsync(string uuid, string newStatus);
    }
}
