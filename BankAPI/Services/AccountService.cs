using BankAPI.Data;
using BankAPI.Data.BankModels;

namespace BankAPI.Services;

public class AccountService
{
    private readonly BankContext _context;

    public AccountService(BankContext context)
    {
        _context = context;
    }

    public IEnumerable<Account> GetAll()
    {
        return _context.Accounts.ToList();
    }

    public Account? GetById(int id)
    {
        return _context.Accounts.Find(id);
    }

    public string CheckId(int id)
    {
        var existingClient = _context.Clients.Find(id);

        if(existingClient is null)
        {
            return "Error";
        }
        else
        {
            return "Ok";
        }
    }

    public Account Create(Account newAccount)
    {
        _context.Accounts.Add(newAccount);
        _context.SaveChanges();

        return newAccount;
    }

    public string CheckClientId(int id, int clientId)
    {
        var originalAccount = GetById(id);

        if(originalAccount is not null)
        {
            if(originalAccount.ClientId != clientId)
            {
                return "Error";
            }
            else
            {
                return "Ok";
            }
        }
        else
        {
            return "Error";
        }
    }

    public void Update(int id, Account account)
    {
        var existingAccount = GetById(id);

        if(existingAccount is not null)
        {
            existingAccount.AccountType = account.AccountType;
            existingAccount.Balance = account.Balance;

            _context.SaveChanges();
        }
    }

    public void Delete(int id)
    {
        var accountToDelete = GetById(id);

        if(accountToDelete is not null)
        {
            _context.Accounts.Remove(accountToDelete);
            _context.SaveChanges();
        }
    }
}