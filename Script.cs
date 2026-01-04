#!/usr/bin/env dotnet 

var logger1 = new Logger();
logger1.Log();
// 実行結果：Logged by LoggerExtension.

ILogger logger2 = new Logger();
logger2.Log();
// 実行結果：Logged by ILogger default implementation.


public interface ILogger {
	void Log(){
		Console.WriteLine("Logged by ILogger default implementation.");
	}
 }
public static class LoggerExtension
{
	extension(ILogger logger)
	{
		public void Log() => Console.WriteLine("Logged by LoggerExtension.");
	}
}
public class Logger: ILogger
{
	public void Log() => Console.WriteLine("Logged by Logger."); 
}