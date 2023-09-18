using ChatGptNet;
using ChatGptPlayground.BusinessLayer.Services.Interfaces;
using ChatGptPlayground.Shared.Models;
using OperationResults;

namespace ChatGptPlayground.BusinessLayer.Services;

public class ChatService : IChatService
{
    private readonly IChatGptClient chatGptClient;

    private const string ContentFilteredMessage = "***** (The response was filtered by the content filtering system. Please modify your prompt and retry. To learn more about content filtering policies please read the documentation: https://go.microsoft.com/fwlink/?linkid=2198766)";

    public ChatService(IChatGptClient chatGptClient)
    {
        this.chatGptClient = chatGptClient;
    }

    public async Task<Result<ChatResponse>> AskAsync(ChatRequest request)
    {
        var response = await chatGptClient.AskAsync(request.ConversationId, request.Message);

        var message = !response.IsContentFiltered ? response.GetContent() : ContentFilteredMessage;
        return new ChatResponse(message);
    }

    public async IAsyncEnumerable<string> AskStreamAsync(ChatRequest request)
    {
        var responseStream = chatGptClient.AskStreamAsync(request.ConversationId, request.Message);

        await foreach (var response in responseStream)
        {
            var message = !response.IsContentFiltered ? response.GetContent() : ContentFilteredMessage;
            yield return message;
        }
    }

    public async Task<Result> DeleteAsync(Guid conversationId)
    {
        await chatGptClient.DeleteConversationAsync(conversationId);
        return Result.Ok();
    }
}
