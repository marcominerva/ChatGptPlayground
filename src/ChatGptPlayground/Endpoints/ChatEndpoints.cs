using ChatGptPlayground.BusinessLayer.Services.Interfaces;
using ChatGptPlayground.Shared.Models;
using MinimalHelpers.Routing;
using OperationResults.AspNetCore.Http;

namespace ChatGptPlayground.Endpoints;

public class ChatEndpoints : IEndpointRouteHandler
{
    public void MapEndpoints(IEndpointRouteBuilder endpoints)
    {
        var chatGroupApi = endpoints.MapGroup("/api/chat");

        chatGroupApi.MapPost("ask", AskAsync)
            .WithName("Ask")
            .Produces<ChatResponse>()
            .ProducesValidationProblem()
            .WithOpenApi();

        chatGroupApi.MapPost("stream", StreamAsync)
            .WithName("AskStream")
            .ProducesValidationProblem()
            .WithOpenApi();

        chatGroupApi.MapDelete("{conversationId:guid}", DeleteAsync)
            .WithName("DeleteConversation")
            .Produces(StatusCodes.Status204NoContent)
            .WithOpenApi();
    }

    public async Task<IResult> AskAsync(ChatRequest request, IChatService chatService, HttpContext httpContext)
    {
        var result = await chatService.AskAsync(request);

        var response = httpContext.CreateResponse(result);
        return response;
    }

    public IAsyncEnumerable<string> StreamAsync(ChatRequest request, IChatService chatService, HttpContext httpContext)
    {
        async IAsyncEnumerable<string> Stream()
        {
            // Requests a streaming response.
            var responseStream = chatService.AskStreamAsync(request);

            await foreach (var delta in responseStream)
            {
                yield return delta;
                await Task.Delay(100);
            }
        }

        return Stream();
    }

    public async Task<IResult> DeleteAsync(Guid conversationId, IChatService chatService, HttpContext httpContext)
    {
        var result = await chatService.DeleteAsync(conversationId);

        var response = httpContext.CreateResponse(result);
        return response;
    }
}