namespace Tapper.Core.Services;

public interface IInputService
{
    void SendButtonTap(int pid, InputKey key);
}
