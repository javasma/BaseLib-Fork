using BaseLib.Core.Models;

namespace BaseLib.Core.Services
{
    public interface IJournalEntryWriter
    {
        Task<int> WriteAsync(JournalEntry entry);
    }
}