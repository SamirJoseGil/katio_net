using katio.Data.Models;
using katio.Data;
using Microsoft.EntityFrameworkCore;

namespace Katio.Data;

public class UnitOfWork : IUnitOfWork, IDisposable
{

    private readonly KatioContext _context;
    private bool _disposed = false;
    private IRepository<int, Book> _bookRepository;
    private IRepository<int, Author> _authorRepository;

    public UnitOfWork(KatioContext context)
    {
        _context = context;
    }


    #region Repositories
    public IRepository<int, Book> BookRepository
    {
        get
        {
            _bookRepository ??= new Repository<int, Book>(_context);
            return _bookRepository;
        }
    }

    public IRepository<int, Author> AuthorRepository
    {
        get
        {
            _authorRepository ??= new Repository<int, Author>(_context);
            return _authorRepository;
        }
    }
    #endregion

    public async Task SaveAsync()
    {
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException ex)
        {

            ex.Entries.Single().Reload();
        }
    }


    #region IDisposable
    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _context.DisposeAsync();
            }
            _disposed = true;
        }
    }

    public void Dispose()
    {
        Dispose(true);
    }
    #endregion

}