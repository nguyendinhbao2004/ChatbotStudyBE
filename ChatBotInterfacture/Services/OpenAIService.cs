using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatBotApplication.Common.Interfaces;
using Domain.Entity;
using Domain.Enums;
using Microsoft.Extensions.Configuration;
using OpenAI.Chat;
using OpenAI.Embeddings;
using OpenAiMessage = OpenAI.Chat.ChatMessage;

namespace ChatBotInterfacture.Services
{
    public class OpenAIService : IAiService
    {
        private readonly string _apiKey;
        private readonly string _modelName;
        private readonly string _embeddingModel = "text-embedding-3-small"; // Model tạo vector rẻ và nhanh

        public OpenAIService(IConfiguration configuration)
        {
            _apiKey = configuration["AiSettings:OpenAiApiKey"];
            _modelName = configuration["AiSettings:ModelName"];
        }

        public async Task<float[]> GenerateEmbeddingAsync(string text)
        {
            //create client embedding
            EmbeddingClient client = new(_embeddingModel, _apiKey);

            //call api embedding
            OpenAIEmbedding embedding = await client.GenerateEmbeddingAsync(text);

            //return vector
            return embedding.ToFloats().ToArray();
        }

        public async Task<string> GenerateResponseAsync(string userMessage, List<Domain.Entity.ChatMessage> History, string ragContext)
        {
            ChatClient client = new(_modelName, _apiKey);
            var messages = new List<OpenAiMessage>();

            // System Prompt: Dạy bot cách dùng tài liệu
            string systemPrompt = "Bạn là trợ lý AI học tập. ";
            if (!string.IsNullOrEmpty(ragContext))
            {
                systemPrompt += $"Hãy trả lời câu hỏi dựa trên thông tin sau đây:\n\n{ragContext}\n\n";
                systemPrompt += "Nếu thông tin không có trong ngữ cảnh, hãy nói 'Tôi không tìm thấy thông tin trong tài liệu'.";
            }
            messages.Add(new SystemChatMessage(systemPrompt));
            // Convert từ History (Entity) sang OpenAI Message
            foreach(var msg in History)
            {
                if(string.IsNullOrWhiteSpace(msg.Content))
                {
                    continue;
                }
                if(msg.Role == MessageRole.User)
                {
                    messages.Add(new UserChatMessage(msg.Content));
                }
                else if(msg.Role == MessageRole.Assistant)
                {
                    messages.Add(new AssistantChatMessage(msg.Content));
                }
            }

            messages.Add(new UserChatMessage(userMessage));

            ChatCompletion completion = await client.CompleteChatAsync(messages);
            return completion.Content[0].Text;
        }

        
    }
}
