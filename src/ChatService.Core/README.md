# ChatService.Core

The main application project for SKB.ChatService, containing the core business logic, service registration, and application entry point.

## 🎯 Purpose

This project serves as the primary executable application that orchestrates all chat service components, including AI integration, event processing, and configuration management.

## 📁 Project Structure

```
ChatService.Core/
├── Extensions/                     # Service registration extensions
│   ├── AiExtensions.cs            # AI service configuration (Ollama + MCP)
│   ├── PromptExtensions.cs        # Prompt options configuration
│   ├── ConfigurationExtensions.cs # General configuration setup
│   └── MassTransit/               # MassTransit-specific extensions
├── Options/                       # Configuration options
│   └── OllamaOptions.cs          # Ollama connection settings
├── Program.cs                     # Application entry point
├── ChatService.cs                 # Core service registration
├── TestControlService.cs          # Testing service (optional)
├── appsettings.json              # Application configuration
└── README.md                     # This file
```

## 🔧 Key Components

### Program.cs
The application entry point that:
- Sets up the .NET Host Builder
- Configures logging
- Registers all chat services
- Builds and runs the application

### ChatService.cs
Static class providing the main service registration method:
```csharp
public static IServiceCollection AddCoreChatService(
    this IServiceCollection services,
    IConfiguration configuration)
```

### Extensions

#### AiExtensions.cs
Configures AI services including:
- **Ollama Integration**: Sets up OllamaSharp client with retry policies
- **MCP Client**: Configures Model Context Protocol client with SSE transport
- **Tool Registration**: Registers available MCP tools for chat processing

#### PromptExtensions.cs
Manages prompt configuration:
- System chat prompts
- Default user prompts
- MCP tool instruction prompts

#### ConfigurationExtensions.cs
Handles general configuration setup and validation.

### Options

#### OllamaOptions.cs
Configuration model for Ollama connection:
```csharp
public class OllamaOptions
{
    public string Host { get; set; } = "localhost";
    public int Port { get; set; } = 11434;
    public string Model { get; set; } = "llama2";
}
```

## 🚀 Getting Started

### Prerequisites
- .NET 9.0 SDK
- Ollama running locally or remotely
- MCP Server (optional)

### Configuration
Update `appsettings.json` with your settings:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Debug",
      "System": "Information",
      "Microsoft": "Information"
    }
  },
  "OllamaOptions": {
    "Host": "localhost",
    "Port": 11434,
    "Model": "llama2"
  },
  "SseClientTransportOptions": {
    "Endpoint": "http://localhost:5025"
  }
}
```

### Running the Application
```bash
dotnet run
```

## 🔗 Dependencies

### Internal Projects
- `ChatService.Abstractions` - Shared interfaces and models
- `ChatService.EventConsumers` - Event processing components

### NuGet Packages
- **Microsoft.Extensions.AI**: AI service abstractions
- **OllamaSharp**: Ollama API client
- **MassTransit**: Message bus and event processing
- **ModelContextProtocol.Core**: MCP client implementation
- **SKB.Core.Policies**: Retry and resilience policies
- **JetBrains.Annotations**: Code annotations

## 🧪 Testing

The project includes a `TestControlService` for testing purposes. To enable:

1. Uncomment the following line in `Program.cs`:
```csharp
builder.Services.AddHostedService<TestControlService>();
```

2. The service will start automatically when the application runs.

## 📝 Logging

The application uses structured logging with:
- Console logging by default
- Configurable log levels
- AI-specific log extensions from the EventConsumers project

## 🔄 Service Lifecycle

1. **Startup**: Application builder creates host
2. **Configuration**: Services are registered via extensions
3. **Dependency Injection**: All services are configured and injected
4. **Event Processing**: MassTransit starts processing chat events
5. **AI Integration**: Ollama and MCP clients are initialized
6. **Runtime**: Application runs until shutdown

## 🛠️ Development

### Adding New Services
1. Create your service class
2. Add it to the appropriate extension method
3. Register it in the DI container

### Configuration Changes
1. Update the configuration model in `Options/`
2. Modify the corresponding extension method
3. Update `appsettings.json` if needed

### Testing
- Use `TestControlService` for integration testing
- Add unit tests in a separate test project
- Use the logging system for debugging

## 📚 Related Documentation

- [Main README](../../README.md) - Overall project documentation
- [ChatService.Abstractions](../ChatService.Abstractions/README.md) - Shared interfaces
- [ChatService.EventConsumers](../ChatService.EventConsumers/README.md) - Event processing
