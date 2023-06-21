# ChatGPT Playground for .NET

[![Lint Code Base](https://github.com/marcominerva/ChatGptPlayground/actions/workflows/linter.yml/badge.svg)](https://github.com/marcominerva/ChatGptPlayground/actions/workflows/linter.yml)
[![CodeQL](https://github.com/marcominerva/ChatGptPlayground/actions/workflows/codeql.yml/badge.svg)](https://github.com/marcominerva/ChatGptPlayground/actions/workflows/codeql.yml)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://github.com/marcominerva/TinyHelpers/blob/master/LICENSE)


A ready-to-use ASP.NET Core chat application backed by a Minimal API that can be used to test ChatGPT workflows.

![](https://raw.githubusercontent.com/marcominerva/ChatGptPlayground/master/assets/Playground.png)

## Introduction

Chat Playground provided by [OpenAI](https://chat.openai.com) and [Azure OpenAI service](https://oai.azure.com/chat) are great tools to showcase ChatGPT capabilities, but they are not always suitable for testing purposes. In a real-world application, we typically need to integrate ChatGPT with our workflows, and we need to test it in a controlled environment. For example, we may want to use ChatGPT in conjuction with other services (like Azure Cognitive Search or third-party APIs). In these cases, we need a simple chat application that can be easily customized to fit our needs. **ChatGPT Playgroud for .NET** aims to address this requirement by providing a simple chat-like application already integrated with ChatGPT, with a Minimal API that can be easily customize to add custom workflows. In other words, it is a starting point for testing custom interaction with ChatGPT.

## Getting Started

The Playground uses [ChatGptNet](https://github.com/marcominerva/ChatGptNet) for interacting with ChatGPT. The registration is made in [Program.cs](https://github.com/marcominerva/ChatGptPlayground/blob/master/src/ChatGptPlayground/Program.cs#L30):

    builder.Services.AddChatGpt(builder.Configuration);
    
You can refer to the official documentation available on GitHub for more details about this library and how to configure it.

In any case, you can safely replace this library with the one you prefer.

When the user sends a message using the Playground, it is received by the Minimal API that, in turn, calls the [ChatService](https://github.com/marcominerva/ChatGptPlayground/blob/master/src/ChatGptPlayground.BusinessLayer/Services/ChatService.cs):

    public class ChatService : IChatService
    {
        public async Task<Result<ChatResponse>> AskAsync(ChatRequest request)
        {
            // ...
        }

        public IAsyncEnumerable<string> AskStreamAsync(ChatRequest request)
        {
            // ...
        }
    }

The [AskAsync](https://github.com/marcominerva/ChatGptPlayground/blob/master/src/ChatGptPlayground.BusinessLayer/Services/ChatService.cs#L18-L22) method is called when the _Enable Response Streaming_ switch is disabled in the Playground, while [AskStreamAsync](https://github.com/marcominerva/ChatGptPlayground/blob/master/src/ChatGptPlayground.BusinessLayer/Services/ChatService.cs#L24-L28) is invoked when it is enabled:

![](https://raw.githubusercontent.com/marcominerva/ChatGptPlayground/master/assets/ResponseStreaming.png)

The [ChatRequest](https://github.com/marcominerva/ChatGptPlayground/blob/master/src/ChatGptPlayground.Shared/Models/ChatRequest.cs) argument contains the user's message. The default implementations call the corresponding methods on [ChatGptNet](https://github.com/marcominerva/ChatGptNet). You can replace them with your own logic. In this way, you can test your custom workflows, without having to worry about implementing the chat service itself.

### Contribute

The project is constantly evolving. Contributions are welcome. Feel free to file issues and pull requests on the repo and we'll address them as we can.

