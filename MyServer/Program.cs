namespace MyServer {
    public static class Program {
        public static void Main() {
            var server = new MyServer(3130);
            server.Start();
        }
    }
}