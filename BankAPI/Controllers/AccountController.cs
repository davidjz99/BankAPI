using Microsoft.AspNetCore.Mvc;
using BankAPI.Services;
using BankAPI.Data.BankModels;

namespace BankAPI.Controllers;

[ApiController]
[Route("[controller]")]

public class AccountController : ControllerBase
{
    private readonly AccountService _service;
    public AccountController(AccountService account)
    {
        _service = account;
    }

    [HttpGet]
    public IEnumerable<Account> Get()
    {
        return _service.GetAll();
    }

    [HttpGet("{id}")]
    public ActionResult<Account> GetById(int id)
    {
        var account = _service.GetById(id);

        if(account is null)
        {
            return NotFound();
        }

        return account;
    }

    [HttpPost]
    public IActionResult Create(Account account)
    {   
        string validateId = _service.CheckId(account.ClientId.Value);
        if(validateId.Equals("Error"))
        {
            return BadRequest();
        }

        var newAccount = _service.Create(account);

        return CreatedAtAction(nameof(GetById), new { id = newAccount.Id}, newAccount);
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, Account account)
    {
        if(id != account.Id)
        {
            return BadRequest();
        }
        
        var accountToUpdate = _service.GetById(id);

        if (accountToUpdate is not null)
        {
            string validateClientId = _service.CheckClientId(id, account.ClientId.Value);
            if(validateClientId.Equals("Ok"))
            {
                _service.Update(id, account);
                return NoContent();
            }
            else
            {
                return BadRequest();
            }
        }
        else
        {
            return NotFound();
        }
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var clientToDelete = _service.GetById(id);
        if(clientToDelete is not null)
        {
            _service.Delete(id);
            return Ok();
        }
        else
        {
            return NotFound();
        }
    }
}