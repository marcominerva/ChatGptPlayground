# ChatGPT Playground for .NET

[![Lint Code Base](https://github.com/marcominerva/ChatGptPlayground/actions/workflows/linter.yml/badge.svg)](https://github.com/marcominerva/ChatGptPlayground/actions/workflows/linter.yml)
[![CodeQL](https://github.com/marcominerva/ChatGptPlayground/actions/workflows/codeql.yml/badge.svg)](https://github.com/marcominerva/ChatGptPlayground/actions/workflows/codeql.yml)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://github.com/marcominerva/TinyHelpers/blob/master/LICENSE)


A ready-to-use ASP.NET Core chat application backed by a Minimal API that can be used to test ChatGPT workflows.

## Introduction

Chat Playground provided by [OpenAI](https://chat.openai.com) and [Azure OpenAI service](https://oai.azure.com/chat) are great tools to showcase ChatGPT capabilities, but they are not always suitable for testing purposes. In a real-world application, we typically need to integrate ChatGPT with our workflows, and we need to test it in a controlled environment. For example, we may want to use ChatGPT in conjuction with other services (like Azure Cognitive Search or third-party APIs). In these cases, we need a simple chat application that can be easily customized to fit our needs. **ChatGPT Playgroud for .NET** aims to address this requirement by providing a simple chat-like application already integrated with ChatGPT, with a Minimal API that can be easily customize to add custom workflows. In other words, it is a starting point for testing custom interaction with ChatGPT.

## Getting Started
