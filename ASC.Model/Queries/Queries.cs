using ASC.Model.BaseTypes;
using ASC.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ASC.Model.Queries
{
    public static class Queries
    {
        public static Expression<Func<ServiceRequest, bool>> GetDashboardQuery(DateTime? requestedDate,
            List<string> status = null,
            string email = "",
            string serviceEngineerEmail = "")
        {
            var query = (Expression<Func<ServiceRequest, bool>>)(u => true);
            if (requestedDate.HasValue)
            {
                var requestedDataFilter = (Expression<Func<ServiceRequest, bool>>)(u => u.RequestedDate >= requestedDate);
                query = query.And(requestedDataFilter);
            }
            if (!string.IsNullOrWhiteSpace(email))
            {
                var requestedDataFilter = (Expression<Func<ServiceRequest, bool>>)(u => u.PartitionKey == email);
                query = query.And(requestedDataFilter);
            }
            // Add Email clause if email is passed as a parameter
            if (!string.IsNullOrWhiteSpace(serviceEngineerEmail))
            {
                var requestedDataFilter = (Expression<Func<ServiceRequest, bool>>)(u => u.ServiceEngineer == serviceEngineerEmail);
                query = query.And(requestedDataFilter);
            }

            // Add Status clause if status is appended as OR condition
            var statusQueries = (Expression<Func<ServiceRequest, bool>>)(u =>false);
            if (status != null)
            {
                foreach (var state in status)
                {
                    var statusFilter = (Expression<Func<ServiceRequest, bool>>)(u => u.Status == state);
                    statusQueries = statusQueries.Or(statusFilter);

                }
                query = query.And(statusQueries);
            }
            return query;
        }
    }
}
