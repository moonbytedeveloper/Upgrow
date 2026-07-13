using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerifyIndia.Application.DTOs;
using VerifyIndia.Application.IServices;

namespace VerifyIndia.Application.Services
{
    public class DataTableParser : IDataTableParser
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DataTableParser(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public DataTableRequest ParseRequest()
        {
            var httpRequest = _httpContextAccessor.HttpContext?.Request;

            // Only read form if the request actually has a form content type
            IFormCollection? form = null;
            if (httpRequest?.HasFormContentType == true)
            {
                form = httpRequest.Form;
            }

            _ = int.TryParse(form?["start"].FirstOrDefault(), out var start);
            _ = int.TryParse(form?["length"].FirstOrDefault(), out var length);
            _ = int.TryParse(form?["order[0][column]"].FirstOrDefault(), out var sortColumnIndex);

            return new DataTableRequest
            {
                Start = start,
                Length = length <= 0 ? 10 : length,
                Search = form?["search[value]"].FirstOrDefault(),
                SortColumn = form?[$"columns[{sortColumnIndex}][data]"].FirstOrDefault(),
                SortDirection = form?["order[0][dir]"].ToString() ?? "desc"
            };
        }
    }
}
