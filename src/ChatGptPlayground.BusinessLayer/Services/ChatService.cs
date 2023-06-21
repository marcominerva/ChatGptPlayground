using ChatGptNet;
using ChatGptNet.Extensions;
using ChatGptPlayground.BusinessLayer.Services.Interfaces;
using ChatGptPlayground.Shared.Models;
using OperationResults;

namespace ChatGptPlayground.BusinessLayer.Services;

public class ChatService : IChatService
{
    private readonly IChatGptClient chatGptClient;

    public ChatService(IChatGptClient chatGptClient)
    {
        this.chatGptClient = chatGptClient;
    }

    public async Task<Result<ChatResponse>> AskAsync(ChatRequest request)
    {
        var response = await chatGptClient.AskAsync(request.ConversationId, request.Message);
        return new ChatResponse(response.GetMessage());
    }

    public IAsyncEnumerable<string> AskStreamAsync(ChatRequest request)
    {
        var responseStream = chatGptClient.AskStreamAsync(request.ConversationId, request.Message);
        return responseStream.AsDeltas();
    }

    public async Task<Result> DeleteAsync(Guid conversationId)
    {
        await chatGptClient.DeleteConversationAsync(conversationId);
        return Result.Ok();
    }
}
