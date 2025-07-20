# SKB.ChatService

A modern .NET 9.0 chat service that integrates with AI models (Ollama) and Model Context Protocol (MCP) tools to provide intelligent chat capabilities with event-driven architecture.

## 🚀 Features

- **AI Integration**: Seamless integration with Ollama AI models for natural language processing
- **MCP Support**: Model Context Protocol integration for tool invocation and extended capabilities
- **Event-Driven Architecture**: MassTransit-based event processing for scalable chat operations
- **Configurable Prompts**: Flexible prompt management with system, user, and MCP tool instructions
- **Retry Policies**: Built-in resilience with configurable retry mechanisms
- **Modern .NET**: Built on .NET 9.0 with latest Microsoft.Extensions.AI
- **Structured Logging**: Comprehensive logging with AI-specific log extensions

## 📁 Project Structure

```
SKB.ChatService/
├── src/
│   ├── ChatService.Core/           # Main application and business logic
│   │   ├── Extensions/             # Service registration extensions
│   │   │   ├── AiExtensions.cs     # AI service configuration
│   │   │   ├── PromptExtensions.cs # Prompt options configuration
│   │   │   └── ConfigurationExtensions.cs
│   │   ├── Options/                # Configuration options
│   │   │   └── OllamaOptions.cs    # Ollama connection settings
│   │   ├── Program.cs              # Application entry point
│   │   ├── ChatService.cs          # Core service registration
│   │   └── TestControlService.cs   # Testing service
│   │
│   ├── ChatService.Abstractions/   # Shared interfaces and models
│   │   ├── Events/                 # Event definitions
│   │   │   ├── IChatEvent.cs       # Chat event interface
│   │   │   ├── ChatEventBase.cs    # Base chat event class
│   │   │   └── GenericChatEvent.cs # Generic chat event implementation
│   │   └── Options/                # Shared configuration options
│   │       └── PromptOptions.cs    # Prompt configuration model
│   │
│   └── ChatService.EventConsumers/ # Event processing components
│       ├── Extensions/             # Event consumer extensions
│       │   └── AiLogExtensions.cs  # AI-specific logging
│       ├── GenericChatConsumer.cs  # Main chat event consumer
│       └── IEventConsumersAssemblyMarker.cs
│
├── ChatService.sln                 # Solution file
├── Directory.Packages.props        # Centralized package versions
├── Directory.Build.props           # Build configuration
├── Directory.Build.targets         # Build targets
├── nuget.config                    # NuGet configuration
└── appsettings.json               # Application configuration
```

## 🛠️ Prerequisites

- **.NET 9.0 SDK** or later
- **Ollama** running locally or remotely
- **MCP Server** (optional, for tool integration)

## ⚙️ Configuration

### Ollama Configuration

Configure Ollama connection in `appsettings.json`:

```json
{
  "OllamaOptions": {
    "Host": "localhost",
    "Port": 11434,
    "Model": "llama2"
  }
}
```

### MCP Server Configuration

For Model Context Protocol integration:

```json
{
  "SseClientTransportOptions": {
    "Endpoint": "http://localhost:5025"
  }
}
```

### Prompt Configuration

Configure system prompts, user prompts, and MCP tool instructions:

```json
{
  "PromptOptions": {
    "SystemChatPromptList": [
      "Your name is SKB.ChatAgent.",
      "Your task is to perform operations based on the system states."
    ],
    "DefaultUserChatPromptList": [],
    "McpToolInstructionPrompt": [
      "Your task is to invoke the MCP tool, only if you are provided any MCP tools by the MCP server."
    ]
  }
}
```

## 🚀 Getting Started

### 1. Clone the Repository

```bash
git clone <repository-url>
cd SKB.ChatService
```

### 2. Build the Solution

```bash
dotnet build
```

### 3. Configure Ollama

Ensure Ollama is running and the desired model is available:

```bash
# Start Ollama (if not already running)
ollama serve

# Pull a model (e.g., llama2)
ollama pull llama2
```

### 4. Run the Application

```bash
cd src/ChatService.Core
dotnet run
```

## 📦 Key Dependencies

- **Microsoft.Extensions.AI**: AI service abstractions and implementations
- **OllamaSharp**: Ollama API client for .NET
- **MassTransit**: Message bus and event processing
- **ModelContextProtocol.Core**: MCP client implementation
- **SKB.Core.Policies**: Retry and resilience policies

## 🔧 Architecture

### Event-Driven Design

The service uses MassTransit for event processing:

1. **Event Publishing**: Chat events are published to the message bus
2. **Event Consumption**: `GenericChatConsumer` processes chat events
3. **AI Processing**: Events are processed through Ollama AI models
4. **Tool Integration**: MCP tools are available for enhanced functionality

### Service Registration

The application uses dependency injection with extension methods:

```csharp
// Register core chat services
builder.Services.AddCoreChatService(builder.Configuration);

// This includes:
// - AI services (Ollama + MCP)
// - Prompt options
// - Event consumers
```

### Chat Event Processing

1. **Event Reception**: `GenericChatConsumer` receives `GenericChatEvent`
2. **Prompt Assembly**: System prompts, user prompts, and MCP tool instructions are assembled
3. **AI Processing**: Messages are sent to Ollama for processing
4. **Tool Invocation**: MCP tools are invoked if available and needed
5. **Response Logging**: Chat responses are logged with AI-specific formatting

## 🧪 Testing

The project includes a `TestControlService` for testing purposes. Uncomment the following line in `Program.cs` to enable:

```csharp
builder.Services.AddHostedService<TestControlService>();
```

## 📝 Logging

The service includes AI-specific logging extensions:

- **AI Logging**: Specialized logging for AI operations
- **Structured Logging**: JSON-formatted logs with context
- **Error Handling**: Comprehensive error logging with retry information

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

## 📄 License

This project is licensed under the MIT License - see the [LICENSE.rst](LICENSE.rst) file for details.

## 👥 Authors

- **saapo-ka-baadshah** - Initial work

## 🙏 Acknowledgments

- Microsoft for .NET 9.0 and Microsoft.Extensions.AI
- Ollama team for the OllamaSharp client
- MassTransit team for the message bus implementation
- Model Context Protocol community for MCP tools integration
