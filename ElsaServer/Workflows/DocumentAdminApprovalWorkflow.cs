//using System.Net.Mime;
//using System.Net;
//using Elsa.Workflows;
//using Elsa.Workflows.Contracts;
//using ElsaServer.Models;
//using Elsa.Workflows.Activities;
//using Elsa.Http;
//using Elsa.Workflows.Models;
//using System.Text.Json;
//using ElsaServer.Activities;

//namespace ElsaServer.Workflows
//{
//    public class DocumentApprovalWorkflow : WorkflowBase
//    {
//        private readonly IConfiguration _configuration;

//        public DocumentApprovalWorkflow(IConfiguration configuration)
//        {
//            _configuration = configuration;
//        }

//        protected override void Build(IWorkflowBuilder builder)
//        {
//            var ServerAddress = _configuration["ServerAddress"];
//            var DocName = builder.WithVariable<string>();
//            var DocStatus = builder.WithVariable<bool>();
//            var RequestId = builder.WithVariable<int>();
//            var Response = builder.WithVariable<HttpResponseMessage>();
//            var DocModel = builder.WithVariable<DocumentModel>();
//            var workflowInstanceId = builder.WithVariable<string>("WorkflowInstanceId");

//            builder.Root = new Sequence
//            {
//                Activities =
//                {
//                    new HttpEndpoint
//                    {
//                        Path = new("/user-data-api"),
//                        SupportedMethods = new(new[] { HttpMethods.Post }),
//                        CanStartWorkflow = true,
//                        ParsedContent = new (DocModel)
//                    },

//                    new HttpEndpoint
//                    {
//                        Path = new("/admin-approval-api"),
//                        SupportedMethods = new(new[] { HttpMethods.Post }),
//                        ParsedContent = new (DocStatus)
//                    },

//                    new If
//                    {
//                        Condition = new Input<bool>(context => DocStatus.Get(context) == true),
//                        Then = new Sequence
//                        {
//                            Activities =
//                            {
//                                new SendHttpRequest
//                                {
//                                    Url = new Input<Uri?>(context => new Uri($"{ServerAddress}/api/Document/SaveDocumentAsync")),
//                                    Method = new Input<string>("POST"),
//                                    Content = new Input<object>(context =>
//                                    {
//                                        var docData = DocName.Get(context);
//                                        var docStatusData = DocStatus.Get(context);

//                                        var doc = new DocumentModel
//                                        {
//                                            Name = docData,
//                                            IsAccepted = docStatusData,
//                                            RequestId = RequestId.Get(context)
//                                        };

//                                        return JsonSerializer.Serialize(doc, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
//                                    }),
//                                    ContentType = new Input<string>("application/json"),
//                                    Result = new Output<HttpResponseMessage>(Response)
//                                },

//                                // Log HTTP response
//                                new WriteHttpResponse
//                                {
//                                    ContentType =  new(MediaTypeNames.Text.Plain),
//                                    StatusCode = new Input<HttpStatusCode>(HttpStatusCode.OK),
//                                    Content = new(context => {
//                                        var apiResponse = Response.Get(context);
//                                        var responseContent = apiResponse.Content.ReadAsStringAsync().Result;
//                                        return $"API Response: {responseContent}";
//                                    })
//                                }
//                            }
//                        },
//                        Else = new Sequence
//                        {
//                            Activities =
//                            {
//                                new SendHttpRequest
//                                {
//                                    Url = new Input<Uri?>(context => new Uri($"{ServerAddress}/api/Document/SaveDocumentAsync")),
//                                    Method = new Input<string>("POST"),
//                                    Content = new Input<object>(context =>
//                                    {
//                                        var docData = DocName.Get(context);
//                                        var docStatusData = DocStatus.Get(context);

//                                        var doc = new DocumentModel
//                                        {
//                                            Name = docData,
//                                            IsAccepted = docStatusData,
//                                            RequestId = RequestId.Get(context)
//                                        };

//                                        return JsonSerializer.Serialize(doc, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
//                                    }),
//                                    ContentType = new Input<string>("application/json"),
//                                    Result = new Output<HttpResponseMessage>(Response)
//                                },

//                                new WriteHttpResponse
//                                {
//                                    ContentType =  new(MediaTypeNames.Text.Plain),
//                                    StatusCode = new Input<HttpStatusCode>(HttpStatusCode.OK),
//                                    Content = new(context => {
//                                        return $"Document Rejected By Admin";
//                                    })
//                                }
//                            }
//                        }
//                    }
//                }
//            };
//        }
//    }
//}

using System.Net.Mime;
using System.Net;
using Elsa.Workflows;
using Elsa.Workflows.Contracts;
using Elsa.Http;
using Elsa.Workflows.Activities;
using System.Text.Json;
using ElsaServer.Models;
using Elsa.Workflows.Models;

namespace ElsaServer.Workflows
{
    public class DocumentApprovalWorkflow : WorkflowBase
    {
        private readonly IConfiguration _configuration;

        public DocumentApprovalWorkflow(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected override void Build(IWorkflowBuilder builder)
        {
            // Define workflow variables
            var serverAddress = _configuration["ServerAddress"];
            var docModel = builder.WithVariable<DocumentModel>();
            var docStatus = builder.WithVariable<bool>();
            var userId = builder.WithVariable<string>();

            // Define the workflow root sequence
            builder.Root = new Sequence
            {
                Activities =
                {
                    // Step 1: User submits document data, expecting a DocumentModel
                    new HttpEndpoint
                    {
                        Path = new("/user-data-api"),
                        SupportedMethods = new(new[] { HttpMethods.Post }),
                        CanStartWorkflow = true,
                        ParsedContent = new(docModel),
                    },

                    // Store UserId from DocumentModel into the workflow context
                    //new SetVariable<string>("UserId", context =>
                    //{
                    //    return docModel.Get(context).UserId; // Assuming UserId is part of DocumentModel
                    //}),

                    // Step 2: Admin approval endpoint
                    new HttpEndpoint
                    {
                        Path = new("/admin-approval-api"),
                        SupportedMethods = new(new[] { HttpMethods.Post }),
                        ParsedContent = new(docStatus)
                    },

                    // Step 3: Check approval status
                    new If
                    {
                        Condition = new Input<bool>(context => docStatus.Get(context)),
                        Then = new Sequence
                        {
                            Activities =
                            {
                                // Save approved document
                                new SendHttpRequest
                                {
                                    Url = new Input<Uri?>(context => new Uri($"{serverAddress}/api/Document/SaveDocumentAsync")),
                                    Method = new Input<string>("POST"),
                                    Content = new Input<object>(context =>
                                    {
                                        var document = docModel.Get(context);
                                        document.IsAccepted = true; // Mark as accepted

                                        return JsonSerializer.Serialize(document, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
                                    }),
                                    ContentType = new Input<string>("application/json"),
                                },

                                // Log successful response
                                new WriteHttpResponse
                                {
                                    ContentType = new(MediaTypeNames.Text.Plain),
                                    StatusCode = new Input<HttpStatusCode>(HttpStatusCode.OK),
                                    Content = new(context => "Document Approved and Saved")
                                }
                            }
                        },
                        Else = new Sequence
                        {
                            Activities =
                            {
                                // Handle rejection
                                new SendHttpRequest
                                {
                                    Url = new Input<Uri?>(context => new Uri($"{serverAddress}/api/Document/SaveDocumentAsync")),
                                    Method = new Input<string>("POST"),
                                    Content = new Input<object>(context =>
                                    {
                                        var document = docModel.Get(context);
                                        document.IsAccepted = false; // Mark as rejected

                                        return JsonSerializer.Serialize(document, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
                                    }),
                                    ContentType = new Input<string>("application/json"),
                                },

                                // Log rejection response
                                new WriteHttpResponse
                                {
                                    ContentType = new(MediaTypeNames.Text.Plain),
                                    StatusCode = new Input<HttpStatusCode>(HttpStatusCode.OK),
                                    Content = new(context => "Document Rejected By Admin")
                                }
                            }
                        }
                    }
                }
            };
        }
    }
}
