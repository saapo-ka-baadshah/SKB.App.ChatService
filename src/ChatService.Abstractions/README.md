# ChatService.Abstractions

A shared library containing interfaces, base classes, and configuration models used across the SKB.ChatService ecosystem.

## 🎯 Purpose

This project provides the foundational abstractions that enable loose coupling between different components of the chat service. It defines the contracts and data models that other projects depend on.

## 📁 Project Structure

```
ChatService.Abstractions/
├── Events/                        # Event definitions and interfaces
│   ├── IChatEvent.cs             # Base interface for all chat events
│   ├── ChatEventBase.cs          # Abstract base class for chat events
│   └── GenericChatEvent.cs       # Concrete implementation of chat event
├── Options/                      # Configuration models
│   └── PromptOptions.cs         # Prompt configuration options
├── ChatService.Abstractions.csproj # Project file
└── README.md                    # This file
```

## 🔧 Key Components

### Events

#### IChatEvent.cs
The base interface that all chat events must implement:
```csharp
public interface IChatEvent;
```

This marker interface ensures type safety and allows for generic event processing.

#### ChatEventBase.cs
Abstract base class providing common functionality for chat events:
```csharp
public abstract class ChatEventBase : IChatEvent
{
    public object? HandlingObject { get; set; }
    public required List<string> Prompts { get; set; }
}
```

**Properties:**
- `HandlingObject`: Optional object to be processed by the chat service
- `Prompts`: Required list of prompts to be sent to the AI model

#### GenericChatEvent.cs
Concrete implementation of a chat event:
```csharp
public class GenericChatEvent : ChatEventBase;
```

This is the primary event type used by the `GenericChatConsumer` for processing chat requests.

### Options

#### PromptOptions.cs
Configuration model for managing chat prompts:
```csharp
public class PromptOptions
{
    public List<string>? SystemChatPromptList { get; set; }
    public List<string>? McpToolInstructionPrompt { get; set; }
    public List<string>? DefaultUserChatPromptList { get; set; }
}
```

**Properties:**
- `SystemChatPromptList`: System-level prompts that define the AI's role and behavior
- `McpToolInstructionPrompt`: Instructions for MCP tool usage
- `DefaultUserChatPromptList`: Default user prompts added to every conversation

## 🔗 Usage Examples

### Creating a Chat Event
```csharp
var chatEvent = new GenericChatEvent
{
    Prompts = new List<string> { "Hello, how are you?" },
    HandlingObject = new { userId = "123", context = "greeting" }
};
```

### Configuring Prompt Options
```csharp
services.Configure<PromptOptions>(options =>
{
    options.SystemChatPromptList = new List<string>
    {
        "Your name is SKB.ChatAgent.",
        "Your task is to perform operations based on the system states."
    };
    
    options.DefaultUserChatPromptList = new List<string>
    {
        "Please provide clear and concise responses."
    };
    
    options.McpToolInstructionPrompt = new List<string>
    {
        "Use MCP tools when available and appropriate."
    };
});
```

## 🏗️ Design Principles

### 1. Separation of Concerns
- Events are separated from their processing logic
- Configuration is decoupled from implementation
- Interfaces define contracts without implementation details

### 2. Extensibility
- New event types can inherit from `ChatEventBase`
- Prompt options can be extended with new properties
- The abstraction layer allows for easy testing and mocking

### 3. Type Safety
- Strong typing for all event properties
- Required properties prevent null reference exceptions
- Interface contracts ensure consistency

## 🔄 Event Flow

1. **Event Creation**: Client creates a `GenericChatEvent` with prompts and optional handling object
2. **Event Publishing**: Event is published to the message bus (MassTransit)
3. **Event Consumption**: `GenericChatConsumer` receives the event
4. **Processing**: Event data is used to construct AI chat messages
5. **Response**: AI response is generated and logged

## 🛠️ Development Guidelines

### Adding New Event Types
1. Create a new class inheriting from `ChatEventBase`
2. Add any additional properties specific to your event type
3. Create a corresponding consumer in the EventConsumers project
4. Update documentation and tests

### Extending Prompt Options
1. Add new properties to `PromptOptions`
2. Provide default values where appropriate
3. Update the configuration extension methods
4. Document the new options

### Best Practices
- Keep abstractions simple and focused
- Use meaningful property names
- Provide XML documentation for public APIs
- Follow the existing naming conventions

## 📦 Dependencies

This project has minimal dependencies:
- **.NET 9.0**: Target framework
- **No external NuGet packages**: Pure abstractions only

## 🧪 Testing

Since this project contains only abstractions:
- Unit tests should focus on data validation
- Integration tests should verify event serialization
- Mock objects can be created for testing consumers

## 📚 Related Documentation

- [Main README](../../README.md) - Overall project documentation
- [ChatService.Core](../ChatService.Core/README.md) - Main application
- [ChatService.EventConsumers](../ChatService.EventConsumers/README.md) - Event processing

## 🔮 Future Enhancements

Potential areas for extension:
- Additional event types for specific use cases
- Validation attributes for event properties
- Event versioning support
- Serialization helpers for complex objects 