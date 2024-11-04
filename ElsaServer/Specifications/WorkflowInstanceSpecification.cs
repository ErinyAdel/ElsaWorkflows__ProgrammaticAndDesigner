using Elsa.Models;
using Elsa.Persistence;
using Elsa.Persistence.Specifications;
using Elsa.Persistence.Specifications.WorkflowInstances;
using System.Linq.Expressions;

namespace ElsaServer.Specifications
{
    public class WorkflowInstanceSpecification : Specification<WorkflowInstance>
    {
        private readonly string _requestId;

        public WorkflowInstanceSpecification(string requestId)
        {
            _requestId = requestId;
        }

        public override Expression<Func<WorkflowInstance, bool>> ToExpression()
        {
            return workflowInstance => workflowInstance.Variables.Get<string>("RequestId") == _requestId;
        }
    }
}
