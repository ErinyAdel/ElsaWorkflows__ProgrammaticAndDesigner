using ElsaServer.Interfaces;
using Elsa;
using Elsa.Services.Models;
using System.Threading.Tasks;
using Elsa.Services;
using ElsaServer.Models;
using Elsa.ActivityResults;

namespace ElsaServer.Activities
{
    public class CreateUserActivity : Activity
    {
        private readonly IUserService _userService; // Assuming you have an IUserService for user operations

        public CreateUserActivity()
        {
        }

        public CreateUserActivity(IUserService userService)
        {
            _userService = userService;
        }

        // Ensure the method is public and correctly overrides the base class method
        public override async ValueTask<IActivityExecutionResult> ExecuteAsync(ActivityExecutionContext context)
        {
            var requestBody = context.Input.ToString(); // Assuming JSON input
            var user = ParseUserFromRequest(requestBody);

            await _userService.CreateUserAsync(user); // Call the service to create the user

            // Optionally set some output or context data if needed
            return Done(); // Return the result indicating completion
        }

        private UserModel ParseUserFromRequest(string requestBody)
        {
            // Implement parsing logic here
            return new UserModel(); // Return the created User object
        }
    }
}
