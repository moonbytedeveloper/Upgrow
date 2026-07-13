using Upgrow.Application.DTOs;

namespace Upgrow.Application.IServices;

public interface IDataTableParser
{
    DataTableRequest ParseRequest();
}
