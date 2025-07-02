namespace CarRentalSystem.Business.Helpers
{
    public class QueryRegisterRentalObject
    {
        public string? SearchTerm { get; set; }
        public string? SortBy { get; set; } = null;
        public bool IsDecsending { get; set; } = false;
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
