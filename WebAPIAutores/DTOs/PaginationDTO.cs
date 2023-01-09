using System.Security.Principal;

namespace WebAPIAutores.DTOs
{
    public class PaginationDTO
    {
        public int Page { get; set; }
        private int recordPerPage = 10;
        private readonly int amountMaxPerPage = 50;

        public int RecordPerPage { 
            get
            {
                return recordPerPage;
            }
            set
            {
                recordPerPage = (value > amountMaxPerPage) ? amountMaxPerPage : value;
            }
        }
    }
}
