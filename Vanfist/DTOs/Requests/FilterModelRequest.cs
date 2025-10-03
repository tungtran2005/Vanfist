namespace Vanfist.DTOs.Requests
{
    public class FilterModelRequest
    {
        public List<int> CategoryIds { get; set; } = new List<int>();
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 12;
    }
}
