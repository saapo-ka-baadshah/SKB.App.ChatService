# ChatService.EventConsumers

Event processing components for SKB.ChatService that handle chat events using MassTransit and integrate with AI services.

## 🎯 Purpose

This project contains the event consumers that process chat events from the message bus, integrate with AI models (Ollama), and handle Model Context Protocol (MCP) tool invocations.

## 📁 Project Structure

```
ChatService.EventConsumers/
├── Extensions/                    # Event consumer extensions
│   └── AiLogExtensions.cs       # AI-specific logging extensions
├── GenericChatConsumer.cs        # Main chat event consumer
├── IEventConsumersAssemblyMarker.cs # Assembly marker interface
├── ChatService.EventConsumers.csproj # Project file
└── README.md                    # This file
```

## 🔧 Key Components

### GenericChatConsumer.cs
The primary event consumer that processes `GenericChatEvent` messages:

```csharp
public class GenericChatConsumer : IConsumer<GenericChatEvent>
{
    private readonly ILogger<GenericChatConsumer> _logger;
    private readonly IChatClient _chatClient;
    private readonly IList<McpClientTool> _mcpClientTools;
    private readonly PromptOptions _promptOptions;
}
```

**Dependencies:**
- `IChatClient`: AI chat client (Ollama)
- `IList<McpClientTool>`: Available MCP tools
- `ILogger<GenericChatConsumer>`: Logging service
- `IOptions<PromptOptions>`: Prompt configuration

### Extensions

#### AiLogExtensions.cs
Provides AI-specific logging extensions:
```csharp
public static class AiLogExtensions
{
    public static void LogAi(this ILogger logger, string message, params object[] args);
    public static void WarnAi(this ILogger logger, string message, params object[] args);
}
```

These extensions provide specialized logging for AI operations with consistent formatting.

### Assembly Marker
```csharp
public interface IEventConsumersAssemblyMarker;
```

Marker interface for assembly scanning and dependency injection registration.

## 🔄 Event Processing Flow

### 1. Event Reception
```csharp
public async Task Consume(ConsumeContext<GenericChatEvent> context)
{
    var chatEvent = context.Message;
    // Process the event...
}
```

### 2. Prompt Assembly
The consumer assembles a complete message chain:

1. **System Prompts**: From `PromptOptions.SystemChatPromptList`
2. **Default User Prompts**: From `PromptOptions.DefaultUserChatPromptList`
3. **MCP Tool Instructions**: From `PromptOptions.McpToolInstructionPrompt`
4. **MCP Tool Definitions**: Available tools from the MCP client
5. **User Prompts**: From the `GenericChatEvent.Prompts`
6. **Handling Object**: Optional object from the event

### 3. AI Processing
```csharp
ChatResponse chatResponse = await _chatClient.GetResponseAsync(
    messages: messages,
    options: chatOptions
);
```

### 4. Response Logging
```csharp
_logger.LogAi(
    "{timestamp} Chat response: {chatResponse}",
    DateTime.Now.ToString(CultureInfo.InvariantCulture),
    chatResponse
);
```

## 🛠️ Configuration

### Prompt Options
The consumer uses `PromptOptions` to configure:
- System chat prompts that define AI behavior
- Default user prompts added to every conversation
- MCP tool instruction prompts

### MCP Tools Integration
When MCP tools are available:
- Tool definitions are added to the chat context
- Chat options include tool configuration
- AI can invoke tools based on the conversation

### Error Handling
- Validates that prompts are provided
- Handles exceptions when processing handling objects
- Logs warnings for missing or invalid data

## 🔗 Dependencies

### Internal Projects
- `ChatService.Abstractions` - Event definitions and prompt options

### NuGet Packages
- **MassTransit.Abstractions**: Event consumer interfaces
- **Microsoft.Extensions.AI.Abstractions**: AI service abstractions
- **Microsoft.Extensions.Options**: Configuration options
- **ModelContextProtocol.Core**: MCP client tools

## 🧪 Testing

### Unit Testing
- Mock `IChatClient` to test AI integration
- Mock `IList<McpClientTool>` to test MCP tool handling
- Test prompt assembly logic
- Verify error handling scenarios

### Integration Testing
- Test with real Ollama instance
- Test MCP tool invocation
- Verify logging output
- Test event serialization/deserialization

## 📝 Logging

### AI-Specific Logging
The project provides specialized logging methods:
- `LogAi()`: For AI responses and operations
- `WarnAi()`: For AI-related warnings

### Log Format
```csharp
// Example log output
"2024-01-15 10:30:45 Chat response: Hello! I'm SKB.ChatAgent. How can I help you today?"
```

## 🔄 MassTransit Integration

### Consumer Registration
The consumer is automatically registered with MassTransit when the assembly is scanned.

### Event Processing
- Events are received asynchronously
- Processing is handled in a background thread
- Error handling prevents event processing failures

### Message Bus Configuration
The consumer works with any MassTransit transport:
- In-Memory (for testing)
- RabbitMQ
- Azure Service Bus
- Amazon SQS

## 🛠️ Development Guidelines

### Adding New Consumers
1. Create a new consumer class implementing `IConsumer<T>`
2. Inject required dependencies
3. Implement the `Consume` method
4. Add appropriate logging
5. Register with MassTransit

### Extending AI Logging
1. Add new extension methods to `AiLogExtensions`
2. Follow the existing naming convention
3. Include appropriate log levels
4. Add XML documentation

### Best Practices
- Keep consumers focused on single responsibility
- Use dependency injection for all dependencies
- Implement proper error handling
- Add comprehensive logging
- Follow async/await patterns

## 🔮 Future Enhancements

Potential improvements:
- **Multiple AI Providers**: Support for different AI services
- **Event Filtering**: Conditional processing based on event properties
- **Response Caching**: Cache AI responses for similar prompts
- **Batch Processing**: Process multiple events together
- **Metrics Collection**: Track processing performance and success rates

## 📚 Related Documentation

- [Main README](../../README.md) - Overall project documentation
- [ChatService.Core](../ChatService.Core/README.md) - Main application
- [ChatService.Abstractions](../ChatService.Abstractions/README.md) - Shared interfaces

## 🚀 Performance Considerations

- **Async Processing**: All operations are asynchronous
- **Connection Pooling**: AI clients are singleton instances
- **Memory Management**: Large responses are streamed when possible
- **Error Resilience**: Retry policies handle transient failures 