namespace AgentPlayground.Models;

public enum StreamState
{
    Reasoning,
    FunctionCalling,
    Answering,
    ImageGeneration,
    Completed
}