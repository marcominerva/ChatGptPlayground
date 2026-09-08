using System.ClientModel.Primitives;

namespace AgentPlayground.Services;

internal sealed class ImageDeploymentPolicy(string deployment) : PipelinePolicy
{
    private const string HeaderName = "x-ms-oai-image-generation-deployment";

    public override void Process(PipelineMessage message, IReadOnlyList<PipelinePolicy> pipeline, int index)
    {
        message.Request.Headers.Set(HeaderName, deployment);
        ProcessNext(message, pipeline, index);
    }

    public override ValueTask ProcessAsync(PipelineMessage message, IReadOnlyList<PipelinePolicy> pipeline, int index)
    {
        message.Request.Headers.Set(HeaderName, deployment);
        return ProcessNextAsync(message, pipeline, index);
    }
}
