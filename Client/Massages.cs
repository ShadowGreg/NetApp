using ConsoleApp1;

namespace Client; 

public static class Massages {
    public static string ToString(Message? message) {
        return $"\n {message.Date} " +"\n"+
               $"{message.Text}  " +"\n"+
               $"{message.Author}  {message.Transmitter} \n";
    }
}