using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManagementApp.Domain.Infrastructure
{
    public class OperationResult
    {
        public bool Success { get; }

        public IList<string> Errors { get; } = new List<string>();

        public OperationResult(bool success)
        {
            Success = success;
        }

        public void AddError(string error)
        {
            Errors.Add(error);
        }

        public override bool Equals(object? item)
        {
            var result = false;

            if (item is OperationResult results)
            {
                result = Success == results.Success;
                result = Errors.SequenceEqual(results.Errors);
            }

            return result;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
