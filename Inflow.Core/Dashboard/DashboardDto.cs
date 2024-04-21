using System;
using System.Collections.Generic;

namespace Inflow.Core.Dashboard
{
    public class DashboardDto
    {
        public Summary Summary { get; set; }
        public IEnumerable<SalesByCategoryDto> SalesByCategories { get; set; }
        public IEnumerable<SpliteChartData> SplineCharts { get; set; }
        public IEnumerable<TransactionDto> Transactions { get; set; }
    }

    public class Summary
    {
        public decimal Total { get; set; }
        public int SalesCount { get; set; }
        public int SuppliesCount { get; set; }
    }

    public class SalesByCategoryDto
    {
        public string Category { get; set; }
        public int SalesCount { get; set; }
    }

    public class SpliteChartData
    {
        public string Month { get; set; }
        public decimal Income { get; set; }
        public decimal Expense { get; set; }
    }

    public class TransactionDto
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
    }
}
