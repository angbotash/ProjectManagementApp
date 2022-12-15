using ProjectManagementApp.Domain.Entities;
using System.Linq.Expressions;

namespace ProjectManagementApp.Domain.QueryOrder
{
    public class ProjectQueryExtensions
    {
        public static IQueryable<Project> Filter(IQueryable<Project> query, string filter)
        {
            if (string.IsNullOrWhiteSpace(filter))
            {
                return query;
            }

            filter = filter.ToLower();

            return query.Where(x =>
                x.Name.ToLower().Contains(filter)
                || x.Name.ToLower().Contains(filter)
                || x.Priority.ToString().Contains(filter)
            );
        }

        public static IQueryable<Project> OrderBy(IQueryable<Project> query, string name, SortDirection direction = SortDirection.Ascending)
        {

            Expression<Func<Project, object>> expression = name?.ToLower() switch
            {
                "name" => x => x.Name,
                "startdate" => x => x.StartDate,
                "priority" => x => x.Priority,
                _ => x => x.Name
            };

            return direction == SortDirection.Ascending ? query.OrderBy(expression) : query.OrderByDescending(expression);
        }
    }
}
