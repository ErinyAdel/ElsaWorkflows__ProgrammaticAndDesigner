using System.Net.Mime;
using System.Net;
using Elsa.Workflows;
using Elsa.Workflows.Contracts;
using ElsaServer.Models;
using Elsa.Workflows.Activities;
using Elsa.Http;
using Elsa.Workflows.Models;
using System.Text.Json;
using NodaTime;


namespace ElsaServer.Workflows
{
	public class RecurringTaskWorkflow : WorkflowBase
	{
		private readonly IClock _clock;

		public RecurringTaskWorkflow(IClock clock)
        {
			_clock = clock;
		}

		protected override void Build(IWorkflowBuilder builder)
		{
			//builder
			//	.Timer(Duration.FromSeconds(5))
			//	.WriteLine(() => $"It's now {_clock.GetCurrentInstant()}. Let's do this thing!");
		}
	}
}
