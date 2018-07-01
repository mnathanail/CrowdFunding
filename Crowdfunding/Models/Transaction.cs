using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Threading.Tasks;
namespace Crowdfunding.Models
{
    public class TransactionResult
    {
        public int ErrorCode { get; set; }
        public decimal Amount { get; set; }
        public string ErrorText { get; set; }
        public Guid TransactionId { get; set; }
    }
}