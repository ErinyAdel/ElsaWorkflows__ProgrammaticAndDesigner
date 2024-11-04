using System.Net.Mime;
using System.Net;
using Elsa.Workflows;
using Elsa.Workflows.Contracts;
using ElsaServer.Models;
using Elsa.Workflows.Activities;
using Elsa.Http;
using Elsa.Workflows.Models;
using System.Text.Json;

namespace ElsaServer.Workflows
{
    public class SaveDynamicDataInDBWorkflow : WorkflowBase
    {
        private readonly IConfiguration _configuration;

        public SaveDynamicDataInDBWorkflow(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected override void Build(IWorkflowBuilder builder)
        {
            var ServerAddress = _configuration["ServerAddress"];
            var UserObject = builder.WithVariable<UserModel>();
            var Response = builder.WithVariable<HttpResponseMessage>();

            builder.Root = new Sequence
            {
                Activities =
                {
                    new HttpEndpoint
                    {
                        Path = new("/save-data-api"),
                        SupportedMethods = new(new[] { HttpMethods.Post }),
                        CanStartWorkflow = true,
                        ParsedContent= new(UserObject)
                    },

                    new SendHttpRequest
                    {
                        Url = new Input<Uri?>(context => new Uri($"{ServerAddress}/api/User/CreateUserAsync")),
                        Method = new(HttpMethods.Post), //Method = new Input<string>("POST"),
                        Content = new Input<object>(context =>
                        {
                            var userData = UserObject.Get(context);

                            var user = new UserModel
                            {
                                Name = userData.Name,
                                Email = userData.Email
                            };

                            var serializedUser = JsonSerializer.Serialize(user, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
                            return serializedUser;
                        }),
                        ContentType = new Input<string>("application/json"),
                        Result = new Output<HttpResponseMessage>(Response)
                    },

                    new WriteHttpResponse
                    {
                        ContentType = new(MediaTypeNames.Text.Plain),
                        StatusCode = new(context =>
                        {
                            var apiResponse = Response.Get(context);
                            return (HttpStatusCode)apiResponse.StatusCode;
                        }),                        
                        Content = new(context => {
                            var outp = UserObject.Get(context);
                            var apiResponse = Response.Get(context);
                            var ResponseContent = apiResponse.Content.ReadAsStringAsync().Result;

                            return $"Received Request Email: {outp.Email}, Name: {outp.Name}. API Response: {ResponseContent}";
                        })
                    }
                }
            };
        }
    }
}
