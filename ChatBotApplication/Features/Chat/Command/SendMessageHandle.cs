using ChatBotApplication.Common.Interfaces;
using Domain.Common;
using Domain.Entity;
using Domain.Interface.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Pgvector;
using Pgvector.EntityFrameworkCore;

namespace ChatBotApplication.Features.Chat.Command
{
    public class SendMessageHandle : IRequestHandler<SendMessageCommand, Result<string>>
    {
        private readonly IAiService _aiService;
        private readonly IApplicationDbContext _context;

        public SendMessageHandle(IAiService aiService, IApplicationDbContext context)
        {
            _aiService = aiService;
            _context = context;
        }

        public async Task<Result<string>> Handle(SendMessageCommand request, CancellationToken cancellationToken)
        {
            if(string.IsNullOrWhiteSpace(request.content))
            {
                return Result<string>.Failure("Message content cannot be empty.");
            }

            //find conversation
            var conservation = await _context.Conversations
                                    .Include(c => c.Messages)
                                    .FirstOrDefaultAsync(c => c.Id ==request.ConversationId, cancellationToken);
            
            if(conservation == null)
            {
                return Result<string>.Failure("Conversation is not found.");
            }
            // user inspection
            if(conservation.UserId != request.UserId)
            {
                return Result<string>.Failure("You are not authorized to send message in this conversation.");
            }

            // RAG vector search
            string ragContext = "";
            var validChunks = new List<dynamic>(); //Dùng để lưu tạm các chunk tìm được để lát nữa lưu Citation
            try
            {
                // create vector embedding for user question
                var queryEmbedding = await _aiService.GenerateEmbeddingAsync(request.content);
                var queryVector = new Pgvector.Vector(queryEmbedding);

                //find in database
                //note: CosineDistance càng NHỎ thì càng GIỐNG (0 là giống hệt)
                var relatedDocs = await _context.DocumentChunks
                                    .OrderBy(c => c.Vector.CosineDistance(queryVector))
                                    .Take(3) //lấy 3 chunk giống nhất
                                    .Select(c => new
                                    {
                                        c.Id,
                                        c.Content,
                                        Distance = c.Vector.CosineDistance(queryVector)
                                    })
                                    .ToListAsync(cancellationToken);
                //filter result
                validChunks = relatedDocs.Where(c => c.Distance < 0.6).ToList<dynamic>();
                if(validChunks.Count > 0)
                {
                    ragContext = string.Join("\n---\n", validChunks.Select(c => c.Content));
                }
            }
            catch (Exception ex)
            {
                //Nếu lỗi vector search (ví dụ chưa có data), log lại và vẫn cho chat bình thường (không RAG)
                Console.WriteLine($"Lỗi Vector Search: {ex.Message}");
            }
            // store user message
            conservation.AddUserMessage(request.content);

            await _context.SaveChangesAsync(cancellationToken);

            // call AI Service

            var historyForAi = conservation.Messages
                            .OrderBy(m => m.CreatedAt)
                            .ToList();
            var aiResponse = await _aiService.GenerateResponseAsync(request.content, historyForAi, ragContext);

            // store AI message
            conservation.AddBotMessage(aiResponse, 0);
            await _context.SaveChangesAsync(cancellationToken);

            //store Citation
            if (validChunks.Any())
            {
                // get bot message
                var botMessage = conservation.Messages.LastOrDefault();
                if(botMessage != null)
                {
                    foreach (var chunk in validChunks)
                    {
                        double similarity = 1 - chunk.Distance;

                        var citation = new MessageCitation(
                            botMessage.Id,
                            chunk.Id,
                            similarity
                        );
                    }
                    await _context.SaveChangesAsync(cancellationToken);
                }
            }
            return Result<string>.Success(aiResponse);
        }
    }   
}