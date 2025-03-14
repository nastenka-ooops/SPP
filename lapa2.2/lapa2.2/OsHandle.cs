using System.Runtime.InteropServices;

namespace lapa2._2;

public class OsHandle : IDisposable
{
    private IntPtr _handle = IntPtr.Zero;
    private bool _disposed = false;
    
    public IntPtr Handle
    {
        get => _handle;
        set
        {
            if (_handle != IntPtr.Zero)
            {
                CloseHandle(_handle); 
            }
            _handle = value;
        }
    }

    public OsHandle(IntPtr handle)
    {
        _handle = handle;
    }
    
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
    
    ~OsHandle()
    {
        Dispose(false);
    }
    
    private void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (_handle != IntPtr.Zero)
            {
                CloseHandle(_handle);
                _handle = IntPtr.Zero;
            }
            _disposed = true;
        }
    }
    
    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern bool CloseHandle(IntPtr hObject);
}