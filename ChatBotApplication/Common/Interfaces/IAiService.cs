using Domain.Entity;

namespace ChatBotApplication.Common.Interfaces
{
    public interface IAiService
    {
        /// <summary>
        /// Hàm chat cũ (có kèm context là các đoạn văn bản tìm được)
        /// </summary>
        /// <param name="userMessage"></param>
        /// <param name="History"></param>
        /// <param name="ragContext"></param>
        /// <returns></returns>
        Task<string> GenerateResponseAsync(string userMessage, List<ChatMessage> History, string ragContext);

        /// <summary>
        /// Hàm mới: Biến văn bản thành Vector
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        Task<float[]> GenerateEmbeddingAsync(string text);
    }
}