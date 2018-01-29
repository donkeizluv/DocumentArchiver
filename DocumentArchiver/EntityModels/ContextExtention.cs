using Microsoft.EntityFrameworkCore;

namespace DocumentArchiver.EntityModels
{
    public partial class DocumentArchiverContext
    {
        //preserve
        public DocumentArchiverContext(DbContextOptions<DocumentArchiverContext> options) : base(options)
        {
        }
        public DocumentArchiverContext(string conStr) : base(new DbContextOptionsBuilder().UseSqlServer(conStr).Options)
        {
        }
        //preserve
    }
}
