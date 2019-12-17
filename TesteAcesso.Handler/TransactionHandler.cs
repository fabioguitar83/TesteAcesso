using MediatR;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Threading;
using System.Threading.Tasks;
using TesteAcesso.Model.Acesso;
using TesteAcesso.Model.Entity;
using TesteAcesso.Model.Handlers;
using TesteAcesso.Model.Interfaces;
using static TesteAcesso.Model.Acesso.TransactionAccountRequest;
using static TesteAcesso.Model.Entity.Status;

namespace TesteAcesso.Handler
{
    public class TransactionHandler : IRequestHandler<TransactionRequest, TransactionResponse>,
                                      IRequestHandler<StatusTransactionRequest, StatusTransactionResponse>,
                                      IRequestHandler<ExecuteTransactionRequest, ExecuteTransactionResponse>
    {

        private readonly ITransactionRepository _transactionRepository;
        private readonly IAcessoAccountService _acessoAccountService;

        public TransactionHandler(ITransactionRepository transactionRepository,
                                  IAcessoAccountService acessoAccountService)
        {
            _transactionRepository = transactionRepository;
            _acessoAccountService = acessoAccountService;
        }

        public async Task<TransactionResponse> Handle(TransactionRequest request, CancellationToken cancellationToken)
        {
            try
            {
                Log.Information("Transaction creation started. Request: {Request}", JsonConvert.SerializeObject(request));

                var transaction = new Model.Entity.Transaction()
                {
                    AccountDestination = request.AccountDestination,
                    AccountOrigin = request.AccountOrigin,
                    Value = request.Value,
                    CreateDate = DateTime.Now,
                    StatusId = eStatus.InQueue.GetHashCode()
                };

                await _transactionRepository.InsertAsync(transaction);

                Log.Information("Transaction created. Request: {Request}", JsonConvert.SerializeObject(request));

                return new TransactionResponse() { TransactionId = transaction.Id };

            }
            catch (Exception ex)
            {
                Log.Error(ex, "An unexpected error occurred while executing transaction. Request: {Request}", JsonConvert.SerializeObject(request));
                throw ex;
            }

        }

        public async Task<StatusTransactionResponse> Handle(StatusTransactionRequest request, CancellationToken cancellationToken)
        {
            try
            {

                Log.Information("Transaction status query started. Request: {Request}", JsonConvert.SerializeObject(request));

                var response = new StatusTransactionResponse();
                var transaction = await _transactionRepository.GetAsync(request.TransactionId);

                if (transaction == null)
                {
                    Log.Warning("Transaction not found. Request: {Request}", JsonConvert.SerializeObject(request));
                    return null;
                }

                response.Status = transaction.Status.Description;
                response.Message = transaction.Message;

                Log.Information("Transaction status query terminated. Request: {Request}, Response: {Response}", JsonConvert.SerializeObject(request), JsonConvert.SerializeObject(response));

                return response;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An unexpected error occurred while executing transaction. Request: {Request}", JsonConvert.SerializeObject(request));
                throw ex;
            }
        }

        public async Task<ExecuteTransactionResponse> Handle(ExecuteTransactionRequest request, CancellationToken cancellationToken)
        {
            try
            {

                Log.Information("Transaction execution started. Request: {Request}", JsonConvert.SerializeObject(request));

                var transaction = await _transactionRepository.GetAsync(request.TransactionId);

                if (!CanProcessTransaction(transaction))
                {
                    Log.Warning("The transaction is not available for processing. Request: {Request}", JsonConvert.SerializeObject(request));

                    return new ExecuteTransactionResponse() { Message = "The transaction is not available for processing", Success = false };
                }

                UpdateTransaction(transaction, eStatus.Processing);

                try
                {

                    if (!CheckBalance(transaction.AccountOrigin, transaction.Value))
                    {
                        var message = "Source account does not have sufficient balance";
                        UpdateTransaction(transaction, eStatus.Error, message);

                        Log.Warning(message + " Request: {Request}", JsonConvert.SerializeObject(request));

                        return new ExecuteTransactionResponse() { Message = message, Success = false };
                    }

                    ExecuteTransaction(transaction);

                    UpdateTransaction(transaction, eStatus.Confirmed);

                }
                catch (Exception ex)
                {
                    UpdateTransaction(transaction, eStatus.Error, "An unexpected error occurred while executing transaction");                    
                    throw ex;
                }

                Log.Information("Transaction execution terminated.Request: {Request}", JsonConvert.SerializeObject(request));

                return new ExecuteTransactionResponse() { Message = "Transaction successful", Success = true }; ;

            }
            catch (Exception ex)
            {
                Log.Error(ex, "An unexpected error occurred while executing transaction. Request: {Request}", JsonConvert.SerializeObject(request));
                throw ex;
            }
        }


        private void UpdateTransaction(Transaction transaction, eStatus status, string message = null)
        {
            transaction.StatusId = status.GetHashCode();
            transaction.UpdateDate = DateTime.Now;
            transaction.Message = message;

            _transactionRepository.UpdateAsync(transaction);

            Log.Information("Transaction {TransactionId} a has been updated to {Status}.", transaction.Id, status.ToString());
        }

        private bool CanProcessTransaction(Transaction transaction)
        {
            if (transaction.StatusId == eStatus.InQueue.GetHashCode())
                return true;

            return false;
        }

        private bool CheckBalance(string accountId, decimal value)
        {
            var account = _acessoAccountService.GetAccountAsync(accountId).Result;

            if (account.Balance < value)
                return false;

            return true;
        }

        /// <summary>
        /// Make debit to source account and credit to destination account
        /// </summary>
        /// <param name="transaction"></param>
        private void ExecuteTransaction(Transaction transaction)
        {

            ExecuteTransaction(transaction.AccountOrigin, transaction.Value, eTransactionType.Debit);

            try
            {
                ExecuteTransaction(transaction.AccountDestination, transaction.Value, eTransactionType.Credit);
            }
            catch (Exception ex)
            {
                //ROLLBACK TRANSACTION ORIGIN ACCOUNT
                ExecuteTransaction(transaction.AccountOrigin, transaction.Value, eTransactionType.Credit);

                Log.Error(ex, "An unexpected error occurred while executing transaction. {Transaction}.", JsonConvert.SerializeObject(transaction));

                throw ex;
            }
        }

        private void ExecuteTransaction(string accountNumber, decimal value, eTransactionType type)
        {
            var transactionRequest = new TransactionAccountRequest();

            transactionRequest.AccountNumber = accountNumber;
            transactionRequest.Type = type.ToString();
            transactionRequest.Value = value;

            _acessoAccountService.PostTransactionAsync(transactionRequest);

            Log.Information("Account {AccountNumber} has been {Type}.", accountNumber, type.ToString());

        }

    }
}
