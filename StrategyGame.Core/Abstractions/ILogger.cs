namespace StrategyGame.Abstractions;
public interface ILogger
{
    void Info(string msg);
    void Warn(string msg);
}