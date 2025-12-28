using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Enums
{
    public enum MessageRole
    {
        System = 1,    // Câu lệnh mồi cho AI (System Prompt)
        User = 2,      // Người dùng hỏi
        Assistant = 3  // Bot trả lời
    }
}
